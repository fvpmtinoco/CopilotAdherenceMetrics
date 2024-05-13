using CopilotAdherence.Features.Metrics.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CopilotAdherence.Features.Metrics.Retrieve
{
    //[Authorize]
    public class RetrieveController(IMediator mediator) : MetricsControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Retrieve data from Github API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("retrieve")]
        public async Task<IEnumerable<DailyStatistics>> Retrieve()
        {
            return await _mediator.Send(new RetrieveCopilotMetricsRequest());
        }
    }

    public class RetrieveCopilotMetricsRequest : IRequest<IEnumerable<DailyStatistics>> { }

    public class RetrieveCopilotMetricsRequestHandler : IRequestHandler<RetrieveCopilotMetricsRequest, IEnumerable<DailyStatistics>>
    {
        private readonly IMetricsService _metricService;

        public RetrieveCopilotMetricsRequestHandler(IMetricsService metricService)
        => _metricService = metricService;

        public async Task<IEnumerable<DailyStatistics>> Handle(RetrieveCopilotMetricsRequest request, CancellationToken cancellationToken)
            => await _metricService.RetrieveMetricsAsync();
    }
}