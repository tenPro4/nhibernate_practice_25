using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NH.Service.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNHibernateServices(this IServiceCollection services)
        {
            // Auto-register all services
            var assembly = Assembly.GetExecutingAssembly();
            
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface && !t.IsAbstract)
                .ToList();

            foreach (var serviceType in serviceTypes)
            {
                var serviceInterface = serviceType.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{serviceType.Name}");
                
                if (serviceInterface != null)
                {
                    services.AddScoped(serviceInterface, serviceType);
                }
            }

            return services;
        }
    }
} 