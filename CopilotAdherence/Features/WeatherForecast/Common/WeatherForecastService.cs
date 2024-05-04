using CopilotAdherence.Database.Entities;

namespace CopilotAdherence.Features.WeatherForecast.Common
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<Forecast>> ListWeatherForecastsAsync();
        Task CreateWeatherForecasAsync(Forecast forecast);
    }

    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherForecastRepository _repository;
        public WeatherForecastService(IWeatherForecastRepository repository)
        {
            _repository = repository;

        }

        public async Task<IEnumerable<Forecast>> ListWeatherForecastsAsync()
        {
            return await _repository.GetWeatherForecastsAsync();
        }

        public async Task CreateWeatherForecasAsync(Forecast forecast)
        {
            await _repository.CreateWeatherForecastAsync(forecast);
        }
    }
}
