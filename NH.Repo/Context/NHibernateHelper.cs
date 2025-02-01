using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Microsoft.Extensions.Configuration;
using System.IO;
using NH.Entity.Entities;

namespace NH.Repo.Context
{
    public static class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        
        private static (string connectionString, string dbType) GetConnectionInfo()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            return (
                config.GetConnectionString("DefaultConnection"),
                config.GetConnectionString("Type")?.ToLower() ?? "mysql"
            );
        }

        public static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
            {
                var (connectionString, dbType) = GetConnectionInfo();
                var configuration = Fluently.Configure();

                // Configure database based on type
                switch (dbType)
                {
                    case "sqlite":
                        configuration.Database(SQLiteConfiguration.Standard
                            .ConnectionString(connectionString)
                            //.ShowSql()
                        );
                        break;

                    case "mysql":
                    default:
                        configuration.Database(MySQLConfiguration.Standard
                            .ConnectionString(connectionString)
                            //.ShowSql()
                        );
                        break;
                }

                // Add mappings and build session factory
                configuration.Mappings(m => m.FluentMappings
                    .AddFromAssemblyOf<Department>())
                .ExposeConfiguration(cfg =>
                {
                    new SchemaUpdate(cfg)
                        .Execute(false, true);
                })
                .BuildSessionFactory();

                _sessionFactory = configuration.BuildSessionFactory();
            }
            return _sessionFactory;
        }

        public static ISession OpenSession()
        {
            return GetSessionFactory().OpenSession();
        }
    }
} 