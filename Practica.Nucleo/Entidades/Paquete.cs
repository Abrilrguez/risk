using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica.Nucleo.Entidades
{
    public class Paquete : Persistent
    {
        //Test
        public  override int Id { get; set; }
        public string Peso { get; set; }
        public string Tamanio { get; set; }
        public string Contenido { get; set; }
        public string Descripcion { get; set; }

        public static Paquete ObtenerPorId(int id)
        {
            Paquete p = new Paquete();
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(p.GetType());
                    crit.Add(Expression.Eq("Id", id));
                    p = (crit.UniqueResult<Paquete>());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return p;
        }
    }
}
