using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AguaIngenieria.Models;
using DataModels;

namespace AguaIngenieria.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Index()
        {
            return View();
        }

        //Inicio de sesión
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try 
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    // Ejecutar el procedimiento almacenado
                    var usuario = db.SpLoginUsuario(model.Usuario, model.Contrasena).FirstOrDefault();

                    if (usuario != null)
                    {
                        //Login correcto: guardar los datos en sesión
                        Session["IdUsuario"] = usuario.Id;
                        Session["Usuario"] = usuario.Usuario;
                        Session["Email"] = usuario.Email;
                        Session["Nombre"] = usuario.Nombre;
                        Session["Apellido"] = usuario.Apellido;
                        Session["NombreCompleto"] = usuario.Nombre + " " + usuario.Apellido;

                        // Redirigir al panel principal (por ejemplo)
                        return RedirectToAction("Admin", "Principal");
                    }
                    else
                    {
                        //Credenciales incorrectas
                        ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo básico de errores (puedes mejorar el log)
                ModelState.AddModelError("", "Ocurrió un error al iniciar sesión. Por favor, intente nuevamente.");
                return View(model);
            }

        }

        //Cerrar sesión
        public ActionResult Logout()
        {
            // Limpiar la sesión
            Session.Clear();
            Session.Abandon();
            // Redirigir a la página de inicio de sesión
            return RedirectToAction("Index", "Authentication");
        }
    }
}