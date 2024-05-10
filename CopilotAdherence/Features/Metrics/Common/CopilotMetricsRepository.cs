using CopilotAdherence.Database.Contexts;
using CopilotAdherence.Database.Entities;
using CopilotAdherence.Features.Metrics.Retrieve;
using CopilotAdherence.Features.WeatherForecast.Common;
using MongoDB.Driver;

namespace CopilotAdherence.Features.Metrics.Common
{
    public interface ICopilotMetricsRepository
    {
        Task CreateDailyStatisticsAsync(List<CopilotDailyStatistic> copilotDailyStatistics);
        Task<bool> DayExistsAsync(DateTime day);
        Task<List<CopilotDailyStatistic>> ListMetricsAsync();
    }

    public class CopilotMetricsRepository(MongoDbContext context) : ICopilotMetricsRepository
    {
        private readonly MongoDbContext _context = context;

        public async Task CreateDailyStatisticsAsync(List<CopilotDailyStatistic> copilotDailyStatistics)
        {
            await _context.DailyStatistics.InsertManyAsync(copilotDailyStatistics);
        }

        public async Task<bool> DayExistsAsync(DateTime day)
        {
            var filter = Builders<CopilotDailyStatistic>.Filter.Eq(stat => stat.Day, day.Date);
            var result = await _context.DailyStatistics.Find(filter).AnyAsync();
            return result;
        }

        public async Task<List<CopilotDailyStatistic>> ListMetricsAsync()
        {
            var filter = Builders<CopilotDailyStatistic>.Filter.Empty;
            //return await _context.Forecasts.Find(filter).ToListAsync();
            return await _context.DailyStatistics.Find(_ => true).ToListAsync();
        }
    }
}
