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
    public class NovedadesAdminController : Controller
    {

        // GET: NovedadesAdmin
        public ActionResult Index()
        {
            return View();
        }

        //Vista para el CRUD de novedades
        public ActionResult NovedadesCRUD()
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
                ViewBag.ErrorMessage = "Ocurrió un error al cargar las novedades. Por favor, inténtalo de nuevo más tarde." + ex;
                return View("Error");
            }
        }

        //Crear Novedad
        public ActionResult CrearNovedad()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CrearNovedad(Novedade nuevaNovedad)
        {
            // Valida que el modelo sea correcto (campos requeridos, formatos, etc.)
            if (!ModelState.IsValid)
            {
                return View(nuevaNovedad);
            }
            try
            {
                //recuperamos el id del admin logueado
                var idAdmin= Session["IdUsuario"] as int?;

                // Verifica que el IdAdmin exista en la sesión
                if (idAdmin == null)
                {
                    ModelState.AddModelError("", "No se ha encontrado un administrador logueado.");
                    return View(nuevaNovedad);
                }

                // Asigna el IdAdmin al objeto nuevaNovedad
                nuevaNovedad.IdAdmin = idAdmin.Value;

                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    db.SpCrearNovedad(nuevaNovedad.Titulo, nuevaNovedad.Fuente, nuevaNovedad.Resumen, nuevaNovedad.UrlNoticia,
                       nuevaNovedad.ImagenPath, nuevaNovedad.IdAdmin, nuevaNovedad.Fecha);
                }
                TempData["MensajeExito"] = "Novedad creada correctamente";
                return RedirectToAction("NovedadesCRUD", "NovedadesAdmin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al crear la novedad: " + ex.Message);
                return View(nuevaNovedad);
            }
        }

        //GET: Novedad Editar
        //Acción get para editar una novedad existente
        public ActionResult EditarNovedad(int id)
        {
            try
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    var novedad = db.SpObtenerNovedadPorId(id).FirstOrDefault();
                    if (novedad == null)
                    {
                        return HttpNotFound();
                    }
                    return View(novedad);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al cargar la novedad. Por favor, inténtalo de nuevo más tarde." + ex;
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult EditarNovedad(Novedade novedadEditada)
        {
            // Valida que el modelo sea correcto (campos requeridos, formatos, etc.)
            if (!ModelState.IsValid)
            {
                return View(novedadEditada);
            }
            try
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    db.SpEditarNovedad(novedadEditada.Id, novedadEditada.Titulo, novedadEditada.Fuente,
                        novedadEditada.Resumen, novedadEditada.ImagenPath, novedadEditada.UrlNoticia,
                        novedadEditada.IdAdmin, novedadEditada.Fecha);
                }
                TempData["MensajeExito"] = "Novedad actualizada correctamente";
                return RedirectToAction("NovedadesCRUD", "NovedadesAdmin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al actualizar la novedad: " + ex.Message);
                return View(novedadEditada);
            }

        }

        //Get: Eliminar Novedad
        public ActionResult EliminarNovedad(int id)
        {
            try
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    db.SpEliminarNovedad(id);
                }
                TempData["MensajeExito"] = "Novedad eliminada correctamente";
                return RedirectToAction("NovedadesCRUD", "NovedadesAdmin");
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Ocurrió un error al eliminar la novedad: " + ex.Message;
                return RedirectToAction("NovedadesCRUD", "NovedadesAdmin");
            }
        }

        [HttpPost, ActionName ("Eliminar")]
        public ActionResult EliminarNovedadConfirmado(int id)
        {
            try
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    db.SpEliminarNovedad(id);
                }
                TempData["MensajeExito"] = "Novedad eliminada correctamente";
                return RedirectToAction("NovedadesCRUD", "NovedadesAdmin");
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Ocurrió un error al eliminar la novedad: " + ex.Message;
                return RedirectToAction("NovedadesCRUD", "NovedadesAdmin");
            }
        }
    }
}