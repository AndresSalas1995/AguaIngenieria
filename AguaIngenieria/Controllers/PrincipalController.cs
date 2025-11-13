using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AguaIngenieria.Permisos;
using DataModels;

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
            try
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    var listaNovedades = db.SpObtenerNovedades().ToList();
                    return View(listaNovedades);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al cargar las novedades. Por favor, inténtalo de nuevo más tarde.";
                return View("Error");
            }
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