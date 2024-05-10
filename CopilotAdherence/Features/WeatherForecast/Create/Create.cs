// Add this to your ListController
using CopilotAdherence.Database.Entities;
using CopilotAdherence.Features.WeatherForecast.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Features.WeatherForecast.Create;

public class CreateController : WeatherForecastControllerBase
{
    private readonly IMediator _mediator;

    public CreateController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost, ActionName("create")]
    public async Task<ActionResult<Forecast>> Create(CreateForecastRequest request)
    {
        await _mediator.Send(request);
        return Ok();
    }
}

public record CreateForecastRequest(ForecastCreateModel CreateForecast) : IRequest;

public class CreateForecastRequestHandler : IRequestHandler<CreateForecastRequest>
{
    private readonly IWeatherForecastService _forecastService;

    public CreateForecastRequestHandler(IWeatherForecastService forecastService)
        => _forecastService = forecastService;

    public async Task Handle(CreateForecastRequest request, CancellationToken cancellationToken)
    {
        var forecast = new Forecast
        {
            Id = ObjectId.GenerateNewId(),
            Date = request.CreateForecast.Date,
            TemperatureC = request.CreateForecast.TemperatureC,
            Summary = request.CreateForecast.Summary
        };

        await _forecastService.CreateWeatherForecastAsync(forecast);
    }

}

public class ForecastCreateModel
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}