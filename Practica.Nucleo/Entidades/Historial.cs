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
    public class Historial : Persistent
    {
        public override int Id { get; set; }
        public string Fecha { get; set; }
        public string Descripcion { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }

        public Usuario Usuario { get; set; }

        public static IList<Historial> ObtenerTodos()
        {
            IList<Historial> historiales;
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(new Historial().GetType());
                    crit.SetProjection(Projections.ProjectionList()
                        .Add(Projections.Property("Id"), "Id")
                        .Add(Projections.Property("Fecha"), "Fecha")
                        .Add(Projections.Property("Descripcion"), "Descripcion")
                        .Add(Projections.Property("Ciudad"), "Ciudad")
                        .Add(Projections.Property("Estado"), "Estado"));
                    crit.SetResultTransformer(Transformers.AliasToBean<Historial>());
                    historiales = crit.List<Historial>();
                    session.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return historiales;
        }

        public static Historial ObtenerPorId(int id)
        {
            Historial h = new Historial();
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(h.GetType());
                    crit.Add(Expression.Eq("Id", id));
                    h = (crit.UniqueResult<Historial>());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return h;
        }


        public static bool Guardar(int id, string fecha, string descripcion, string ciudad, string estado, int idUsuario)
        {
            bool realizado = false;
            try
            {

                Usuario u = Usuario.ObtenerPorId(idUsuario);
                //if (id != 0) u = ObtenerPorId(id);
                Historial h = id == 0 ? new Historial() : ObtenerPorId(id);
                h.Fecha = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                h.Descripcion = descripcion;
                h.Ciudad = ciudad;
                h.Estado = estado;
                h.Usuario = u;
                u.Save();

                if (id != 0)
                {
                    h.Update();
                }
                else
                {
                    h.Save();
                }
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
