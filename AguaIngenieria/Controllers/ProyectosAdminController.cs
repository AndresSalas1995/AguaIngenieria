using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AguaIngenieria.Permisos;
using DataModels;

namespace AguaIngenieria.Controllers
{
    [ValidarSesion]
    public class ProyectosAdminController : Controller
    {
        // GET: ProyectosAdmin
        public ActionResult Index()
        {
            return View();
        }

        //CRUD de Proyectos
        public ActionResult ProyectosCRUD()
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

        //Crear Proyecto
        public ActionResult CrearProyecto()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearProyecto(Galeria nuevaGaleria, HttpPostedFileBase ImagenProyecto)
        {
            try
            {
                if (ImagenProyecto == null || ImagenProyecto.ContentLength == 0)
                {
                    ModelState.AddModelError("ImagenProyecto", "Debe seleccionar una imagen.");
                    return View(nuevaGaleria);
                }

                //Validar modelo
                if (!ModelState.IsValid)
                    return View(nuevaGaleria);

                //recuperamos el id del admin logueado
                var idAdmin = Session["IdUsuario"] as int?;

                // Verifica que el IdAdmin exista en la sesión
                if (idAdmin == null)
                {
                    ModelState.AddModelError("", "No se ha encontrado un administrador logueado.");
                    return View(nuevaGaleria);
                }

                // Asigna el IdAdmin al objeto nuevaNovedad
                nuevaGaleria.IdAdmin = idAdmin.Value;

                //GUARDAR IMAGEN EN /Content/Proyectos 
                string carpeta = Server.MapPath("~/Content/Proyectos/");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string nombreArchivo = DateTime.Now.Ticks + Path.GetExtension(ImagenProyecto.FileName);
                string rutaCompleta = Path.Combine(carpeta, nombreArchivo);
                ImagenProyecto.SaveAs(rutaCompleta);

                //Guardar ruta en la BD
                nuevaGaleria.ImagenPath = "/Content/Proyectos/" + nombreArchivo;
                nuevaGaleria.FechaSubida = DateTime.Now;

                //GUARDAR EN BASE DE DATOS
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    db.SpCrearGaleria(
                        nuevaGaleria.Titulo,
                        nuevaGaleria.ImagenPath,
                        nuevaGaleria.FechaSubida,
                        nuevaGaleria.IdAdmin
                    );
                }
                TempData["MensajeExito"] = "Proyecto creado correctamente";
                return RedirectToAction("ProyectosCRUD");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al crear el proyecto: " + ex.Message;
                return View("Error");
            }
        }

        //GET: proyectos Editar
        //Acción get para editar un proyecto existente
        public ActionResult EditarProyecto(int id)
        {
            try
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    var proyecto = db.SpObtenerGaleriaPorId(id).FirstOrDefault();
                    if (proyecto == null)
                    {
                        return HttpNotFound();
                    }
                    return View(proyecto);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al cargar el proyecto: Por favor, inténtalo de nuevo más tarde." + ex;
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult EditarProyecto(Galeria proyectoEditado, HttpPostedFileBase NuevaImagenProyecto)
        {
            try
            {
                // Validar modelo
                if (!ModelState.IsValid)
                    return View(proyectoEditado);

                //recuperamos el id del admin logueado
                var idAdmin = Session["IdUsuario"] as int?;

                // Verifica que el IdAdmin exista en la sesión
                if (idAdmin == null)
                {
                    ModelState.AddModelError("", "No se ha encontrado un administrador logueado.");
                    return View(proyectoEditado);
                }

                // Asigna el IdAdmin al objeto nuevaNovedad
                proyectoEditado.IdAdmin = idAdmin.Value;

                // Si se ha subido una nueva imagen, guardarla
                if (NuevaImagenProyecto != null && NuevaImagenProyecto.ContentLength > 0)
                {
                    string carpeta = Server.MapPath("~/Content/Proyectos/");
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);
                    string nombreArchivo = DateTime.Now.Ticks + Path.GetExtension(NuevaImagenProyecto.FileName);
                    string rutaCompleta = Path.Combine(carpeta, nombreArchivo);
                    NuevaImagenProyecto.SaveAs(rutaCompleta);
                    // Actualizar la ruta de la imagen en el objeto
                    proyectoEditado.ImagenPath = "/Content/Proyectos/" + nombreArchivo;
                }
                // Actualizar en la base de datos
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    db.SpEditarGaleria(
                        proyectoEditado.Id,
                        proyectoEditado.Titulo,
                        proyectoEditado.ImagenPath,
                        proyectoEditado.IdAdmin
                    );
                }
                TempData["MensajeExito"] = "Proyecto actualizado correctamente";
                return RedirectToAction("ProyectosCRUD");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al actualizar el proyecto: " + ex.Message;
                return View("Error");
            }
        }

        // GET: ProyectosAdmin/EliminarProyecto
        public ActionResult EliminarProyecto(int id)
        {
            try
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    var proyecto = db.SpObtenerGaleriaPorId(id).FirstOrDefault();
                    if (proyecto == null)
                    {
                        return HttpNotFound();
                    }
                    return View(proyecto);
                }
            } 
            catch (Exception ex)
            {
               ViewBag.ErrorMessage = "Ocurrió un error al cargar el proyecto. Por favor, inténtalo de nuevo más tarde." + ex;
               return View("Error");
            }
        }
        [HttpPost, ActionName("EliminarProyecto")]
        public ActionResult EliminarProyectoConfirmado(int id)
        {
            try
            {
                using (var db = new AguaIngenieriaDB("MyDatabase"))
                {
                    // 1. Obtener proyecto actual para obtener ruta física
                    var proyecto = db.SpObtenerGaleriaPorId(id).FirstOrDefault();
                    if (proyecto == null)
                    {
                        TempData["MensajeError"] = "No se encontró el proyecto.";
                        return RedirectToAction("ProyectosCRUD");
                    }

                    // 2. Eliminar archivo físico si existe
                    if (!string.IsNullOrEmpty(proyecto.ImagenPath))
                    {
                        string rutaServidor = Server.MapPath(proyecto.ImagenPath);
                        if (System.IO.File.Exists(rutaServidor))
                        {
                            System.IO.File.Delete(rutaServidor);
                        }
                    }

                    // 3. Eliminar de la base de datos
                    db.SpEliminarGaleria(id);
                }

                TempData["MensajeExito"] = "Proyecto eliminado correctamente";
                return RedirectToAction("ProyectosCRUD");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage =
                    "Ocurrió un error al eliminar el proyecto. Por favor, inténtalo de nuevo más tarde. " + ex.Message;

                return View("Error");
            }
        }
    }
}