﻿using Practica.Nucleo.Entidades;
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
            ViewBag.IdEstado = id;
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

        public ActionResult ObtenerPorId(int id)
        {
            Historial h = new Historial();
            try
            {
                h = Historial.ObtenerPorId(id);
            }
            catch (Exception ae)
            {
                return RedirectToAction("Error", "Home");
            }
            return Json(h, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerPorOrden(int id)
        {
            try
            {
                IList<Historial> historiales = Historial.ObtenerPorOrden(id);
                List<string> fechas = new List<string>();

                foreach (var estado in historiales)
                { 
                    fechas.Add(estado.Fecha.ToString("dd MMM yyy"));
                }
                return Json(new { data = historiales, data2 = fechas }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        public ActionResult Guardar(int id, String descripcion, String ciudad, String estado, int idUsuario, int idOrden)
        {
            ActionResult action = null;
            try
            {
                if (Historial.Guardar(id, descripcion, ciudad, estado, idUsuario, idOrden))
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
                if (Historial.Borrar(id))
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