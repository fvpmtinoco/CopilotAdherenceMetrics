﻿namespace CopilotAdherence.Features.WeatherForecast
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecast();
    }

    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm",
        "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly WeatherForecast[] _forecasts;

        public WeatherForecastService()
        {
            _forecasts = Enumerable.Range(1, 30).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        public Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
            => Task.FromResult(_forecasts.AsEnumerable());
    }
}
