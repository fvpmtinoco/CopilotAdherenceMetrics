using CopilotAdherence.Features.Metrics.Common;

namespace CopilotAdherence.Hangfire
{
    public class MetricsJob
    {
        private readonly IMetricsService _metricsService;

        public MetricsJob(IMetricsService metricsService)
        {
            _metricsService = metricsService;
        }

        public async Task RetrieveMetricsAsync()
        {
            await _metricsService.RetrieveMetricsAsync();
        }
    }
}
