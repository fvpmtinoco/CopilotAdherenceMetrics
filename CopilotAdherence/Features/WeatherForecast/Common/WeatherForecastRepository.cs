using CopilotAdherence.Database.Contexts;
using CopilotAdherence.Database.Entities;
using MongoDB.Driver;
using System;

namespace CopilotAdherence.Features.WeatherForecast.Common
{
    public interface IWeatherForecastRepository
    {
        Task CreateWeatherForecastAsync(Forecast forecast);
        Task<List<Forecast>> GetWeatherForecastsAsync();
    }

    public class WeatherForecastRepository(MongoDbContext context) : IWeatherForecastRepository
    {
        private readonly MongoDbContext _context = context;

        public async Task<List<Forecast>> GetWeatherForecastsAsync()
        {
            var filter = Builders<Forecast>.Filter.Empty;
            //return await _context.Forecasts.Find(filter).ToListAsync();
            return await _context.Forecasts.Find(_ => true).ToListAsync();
        }

        public async Task CreateWeatherForecastAsync(Forecast forecast)
        {
            await _context.Forecasts.InsertOneAsync(forecast);
        }
    }
}
