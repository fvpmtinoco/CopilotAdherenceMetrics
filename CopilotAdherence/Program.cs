
using CopilotAdherence.Configurations;
using CopilotAdherence.Database.Contexts;
using CopilotAdherence.Features.Metrics.Common;
using CopilotAdherence.Hangfire;
using CopilotAdherence.Settings;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Redis.StackExchange;
using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace CopilotAdherence
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a new WebApplication builder
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console()  // This will print logs to the console as well
                .WriteTo.Seq("http://seq:80")
                .WriteTo.GrafanaLoki("http://loki:3100/loki/api/v1/push")
                .CreateLogger();

            Log.Information("Starting application");


            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Verbose()
            //    .Enrich.FromLogContext()
            //    .WriteTo.Seq("http://seq:80")  // Replace with your Seq server URL
            //    //.WriteTo.GrafanaLoki("http://seq:3100/loki/api/v1/push", labels: new List<LokiLabel>() { applicationLabel })
            //    .CreateLogger();


            // Add the Controllers service to the DI container
            builder.Services.AddControllers();

            // Add MongoDB context
            builder.Services.AddSingleton<MongoDbContext>();

            // Add auto mapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Register MediatR handlers
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            // Add the Endpoints API Explorer service which is required for Swagger
            builder.Services.AddEndpointsApiExplorer();

            // Add custom services
            builder.Services.AddScopedServicesCustom();

            // Add custom Swagger generation services
            builder.Services.AddSwaggerGenCustom();

            // Get the Jwt section from the appsettings file
            var jwtSettings = builder.Configuration.GetSection(nameof(Jwt));

            // Register Jwt settings to the DI container
            builder.Services.Configure<Jwt>(jwtSettings);
            builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));
            builder.Services.Configure<GitHubSettings>(builder.Configuration.GetSection(nameof(GitHubSettings)));

            // Add custom authentication services
            builder.Services.AddAuthenticationCustom(jwtSettings);

            // Add hangfire configuration services
            builder.Services.AddHangfireCustom(builder.Configuration.GetSection(nameof(ConnectionStrings)));

            var applicationLabel = new LokiLabel()
            {
                Key = "Application",
                Value = Assembly.GetExecutingAssembly().GetName().Name + "temp2"
            };

            // Build the application
            var app = builder.Build();

            // Enable middleware to serve Hangfire Dashboard without authentication
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new AllowAllDashboardAuthorizationFilter() }
            });

            // If the environment is Development
            // {
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Get the Hangfire server instance to create a scope for job activation
            var backgroundJobClient = app.Services.GetRequiredService<IBackgroundJobClient>();

            // Schedule recurring jobs
            app.UseHangfireJobs();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoPilot Adherence Metrics V1");

                // Set Swagger UI to not expand the operations in the UI
                c.DocExpansion(DocExpansion.None);
            });
            // }

            // Redirect HTTP requests to HTTPS
            app.UseHttpsRedirection();

            // Use the authentication middleware
            app.UseAuthentication();

            // Use the authorization middleware
            app.UseAuthorization();

            // Map controller routes
            app.MapControllers();

            // Run the application
            app.Run();

            Log.CloseAndFlush();
        }
    }

    public class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true; // Allows external access to the dashboard without any authorization
        }
    }
}
