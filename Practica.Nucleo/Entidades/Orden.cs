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

        public string ObtenerFolio()
        {
            DateTime date = DateTime.Now;
            
            string folio = Convert.ToString(date.Year);
            return folio;
        }
        public static IList<Orden> ObtenerTodos()
        {
            IList<Orden> ordenes;
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(new Orden().GetType());
                    crit.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Property("Id"), "Id")
                    .Add(Projections.Property("Folio"), "Folio")
                    .Add(Projections.Property("NumeroRastreo"), "NumeroRastreo")
                    .Add(Projections.Property("Estado"), "Estado")
                    .Add(Projections.Property("Fecha"), "Fecha"));
                    crit.SetResultTransformer(Transformers.AliasToBean<Orden>());
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
        public static bool Guardar(int idOrden,
                                    int idPaquete, string paquetePeso, string paqueteTamanio, string paqueteContenido, string paqueteDescripcion,
                                    int idCliente, string clienteNombre, string clienteTelefono, string clienteCorreo, string clienteRfc, string clienteDomicilio,
                                    int idDestinatario, string destinatarioNombre, string destinatarioTelefono, string destinatarioCorreo, string destinatarioPersona,
                                    string destinatarioCalle, string destinatarioNumero, string destinatarioAvenida, string destinatarioColonia, string destinatarioCp,
                                    string destinatarioCiudad, string destinatarioEstado, string destinatarioReferencia)
        {
            bool realizado = false;
            try
            {
               
                Usuario u = new Usuario();
                u.Nombre = "Bryant";
                u.Telefono = "612313213";
                u.Cuenta = "bryant";
                u.Direccion = "Itson";
                u.Rol = Enumeradores.Rol.ADMINISTRADOR;
                u.Password = "123";
                u.Save();

                Paquete p = idPaquete == 0 ? new Paquete() : Paquete.ObtenerPorId(idPaquete);
                p.Peso = paquetePeso;
                p.Tamanio = paqueteTamanio;
                p.Contenido = paqueteContenido;
                p.Descripcion = paqueteDescripcion;
                if (idPaquete != 0) { p.Update(); } else { p.Save(); }

                Cliente c = idCliente == 0 ? new Cliente() : Cliente.ObtenerPorId(idCliente);
                c.Nombre = clienteNombre;
                c.Domicilio = clienteDomicilio;
                c.Telefono = clienteTelefono;
                c.Correo = clienteCorreo;
                c.Rfc = clienteRfc;
                if (idCliente != 0) { c.Update(); } else { c.Save(); }

                Destinatario d = idDestinatario == 0 ? new Destinatario() : Destinatario.ObtenerPorId(idDestinatario);
                d.Nombre = destinatarioNombre;
                d.Calle = destinatarioCalle;
                d.Numero = destinatarioNumero;
                d.Avenida = destinatarioAvenida;
                d.Colonia = destinatarioColonia;
                d.Cp = destinatarioCp;
                d.Ciudad = destinatarioCiudad;
                d.Estado = destinatarioEstado;
                d.Referencia = destinatarioReferencia;
                d.Telefono = destinatarioTelefono;
                d.Correo = destinatarioCorreo;
                d.Persona = destinatarioPersona;
                if (idDestinatario != 0) { d.Update(); } else { d.Save(); }
                

                Orden o = idOrden == 0 ? new Orden() : Orden.ObtenerPorId(idOrden);
                o.Folio = "123124";
                o.Fecha = DateTime.Now;
                o.Cliente = c;
                o.Destinatario = d;
                o.Usuario = u;
                o.Paquete = p;
                o.Precio = 12.32;
                o.NumeroRastreo = "1231241";
                o.Estado = "Entregado";

                if (idOrden != 0)
                {
                    o.Update();
                }
                else
                {
                    o.Save();
                }
                realizado = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return realizado;
        }

        public static bool Eliminar(int id)
        {
            bool realizado = false;
            try
            {
                Orden o = ObtenerPorId(id);
                Paquete p = o.Paquete;
                o.Delete();
                p.Delete();
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
