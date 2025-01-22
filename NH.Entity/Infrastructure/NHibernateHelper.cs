using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NH.Entity.Entities;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NH.Entity.Infrastructure
{
    public static class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        private static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            return config.GetConnectionString("DefaultConnection");
        }

        public static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
            {
                var connectionString = GetConnectionString();

                var configuration = Fluently.Configure()
                    .Database(MySQLConfiguration.Standard
                        .ConnectionString(connectionString)
                        //.ShowSql() //show sql query on console when query is executed
                    )
                    .Mappings(m => m.FluentMappings
                        .AddFromAssemblyOf<Department>())
                    .ExposeConfiguration(cfg =>
                    {
                        new SchemaUpdate(cfg)
                            .Execute(false, true);
                    })
                    .BuildSessionFactory();

                _sessionFactory = configuration;
            }
            return _sessionFactory;
        }

        public static ISession OpenSession()
        {
            return GetSessionFactory().OpenSession();
        }
    }
} 