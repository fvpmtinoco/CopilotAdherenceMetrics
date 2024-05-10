using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CopilotAdherence.Features.Metrics.Common
{
    [Route("api/metrics.[Action]")]
    [ApiController]
    public class MetricsControllerBase : ControllerBase
    {
    }
}
