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

        
    }
}