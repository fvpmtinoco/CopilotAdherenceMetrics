using CopilotAdherence.Database.Entities;
using CopilotAdherence.Features.WeatherForecast.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CopilotAdherence.Features.WeatherForecast.List;

//[Authorize]
[ApiController]
[Route("[controller]")]
public class ListController(IMediator mediator, ILogger<ListController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger _logger = logger;

    [HttpGet]
    [ActionName("list")]
    public async Task<IEnumerable<Forecast>> List()
    {
        return await _mediator.Send(new ListForecastsRequest());
    }
}

public record ListForecastsRequest : IRequest<IEnumerable<Forecast>> { }

public class ListForecastsRequestHandler : IRequestHandler<ListForecastsRequest, IEnumerable<Forecast>>
{
    private readonly IWeatherForecastService _forecastService;

    public ListForecastsRequestHandler(IWeatherForecastService forecastService)
    => _forecastService = forecastService;

    public async Task<IEnumerable<Forecast>> Handle(ListForecastsRequest request, CancellationToken cancellationToken)
        => await _forecastService.ListWeatherForecastsAsync();
}