using Practica.Nucleo.Entidades;
using Practica.Nucleo.Enumeradores;
using System;
using System.Collections.Generic;
using Practica.Nucleo.Enumeradores;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Aplicacion.Controllers
{
    public class UsuarioController : Controller
    {

        [Attribute.ValidateSession(Rls = new Rol[] {Rol.ADMINISTRADOR})]
        public ActionResult Index()
        {
            IList<OrdenDTO> ordenes = Orden.ObtenerTodos();
            return View();
        }

        public ActionResult Add(int id)
        {
            ViewBag.IdUsuario = id;
            return PartialView("~/Views/Usuario/Add.cshtml");
        }

        public ActionResult ChangePassword(int id)
        {
            ViewBag.IdUsuario = id;
            return PartialView("~/Views/Usuario/ChangePassword.cshtml");
        }

        public ActionResult ObtenerTodos()
        {
            try
            {
                IList<Usuario> usuarios = Usuario.ObtenerTodos();
                return Json(new { data = usuarios }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult ObtenerPorId(int id)
        {
            Usuario u = new Usuario();
            try
            {
                u = Usuario.ObtenerPorId(id);
            }
            catch (Exception ae)
            {
                return RedirectToAction("Error", "Home");
            }
            return Json(u, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Guardar(int id, String nombre, String direccion, String telefono, String cuenta, int rol, String password)
        {
            ActionResult action = null;
            try
            {
                if (Usuario.Guardar(id, nombre, direccion, telefono, cuenta, rol, password))
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

        public ActionResult ActualizarPassword(int id, String password, String passwordValidar, String passwordNueva)
        {
            ActionResult action = null;
            try
            {
                if (Usuario.ActualizarPassword(id, password, passwordValidar, passwordNueva))
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

        public ActionResult Borrar(int id)
        {
            ActionResult action = null;
            try
            {
                if (Usuario.Borrar(id))
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