using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModels;

namespace AguaIngenieria.Controllers
{
    public class ProyectosController : Controller
    {
        // GET: Proyectos
        public ActionResult Index()
        {
            return View();
        }

        //GET: Proyectos
        public ActionResult Proyectos()
        {
            try 
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    var listaProyectos = db.SpObtenerGaleria().ToList();
                    return View(listaProyectos);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al cargar los proyectos. Por favor, inténtalo de nuevo más tarde." + ex;
                return View("Error");
            }
        }
    }
}