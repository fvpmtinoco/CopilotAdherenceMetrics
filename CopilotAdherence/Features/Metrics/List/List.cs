using CopilotAdherence.Database.Entities;
using CopilotAdherence.Features.Metrics.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CopilotAdherence.Features.Metrics.List
{
    public class ListController(IMediator mediator) : MetricsControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// List daily metrics
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("list")]
        public async Task<IEnumerable<DailyStatistics>> List()
        {
            return await _mediator.Send(new ListCopilotMetricsRequest());
        }
    }

    public class ListCopilotMetricsRequest : IRequest<IEnumerable<DailyStatistics>> { }

    public class ListCopilotMetricsRequestHandler : IRequestHandler<ListCopilotMetricsRequest, IEnumerable<DailyStatistics>>
    {
        private readonly IMetricsService _metricService;

        public ListCopilotMetricsRequestHandler(IMetricsService metricService)
        => _metricService = metricService;

        public async Task<IEnumerable<DailyStatistics>> Handle(ListCopilotMetricsRequest request, CancellationToken cancellationToken)
            => await _metricService.ListMetricsAsync();
    }
}
