using Microsoft.OpenApi.Models;

namespace CopilotAdherence.Configurations
{
    public static class SwaggerExtensions
    {
        // Define an extension method on IServiceCollection
        public static IServiceCollection AddSwaggerGenCustom(this IServiceCollection services)
        {
            // Add Swagger generation services
            services.AddSwaggerGen(c =>
            {
                // Define a Swagger document for version 1 of the API
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoPilot Adherence Metrics", Version = "v1" });

                // Define the BearerAuth scheme for JWT authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    // Provide a description for the scheme
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    // The name of the header that will be used for the scheme
                    Name = "Authorization",
                    // The location of the security scheme parameter (in this case, the header)
                    In = ParameterLocation.Header,
                    // The type of the security scheme (in this case, HTTP for JWT authentication)
                    Type = SecuritySchemeType.Http,
                    // The name of the HTTP authorization scheme (in this case, Bearer)
                    Scheme = "bearer"
                });

                // Add a security requirement for the BearerAuth scheme
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            // Reference the BearerAuth scheme
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        // No scopes are required for the scheme
                        new string[] {}
                    }
                });
            });

            // Return the service collection so that more services can be added
            return services;
        }
    }
}