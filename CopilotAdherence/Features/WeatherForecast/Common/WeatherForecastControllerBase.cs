using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CopilotAdherence.Features.WeatherForecast.Common
{
    [Route("api/sample.[Action]")]
    [ApiController]
    public class WeatherForecastControllerBase : ControllerBase
    {
    }
}
