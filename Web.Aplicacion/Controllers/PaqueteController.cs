using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practica.Nucleo.Enumeradores;

namespace Web.Aplicacion.Controllers
{
    public class PaqueteController : Controller
    {
        // GET: Paquete
        [Attribute.ValidateSession(Rls = new Rol[] { Rol.ADMINISTRADOR })]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return PartialView("~/Views/Paquete/Add.cshtml");
        }
    }
}