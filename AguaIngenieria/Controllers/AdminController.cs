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

        // GET: Admin/Editar/{id}
        public ActionResult Editar(int id)
        {
            using (var db = new AguaIngenieriaDB("MyDatabase"))
            {
                var usuario = db.SpLeerUsuarios().FirstOrDefault(u => u.Id == id);
                if (usuario == null)
                {
                    TempData["MensajeError"] = "Usuario no encontrado";
                    return RedirectToAction("Usuarios");
                }

                var detalle = new DetalleUsuarioAdmin
                {
                    Id = usuario.Id,
                    Usuario = usuario.Usuario,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    FechaCreacion = usuario.FechaCreacion
                };

                return View(detalle);
            }
        }

        [HttpPost]
        public ActionResult Editar(DetalleUsuarioAdmin usuarioEditado)
        {
            if (!ModelState.IsValid)
            {
                return View(usuarioEditado);
            }

            try
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    db.SpEditarUsuarioAdmin(usuarioEditado.Id, usuarioEditado.Usuario, usuarioEditado.Contrasena, usuarioEditado.Nombre, usuarioEditado.Apellido, usuarioEditado.Email);
                }

                TempData["MensajeExito"] = "Usuario actualizado correctamente";
                return RedirectToAction("Usuarios");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al actualizar el usuario: " + ex.Message);
                return View(usuarioEditado);
            }
        }

        // GET: Admin/Eliminar/{id}
        public ActionResult Eliminar(int id)
        {
            using (var db = new AguaIngenieriaDB("MyDatabase"))
            {
                var usuario = db.SpLeerUsuarios().FirstOrDefault(u => u.Id == id);
                if (usuario == null)
                {
                    TempData["MensajeError"] = "Usuario no encontrado";
                    return RedirectToAction("Usuarios");
                }

                var detalle = new DetalleUsuarioAdmin
                {
                    Id = usuario.Id,
                    Usuario = usuario.Usuario,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    FechaCreacion = usuario.FechaCreacion
                };

                return View(detalle);
            }
        }

        [HttpPost, ActionName("Eliminar")]
        public ActionResult EliminarConfirmado(int id)
        {
            try
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    db.SpEliminarUsuarioAdmin(id);
                }

                TempData["MensajeExito"] = "Usuario eliminado correctamente";
                return RedirectToAction("Usuarios");
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Ocurrió un error al eliminar el usuario: " + ex.Message;
                return RedirectToAction("Usuarios");
            }
        }

    }
}