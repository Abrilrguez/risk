using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica.Nucleo.Entidades
{
    public class Cliente : Persistent
    {
        public override int Id { get; set; }
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Rfc { get; set; }

        public static IList<Cliente> ObtenerTodos()
        {
            IList<Cliente> clientes;
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(new Cliente().GetType());
                    crit.SetProjection(Projections.ProjectionList()
                        .Add(Projections.Property("Id"), "Id")
                        .Add(Projections.Property("Nombre"), "Nombre")
                        .Add(Projections.Property("Domicilio"), "Domicilio")
                        .Add(Projections.Property("Telefono"), "Telefono")
                        .Add(Projections.Property("Correo"), "Correo")
                        .Add(Projections.Property("Rfc"), "Rfc"));
                    crit.SetResultTransformer(Transformers.AliasToBean<Cliente>());
                    clientes = crit.List<Cliente>();
                    session.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return clientes;
        }

        public static Cliente ObtenerPorId(int id)
        {
            Cliente c = new Cliente();
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(c.GetType());
                    crit.Add(Expression.Eq("Id", id));
                    c = (crit.UniqueResult<Cliente>());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return c;
        }

        public static bool Guardar(string nombre, string domicilio, string telefono, string correo, string rfc)
        {
            bool realizado = false;
            try
            {
                //Usuario u = new Usuario();
                //if (id != 0) u = ObtenerPorId(id);

                //Usuario u = id == 0 ? new Usuario() : ObtenerPorId(id);

                Cliente u = new Cliente();
                u.Nombre = nombre;
                u.Domicilio = domicilio;
                u.Telefono = telefono;
                u.Correo = correo;
                u.Rfc = rfc;
                u.Save();
               
                realizado = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return realizado;
        }

        public static bool Borrar(int id)
        {
            bool realizado = false;
            try
            {
                Cliente u = ObtenerPorId(id);
                u.Delete();

                realizado = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return realizado;
        }
    }
}