using Practica.Nucleo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Aplicacion.Controllers
{
    public class HistorialController : Controller
    {
        // GET: Historial
        public ActionResult Index()
        {
            IList<Historial> historiales = Historial.ObtenerTodos();
            return View();
        }
        public ActionResult Add(int id)
        {
            ViewBag.IdUsuario = id;
            return PartialView("~/Views/Historial/Add.cshtml");
        }
        public ActionResult ObtenerTodos()
        {
            try
            {
                IList<Historial> historiales = Historial.ObtenerTodos();
                return Json(new { data = historiales }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        public ActionResult Guardar(int id, String fecha, String descripcion, String ciudad, String estado, int idUsuario)
        {
            ActionResult action = null;
            try
            {
                if (Historial.Guardar(id, fecha, descripcion, ciudad, estado, idUsuario))
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