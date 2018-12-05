using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Practica.Nucleo.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        public Estado Estado { get; set; }

        public static string ObtenerFolio()
        {
            DateTime date = DateTime.Now;
            string folio ="";
            string idu = "";
            int n;
            string año = Convert.ToString(date.Year);

            using (ISession session = Persistent.SessionFactory.OpenSession())
            {
                var id = session.CreateSQLQuery("Select max(folio) from trackpackdb.orden;")
                            .UniqueResult();
                if (id != null) {
                    idu = Convert.ToString(id);
                    idu = idu.Remove(0, 4);
                }
                else
                {
                    idu = "0";
                }


            }
            
            

            n = Convert.ToInt32(idu) +1;
            string ceros;

            
            if (n > 999999)
            {                //001999
                if (n != 0)
                {
                    n = 1;
                }
                ceros = "00000";
                string letra = "A";
                folio = letra + año + ceros + n;
            }
            if (n > 99999)
            {
                ceros = "";
                folio = año + ceros + n;
            }
            if (n > 9999)
            {
                ceros = "0";
                folio = año + ceros + n;
            }
            if (n > 999)
            {
                ceros = "00";
                folio = año + ceros + n;
            }
            if (n > 99)
            {
                ceros = "000";
                folio = año + ceros + n;
            }
            if (n > 9)
            {
                ceros = "0000";
                folio = año + ceros + n;
            }
            if (n <= 9)
            {
                ceros = "00000";
                folio = año + ceros + n;
            }

            return folio;
        }

        public static IList<OrdenDTO> ObtenerTodos()
        {
            IList<Orden> ordenes;
            IList<OrdenDTO> ordenesTransporte = new List<OrdenDTO>();
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(new Orden().GetType());
                    ordenes = crit.List<Orden>();


                    for(int i = 0; i<ordenes.Count; i++)
                    {
                        OrdenDTO odt = new OrdenDTO();
                        odt.Id = ordenes[i].Id;
                        odt.Folio = ordenes[i].Folio;
                        odt.NumeroRastreo = ordenes[i].NumeroRastreo;
                        int estado = (int)ordenes[i].Estado;
                        if(estado == 1)
                        {
                            odt.Estado = "PENDIENTE";
                        }else if (estado== 2)
                        {
                            odt.Estado = "ENTREGADO";
                        }
                        else
                        {
                            odt.Estado = "CANCELADO";
                        }

                        odt.Fecha = ordenes[i].Fecha.ToString("MM/dd/yyyy");
                        ordenesTransporte.Add(odt);
                    }
                    
                    session.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ordenesTransporte;
        }

        public static OrdenDTO ObtenerDatosOrden()
        {
            OrdenDTO o = new OrdenDTO();
            o.Folio = ObtenerFolio();
            o.Fecha = DateTime.Now.ToString("MM/dd/yyyy");
            o.NumeroRastreo = o.Folio;
            o.Estado = Estado.PENDIENTE.ToString();
            return o;
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

        public static Orden ObtenerPorFolio(String folio)
        {
            String hola = folio;
            Orden o = new Orden();
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(o.GetType());
                    crit.Add(Expression.Eq("Folio", folio));
                    o = (crit.UniqueResult<Orden>());
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return o;
        }
        public static bool Guardar(int idOrden, int ordenEstado, double ordenPrecio, string ordenFolio, string ordenNumRastreo, DateTime ordenFecha,
                                    int idUsuario,
                                    int idPaquete, string paquetePeso, string paqueteTamanio, string paqueteContenido, string paqueteDescripcion,
                                    int idCliente, string clienteNombre, string clienteTelefono, string clienteCorreo, string clienteRfc, string clienteDomicilio,
                                    int idDestinatario, string destinatarioNombre, string destinatarioTelefono, string destinatarioCorreo, string destinatarioPersona,
                                    string destinatarioCalle, string destinatarioNumero, string destinatarioAvenida, string destinatarioColonia, string destinatarioCp,
                                    string destinatarioCiudad, string destinatarioEstado, string destinatarioReferencia)
        {
            bool realizado = false;
            try
            {

                Usuario u = Usuario.ObtenerPorId(idUsuario);

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
                o.Folio = ordenFolio;
                o.Fecha = ordenFecha;
                o.Cliente = c;
                o.Destinatario = d;
                o.Usuario = u;
                o.Paquete = p;
                o.Precio = ordenPrecio;
                o.NumeroRastreo = ordenNumRastreo;
                o.Estado = (Estado) ordenEstado;

                string subject = "";
                if (idOrden != 0)
                {
                    subject = "Información de orden de envío actualizado Trackpack";
                    o.Update();
                }
                else
                {
                    subject = "Información de orden de envío Trackpack";
                    o.Save();
                }

                EnviarEmail(o, subject);
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

        public static void EnviarEmail(Orden orden, string subject)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("track.paack@gmail.com", "tp2018**");
                MailMessage mmsg = new MailMessage();

                mmsg.To.Add(orden.Cliente.Correo);
                mmsg.To.Add(orden.Destinatario.Correo);
                mmsg.Subject = subject;
                mmsg.SubjectEncoding = Encoding.UTF8;

                body = body.Replace("[NOMBRECLIENTE]", orden.Cliente.Nombre)
                    .Replace("[DOMICILIOCLIENTE]", orden.Cliente.Domicilio)
                    .Replace("[TELEFONOCLIENTE]", orden.Cliente.Telefono)
                    .Replace("[CORREOCLIENTE]", orden.Cliente.Correo)
                    .Replace("[RFCCLIENTE]", orden.Cliente.Rfc)

                    .Replace("[NOMBREDESTINATARIO]", orden.Destinatario.Nombre)
                    .Replace("[CALLEDESTINATARIO]", orden.Destinatario.Calle)
                    .Replace("[AVENIDADESTINATARIO]", orden.Destinatario.Avenida)
                    .Replace("[COLONIADESTINATARIO]", orden.Destinatario.Colonia)
                    .Replace("[CPDESTINATARIO]", orden.Destinatario.Cp)
                    .Replace("[CIUDADDESTINATARIO]", orden.Destinatario.Ciudad)
                    .Replace("[ESTADODESTINATARIO]", orden.Destinatario.Estado)
                    .Replace("[REFERENCIADESTINATARIO]", orden.Destinatario.Referencia)
                    .Replace("[TELEFONODESTINATARIO]", orden.Destinatario.Telefono)
                    .Replace("[CORREODESTINATARIO]", orden.Destinatario.Correo)
                    .Replace("[PERSONADESTINATARIO]", orden.Destinatario.Persona)

                    .Replace("[PESOPAQUETE]", orden.Paquete.Peso)
                    .Replace("[TAMANIOPAQUETE]", orden.Paquete.Tamanio)
                    .Replace("[CONTENIDOPAQUETE]", orden.Paquete.Contenido)
                    .Replace("[DESCRIPCIONPAQUETE]", orden.Paquete.Descripcion)

                    .Replace("[FECHAORDEN]", orden.Fecha.ToString("dd/MM/YYYY"))
                    .Replace("[PRECIOORDEN]", orden.Precio.ToString())
                    .Replace("[NUMRASTREOORDEN]", orden.NumeroRastreo)
                    .Replace("[ESTADOORDEN]", orden.Estado.ToString());

                mmsg.Body = body;
                mmsg.BodyEncoding = Encoding.UTF8;
                mmsg.IsBodyHtml = true;
                mmsg.From = new MailAddress("track.paack@gmail.com");
                client.Send(mmsg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static string body = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/> <meta name='viewport' content='width=device-width, initial-scale=1'/> <title>Oxygen Confirm</title> <style type='text/css'> img{max-width: 600px; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic;}a img{border: none;}table{border-collapse: collapse !important;}#outlook a{padding:0;}.ReadMsgBody{width: 100%;}.ExternalClass{width: 100%;}.backgroundTable{margin: 0 auto; padding: 0; width: 100% !important;}table td{border-collapse: collapse;}.ExternalClass *{line-height: 115%;}.container-for-gmail-android{min-width: 600px;}*{font-family: Helvetica, Arial, sans-serif;}body{-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; width: 100% !important; margin: 0 !important; height: 100%; color: #676767;}td{font-family: Helvetica, Arial, sans-serif; font-size: 14px; color: #777777; text-align: center; line-height: 21px;}a{color: #676767; text-decoration: none !important;}.pull-left{text-align: left;}.pull-right{text-align: right;}.header-lg, .header-md, .header-sm{font-size: 32px; font-weight: 700; line-height: normal; padding: 35px 0 0; color: #4d4d4d;}.header-md{font-size: 24px;}.header-sm{padding: 5px 0; font-size: 18px; line-height: 1.3;}.content-padding{padding: 20px 0 5px;}.mobile-header-padding-right{width: 290px; text-align: right; padding-left: 10px;}.mobile-header-padding-left{width: 290px; text-align: left; padding-left: 10px;}.free-text{width: 100% !important; padding: 10px 60px 0px;}.button{padding: 30px 0;}.mini-block{border: 1px solid #e5e5e5; border-radius: 5px; background-color: #ffffff; padding: 12px 15px 15px; text-align: left; width: 253px;}.mini-container-left{width: 278px; padding: 10px 0 10px 15px;}.mini-container-right{width: 278px; padding: 10px 14px 10px 15px;}.product{text-align: left; vertical-align: top; width: 175px;}.total-space{padding-bottom: 8px; display: inline-block;}.item-table{padding: 50px 20px; width: 560px;}.item{width: 300px;}.mobile-hide-img{text-align: left; width: 125px;}.mobile-hide-img img{border: 1px solid #e6e6e6; border-radius: 4px;}.title-dark{text-align: left; border-bottom: 1px solid #cccccc; color: #4d4d4d; font-weight: 700; padding-bottom: 5px;}.item-col{padding-top: 20px; text-align: left; vertical-align: top;}.force-width-gmail{min-width:600px; height: 0px !important; line-height: 1px !important; font-size: 1px !important;}</style> <style type='text/css' media='screen'> @import url(http://fonts.googleapis.com/css?family=Oxygen:400,700); </style> <style type='text/css' media='screen'> @media screen{*{font-family: 'Oxygen', 'Helvetica Neue', 'Arial', 'sans-serif' !important;}}</style> <style type='text/css' media='only screen and(max-width: 480px)'> @media only screen and (max-width: 480px){table[class*='container-for-gmail-android']{min-width: 290px !important; width: 100% !important;}img[class='force-width-gmail']{display: none !important; width: 0 !important; height: 0 !important;}table[class='w320']{width: 320px !important;}td[class*='mobile-header-padding-left']{width: 160px !important; padding-left: 0 !important;}td[class*='mobile-header-padding-right']{width: 160px !important; padding-right: 0 !important;}td[class='header-lg']{font-size: 24px !important; padding-bottom: 5px !important;}td[class='content-padding']{padding: 5px 0 5px !important;}td[class='button']{padding: 5px 5px 30px !important;}td[class*='free-text']{padding: 10px 18px 30px !important;}td[class~='mobile-hide-img']{display: none !important; height: 0 !important; width: 0 !important; line-height: 0 !important;}td[class~='item']{width: 140px !important; vertical-align: top !important;}td[class~='quantity']{width: 50px !important;}td[class~='price']{width: 90px !important;}td[class='item-table']{padding: 30px 20px !important;}td[class='mini-container-left'], td[class='mini-container-right']{padding: 0 15px 15px !important; display: block !important; width: 290px !important;}}</style></head><body bgcolor='#f7f7f7'><table align='center' cellpadding='0' cellspacing='0' class='container-for-gmail-android' width='100%'> <tr> <td align='left' valign='top' width='100%' style='background:repeat-x url(http://s3.amazonaws.com/swu-filepicker/4E687TRe69Ld95IDWyEg_bg_top_02.jpg) #ffffff;'> <center> <img src='http://s3.amazonaws.com/swu-filepicker/SBb2fQPrQ5ezxmqUTgCr_transparent.png' class='force-width-gmail'> <table cellspacing='0' cellpadding='0' width='100%' bgcolor='#ffffff' background='http://s3.amazonaws.com/swu-filepicker/4E687TRe69Ld95IDWyEg_bg_top_02.jpg' style='background-color:transparent'> <tr> <td width='100%' height='80' valign='top' style='text-align: center; vertical-align:middle;'> <center> <table cellpadding='0' cellspacing='0' width='600' class='w320'> <tr> <td class='pull-left mobile-header-padding-left' style='vertical-align: middle; text-align:center'> <a href=''><img width='auto' height='47' src='logo.png' alt='logo'></a> </td></tr></table> </center> </td></tr></table> </center> </td></tr><tr> <td align='center' valign='top' width='100%' style='background-color: #f7f7f7;' class='content-padding'> <center> <table cellspacing='0' cellpadding='0' width='600' class='w320'> <tr> <td class='header-lg'> Your order has shipped! </td></tr><tr> <td class='free-text'> We wanted to let you know that we just shipped off your order <a href=''>#23141</a>. You'll find all the details below. </td></tr><tr> <td class='button'> <div><a href='' style='background-color:#5f4b8b;border-radius:5px;color:#ffffff;display:inline-block;font-family:'Cabin', Helvetica, Arial, sans-serif;font-size:14px;font-weight:regular;line-height:45px;text-align:center;text-decoration:none;width:155px;-webkit-text-size-adjust:none;mso-hide:all;'>Rastrea tu orden</a></div></td></tr><tr> <td class='w320'> <table cellpadding='0' cellspacing='0' width='100%'> <tr> <td class='mini-container-left'> <table cellpadding='0' cellspacing='0' width='100%'> <tr> <td class='mini-block-padding'> <table cellspacing='0' cellpadding='0' width='100%' style='border-collapse:separate !important;'> <tr> <td class='mini-block'> <span class='header-sm'>Shipping Address</span><br/> Jane Doe <br/> 123 Street <br/> Victoria, BC <br/> Canada </td></tr></table> </td></tr></table> </td><td class='mini-container-right'> <table cellpadding='0' cellspacing='0' width='100%'> <tr> <td class='mini-block-padding'> <table cellspacing='0' cellpadding='0' width='100%' style='border-collapse:separate !important;'> <tr> <td class='mini-block'> <span class='header-sm'>Date Shipped</span><br/> January 12, 2015 <br/> <br/> <span class='header-sm'>Order</span> <br/> #12342 </td></tr></table> </td></tr></table> </td></tr></table> </td></tr></table> </center> </td></tr><tr> <td align='center' valign='top' width='100%' style='background-color: #ffffff; border-top: 1px solid #e5e5e5; border-bottom: 1px solid #e5e5e5;'> <center> <table cellpadding='0' cellspacing='0' width='600' class='w320'> <tr> <td class='item-table'> <table cellspacing='0' cellpadding='0' width='100%'> <tr> <td class='title-dark' width='300'> Item </td><td class='title-dark' width='163'> Qty </td><td class='title-dark' width='97'> Total </td></tr><tr> <td class='item-col item'> <table cellspacing='0' cellpadding='0' width='100%'> <tr> <td class='product'> <span style='color: #4d4d4d; font-weight:bold;'>Golden Earings</span> <br/> Hot city looks </td></tr></table> </td><td class='item-col quantity'> 1 </td><td class='item-col'> $2.52 </td></tr><tr> <td class='item-col item mobile-row-padding'></td><td class='item-col quantity'></td><td class='item-col price'></td></tr></table> </td></tr></table> </center> </td></tr></table></div></body></html>";
    }
}
