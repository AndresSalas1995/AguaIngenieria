using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AguaIngenieria.Controllers
{
    public class ServiciosController : Controller
    {
        // GET: Servicios
        public ActionResult Index()
        {
            return View();
        }

        //GetServicios Principal
        public ActionResult Servicios()
        {
            return View();
        }

        //GetServicios Concesiones
        public ActionResult Concesiones()
        {
            return View();
        }
        //GetServicios Estudios
        public ActionResult Estudios()
        {
            return View();
        }

        //GetServicios Viabilidad
        public ActionResult Viabilidad()
        {
            return View();
        }

    }
}