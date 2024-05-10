using AutoMapper;
using CopilotAdherence.Database.Entities;
using CopilotAdherence.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CopilotAdherence.Features.Metrics.Common
{
    public interface IMetricsService
    {
        Task<List<DailyStatistics>> RetrieveMetricsAsync();
        Task<List<DailyStatistics>> ListMetricsAsync();
    }

    public class MetricsService : IMetricsService
    {
        IOptions<GitHubSettings> _githubSettings;
        ICopilotMetricsRepository _metricsRepository;
        private readonly IMapper _mapper;

        public MetricsService(IOptions<GitHubSettings> githubSettings, ICopilotMetricsRepository copilotMetricsRepository, IMapper mapper)
        {
            _githubSettings = githubSettings;
            _metricsRepository = copilotMetricsRepository;
            _mapper = mapper;
        }

        public async Task<List<DailyStatistics>> RetrieveMetricsAsync()
        {
            using var httpClient = new HttpClient();
            // Set up the necessary headers for the request
            httpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("CopilotAdherence", "1.0"));
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", _githubSettings.Value.PAT);

            var response = await httpClient.GetAsync($"https://api.github.com/orgs/{_githubSettings.Value.OrganizationName}/copilot/usage");

            // Throws an exception if the HTTP response status is an error code.
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            //Deserialize responseBody into DailyStatistics object with newtonsoft
            var result = JsonConvert.DeserializeObject<List<DailyStatistics>>(responseBody);

            var copilotDailyStatistics = _mapper.Map<List<CopilotDailyStatistic>>(result);

            var insertList = await CheckIfRecordsExistsAsync(copilotDailyStatistics);
            if (insertList.Any())
                await _metricsRepository.CreateDailyStatisticsAsync(copilotDailyStatistics);

            return _mapper.Map<List<DailyStatistics>>(insertList);
        }

        private async Task<List<CopilotDailyStatistic>> CheckIfRecordsExistsAsync(List<CopilotDailyStatistic> dailyStatistics)
        {
            List<CopilotDailyStatistic> insertList = new();
            foreach (var dailyStat in dailyStatistics)
            {
                var exists = await _metricsRepository.DayExistsAsync(dailyStat.Day);

                //Not assuming concurrency
                if (!exists)
                    insertList.Add(dailyStat);
            }
            return insertList;
        }

        public async Task<List<DailyStatistics>> ListMetricsAsync()
        {
            var list = await _metricsRepository.ListMetricsAsync();
            return _mapper.Map<List<DailyStatistics>>(list);
        }
    }
}