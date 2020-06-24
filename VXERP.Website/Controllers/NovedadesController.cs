using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Business.Entities;
using CRM.Website.Security.Infrastructure;
using CRM.Business.DAL;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
namespace CRM.Website.Controllers
{
    public class NovedadesController : GenericController<Novedad>
    {
        NovedadRepository novedadRepository = new NovedadRepository();
        TipoNovedadRepository tipoNovedadRepository = new TipoNovedadRepository();
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        NovedadUsuariosRepository novedadUsuariosRepository = new NovedadUsuariosRepository();
        ArchivoModuloRepository archivoRepository = new ArchivoModuloRepository();

        public ActionResult GridViewAllPartial()
        {
            List<Novedad> notificaciones = new List<Novedad>();
            //if (!IsAdmin)
            //{
            //    NovedadUsuariosRepository novedadUsuariosRepository = new NovedadUsuariosRepository();
            //    NovedadRepository novedadRepository = new NovedadRepository();
            //    List<NovedadUsuarios> novedadesUsuario = new List<NovedadUsuarios>();

            //    if (this.User != null)
            //    {
            //        novedadesUsuario = novedadUsuariosRepository.GetFiltered(x => x.Usuario_Id == this.User.UserID).ToList();
            //    }

            //    foreach (NovedadUsuarios novedadUsuario in novedadesUsuario)
            //    {
            //        Novedad novedad = novedadRepository.Get(novedadUsuario.Novedad_Id).FirstOrDefault();
            //        if (novedad != null && novedad.Estado == true)
            //            notificaciones.Add(novedad);
            //    }
            //}
            //else
            //{
            //    notificaciones = novedadRepository.GetFiltered(x => x.Estado == true, x => x.TipoNovedad).ToList();
            //}
            notificaciones = novedadRepository.GetFiltered(x => x.Estado == true, x => x.TipoNovedad).ToList();
            return PartialView("_GridViewNovedades", notificaciones);
        }

        [LogonAuthorize(Roles = "VIEW,EDIT")]
        public ActionResult Index()
        {
            ClearTempFolder();
            GetNotifications();

            //if (!IsAdmin)
            //{
            //    NovedadUsuariosRepository novedadUsuariosRepository = new NovedadUsuariosRepository();
            //    NovedadRepository novedadRepository = new NovedadRepository();
            //    List<NovedadUsuarios> novedadesUsuario = new List<NovedadUsuarios>();

            //    if (this.User != null)
            //    {
            //        novedadesUsuario = novedadUsuariosRepository.GetFiltered(x => x.Usuario_Id == this.User.UserID ).ToList();
            //    }
            //    List<Novedad> notificaciones = new List<Novedad>();

            //    foreach (NovedadUsuarios novedadUsuario in novedadesUsuario)
            //    {
            //        Novedad novedad = novedadRepository.Get(novedadUsuario.Novedad_Id).FirstOrDefault();
            //        if (novedad != null && novedad.Estado == true)
            //            notificaciones.Add(novedad);
            //    }

            //    return View("Index", notificaciones);
            //}
            //else
            //{
            //    return View("Index", novedadRepository.GetFiltered(x => x.Estado == true, x => x.TipoNovedad).ToList());
            //}
            var novedades = novedadRepository.GetFiltered(x => x.Estado == true, x => x.TipoNovedad).ToList();
            return View("Index", novedades);
        }

        public ActionResult GridViewModalUsuarios()
        {
            return PartialView("_GridViewModalUsuarios", usuarioRepository.GetFiltered(x => x.Estado == true && x.RolesEmpresa.Any(s => s.Rol_Id != 25  )).ToList());

            //if(IsAdmin)
            //  return PartialView("_GridViewModalUsuarios", usuarioRepository.GetFiltered(x => x.Estado == true).ToList());
            //else
            //    return PartialView("_GridViewModalUsuarios", usuarioRepository.GetFiltered(x => x.Estado == true && x.RolesEmpresa.Any(s=>s.Rol_Id == 1)).ToList());
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            ViewBag.TipoNovedades = tipoNovedadRepository.GetFiltered(x => x.Estado == true).ToList();
            Novedad newNovedad = new Novedad();
            newNovedad.ArchivosModulo = new List<ArchivoModulo>().ToArray();
            return View(newNovedad);
        }

        public ActionResult AddArchivoModulo()
        {
            ViewBag.TipoArchivoModulo = _ControllerName;

            return PartialView("_FormArchivo", new ArchivoModulo());
        }

        public ActionResult GetUsuariosNotificacion(string Usuarios)
        {
            //var users = GetUsuariosPermitidosByRolEmpresa();

            var users = usuarioRepository.GetFiltered(x => x.Estado == true && x.RolesEmpresa.Any(s => s.Rol_Id != 25)).ToList();
            List<Usuario> allUsuarios = usuarioRepository.GetFiltered(x => x.Estado == true && x.RolesEmpresa.Any(s=>s.Rol_Id != 25)).ToList();

            if (Usuarios != "")
            {
                List<Usuario> usuariosNovedad = new List<Usuario>();

                string[] arrayUsuarios = Usuarios.Split(',');

                foreach (string idUsuario in arrayUsuarios)
                {
                    int idUsuarioInt = Int32.Parse(idUsuario);
                    Usuario usuarioBd = usuarioRepository.Get(idUsuarioInt).FirstOrDefault();
                    if (usuarioBd != null)
                        usuariosNovedad.Add(usuarioBd);
                }
                ViewBag.UsuariosNovedad = usuariosNovedad;
                //usuariosNoNovedad = usuariosNoNovedad.Where(x => !usuariosNovedad.Any(y => y.Id == x.Id)).ToList();
            }

            return PartialView("_Usuarios", users);
        }

        [HttpPost]
        public ActionResult AddUsuariosNotificacion(string SelectedUsuarios)
        {
            List<NovedadUsuarios> usuariosSeleccionados = new List<NovedadUsuarios>();
            if (SelectedUsuarios != "")
            {
                string[] arrayUsuarios = SelectedUsuarios.Split(',');

                foreach (string idUsuario in arrayUsuarios)
                {
                    if (idUsuario != "")
                    {
                        int idUsuarioInt = Int32.Parse(idUsuario);
                        Usuario usuarioBd = usuarioRepository.Get(idUsuarioInt).FirstOrDefault();
                        if (usuarioBd != null)
                        {
                            if (usuarioBd.Estado == true)
                            {
                                NovedadUsuarios novedadUsuario = new NovedadUsuarios();
                                novedadUsuario.Usuario = usuarioBd;
                                novedadUsuario.Usuario_Id = usuarioBd.Id;
                                usuariosSeleccionados.Add(novedadUsuario);
                            }
                        }
                    }
                }
            }
            
            return PartialView("_UsuariosNotificados", usuariosSeleccionados);
        }

        [HttpPost]
        public string AjaxAddArchivo()
        {
            string descripcion = Request["descripcion"];
            string[] descripciones = Request["descArchivos"].Split(',');
            string[] deletedArchivos = Request["deletedArchivos"].Split(',');

            for (int i = 0; i < descripciones.Length; i++)
            {
                if (deletedArchivos[i].Trim() == "false")
                {
                    if (descripciones[i].ToLower().Trim().Equals(descripcion.ToLower().Trim()))
                    {
                        return "false";
                    }
                }
            }

            HttpPostedFileBase currentFile = Request.Files["file"];

            string extension = Path.GetExtension(currentFile.FileName);
            string name = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString()
                                + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00")
                                + extension;
            try
            {
                string fileName = Server.MapPath("~/Content/TempFiles/") + name;
                currentFile.SaveAs(fileName);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, "No se pudo guardar el archivo: " + ex.Message);
            }

            return name;
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create([Bind(Include = "Titulo,Descripcion,TipoNovedad_Id,TipoNovedad,UsuariosNotificados,ArchivosModulo")] Novedad novedad, string[] UsuariosNotificar)
        {
            if (novedad.ArchivosModulo == null)
                novedad.ArchivosModulo = new List<ArchivoModulo>().ToArray();

            ViewBag.TipoNovedades = tipoNovedadRepository.GetFiltered(x => x.Estado == true).ToList();
            novedad.UsuariosNotificados = GetNovedadUsuariosFromArray(UsuariosNotificar);
            if (!ModelState.IsValid)
            {
                return View(novedad);
            }

            try
            {
                //Guardo la Novedad
                novedadRepository.Add(novedad, this.User.UserID);

                //Guardo los usuarios a notificar pasandoles el Id de la novedad en Novedad_Id
                List<MailDestinatario> mailDestinatarios = new List<MailDestinatario>();
                foreach (NovedadUsuarios novedadUsuario in novedad.UsuariosNotificados)
                {
                    MailDestinatario emailDest = new MailDestinatario();
                    emailDest.Destinatario = novedadUsuario.Usuario.NombreApellido;
                    emailDest.DestinatarioMail = novedadUsuario.Usuario.Email;
                    emailDest.Hilo = true;

                    mailDestinatarios.Add(emailDest);

                    novedadUsuario.Novedad_Id = novedad.Id;
                    novedadUsuariosRepository.Add(novedadUsuario, this.User.UserID);
                }

                if (novedad.ArchivosModulo != null)
                {
                    foreach (ArchivoModulo newArchivo in novedad.ArchivosModulo)
                    {
                        if (newArchivo.Deleted == false)
                        {
                            AltaArchivosModulo(newArchivo, novedad.Id.ToString());
                        }
                    }
                }

                Mail mail = new Mail();
                mail.Asunto = "Novedad N° " + novedad.Id.ToString() + " Creada";
                mail.Cuerpo = ArmarCuerpoMail(novedad);
                Usuario currentUser = usuarioRepository.Get(this.User.UserID).First();
                mail.Remitente = currentUser.Email;
                mail.NombreRemitente = currentUser.UserName + " (" + currentUser.Email + ")";


                SendEmails(mail, mailDestinatarios, novedad.ArchivosModulo.ToList());

                SetMessage(SUCCESS, " Guardado.");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, " Error al guardar la Novedad: " + ex.Message);
            }

            return Index();
        }

        private void SendEmails(Mail mail, List<MailDestinatario> mailDestinatarios, List<ArchivoModulo> adjuntos)
        {
            //string[] ems = { "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com",
            //                 "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com",
            //                 "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com"
            //               , "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com"
            //               , "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com"
            //               , "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com"
            //               , "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com"
            //               , "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com"
            //               , "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com"};
            foreach (MailDestinatario destinatario in mailDestinatarios)
            //foreach(string em in ems)
            {
                MailMessage mm = new MailMessage(mail.Remitente, destinatario.DestinatarioMail, mail.Asunto, mail.Cuerpo);
                //MailMessage mm = new MailMessage(mail.Remitente, em, mail.Asunto, mail.Cuerpo);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mm.IsBodyHtml = true;
                foreach (ArchivoModulo adjunto in adjuntos)
                {
                    string fileName = Server.MapPath("~/Content/Files/") + adjunto.Path;
                    mm.Attachments.Add(new Attachment(fileName));
                }

                SendEmailInBackgroundThread(mm);
            }
        }

        public void SendEmailInBackgroundThread(object mm)
        {
            //Thread bgThread = new Thread(new ParameterizedThreadStart(SendEmail));
            //bgThread.IsBackground = true;
            //bgThread.Start(mm);

            Task beginSendEmailTask = new Task(SendEmail, mm);
            beginSendEmailTask.Start();
        }

        public void SendEmail(object mm)
        {
            Thread.Sleep(5000);
            MailMessage mailMessage = (MailMessage)mm;
            SmtpClient ss = new SmtpClient("smtp.gmail.com", 587);
            ss.EnableSsl = true;
            ss.Timeout = 10000;
            ss.DeliveryMethod = SmtpDeliveryMethod.Network;
            ss.UseDefaultCredentials = true;
            var mail = ConfigurationManager.AppSettings["ADMIN.MAIL"];
            var password = ConfigurationManager.AppSettings["ADMIN.MAILPASS"];

            ss.Credentials = new NetworkCredential(mail, password);

            ss.SendCompleted += (s, e) =>
            {
                ss.Dispose();
                mailMessage.Dispose();
            };

            ss.SendAsync(mailMessage, mailMessage);
        }


        public List<NovedadUsuarios> GetNovedadUsuariosFromArray(string[] Usuarios)
        {
            List<NovedadUsuarios> usuariosNovedad = new List<NovedadUsuarios>();

            if (Usuarios != null)
            {
                foreach (string idUsuario in Usuarios)
                {
                    int idUsuarioInt = Int32.Parse(idUsuario);
                    Usuario usuarioBd = usuarioRepository.Get(idUsuarioInt).FirstOrDefault();
                    if (usuarioBd != null)
                    {
                        if (usuarioBd.Estado == true)
                        {
                            NovedadUsuarios novedadUsuario = new NovedadUsuarios();
                            novedadUsuario.Usuario = usuarioBd;
                            novedadUsuario.Usuario_Id = usuarioBd.Id;
                            usuariosNovedad.Add(novedadUsuario); 
                        }
                    }
                }
                ViewBag.UsuariosNovedad = usuariosNovedad;
            }

            return usuariosNovedad;
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(int? id)
        {
            ViewBag.TipoNovedades = tipoNovedadRepository.GetFiltered(x => x.Estado == true).ToList();
            Novedad novedad = null;
            try
            {
                novedad = novedadRepository.Get(id.Value, x => x.Usuario, s=>s.TipoNovedad).FirstOrDefault();
                if (novedad == null)
                    throw new Exception(" La Novedad no existe");

                novedad.UsuariosNotificados = novedadUsuariosRepository.GetFiltered(x => x.Novedad_Id == id, x => x.Usuario).ToList();

    
                if (!IsAdmin && ! (novedad.idUsuario.Value == base.UserContext.UserID))
                    throw new Exception(" No tiene permiso para Editar esta novedad");




                string idString = id.ToString();
                novedad.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                                                        && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                return Index();
            }

            return View(novedad);
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit([Bind(Include = "Id,Titulo,Descripcion,TipoNovedad_Id,TipoNovedad,UsuariosNotificados,ArchivosModulo,DescripcionEstado,Estado,FechaCreacion,FechaActualizacion,idUsuario")]
                                Novedad novedad, string[] UsuariosNotificar)
        {
            if (novedad.ArchivosModulo == null)
                novedad.ArchivosModulo = new List<ArchivoModulo>().ToArray();

            Usuario usuario = usuarioRepository.Get(novedad.idUsuario.Value).FirstOrDefault();
            novedad.Usuario = usuario;
            ViewBag.TipoNovedades = tipoNovedadRepository.GetFiltered(x => x.Estado == true).ToList();
            novedad.UsuariosNotificados = GetNovedadUsuariosFromArray(UsuariosNotificar);
            if (!ModelState.IsValid)
            {
                return View(novedad);
            }
            
            try
            {
                //Guardo la Novedad
                novedadRepository.Modify(novedad, this.User.UserID);

                //Guardo los usuarios a notificar nuevos
                List<MailDestinatario> mailDestinatarios = new List<MailDestinatario>();
                foreach (NovedadUsuarios novedadUsuario in novedad.UsuariosNotificados)
                {
                    MailDestinatario emailDest = new MailDestinatario();
                    emailDest.Destinatario = novedadUsuario.Usuario.NombreApellido;
                    emailDest.DestinatarioMail = novedadUsuario.Usuario.Email;
                    emailDest.Hilo = true;

                    mailDestinatarios.Add(emailDest);

                    novedadUsuario.Novedad_Id = novedad.Id;
                    if (!novedadUsuariosRepository.GetFiltered(x => x.Novedad_Id == novedadUsuario.Novedad_Id && x.Usuario_Id == novedadUsuario.Usuario_Id).Any())
                    {
                        novedadUsuariosRepository.Add(novedadUsuario, this.User.UserID);
                    }
                }
                //Elimino los usuarios a notificar que ya no estan en el listado
                List<NovedadUsuarios> novedadUsuariosBd = novedadUsuariosRepository.GetFiltered(x => x.Novedad_Id == novedad.Id).ToList();
                foreach (NovedadUsuarios novedadUsuario in novedadUsuariosBd)
                {
                    if (!novedad.UsuariosNotificados.Where(x => x.Novedad_Id == novedadUsuario.Novedad_Id && x.Usuario_Id == novedadUsuario.Usuario_Id).Any())
                    {
                        novedadUsuariosRepository.RemoveFromDataBase(novedadUsuario, this.User.UserID);
                    }
                }

                if (novedad.ArchivosModulo != null)
                {
                    foreach (ArchivoModulo archivoModulo in novedad.ArchivosModulo)
                    {
                        if ((archivoRepository.GetFiltered(x => x.Id == archivoModulo.Id && x.Estado == true).Any() == false) && archivoModulo.Deleted != true)
                        {
                            AltaArchivosModulo(archivoModulo, novedad.Id.ToString());
                        }

                        if (archivoModulo.Deleted == true)
                        {
                            archivoRepository.Remove(archivoModulo, this.User.UserID);
                        }
                    }
                }

                Mail mail = new Mail();
                mail.Asunto = "Novdedad N° " + novedad.Id.ToString() + " Modificada";
                mail.Cuerpo = ArmarCuerpoMail(novedad);
                Usuario currentUser = usuarioRepository.Get(this.User.UserID).First();
                mail.Remitente = currentUser.Email;
                mail.NombreRemitente = currentUser.UserName + " (" + currentUser.Email + ")";


                SendEmails(mail, mailDestinatarios, novedad.ArchivosModulo.ToList());

                SetMessage(SUCCESS, " Guardado.");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, " Error al guardar la Novedad: " + ex.Message);
            }

            return Index();
        }

        private string ArmarCuerpoMail(Novedad n)
        {
            var fullUrl = this.Url.Action("View", "Novedades", new { Id = n.Id }, this.Request.Url.Scheme);

            string cuerpo = "<b>Novedad: N° </b>" + n.Id.ToString() + "<br>" +
                "<b>Titulo: </b>" + n.Titulo + "<br>" +
                "<b>Fecha de Registro: </b>" + n.Fecha_Creacion.ToString("dd/MM/yyyy") + "<br>" +
                "<b>Descripcion: </b>" + n.Descripcion + "<br>" +                
                "<b>Link: </b>" + fullUrl.ToString() + "<br>";

            return cuerpo;
        }

        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult View(int? id)
        {
            Novedad novedad = null;
            try
            {
                novedad = novedadRepository.Get(id.Value, x => x.Usuario, x => x.TipoNovedad).FirstOrDefault();
                if (novedad == null)
                    throw new Exception(" La Novedad no existe");

          

                //novedad.UsuariosNotificados = novedadUsuariosRepository.GetFiltered(x => x.Novedad_Id == id, x => x.Usuario).ToList();
                //if (!IsAdmin && !novedad.UsuariosNotificados.Any(s => s.Usuario_Id == User.UserID))
                //    throw new Exception(" No tiene permiso para ver esta novedad");

                string idString = id.ToString();
                novedad.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                                                        && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                return Index();
            }

            try 
            {
                NovedadUsuarios novedadUsuario = novedadUsuariosRepository.GetFiltered(x => x.Novedad_Id == novedad.Id && x.Usuario_Id == this.User.UserID).FirstOrDefault();
                if (novedadUsuario != null)
                {
                    novedadUsuario.Visto = true;
                    novedadUsuariosRepository.Modify(novedadUsuario, this.User.UserID);
                }
            }
            catch(Exception){}

            GetNotifications();
            return View(novedad);
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var objectViewModel = novedadRepository.Get(id.Value).SingleOrDefault();
            if (objectViewModel == null)
            {
                return HttpNotFound();
            }
            novedadRepository.Remove(objectViewModel, this.User.UserID);

            return RedirectToAction("Index");
        }
    }
}
