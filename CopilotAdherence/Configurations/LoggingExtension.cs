using MediatR;
using Serilog;

namespace CopilotAdherence.Configurations
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Log the request details using Serilog directly
            Log.Information($"Handling {typeof(TRequest).Name}");

            // Call the next delegate/middleware in the pipeline
            var response = await next();

            // Log the response details using Serilog directly
            Log.Information($"Handled {typeof(TRequest).Name}");

            return response;
        }
    }
}
