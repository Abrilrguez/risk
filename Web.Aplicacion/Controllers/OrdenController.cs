using Practica.Nucleo.Entidades;
using Practica.Nucleo.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Aplicacion.Controllers
{
    public class OrdenController : Controller
    {

        //[Attribute.ValidateSession(Rls = new Rol[] {Rol.ADMINISTRADOR})]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add(int idOrden, int idPaquete, int idCliente, int idDestinatario)
        {
            ViewBag.IdOrden = idOrden;
            ViewBag.IdPaquete = idPaquete;
            ViewBag.IdCliente = idCliente;
            ViewBag.IdDestinatario = idDestinatario;

            return PartialView("~/Views/Orden/Add.cshtml");
        }

        public ActionResult ObtenerTodos()
        {
            try
            {
                IList<Orden> ordenes = Orden.ObtenerTodos();
                return Json(new { data = ordenes }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
                
            }
        }

        public ActionResult Guardar(int idOrden, 
                                    int idPaquete, string paquetePeso, string paqueteTamanio, string paqueteContenido, string paqueteDescripcion,
                                    int idCliente, string clienteNombre, string clienteTelefono, string clienteCorreo, string clienteRfc, string clienteDomicilio,
                                    int idDestinatario, string destinatarioNombre, string destinatarioTelefono, string destinatarioCorreo, string destinatarioPersona,
                                    string destinatarioCalle, string destinatarioNumero, string destinatarioAvenida, string destinatarioColonia, string destinatarioCp,
                                    string destinatarioCiudad, string destinatarioEstado, string destinatarioReferencia){
            ActionResult action = null;
            try
            {
                if (Orden.Guardar(idOrden,
                    idPaquete, paquetePeso, paqueteTamanio, paqueteContenido, paqueteDescripcion,
                    idCliente, clienteNombre, clienteTelefono, clienteCorreo, clienteRfc, clienteDomicilio,
                    idDestinatario, destinatarioNombre, destinatarioTelefono, destinatarioCorreo, destinatarioPersona,
                    destinatarioCalle, destinatarioNumero, destinatarioAvenida, destinatarioColonia, destinatarioCp,
                    destinatarioCiudad, destinatarioEstado, destinatarioReferencia))
                {
                    action = Content("true");
                }
                else
                {
                    action = Content("false");
                }

            }
            catch (Exception ex)
            {
                return Content("false");
            }

            return action;
        }

        public ActionResult Eliminar(int id)
        {
            ActionResult action = null;
            try
            {
                if (Orden.Eliminar(id))
                {
                    action = Content("true");
                }
                else
                {
                    action = Content("false");
                }
            }
            catch (Exception ex)
            {
                return Content("false");
            }
            return action;
        }

        public ActionResult ObtenerPorId(int id)
        {
            Orden o = new Orden();
            try
            {
                o = Orden.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }

    }
}