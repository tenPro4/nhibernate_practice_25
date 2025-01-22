using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NH.Entity.Entities;

namespace nhibernate_practice
{
    public static class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        public static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
            {
                var connectionString = "Data Source=nhibernate.db;Version=3";
                
                _sessionFactory = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard
                        .ConnectionString(connectionString))
                    .Mappings(m => m.FluentMappings
                        .AddFromAssemblyOf<Department>())
                    .ExposeConfiguration(cfg => new SchemaExport(cfg)
                        .Create(true, true))
                    .BuildSessionFactory();
            }
            return _sessionFactory;
        }

        public static NHibernate.ISession OpenSession()
        {
            return GetSessionFactory().OpenSession();
        }
    }
} 