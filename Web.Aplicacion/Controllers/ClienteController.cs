using Practica.Nucleo.Entidades;
using Practica.Nucleo.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Aplicacion.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Usuario
        [Attribute.ValidateSession(Rls = new Rol[] { Rol.ADMINISTRADOR })]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(int id)
        {
            ViewBag.IdCliente = id;
            return PartialView("~/Views/Cliente/Add.cshtml");
        }

        public ActionResult ObtenerTodos()
        {
            try
            {
                IList<Cliente> clientes = Cliente.ObtenerTodos();
                return Json(new { data = clientes }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult ObtenerPorId(int id)
        {
            Cliente u = new Cliente();
            try
            {
                u = Cliente.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return Json(u, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Guardar(int id,String nombre, String domicilio, String telefono, String correo, String rfc)
        {
            ActionResult action = null;
            if (nombre!=""&& domicilio != "" && telefono != "" && correo != "" && rfc != "")
            {
                try
                {
                    if (Cliente.Guardar(id,nombre, domicilio, telefono, correo, rfc))
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

        public ActionResult Borrar(int id)
        {
            ActionResult action = null;
            try
            {
                if (Cliente.Borrar(id))
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