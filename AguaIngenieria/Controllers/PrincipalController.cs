using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AguaIngenieria.Permisos;

namespace AguaIngenieria.Controllers
{
    public class PrincipalController : Controller
    {
        // GET: Principal
        public ActionResult Index()
        {
            return View();
        }

        //Vista para novedades/principal
        public ActionResult Novedades()
        {
            return View();
        }

        //Vista para usuario admin logeado exitosamente
        [ValidarSesion]
        public ActionResult Admin()
        {
            return View();
        }

        //Vista CRUD de usuarios (solo admin)
        [ValidarSesion]
        public ActionResult Usuarios()
        {
            return View();
        }

    }
}