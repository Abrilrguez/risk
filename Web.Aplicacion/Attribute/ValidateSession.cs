using Practica.Nucleo.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Aplicacion.Attribute
{
    public class ValidateSession : AuthorizeAttribute
    {
        public Rol[] Rls { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            if (HttpContext.Current.Request.Cookies["ckPractica"]!= null)
            {
                string roles = HttpContext.Current.Request.Cookies["ckPractica"].Values["rolUsuario"].ToString();
                    if(Rls != null)
                {
                    foreach(Rol r in Rls){
                        if((int)r == Int32.Parse(roles))
                        {
                            authorize = true;
                            break;
                        }
                    }
                }

            }
            if(authorize == false)
            {
                
            }
            return base.AuthorizeCore(httpContext);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            Web.Aplicacion.Controllers.HomeController home = new Web.Aplicacion.Controllers.HomeController();
            context.Result = home.Error();
        }

    }
}