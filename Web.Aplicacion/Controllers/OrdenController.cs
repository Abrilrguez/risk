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
                IList<OrdenDTO> ordenes = Orden.ObtenerTodos();
                return Json(new { data = ordenes }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
                
            }
        }

        public ActionResult Guardar(int idOrden, int ordenEstado, double ordenPrecio, string ordenFolio, string ordenNumRastreo, DateTime ordenFecha,
                                    int idUsuario,
                                    int idPaquete, string paquetePeso, string paqueteTamanio, string paqueteContenido, string paqueteDescripcion,
                                    int idCliente, string clienteNombre, string clienteTelefono, string clienteCorreo, string clienteRfc, string clienteDomicilio,
                                    int idDestinatario, string destinatarioNombre, string destinatarioTelefono, string destinatarioCorreo, string destinatarioPersona,
                                    string destinatarioCalle, string destinatarioNumero, string destinatarioAvenida, string destinatarioColonia, string destinatarioCp,
                                    string destinatarioCiudad, string destinatarioEstado, string destinatarioReferencia){
            ActionResult action = null;
            if (ordenEstado.ToString() !="" &&ordenPrecio.ToString() != ""&& ordenFolio != "" && ordenNumRastreo != "" && ordenFecha.ToString() != "" &&
                paquetePeso != "" && paqueteTamanio != "" && paqueteContenido != "" && paqueteDescripcion != "" && 
                clienteNombre != "" && clienteTelefono != "" && clienteCorreo != "" && clienteRfc != "" && clienteDomicilio != "" && 
                destinatarioNombre != "" && destinatarioTelefono != "" && destinatarioCorreo != "" && destinatarioPersona != "" &&destinatarioCalle != "" && destinatarioNumero != "" &&
                destinatarioAvenida != "" && destinatarioColonia != "" && destinatarioCp != "" && destinatarioCiudad != "" && destinatarioEstado != "" && destinatarioReferencia != "" )
            {
                try
                {

                    idUsuario = Convert.ToInt32(Session["usuarioId"]);

                    if (Orden.Guardar(idOrden, ordenEstado, ordenPrecio, ordenFolio, ordenNumRastreo, ordenFecha,
                        idUsuario,
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
            }
            else
            {
                action = Content("false");
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
            OrdenDTO o = new OrdenDTO();
            try
            {
                o = Orden.ObtenerPorIdDTO(id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerPorFolio(string id)
        {
            Orden o = new Orden();
            try
            {
                o = Orden.ObtenerPorFolio(id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerDatosOrden()
        {
            OrdenDTO o = new OrdenDTO();
            try
            {
                o = Orden.ObtenerDatosOrden();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }


    }
}