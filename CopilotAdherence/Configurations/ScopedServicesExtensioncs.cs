using System.Reflection;

namespace CopilotAdherence.Configurations
{
    public static class ScopedServicesExtensioncs
    {
        public static IServiceCollection AddScopedServicesCustom(this IServiceCollection collection)
        {
            // Get all types in the current assembly
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();

            // Filter the types to only include classes that end with "Service"
            var serviceTypes = allTypes.Where(t => t.IsClass && t.Name.EndsWith("Service"));

            // Register each service type with the DI container
            foreach (var type in serviceTypes)
            {
                // Get the first interface implemented by the service type
                var serviceInterface = type.GetInterfaces().FirstOrDefault();

                // If the service type implements an interface, register it with the DI container
                if (serviceInterface != null)
                {
                    collection.AddScoped(serviceInterface, type);
                }
            }
            return collection;
        }
    }
}
