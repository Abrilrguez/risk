using Practica.Nucleo.Entidades;
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
            return View();
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
                Usuario u = Usuario.ObtenerPorLogin(cuenta, password);
                
                if (u != null)
                {
                    HttpCookie ck = new HttpCookie("ckPractica");

                    ck.Values.Add("rolUsuario", ((int)u.Rol).ToString());
                    ck.Expires = DateTime.Now.AddHours(8);
                    Response.Cookies.Add(ck);

                    Session["usuarioId"] = u.Id;
                    Session["usuarioCuenta"] = u.Cuenta;
                    Session["usuarioNombre"] = u.Nombre;
                    Session["usuarioRol"] = u.Rol;
                    action = RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
        }
    }
}