using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NH.Entity.Entities;
using MySql.Data.MySqlClient;

namespace nhibernate_practice
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

        private static bool SchemaExists(string connectionString)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT COUNT(*) 
                        FROM information_schema.tables 
                        WHERE table_schema = 'nhibernate_test' 
                        AND table_name IN ('Department', 'Employee')";
                    
                    var result = Convert.ToInt32(command.ExecuteScalar());
                    return result == 2; // Both tables exist
                }
            }
            catch
            {
                return false;
            }
        }

        public static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
            {
                var connectionString = GetConnectionString();
                var schemaExists = SchemaExists(connectionString);

                var configuration = Fluently.Configure()
                    .Database(MySQLConfiguration.Standard
                        .ConnectionString(connectionString)
                        //.ShowSql() //show sql query on console when query is executed
                    )
                    .Mappings(m => m.FluentMappings
                        .AddFromAssemblyOf<Department>())
                    .ExposeConfiguration(cfg =>
                    {
                        // Use SchemaUpdate instead of SchemaExport
                        new SchemaUpdate(cfg)
                            .Execute(false, true); // false = don't script, true = execute
                    })
                    .BuildSessionFactory();

                _sessionFactory = configuration;
            }
            return _sessionFactory;
        }

        public static NHibernate.ISession OpenSession()
        {
            return GetSessionFactory().OpenSession();
        }
    }
}