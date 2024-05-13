using CopilotAdherence.Features.Metrics.Common;

namespace CopilotAdherence.Hangfire
{
    public interface IMetricsJob
    {
        Task RetrieveMetricsAsync();
    }

    public class MetricsJob : IMetricsJob
    {
        private readonly IMetricsService _metricsService;

        public MetricsJob(IMetricsService metricsService)
        {
            _metricsService = metricsService;
        }

        public async Task RetrieveMetricsAsync()
        {
            await _metricsService.RetrieveMetricsAsync();
        }
    }
}
