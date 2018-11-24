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
        // GET: Orden

        //[Attribute.ValidateSession(Rls = new Rol[] {Rol.ADMINISTRADOR})]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
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



    }
}