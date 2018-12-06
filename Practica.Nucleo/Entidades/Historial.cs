using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Practica.Nucleo.Enumeradores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Practica.Nucleo.Entidades
{
    public class Historial : Persistent
    {
        public override int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string Ciudad { get; set; }
        public Estado EstadoPaquete { get; set; }
        public Usuario Usuario { get; set; }
        

        public static IList<HistorialDTO> ObtenerTodos()
        {
            IList<Historial> historiales;
            IList<HistorialDTO> historialesTransporte = new List<HistorialDTO>();
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(new Historial().GetType());
                    historiales = crit.List<Historial>();


                    for (int i = 0; i < historiales.Count; i++)
                    {
                        HistorialDTO hdt = new HistorialDTO();
                        hdt.Id = historiales[i].Id;
                        hdt.Fecha = historiales[i].Fecha.ToString("MM/dd/yyyy");
                        hdt.Ciudad = historiales[i].Ciudad;
                        hdt.Estado = historiales[i].Estado;
                        hdt.Descripcion = historiales[i].Descripcion;
                        int estado = (int)historiales[i].EstadoPaquete;

                        if (estado == 1)
                        {
                            hdt.EstadoPaquete = "PENDIENTE";
                        }
                        else if (estado == 2)
                        {
                            hdt.EstadoPaquete = "ENTREGADO";
                        }
                        else
                        {
                            hdt.EstadoPaquete = "CANCELADO";
                        }
                        
                        historialesTransporte.Add(hdt);
                    }

                    session.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return historialesTransporte;
        }

        public static IList<HistorialDTO> ObtenerPorOrden(String id)
        {
            IList<Historial> historiales;
            IList<HistorialDTO> historialTransporte = new List<HistorialDTO>(); ;
            try
            {
                Orden o = Orden.ObtenerPorFolio(id);
                historiales = o.Historiales;
                for (int i = 0; i < historiales.Count; i++)
                {
                    HistorialDTO hdt = new HistorialDTO();
                    hdt.Id = historiales[i].Id;
                    hdt.Fecha = historiales[i].Fecha.ToString("MM/dd/yyyy");
                    hdt.Descripcion = historiales[i].Descripcion;
                    hdt.Estado = historiales[i].Estado;
                    hdt.Ciudad = historiales[i].Ciudad;
                    int estado = (int)historiales[i].EstadoPaquete;

                    if (estado == 1)
                    {
                        hdt.EstadoPaquete = "PENDIENTE";
                    }
                    else if (estado == 2)
                    {
                        hdt.EstadoPaquete = "ENTREGADO";
                    }
                    else
                    {
                        hdt.EstadoPaquete = "CANCELADO";
                    }

                    historialTransporte.Add(hdt);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return historialTransporte;
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

        public static bool Guardar(int id, string descripcion, string ciudad, string estado,int estadoPaquete, int idUsuario, String idOrden)
        {
            bool realizado = false;
            try
            {

                Usuario u = Usuario.ObtenerPorId(idUsuario);
                Historial h = id == 0 ? new Historial() : ObtenerPorId(id);
                h.Fecha = DateTime.Now;
                h.Descripcion = descripcion;
                h.Estado = estado;
                h.Ciudad = ciudad;
                h.EstadoPaquete = (Estado) estadoPaquete;
                h.Usuario = u;

                string subject  = "La localizacion de tu paquete se ha actualizado.";

                Orden o = Orden.ObtenerPorFolio(idOrden);
                if (id != 0)
                {
                    
                    if ((int)h.EstadoPaquete == 2) {
                        subject = "Tu paquete ha sido entregado.";
                        o.Estado = (Estado)2;
                    }
                    if ((int)h.EstadoPaquete == 3)
                    {
                        subject = "Tu paquete ha sido cancelado.";
                        o.Estado = (Estado)2;
                    }
                    o.Update();
                    h.Update();
                }
                else
                {
                    
                    IList<Historial> historiales = o.Historiales;
                    historiales.Add(h);
                    o.Historiales = historiales;
                    if ((int)h.EstadoPaquete == 2)
                    {
                        subject = "Tu paquete ha sido entregado.";
                        o.Estado = (Estado) 2;
                    }
                    if ((int)h.EstadoPaquete == 3)
                    {
                        subject = "Tu paquete ha sido cancelado.";
                        o.Estado = (Estado)3;
                    }
                    o.Update();
                }
                EnviarEmail(h, idOrden, subject);
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
                Historial h = ObtenerPorId(id);
                h.Delete();

                realizado = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return realizado;
        }


        public static void EnviarEmail(Historial h, string idOrden, string subject)
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

                Orden o = Orden.ObtenerPorFolio(idOrden);
                int idCliente = o.Cliente.Id;
                int idDestinatario = o.Destinatario.Id;
                Cliente c = Cliente.ObtenerPorId(idCliente);
                string correoCliente = c.Correo;
                Destinatario d = Destinatario.ObtenerPorId(idDestinatario);
                string correoDestinatario = d.Correo;


                mmsg.To.Add(correoCliente);
                mmsg.To.Add(correoDestinatario);
                mmsg.Subject = subject;
                mmsg.SubjectEncoding = Encoding.UTF8;

                body = body.Replace("[NOMBRECLIENTE]", c.Nombre)
                    .Replace("[NOMBREDESTINATARIO]", d.Nombre)
                    .Replace("[SUBJECT]", subject)
                    .Replace("[FECHAORDEN]", o.Fecha.ToString("dd/MM/YYYY"))
                    .Replace("[PRECIOORDEN]", o.Precio.ToString())
                    .Replace("[NUMRASTREOORDEN]", o.NumeroRastreo)
                    .Replace("[ESTADOORDEN]", o.Estado.ToString())
                    .Replace("[FECHAHISTORIAL]", h.Fecha.ToString("dd/MM/yyyy"))
                    .Replace("[DESCRIPCIONHISTORIAL]", h.Descripcion)
                    .Replace("[ESTADOHISTORIAL]", h.Estado)
                    .Replace("[CIUDADHISTORIAL]", h.Ciudad)
                    .Replace("[CIUDADESTADODESTINATARIO]", d.Ciudad + "," + d.Estado)
                    .Replace("[EPHISTORIAL]", h.EstadoPaquete.ToString());


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
        /*
        static string body = "<html>" +
                           "<head>" +
                           "<title>Historial de paquete.</title>" +
                           "</head>" +
                           "<body>" +
                           "Hola [NOMBRECLIENTE], tu orden ha sido actualizada.</br>" +
                           "Con los siguientes datos: </br>" +
                           "Fecha: [FECHAORDEN] Número de rastreo: [NUMRASTREOORDEN] Precio: [PRECIOORDEN] Estado de la orden: [ESTADOORDEN].</br>" +
                           "Remitente: </br>" +
                           "Nombre: [NOMBRECLIENTE] </br>" +
                           "Destinatario: </br>" +
                           "Nombre: [NOMBREDESTINATARIO] </br>" +
                           "Historial: </br>" +
                           "Fecha: [FECHAHISTORIAL] Descripcion: [DESCRIPCIONHISTORIAL] Estado: [ESTADOHISTORIAL] Ciudad: [CIUDADHISTORIAL] Ciudad: [CIUDADHISTORIAL] Estado de paquete [EPHISTORIAL]</br>" +
                           "</body>" +
                           "</html>";
                           */

        //static string body = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/> <meta name='viewport' content='width=device-width, initial-scale=1'/> <title>Oxygen Confirm</title> <style type='text/css'> img{max-width: 600px; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic;}a img{border: none;}table{border-collapse: collapse !important;}#outlook a{padding:0;}.ReadMsgBody{width: 100%;}.ExternalClass{width: 100%;}.backgroundTable{margin: 0 auto; padding: 0; width: 100% !important;}table td{border-collapse: collapse;}.ExternalClass *{line-height: 115%;}.container-for-gmail-android{min-width: 600px;}*{font-family: Helvetica, Arial, sans-serif;}body{-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; width: 100% !important; margin: 0 !important; height: 100%; color: #676767;}td{font-family: Helvetica, Arial, sans-serif; font-size: 14px; color: #777777; text-align: center; line-height: 21px;}a{color: #676767; text-decoration: none !important;}.pull-left{text-align: left;}.pull-right{text-align: right;}.header-lg, .header-md, .header-sm{font-size: 32px; font-weight: 700; line-height: normal; padding: 35px 0 0; color: #4d4d4d;}.header-md{font-size: 24px;}.header-sm{padding: 5px 0; font-size: 18px; line-height: 1.3;}.content-padding{padding: 20px 0 5px;}.mobile-header-padding-right{width: 290px; text-align: right; padding-left: 10px;}.mobile-header-padding-left{width: 290px; text-align: left; padding-left: 10px;}.free-text{width: 100% !important; padding: 10px 60px 0px;}.button{padding: 30px 0;}.mini-block{border: 1px solid #e5e5e5; border-radius: 5px; background-color: #ffffff; padding: 12px 15px 15px; text-align: left; width: 253px;}.mini-container-left{width: 278px; padding: 10px 0 10px 15px;}.mini-container-right{width: 278px; padding: 10px 14px 10px 15px;}.product{text-align: left; vertical-align: top; width: 175px;}.total-space{padding-bottom: 8px; display: inline-block;}.item-table{padding: 50px 20px; width: 560px;}.item{width: 300px;}.mobile-hide-img{text-align: left; width: 125px;}.mobile-hide-img img{border: 1px solid #e6e6e6; border-radius: 4px;}.title-dark{text-align: left; border-bottom: 1px solid #cccccc; color: #4d4d4d; font-weight: 700; padding-bottom: 5px;}.item-col{padding-top: 20px; text-align: left; vertical-align: top;}.force-width-gmail{min-width:600px; height: 0px !important; line-height: 1px !important; font-size: 1px !important;}</style> <style type='text/css' media='screen'> @import url(http://fonts.googleapis.com/css?family=Oxygen:400,700); </style> <style type='text/css' media='screen'> @media screen{*{font-family: 'Oxygen', 'Helvetica Neue', 'Arial', 'sans-serif' !important;}}</style> <style type='text/css' media='only screen and(max-width: 480px)'> @media only screen and (max-width: 480px){table[class*='container-for-gmail-android']{min-width: 290px !important; width: 100% !important;}img[class='force-width-gmail']{display: none !important; width: 0 !important; height: 0 !important;}table[class='w320']{width: 320px !important;}td[class*='mobile-header-padding-left']{width: 160px !important; padding-left: 0 !important;}td[class*='mobile-header-padding-right']{width: 160px !important; padding-right: 0 !important;}td[class='header-lg']{font-size: 24px !important; padding-bottom: 5px !important;}td[class='content-padding']{padding: 5px 0 5px !important;}td[class='button']{padding: 5px 5px 30px !important;}td[class*='free-text']{padding: 10px 18px 30px !important;}td[class~='mobile-hide-img']{display: none !important; height: 0 !important; width: 0 !important; line-height: 0 !important;}td[class~='item']{width: 140px !important; vertical-align: top !important;}td[class~='quantity']{width: 50px !important;}td[class~='price']{width: 90px !important;}td[class='item-table']{padding: 30px 20px !important;}td[class='mini-container-left'], td[class='mini-container-right']{padding: 0 15px 15px !important; display: block !important; width: 290px !important;}}</style></head><body bgcolor='#f7f7f7'><table align='center' cellpadding='0' cellspacing='0' class='container-for-gmail-android' width='100%'> <tr> <td align='left' valign='top' width='100%' style='background:repeat-x url(http://s3.amazonaws.com/swu-filepicker/4E687TRe69Ld95IDWyEg_bg_top_02.jpg) #ffffff;'> <center> <img src='http://s3.amazonaws.com/swu-filepicker/SBb2fQPrQ5ezxmqUTgCr_transparent.png' class='force-width-gmail'> <table cellspacing='0' cellpadding='0' width='100%' bgcolor='#ffffff' background='http://s3.amazonaws.com/swu-filepicker/4E687TRe69Ld95IDWyEg_bg_top_02.jpg' style='background-color:transparent'> <tr> <td width='100%' height='80' valign='top' style='text-align: center; vertical-align:middle;'> <center> <table cellpadding='0' cellspacing='0' width='600' class='w320'> <tr> <td class='pull-left mobile-header-padding-left' style='vertical-align: middle; text-align:center'> <a href=''><img width='auto' height='47' src='https://i.pinimg.com/originals/52/ed/4b/52ed4b635f8e73a4e17626341b449afb.png' alt='logo'></a> </td></tr></table> </center> </td></tr></table> </center> </td></tr><tr> <td align='center' valign='top' width='100%' style='background-color: #f7f7f7;' class='content-padding'> <center> <table cellspacing='0' cellpadding='0' width='600' class='w320'> <tr> <td class='header-lg'> Your order has shipped! </td></tr><tr> <td class='free-text'> Hola [NOMBRECLIENTE], tu numero de rastreo es: <a href=''>[NUMRASTREOORDEN]</a>. Obten tus detalles. </td></tr><tr> <td class='button'> <div><a href='' style='background-color:#5f4b8b;border-radius:5px;color:#ffffff;display:inline-block;font-family:'Cabin', Helvetica, Arial, sans-serif;font-size:14px;font-weight:regular;line-height:45px;text-align:center;text-decoration:none;width:155px;-webkit-text-size-adjust:none;mso-hide:all;'>Rastrea tu orden</a></div></td></tr><tr> <td class='w320'> <table cellpadding='0' cellspacing='0' width='100%'> <tr> <td class='mini-container-left'> <table cellpadding='0' cellspacing='0' width='100%'> <tr> <td class='mini-block-padding'> <table cellspacing='0' cellpadding='0' width='100%' style='border-collapse:separate !important;'> <tr> <td class='mini-block'> <span class='header-sm'>Remitente: </span><br/> [NOMBRECLIENTE] </br> <span class='header-sm'>Destinatario: </span> <br/>[NOMBREDESTINATARIO]</td></tr></table> </td></tr></table> </td><td class='mini-container-right'> <table cellpadding='0' cellspacing='0' width='100%'> <tr> <td class='mini-block-padding'> <table cellspacing='0' cellpadding='0' width='100%' style='border-collapse:separate !important;'> <tr> <td class='mini-block'> <span class='header-sm'>Fecha orden</span><br/> [FECHAORDEN] <br/> <br/> <span class='header-sm'>Numero rastreo</span> <br/> [NUMRASTREOORDEN] </td></tr></table> </td></tr></table> </td></tr></table> </td></tr></table> </center> </td></tr><tr> <td align='center' valign='top' width='100%' style='background-color: #ffffff; border-top: 1px solid #e5e5e5; border-bottom: 1px solid #e5e5e5;'> <center> <table cellpadding='0' cellspacing='0' width='600' class='w320'> <tr> <td class='item-table'> <table cellspacing='0' cellpadding='0' width='100%'> <tr> <td class='title-dark' width='300'> Contenido paquete </td><td class='title-dark' width='163'> Qty </td><td class='title-dark' width='97'> Total </td></tr><tr> <td class='item-col item'> <table cellspacing='0' cellpadding='0' width='100%'> <tr> <td class='product'> <span style='color: #4d4d4d; font-weight:bold;'>Estado</span> <br/> [ESTADOORDEN] </table> </td><td class='item-col quantity'> 1 </td><td class='item-col'> $[PRECIOORDEN] </td></tr> <table cellspacing='0' cellpadding='0' width='100%'> <tr> <td class='product'> <span style='color: #4d4d4d; font-weight:bold;'>Estado</span> <br/> [ESTADOORDEN] </table> </td><tr> <td class='item-col item mobile-row-padding'></td><td class='item-col quantity'></td><td class='item-col price'></td></tr></table> </td></tr></table> </center> </td></tr></table></div></body></html>";

        static string body = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/> <meta name='viewport' content='width=device-width, initial-scale=1'/> <title>Oxygen Confirm</title> <style type='text/css'> img{max-width: 600px; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic;}a img{border: none;}table{border-collapse: collapse !important;}#outlook a{padding:0;}.ReadMsgBody{width: 100%;}.ExternalClass{width: 100%;}.backgroundTable{margin: 0 auto; padding: 0; width: 100% !important;}table td{border-collapse: collapse;}.ExternalClass *{line-height: 115%;}.container-for-gmail-android{min-width: 600px;}*{font-family: Helvetica, Arial, sans-serif;}body{-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; width: 100% !important; margin: 0 !important; height: 100%; color: #676767;}td{font-family: Helvetica, Arial, sans-serif; font-size: 14px; color: #777777; text-align: center; line-height: 21px;}a{color: #676767; text-decoration: none !important;}.pull-left{text-align: left;}.pull-right{text-align: right;}.header-lg, .header-md, .header-sm{font-size: 32px; font-weight: 700; line-height: normal; padding: 35px 0 0; color: #4d4d4d;}.header-md{font-size: 24px;}.header-sm{padding: 5px 0; font-size: 18px; line-height: 1.3;}.content-padding{padding: 20px 0 5px;}.mobile-header-padding-right{width: 290px; text-align: right; padding-left: 10px;}.mobile-header-padding-left{width: 290px; text-align: left; padding-left: 10px;}.free-text{width: 100% !important; padding: 10px 60px 0px;}.button{padding: 30px 0;}.mini-block{border: 1px solid #e5e5e5; border-radius: 5px; background-color: #ffffff; padding: 12px 15px 15px; text-align: left; width: 253px;}.mini-container-left{width: 278px; padding: 10px 0 10px 15px;}.mini-container-right{width: 278px; padding: 10px 14px 10px 15px;}.product{text-align: left; vertical-align: top; width: 175px;}.total-space{padding-bottom: 8px; display: inline-block;}.item-table{padding: 50px 20px; width: 560px;}.item{width: 300px;}.mobile-hide-img{text-align: left; width: 125px;}.mobile-hide-img img{border: 1px solid #e6e6e6; border-radius: 4px;}.title-dark{text-align: left; border-bottom: 1px solid #cccccc; color: #4d4d4d; font-weight: 700; padding-bottom: 5px;}.item-col{padding-top: 20px; text-align: left; vertical-align: top;}.force-width-gmail{min-width:600px; height: 0px !important; line-height: 1px !important; font-size: 1px !important;}</style> <style type='text/css' media='screen'> @import url(http://fonts.googleapis.com/css?family=Oxygen:400,700); </style> <style type='text/css' media='screen'> @media screen{*{font-family: 'Oxygen', 'Helvetica Neue', 'Arial', 'sans-serif' !important;}}</style> <style type='text/css' media='only screen and(max-width: 480px)'> @media only screen and (max-width: 480px){table[class*='container-for-gmail-android']{min-width: 290px !important; width: 100% !important;}img[class='force-width-gmail']{display: none !important; width: 0 !important; height: 0 !important;}table[class='w320']{width: 320px !important;}td[class*='mobile-header-padding-left']{width: 160px !important; padding-left: 0 !important;}td[class*='mobile-header-padding-right']{width: 160px !important; padding-right: 0 !important;}td[class='header-lg']{font-size: 24px !important; padding-bottom: 5px !important;}td[class='content-padding']{padding: 5px 0 5px !important;}td[class='button']{padding: 5px 5px 30px !important;}td[class*='free-text']{padding: 10px 18px 30px !important;}td[class~='mobile-hide-img']{display: none !important; height: 0 !important; width: 0 !important; line-height: 0 !important;}td[class~='item']{width: 140px !important; vertical-align: top !important;}td[class~='quantity']{width: 50px !important;}td[class~='price']{width: 90px !important;}td[class='item-table']{padding: 30px 20px !important;}td[class='mini-container-left'], td[class='mini-container-right']{padding: 0 15px 15px !important; display: block !important; width: 290px !important;}}</style></head><body bgcolor='#f7f7f7'><table align='center' cellpadding='0' cellspacing='0' class='container-for-gmail-android' width='100%'> <tr> <td align='left' valign='top' width='100%' style='background:repeat-x url(http://s3.amazonaws.com/swu-filepicker/4E687TRe69Ld95IDWyEg_bg_top_02.jpg) #ffffff;'> <center> <img src='http://s3.amazonaws.com/swu-filepicker/SBb2fQPrQ5ezxmqUTgCr_transparent.png' class='force-width-gmail'> <table cellspacing='0' cellpadding='0' width='100%' bgcolor='#ffffff' background='http://s3.amazonaws.com/swu-filepicker/4E687TRe69Ld95IDWyEg_bg_top_02.jpg' style='background-color:transparent'> <tr> <td width='100%' height='80' valign='top' style='text-align: center; vertical-align:middle;'> <center> <table cellpadding='0' cellspacing='0' width='600' class='w320'> <tr> <td class='pull-left mobile-header-padding-left' style='vertical-align: middle; text-align:center'> <a href=''><img width='auto' height='47' src='https://i.pinimg.com/originals/52/ed/4b/52ed4b635f8e73a4e17626341b449afb.png' alt='logo'></a> </td></tr></table> </center> </td></tr></table> </center> </td></tr><tr> <td align='center' valign='top' width='100%' style='background-color: #f7f7f7;' class='content-padding'> <center> <table cellspacing='0' cellpadding='0' width='600' class='w320'> <tr> <td class='header-lg'> [SUBJECT] </td></tr><tr> <td class='free-text'> Hola [NOMBRECLIENTE, tu numero de rastreo es: <a href=''>[NUMRASTREOORDEN]</a>. Obten tus detalles. </td></tr><tr> <td class='button'> <div><a href='' style='background-color:#5f4b8b;border-radius:5px;color:#ffffff;display:inline-block;font-family:'Cabin', Helvetica, Arial, sans-serif;font-size:14px;font-weight:regular;line-height:45px;text-align:center;text-decoration:none;width:155px;-webkit-text-size-adjust:none;mso-hide:all;'>Rastrea tu orden</a></div></td></tr><tr> <td class='w320'> <table cellpadding='0' cellspacing='0' width='100%'> <tr> <td class='mini-container-left'> <table cellpadding='0' cellspacing='0' width='100%'> <tr> <td class='mini-block-padding'> <table cellspacing='0' cellpadding='0' width='100%' style='border-collapse:separate !important;'> <tr> <td class='mini-block'> <span class='header-sm'>Remitente: </span><br/> [NOMBRECLIENTE] <br/> <br/> <span class='header-sm'>Destinatario: </span><br/> [NOMBREDESTINATARIO] <br/> <br/> <span class='header-sm'>Ciudad, Estado:</span><br/> [CIUDADESTADODESTINATARIO]<br/> <br/> <span class='header-sm'>Fecha: </span><br/> [FECHAORDEN]<br/> </td></tr></table> </td></tr></table> </td><td class='mini-container-right'> <table cellpadding='0' cellspacing='0' width='100%'> <tr> <td class='mini-block-padding'> <table cellspacing='0' cellpadding='0' width='100%' style='border-collapse:separate !important;'> <tr> <td class='mini-block'> <span class='header-sm'>Estado</span><br/> [ESTADOORDEN]<br/> <br/> <span class='header-sm'>Fecha historial</span> <br/> [FECHAHISTORIAL]<br/> <br/> <span class='header-sm'>Estado</span> <br/> [ESTADOHISTORIAL]<br/> <br/> <span class='header-sm'>Ciudad</span> <br/> [CIUDADHISTORIAL]<br/> </td></tr></table> </td></tr></table> </td></tr></table> </td></tr></table> </center> </td></tr><tr> <td align='center' valign='top' width='100%' style='background-color: #ffffff; border-top: 1px solid #e5e5e5; border-bottom: 1px solid #e5e5e5;'> <center> <table cellpadding='0' cellspacing='0' width='600' class='w320'> <tr> <td class='item-table'> <table cellspacing='0' cellpadding='0' width='100%'> <tr> <td class='title-dark' width='300'> Informacion paquete </td><td class='title-dark' width='163'> Qty </td><td class='title-dark' width='97'> Total </td></tr><tr> <td class='item-col item'> <table cellspacing='0' cellpadding='0' width='100%'> <tr> <td class='product'> <span style='color: #4d4d4d; font-weight:bold;'>Descripcion </span> <br/> [DESCRIPCIONHISTORIAL] </td></tr></table> </td><td class='item-col quantity'> 1 </td><td class='item-col'> $[PRECIOORDEN] </td></tr><tr> <td class='item-col item mobile-row-padding'></td><td class='item-col quantity'></td><td class='item-col price'></td></tr></table> </td></tr></table> </center> </td></tr></table></div></body></html>";
    }



}
