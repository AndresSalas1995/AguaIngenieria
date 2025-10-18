using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

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

        //Recibimos el Formulario de contacto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Formulario(Models.ContactoFormulario modelo)
        {
            //Validamos el modelo
            if (!ModelState.IsValid)
                return View(modelo);

            try
            {

                //Datos del correo electrónico, lo leemos desde el Web.config para más seguridad.
                //Mas adelante se debe mejorar la contraseña usando algún servicio seguro. Por ahora es local.
                string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                    int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                    string smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
                    string smtpPass = ConfigurationManager.AppSettings["SmtpPass"];
                    string adminEmail = ConfigurationManager.AppSettings["AdminEmail"];


                    //Configuramos el cliente SMTP
                    using (var smtp = new System.Net.Mail.SmtpClient(smtpServer, smtpPort))
                { 
                    smtp.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
                    smtp.EnableSsl = true;
                    //Creamos el mensaje de correo para administrador
                    var mail = new System.Net.Mail.MailMessage();
                    mail.From = new System.Net.Mail.MailAddress(smtpUser);
                    mail.To.Add(adminEmail);
                    mail.Subject = "Nuevo mensaje de contacto desde el sitio web";
                    mail.Body = $"Nombre: {modelo.Nombre} {modelo.Apellido}\n" +
                                $"Correo Eléctronico: {modelo.Email}\n" +
                                $"Teléfono: {modelo.Telefono}\n\n" +
                                $"Mensaje:\n{modelo.Mensaje}";
                    //Enviamos el correo
                    await smtp.SendMailAsync(mail);

                    //Correo de confirmación al usuario que envió el mensaje
                    var correoUsuario = new System.Net.Mail.MailMessage();
                    correoUsuario.From = new System.Net.Mail.MailAddress(smtpUser, "Agua Ingenieria Costa Rica");
                    correoUsuario.To.Add(modelo.Email);
                    correoUsuario.Subject = "Confirmación de recepción de mensaje";
                    correoUsuario.Body = $"Hola {modelo.Nombre},\n\n" +
                                          $"Hemos recibido tu mensaje y te responderemos pronto.\n\n" +
                                          $"Gracias por contactarnos.\n\n" +
                                          $"— Equipo de Agua Ingenieria Costa Rica";

                    //Enviamos el correo de confirmación al usuario
                    await smtp.SendMailAsync(correoUsuario);
                }

            // Si el modelo no es válido, volvemos a mostrar el formulario con los errores
            ViewBag.Mensaje = "Tu mensaje se envió correctamente. Pronto nos pondremos en contacto.";
            return View(new Models.ContactoFormulario());
            }
            catch (SmtpException ex)
            {
                // Manejo de errores
                ViewBag.Error = "Ocurrió un error al enviar el mensaje. Por favor, inténtalo de nuevo más tarde.";
                return View(modelo);
            }
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