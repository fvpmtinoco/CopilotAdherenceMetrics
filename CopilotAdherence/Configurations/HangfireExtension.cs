using CopilotAdherence.Hangfire;
using Hangfire;
using Hangfire.Redis.StackExchange;

namespace CopilotAdherence.Configurations;

public static class HangfireExtension
{
    // Define an extension method on IServiceCollection
    public static IServiceCollection AddHangfireCustom(this IServiceCollection services, IConfigurationSection redisSettings)
    {
        // Add Redis distributed cache services
        services.AddHangfire(configuration => configuration
            .UseRedisStorage(redisSettings.GetValue<string>("RedisConnection"), new RedisStorageOptions
            {
                // Configuration options here
                Prefix = "hangfire:",
                Db = 0,
            }));
        services.AddHangfireServer();

        // Return the service collection so that more services can be added
        return services;
    }

    public static IApplicationBuilder UseHangfireJobs(this IApplicationBuilder app)
    {
        // Schedule recurring jobs
        var recurringJobManager = app.ApplicationServices.GetRequiredService<IRecurringJobManager>();
        recurringJobManager.AddOrUpdate<IMetricsJob>(
            "Github API data retrieval",
            job => job.RetrieveMetricsAsync(),
            Cron.Daily
        );

        // More jobs can be added here

        return app;
    }
}
