using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Aplicacion.Controllers
{
    public class PaqueteController : Controller
    {
        // GET: Paquete
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