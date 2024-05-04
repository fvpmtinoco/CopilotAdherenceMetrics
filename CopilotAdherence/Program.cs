
using CopilotAdherence.Configurations;
using CopilotAdherence.Database.Contexts;
using CopilotAdherence.Settings;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CopilotAdherence
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a new WebApplication builder
            var builder = WebApplication.CreateBuilder(args);

            // Add the Controllers service to the DI container
            builder.Services.AddControllers();

            // Add MongoDB context
            builder.Services.AddSingleton<MongoDbContext>();

            // Register MediatR handlers
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

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

            // Add custom authentication services
            builder.Services.AddAuthenticationCustom(jwtSettings);

            // Build the application
            var app = builder.Build();

            // If the environment is Development
            // {
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

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
        }
    }
}
