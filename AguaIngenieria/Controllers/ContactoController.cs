using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AguaIngenieria.Controllers
{
    public class ContactoController : Controller
    {
        // GET: Contacto
        public ActionResult Index()
        {
            return View();
        }

        //Get: Formulario de contacto
        public ActionResult Formulario()
        {
            return View();
        }

        //GET: Ubicación
        public ActionResult Ubicacion()
        {
            return View();
        }

        //GET: Redes Sociales
        public ActionResult RedesSociales()
        {
            return View();
        }

        //GET: Faqs
        public ActionResult Faqs()
        {
            return View();
        }
    }
}