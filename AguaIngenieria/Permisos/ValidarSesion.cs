using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AguaIngenieria.Permisos
{
    public class ValidarSesion : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            // Verificar si la sesión de usuario está activa
            if (session["IdUsuario"] == null)
            {
                // Si no hay sesión, redirigir al login
                filterContext.Result = new RedirectResult("~/Authentication/Index");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}