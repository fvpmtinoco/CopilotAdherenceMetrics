using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CopilotAdherence.Features.WeatherForecast;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public WeatherForecastController(
        IMediator mediator,
        ILogger<WeatherForecastController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        return await _mediator.Send(new GetAllForecastsRequest());
    }
}

public record GetAllForecastsRequest : IRequest<IEnumerable<WeatherForecast>>
{
}

public class GetAllForecastsHandler : IRequestHandler<GetAllForecastsRequest, IEnumerable<WeatherForecast>>
{
    private readonly IWeatherForecastService _forecastService;

    public GetAllForecastsHandler(IWeatherForecastService forecastService)
    => _forecastService = forecastService;

    public async Task<IEnumerable<WeatherForecast>> Handle(GetAllForecastsRequest request, CancellationToken cancellationToken)
        => await _forecastService.GetWeatherForecast();
}

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}