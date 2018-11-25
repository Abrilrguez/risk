using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web.Aplicacion
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "OrderWithIds",                                              // Route name
                "{controller}/{action}/{idOrden}/{idPaquete}/{idCliente}/{idDestinatario}",                           // URL with parameters
                new { controller = "Orden", action = "Add", idOrden = "0", idPaquete = "0", idCliente = "0", idDestinatario = "0" }  // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Practica.Nucleo.Entidades.Persistent.CreateSessionFactory();
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
