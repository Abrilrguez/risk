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
    public class Orden : Persistent
    {
        public override int Id { get; set; }
        public string Folio { get; set; }
        public DateTime Fecha { get; set; }
        public Cliente Cliente { get; set; }
        public Destinatario Destinatario { get; set; }
        public Usuario Usuario { get; set; }
        public Paquete Paquete { get; set; }
        public IList<Historial> Historiales { get; set; }
        public double Precio { get; set; }
        public string NumeroRastreo { get; set; }
        public string Estado { get; set; }

        public static IList<Orden> ObtenerTodos()
        {
            IList<Orden> ordenes;
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(new Orden().GetType());
                    ordenes = crit.List<Orden>();
                    session.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ordenes;
        }

        public static Orden ObtenerPorId(int id)
        {
            Orden o = new Orden();
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(o.GetType());
                    crit.Add(Expression.Eq("Id", id));
                    o = (crit.UniqueResult<Orden>());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return o;
        }
        public bool Guardar(int id, string Folio, DateTime Fecha, Cliente Cliente, Paquete Paquete, double Precio, string NumeroRastreo, string Estado)
        {
            bool realizado = false;
            try
            {
                Orden o = id == 0 ? new Orden() : ObtenerPorId(id);
                o.Folio = Folio;
                o.Fecha = Fecha;
                o.Estado = Estado;
                o.NumeroRastreo = NumeroRastreo;
                o.Paquete = Paquete;
                o.Usuario = Usuario;
                o.Destinatario = Destinatario;
                o.Cliente = Cliente;
                o.Precio = Precio;
                if (id == 0)
                {
                    o.Save();
                }
                else
                {
                    o.Update();
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
