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

    }
}