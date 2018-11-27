using GoogleMaps.LocationServices;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Practica.Nucleo.Entidades
{
    public class Destinatario : Persistent
    {
        public override int Id { get; set; }
        public string Nombre { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Avenida { get; set; }
        public string Colonia { get; set; }
        public string Cp { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string Referencia { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Persona { get; set; }

        public static IList<Destinatario> ObtenerTodos()
        {
            IList<Destinatario> destinatarios;
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(new Destinatario().GetType());
                    destinatarios = crit.List<Destinatario>();
                    session.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return destinatarios;
        }

        public static Destinatario ObtenerPorId(int id)
        {
            Destinatario u = new Destinatario();
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(u.GetType());
                    crit.Add(Expression.Eq("Id", id));
                    u = (crit.UniqueResult<Destinatario>());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return u;
        }

        public static bool Guardar(string nombre, string calle, string numero, string avenida, string colonia, string cp, string ciudad,
            string estado, string referencia, string telefono, string correo, string persona)
        {
            bool realizado = false;
            try
            {
                //Usuario u = new Usuario();
                //if (id != 0) u = ObtenerPorId(id);

                //Destinatario u = id == 0 ? new Destinatario() : ObtenerPorId(id);
                Destinatario u = new Destinatario();
                u.Nombre = nombre;
                u.Calle = calle;
                u.Numero = numero;
                u.Avenida = avenida;
                u.Colonia = colonia;
                u.Cp = cp;
                u.Ciudad = ciudad;
                u.Estado = estado;

                GetdtLatLong(estado, ciudad);
                u.Referencia = referencia;
                u.Telefono = telefono;
                u.Correo = correo;
                u.Persona = persona;
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
                Destinatario u = ObtenerPorId(id);
                u.Delete();

                realizado = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return realizado;
        }




        public static string GetdtLatLong(string estado, string ciudad)
        {
            //string direccion = ciudad +","+estado;
            //string url = "http://maps.google.com/maps/api/geocode/xml?address=" + direccion + "&sensor=false";
            //string address = ciudad + ", " + estado.ToLower();
            var address = "Stavanger, Norway";

            var locationService = new GoogleLocationService();
            var point = locationService.GetLatLongFromAddress(address);

            var latitude = point.Latitude;
            var longitude = point.Longitude;

            var asd = longitude + latitude;
            return asd.ToString();
        }

    }
}
