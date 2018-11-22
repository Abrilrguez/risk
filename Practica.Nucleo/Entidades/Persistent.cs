using log4net;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practica.Nucleo.Entidades
{
    public abstract class Persistent
    {

        static ILog logger = LogManager.GetLogger(typeof(Persistent));
        static object objLock = new object();
        static ISessionFactory sessionFactory = null;

        public abstract int Id
        {
            get;
            set;
        }

        public static ISessionFactory SessionFactory
        {
            get
            {
                ISessionFactory session;
                lock (objLock)
                {
                    session = Persistent.sessionFactory;
                }
                return session;
            }
            set
            {
                Persistent.sessionFactory = value;
            }
        }

        public static void CreateSessionFactory()
        {
            ISessionFactory factory = null;
            try
            {
                Configuration configuracion = new Configuration();
                configuracion.SetProperty("connection.connection_string", System.Configuration.ConfigurationManager.ConnectionStrings["connection"].ConnectionString);
                configuracion.SetProperty("dialect", "NHibernate.Dialect.MySQL5Dialect");
                configuracion.SetProperty("connection.driver_class", "NHibernate.Driver.MySqlDataDriver");
                configuracion.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                configuracion.AddAssembly("Practica.Nucleo");
                factory = configuracion.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Persistent.SessionFactory = factory;
        }

        public void Query()
        {
            ISession session = null;
            try
            {
                using (session = SessionFactory.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Load(this, this.Id);
                        transaction.Commit();
                    }
                }
            }
            catch (NHibernate.HibernateException ex)
            {
                throw ex;
            }
            finally
            {
                if (session != null)
                {
                    if (session.IsOpen) session.Close();
                    session.Dispose();
                }
            }
        }

        public void Save()
        {
            ISession session = null;
            try
            {
                using (session = SessionFactory.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(this);
                        transaction.Commit();
                    }
                }
            }
            catch (NHibernate.HibernateException exception)
            {
                throw exception;
            }
            finally
            {
                if (session != null)
                {
                    if (session.IsOpen) session.Close();
                    session.Dispose();
                }
            }
        }

        public void Update()
        {
            ISession session = null;
            try
            {
                using (session = SessionFactory.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(this);
                        transaction.Commit();
                    }
                }
            }
            catch (NHibernate.HibernateException exception)
            {
                throw exception;
            }
            finally
            {
                if (session != null)
                {
                    if (session.IsOpen) session.Close();
                    session.Dispose();
                }
            }
        }

        public void Delete()
        {
            ISession session = null;
            try
            {
                using (session = SessionFactory.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(this);
                        transaction.Commit();
                    }
                }
            }
            catch (NHibernate.HibernateException exception)
            {
                throw exception;
            }
            finally
            {
                if (session != null)
                {
                    if (session.IsOpen) session.Close();
                    session.Dispose();
                }
            }
        }

        public void Delete(string queryString)
        {
            ISession session = null;
            try
            {
                using (session = SessionFactory.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(queryString);
                        transaction.Commit();
                    }
                }
            }
            catch (NHibernate.HibernateException exception)
            {
                throw exception;
            }
            finally
            {
                if (session != null)
                {
                    if (session.IsOpen) session.Close();
                    session.Dispose();
                }
            }
        }

        public IList<object> QueryList()
        {
            ISession session = null;
            try
            {
                using (session = SessionFactory.OpenSession())
                {
                    var lista = session.CreateCriteria(this.GetType()).List<object>();
                    return lista;
                }
            }
            catch (NHibernate.HibernateException exception)
            {
                throw exception;
            }
            finally
            {
                if (session != null)
                {
                    if (session.IsOpen) session.Close();
                    session.Dispose();
                }
            }
        }
    }
}
