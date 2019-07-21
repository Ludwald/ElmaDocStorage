using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Elma.Models
{
    public class NHibernateHelper
    {
        private static NHibernateHelper instance;
        private static ISessionFactory sessionFactory;

        private NHibernateHelper()
        {
            sessionFactory = Fluently
                .Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(
                    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ElmaDB.mdf;Integrated Security=True;"
                ).ShowSql())
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<User>();
                    m.FluentMappings.AddFromAssemblyOf<Document>();
                })
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                .BuildSessionFactory();
        }

        public static NHibernateHelper getInstance()
        {
            if (instance == null)
                instance = new NHibernateHelper();
            return instance;
        }

        public ISession OpenSession()
        {
            return sessionFactory.OpenSession();
        }
    }
}