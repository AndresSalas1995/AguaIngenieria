using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AguaIngenieria.Models;
using AguaIngenieria.Permisos;
using DataModels;

namespace AguaIngenieria.Controllers
{
    [ValidarSesion]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // Vista para el CRUD de Usuarios
        public ActionResult Usuarios()
        {
            List<DetalleUsuarioAdmin> lista = new List<DetalleUsuarioAdmin>();

            using (var db = new AguaIngenieriaDB("MyDatabase"))
            {
                var resultado = db.SpLeerUsuarios().ToList();

                lista = resultado.Select(u => new DetalleUsuarioAdmin
                {
                    Id = u.Id,
                    Usuario = u.Usuario,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    FechaCreacion = u.FechaCreacion
                }).ToList();
            }

            return View(lista);
        }

        //Crear Usuario
        public ActionResult CrearUsuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CrearUsuario(DetalleUsuarioAdmin nuevoUsuario)
        {
            if (!ModelState.IsValid)
            {
                return View(nuevoUsuario);
            }

            try
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    db.SpCrearUsuarioAdmin(nuevoUsuario.Usuario, nuevoUsuario.Contrasena, nuevoUsuario.Nombre, nuevoUsuario.Apellido, nuevoUsuario.Email);
                }

                TempData["MensajeExito"] = "Usuario creado correctamente";
                return RedirectToAction("Usuarios", "Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al crear el usuario: " + ex.Message);
                return View(nuevoUsuario);
            }
        }

    }
}