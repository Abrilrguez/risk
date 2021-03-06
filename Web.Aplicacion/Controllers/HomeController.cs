﻿using Practica.Nucleo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Aplicacion.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var error = Request["error"];
            if (error != null)
            {
                ViewBag.error = "Ingresa un número de rastreo valido";
            }
            return View();
        }

        public ActionResult Rastreo(String numeroRastreo)
        {
            if (numeroRastreo != null && numeroRastreo != "")
            {
                Orden orden = new Orden();
                try
                {
                    orden = Orden.ObtenerPorFolio(numeroRastreo);
                    IList<Historial> listaHistoriales = new List<Historial>();
                    orden.Historiales.Reverse();
                    
                    foreach (Historial historial in orden.Historiales.Reverse())
                    {
                        listaHistoriales.Add(historial);
                    }
                    orden.Historiales = listaHistoriales;
                    if (orden == null)
                    {
                        return RedirectToAction("Index", new { error = 1 });
                    }
                }
                catch (Exception ae)
                {
                    return RedirectToAction("Error", "Home");
                }
                return View(orden);
            }
            else
            {
                return RedirectToAction("Index", new { error = 1 });
            }
            
        }

        public ActionResult Ayuda()
        {
            return View();
        }

        public ActionResult Login()
        {
            try
            {
                if (Session != null)
                {
                    Session.Clear();
                    Session.Abandon();
                }
                return View();
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            
        }

        [HttpPost]
        public ActionResult Login (FormCollection model)
        {
            ActionResult action = null;
            try
            {
                string cuenta = model["cuenta"].Trim();
                string password = model["password"].Trim();
                Usuario u= null;
                if (cuenta!="" && password!="")
                {
                    u = Usuario.ObtenerPorLogin(cuenta, password);
                }

                if (u != null )
                {
                    HttpCookie ck = new HttpCookie("ckPractica");

                    ck.Values.Add("rolUsuario", ((int)u.Rol).ToString());
                    ck.Expires = DateTime.Now.AddHours(8);
                    Response.Cookies.Add(ck);

                    Session["usuarioId"] = u.Id;
                    Session["usuarioCuenta"] = u.Cuenta;
                    Session["usuarioNombre"] = u.Nombre;
                    Session["usuarioRol"] = u.Rol;
                    action = RedirectToAction("Index", "Orden");
                }else
                {
                    ViewBag.Message = "Los datos de usuario ingresado son incorrectos";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return action;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Error()
        {
            return RedirectToAction("Index", "Orden");
        }
        public ActionResult Error2()
        {
            return RedirectToAction("Login", "Home");
        }
    }
}