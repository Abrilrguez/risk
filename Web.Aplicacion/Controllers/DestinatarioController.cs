using Practica.Nucleo.Entidades;
using Practica.Nucleo.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Aplicacion.Controllers
{
    public class DestinatarioController : Controller
    {
        // GET: Usuario
        [Attribute.ValidateSession(Rls = new Rol[] { Rol.ADMINISTRADOR })]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(int id)
        {
            ViewBag.IdUsuario = id;
            return PartialView("~/Views/Destinatario/Add.cshtml");
        }


        public ActionResult ObtenerTodos()
        {
            try
            {
                IList<Destinatario> destinatarios = Destinatario.ObtenerTodos();
                return Json(new { data = destinatarios }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult ObtenerPorId(int id)
        {
            Destinatario u = new Destinatario();
            try
            {
                u = Destinatario.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return Json(u, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Guardar(String nombre, String calle, String numero, String avenida, String colonia, String cp, String ciudad,
            String estado, String referencia, String telefono, String correo, String persona)
        {
            ActionResult action = null;
            try
            {
                if (nombre!="" && calle != "" && numero != "" && colonia != "" && avenida != "" && cp != "" && ciudad != "" && estado != "" && referencia != "" && telefono != "" && correo != "" && persona != "" )
                {
                    if (Destinatario.Guardar(nombre, calle, numero, avenida, colonia, cp, ciudad, estado, referencia, telefono, correo, persona))
                    {
                        action = Content("true");
                    }
                    else
                    {
                        action = Content("false");
                    }
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


        public ActionResult Borrar(int id)
        {
            ActionResult action = null;
            try
            {
                if (Destinatario.Borrar(id))
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
    }
}