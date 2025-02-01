using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NH.Repo.Context;
using NH.Repo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NH.Repo.Extensions
{
    public static class RepositoryLayerExtensions
    {
        public static IServiceCollection LoadRepositoryLayerExtensions(this IServiceCollection services)
        {
            // Add db registration here
            services.AddScoped<NHibernate.ISession>(provider =>
                NHibernateHelper.OpenSession());

            // Register generic repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;

        }
    }
}
