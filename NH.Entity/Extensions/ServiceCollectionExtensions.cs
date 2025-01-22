using Microsoft.Extensions.DependencyInjection;
using NH.Entity.Infrastructure;
using NH.Entity.Interfaces;
using NH.Entity.Repositories;
using System.Reflection;

namespace NH.Entity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNHibernateServices(this IServiceCollection services)
        {
            // Register NHibernate session
            services.AddScoped<NHibernate.ISession>(provider => 
                NHibernateHelper.OpenSession());

            // Register generic repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

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