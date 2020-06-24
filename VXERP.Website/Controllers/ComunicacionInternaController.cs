using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using CRM.Business;
using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Business.Views;
using CRM.Website.Models;
using CRM.Website.Security;
using CRM.Website.Security.Infrastructure;
using DevExpress.Web;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml.Linq;
using System.Net.Mime;

namespace CRM.Website.Controllers
{
    [ValidateInput(false)]
    public class ComunicacionInternaController : BaseController
    {
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        MailRepository mailRepository = new MailRepository();
        MailDestinatarioRepository mailDestinatarioRepository = new MailDestinatarioRepository();
        vClientes vClientes = new vClientes();
        vContactosSinFoto vContactosSinFoto = new vContactosSinFoto();
        ArchivoModuloRepository archivoRepository = new ArchivoModuloRepository();
        TipoArchivoModuloRepository tipoArchivoModuloRepositoy = new TipoArchivoModuloRepository();
        List<string> imageNames;
        
        public const string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

        //Bandeja de Entrada
        public ActionResult Index()
        {

            try
            {
                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Content/UploadImages/"));
                Session["Usuarios"] = GetUsuariosPermitidosByRolEmpresa();
                Session["EmailClientes"] = GetEmailsCliente();
                foreach (string filePath in filePaths)
                    System.IO.File.Delete(filePath);

                if (this.UserContext.RolesEmpresa.Any(x => x.Rol.RoleName.Trim().ToUpper().Equals("ADMIN")))
                {
                    Session["IsAdmin"] = true;
                    List<MailDestinatario> mailsDestinatario = mailDestinatarioRepository.GetFiltered(x => x.Estado == true).ToList();
                    List<Mail> mails = mailRepository.GetAll().ToList();

                    GetMensajes();
                    mails = mails.Where(x => mailsDestinatario.Any(y => y.Mail_Id == x.Id) == true).ToList();
                    Session["ListadoMensajes"] = mails.ToList();
                    return View("Index", mails.ToList());
                }
                else
                {
                    Session["IsAdmin"] = false;
                    Usuario currentUser = usuarioRepository.Get(this.User.UserID).FirstOrDefault();
                    string email = currentUser.Email.Replace(";", ",");
                    string cliente = currentUser.NombreApellido.Trim();
                    List<MailDestinatario> mailsDestinatario = mailDestinatarioRepository.GetFiltered(x => x.Destinatario.Contains(cliente) && x.Estado == true).ToList();

                    List<Mail> mails = mailRepository.GetAll().ToList();

                    GetMensajes();
                    mails = mails.Where(x => mailsDestinatario.Any(y => y.Mail_Id == x.Id) == true).ToList();
                    Session["ListadoMensajes"] = mails.ToList();
                    return View("Index", mails.ToList());
                }
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, " Se produjo un error al querer recuperar los mensajes recibidos." + ex.ToString());
                return Index();
            }
        }

        public ActionResult GridViewAllPartial()
        {
            List<Mail> listadoMails = (List<Mail>)Session["ListadoMensajes"];
            return PartialView("_GridViewAllPartial", listadoMails.ToList());
        }

        public ActionResult Enviados()
        {
            try
            {
                if (this.UserContext.RolesEmpresa.Any(x => x.Rol.RoleName.Trim().ToUpper().Equals("ADMIN")))
                {
                    List<Mail> mails = mailRepository.GetFiltered(x => x.Estado == true).ToList();
                    Session["ListadoMensajes"] = mails.ToList();
                    return View("Enviados", mails.ToList());
                }
                else
                {
                    List<Mail> mails = mailRepository.GetFiltered(x => x.Estado == true && x.idUsuario == this.User.UserID).ToList();
                    Session["ListadoMensajes"] = mails.ToList();
                    return View("Enviados", mails.ToList());
                }
            }
            catch (Exception)
            {
                SetMessage(ERROR, " Se produjo un error al querer recuperar los mensajes enviados.");
                return Index();
            }
        }

        public ActionResult Create()
        {
            Mail mail = new Mail();
            mail.ArchivosModulo = new List<ArchivoModulo>().ToArray();
            ViewBag.SubtituloMensaje = "Nuevo Mensaje";
            return View("Create", mail);
        }

        public ActionResult GetEmails(string searchTerm)
        {
            List<KeyValuePair<string, string>> emails = new List<KeyValuePair<string, string>>();
            List<Usuario> allUsuarios = (List<Usuario>)Session["Usuarios"];
           List<string> allEmails = new List<string>();
            //vContactosSinFoto = vContactosSinFoto.get
            List<KeyValuePair<string, string>> emailClientes = (List<KeyValuePair<string, string>>)Session["EmailClientes"]; 
            //RolEmpresaRepository rolEmpresaRepository = new RolEmpresaRepository();
            //List<RolEmpresa> allUsuariosRoles = new List<RolEmpresa>();
            //Usuario usuario = new Usuario();



            //if (this.UserContext.RolesEmpresa.Any(x => x.Rol.RoleName.Trim().ToUpper().Equals("ADMIN")))
            //{
            //    //allEmails = usuarioRepository.GetAll().Select(x => x.Email).ToList();
            //    var usuarios = usuarioRepository.GetFiltered(x => x.Estado == true).ToList();
            //    allUsuariosRoles = rolEmpresaRepository.GetAll().ToList();

            //    allUsuarios = (from u in usuarios
            //                  join r in allUsuariosRoles on u.Id equals r.Usuario_Id
            //                  where r.Rol_Id != 25
            //                  select  usuario = u ).ToList();

            //    emailClientes = (vContactosSinFoto.GetViewModel().AsEnumerable().
            //        Where(c => c.Field<string>("Descripcion").Contains("Emails notificación de Sistema")).
            //        Select(e => new KeyValuePair<string, string>(e.Field<string>("Cliente"), e.Field<string>("Email")))).ToList();

            //    emails.Add(new KeyValuePair<string, string>("Todos", "Todos"));
            //}
            //else
            //{
            //    //allEmails = GetUsuariosPermitidosByRolEmpresa().Select(s=> s.Email).ToList();
            //    emailClientes = GetUsuariosPermitidosByRolEmpresa().Select(s => new KeyValuePair<string, string>(s.NombreApellido, s.Email)).ToList();
            //    allUsuarios = GetUsuariosPermitidosByRolEmpresa().ToList();
            //}

            try
            {
                if (searchTerm.Trim() != "")
                {
                    foreach (var clienteEmail in emailClientes)
                    {
                        if (clienteEmail.Key.ToUpper().Trim().Contains(searchTerm.ToUpper().Trim()) ||
                            clienteEmail.Value.ToUpper().Trim().Contains(searchTerm.ToUpper().Trim()))
                        {
                            emails.Add(new KeyValuePair<string, string>(clienteEmail.Value + "/" + clienteEmail.Key.Trim() + " (" + clienteEmail.Value + ")",
                                clienteEmail.Key.Trim() + " (" + clienteEmail.Value + ")"));
                        }
                    }
                    foreach (Usuario user in allUsuarios)
                    {
                        if (user.Email.ToUpper().Trim().Contains(searchTerm.ToUpper().Trim()) ||
                            user.UserName.ToUpper().Trim().Contains(searchTerm.ToUpper().Trim()))
                        {
                            emails.Add(new KeyValuePair<string, string>(user.Email + "/" + user.NombreApellido.Trim() + " (" + user.Email + ")",
                                user.NombreApellido.Trim() + " (" + user.Email + ")"));
                        }
                    }
                }

                if (searchTerm.Trim() != "")
                {
                    foreach (DataRow row in (base.IsAdmin ? vContactosSinFoto.GetViewModel().Rows : (new vClientes(base.UserContext.RolesCliente.ToList()).Datos.Rows)))
                    {
                        string[] emailsCliente = row["Email"].ToString().Split(';');
                        string emailsCliente_S = string.Join(",", emailsCliente.ToArray());
                        emailsCliente_S = row["Email"].ToString();
                        string cliente = row["Descripcion"].ToString();
                        if (cliente.ToUpper().Trim().Contains(searchTerm.ToUpper().Trim()))
                        {
                            emails.Add(new KeyValuePair<string, string>(emailsCliente_S + "/" + row["Descripcion"].ToString().Trim(),
                                row["Descripcion"].ToString().Trim()));
                        }

                        //foreach (string emailCliente in emailsCliente)
                        //{
                        //    //En caso de que el cliente sea en general probar metiendo los emails con comas
                        //    if (emailCliente.Trim() != "")
                        //    {
                        //        if ((emailCliente.ToUpper().Trim().Contains(searchTerm.ToUpper().Trim())) ||
                        //            (row["Descripcion"].ToString().ToUpper().Trim().Contains(searchTerm.ToUpper().Trim())))
                        //        {
                        //            emails.Add(new KeyValuePair<string, string>(emailCliente + "/" + row["Descripcion"].ToString().Trim() + " (" + emailCliente + ")",
                        //                row["Descripcion"].ToString().Trim() + " (" + emailCliente + ")"));
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch (Exception) { }

            var serializedEmails = from email in emails select new { id = email.Key, name = email.Value };
            return Json(serializedEmails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Create(Mail mail, string Destinatarios, string textEditor)
        {
            try
            {
                //mail.Cuerpo = textEditor;


                mail.Cuerpo = HtmlEditorExtension.GetHtml("htmlEditor");
                //mail.Cuerpo = HtmlEditorExtension.GetHtml("htmlEditor");
                //mail.Asunto = mail.Asunto == "" ? "Comunicación interna CRM";
                Mail mailArmado = mail;
                if (Destinatarios == "")
                    ModelState.AddModelError("ErrorDestinatario", "El Destinatario es requerido");

                if (!ModelState.IsValid)
                {
                    if (mailArmado.ArchivosModulo == null)
                        mailArmado.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                    ViewBag.Destinatarios = Destinatarios;
                    ViewBag.SubtituloMensaje = "Nuevo Mensaje";
                    return View(mailArmado);
                }

                try
                {
                    Usuario currentUser = usuarioRepository.Get(this.User.UserID).First();
                    mailArmado.Remitente = currentUser.Email;
                    mailArmado.NombreRemitente = currentUser.NombreApellido + " (" + currentUser.Email + ")";
                    mailRepository.Add(mail, this.User.UserID);

                    List<MailDestinatario> mailDestinatarios = new List<MailDestinatario>();

                    char[] separador = { ',' };
                    //if(!Destinatarios.Contains(separador.ToString()))
                    //    separador = new char[] { ';' };

                    string[] destinatarios = Destinatarios.Split(separador);
                    if (destinatarios.Contains("Todos/Todos (Todos)"))
                    {

                        // List<string> allEmails = usuarioRepository.GetFiltered(s=> s.RolesEmpresa.Any(i=>i.Rol_Id == 25)).Select(x => x.Email).ToList();
                        List<Usuario> userEmails = usuarioRepository.GetFiltered(s => s.RolesEmpresa.Any(i => i.Rol_Id == 25) && s.Email.Length > 0).ToList();
                        vContactosSinFoto.Datos = vContactosSinFoto.GetByFilter("Descripcion = 'Emails notificación de Sistema' and Email <> '' ");

                        foreach (DataRow r in vContactosSinFoto.Datos.Rows)
                        {
                            string descrip = r["Descripcion"].ToString().Trim();
                            if (descrip == "Emails notificación de Sistema")
                            {
                                bool todosemailsValidos = true;
                                MailDestinatario mailDest = new MailDestinatario();
                                string Cliente = r["Cliente"].ToString().Trim();
                                string todosMail = r["Email"].ToString().Trim();
                                string emailsRep = todosMail.Replace(';', ',');
                                string[] emailsCliente = emailsRep.Split(',');

                                List<string> validMails = new List<string>();
                                foreach (string emailCliente in emailsCliente)
                                {
                                    if (emailsCliente.Count() == 1 && emailCliente == "")
                                    {
                                        todosemailsValidos = false;
                                        AvisoMailErroneo(Cliente, todosMail);
                                        continue;
                                    }

                                    if (emailCliente == "")
                                        continue;

                                    mailDest.Mail_Id = mail.Id;
                                    if (IsEmail(emailCliente.Trim()))
                                        validMails.Add(emailCliente.Trim());
                                    else
                                    {
                                        todosemailsValidos = false;
                                        AvisoMailErroneo(Cliente, todosMail);
                                    }
                                }

                                if (todosemailsValidos)
                                {

                                    if (!validMails.Contains("comercial@ionics.com.ar"))
                                    {
                                        string[] comercial = new string[] { "comercial@ionics.com.ar" };
                                        validMails.AddRange(comercial);
                                    }
                                    mailDest.DestinatarioMail = string.Join(";", validMails).Trim();
                                    mailDest.Destinatario = Cliente + "[" + emailsRep + "]";
                                    mailDest.Hilo = true;
                                    mailDestinatarioRepository.Add(mailDest, this.User.UserID);
                                    mailDestinatarios.Add(mailDest);
                                }

                            }
                        }
                    }
                    else
                    {
                        foreach (string destinatario in destinatarios)
                        {
                            bool todosemailsValidos = true;
                            string[] emailAndDescripMail = destinatario.Split('/');
                            string[] emailsCliente = emailAndDescripMail[0].Split(';');
                            MailDestinatario mailDest = new MailDestinatario();
                            List<string> validMails = new List<string>();
                            foreach (string emailCliente in emailsCliente)
                            {

                                if (emailsCliente.Count() == 1 && emailCliente == "")
                                {
                                    todosemailsValidos = false;
                                    AvisoMailErroneo(emailAndDescripMail[1].Trim(), emailCliente);
                                    continue;
                                }

                                mailDest.Mail_Id = mail.Id;
                                if (IsEmail(emailCliente.Trim()))
                                    validMails.Add(emailCliente.Trim());
                                else
                                {
                                    todosemailsValidos = false;
                                    continue;
                                }
                            }

                            if (todosemailsValidos)
                            {
                                if (!validMails.Contains("comercial@ionics.com.ar"))
                                {
                                    string[] comercial = new string[] { "comercial@ionics.com.ar" };
                                    validMails.AddRange(comercial);
                                }
                                mailDest.DestinatarioMail = string.Join(";", validMails).Trim();
                                mailDest.Destinatario = emailAndDescripMail[1].Trim() + "[" + mailDest.DestinatarioMail + "]";
                                mailDest.Hilo = true;
                                mailDestinatarioRepository.Add(mailDest, this.User.UserID);
                                mailDestinatarios.Add(mailDest);
                            }
                        }
                    }

                    if (mail.ArchivosModulo != null)
                    {
                        foreach (ArchivoModulo newArchivo in mail.ArchivosModulo)
                        {
                            if (newArchivo.Deleted == false)
                            {
                                AltaArchivosModulo(newArchivo, mail.Id.ToString());
                            }
                        }
                    }

                    string idString = mail.Id.ToString();
                    List<ArchivoModulo> archivosAdjuntos = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                        && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToList();

                    try
                    {
                        //Parallel.ForEach(mailDestinatarios, md =>
                        //   {
                               await SendEmails(mail, mailDestinatarios, archivosAdjuntos);
                           //});
                        

                        //Parallel.ForEach(mailDestinatarios, m =>
                        //{
                        //   await SendEmails(mail, m, archivosAdjuntos).Wait();
                        //});

                        //await Task.WhenAll(mailDestinatarios.Select(m => SendEmails(mail, m, archivosAdjuntos)));



                    }
                    catch (Exception ex)
                    {
                        SetMessage(ERROR, " El Mensaje No ha sido enviado correctamente.");
                        return View(mail);
                    }

                }
                catch (Exception ex)
                {
                    if (mail.ArchivosModulo == null)
                        mail.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                    ViewBag.Destinatarios = Destinatarios;
                    ViewBag.SubtituloMensaje = "Nuevo Mensaje";
                    SetMessage(ERROR, " " + ex.Message);
                    return View(mail);
                }

                SetMessage(SUCCESS, " El Mensaje ha sido enviado correctamente.");
                return Index();
            }
            catch (Exception ex)
            {
                Response.StatusDescription = ex.Message;
                return View();
            }
        }


        private async Task SendEmails(Mail mail, MailDestinatario destinatario, List<ArchivoModulo> adjuntos)
        {
            try
            {
                mail.Asunto = mail.Asunto.Split('(')[0].Trim();
                string recipients = string.Join(",", destinatario.DestinatarioMail.Replace(";", ","));
                var html = GetBodyHtml(HtmlEditorExtension.GetHtml("htmlEditor"));
                mail.Cuerpo = html;
                var asun = destinatario.Destinatario.Split('[');
                mail.Asunto += " (" + asun[0] + ")";
                MailMessage mm = new MailMessage(mail.Remitente, recipients, mail.Asunto, mail.Cuerpo);
                AlternateView plainTextView = AlternateView.CreateAlternateViewFromString(mail.Cuerpo, null, MediaTypeNames.Text.Plain);
                mm.AlternateViews.Add(plainTextView);
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Cuerpo, null, MediaTypeNames.Text.Html);
                foreach (string imgName in imageNames)
                {
                    //LinkedResource sampleImage = new LinkedResource(MapPath(string.Format("~/Images/{0}.jpg", imgName)), MediaTypeNames.Image.Jpeg);
                    LinkedResource sampleImage = new LinkedResource(Server.MapPath(string.Format("~/Content/UploadImages/{0}.jpg", imgName)), MediaTypeNames.Image.Jpeg);
                    sampleImage.ContentId = imgName;
                    htmlView.LinkedResources.Add(sampleImage);

                }
                mm.AlternateViews.Add(htmlView);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mm.IsBodyHtml = true;
                foreach (ArchivoModulo adjunto in adjuntos)
                {
                    string fileName = Server.MapPath("~/Content/Files/") + adjunto.Path;
                    mm.Attachments.Add(new Attachment(fileName));
                }


                await SendEmailInBackgroundThread(mm);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                var s = ex.Message;
                //AvisoMailErroneo()
            }
            
        }


        private async Task SendEmails(Mail mail, List<MailDestinatario> mailDestinatarios, List<ArchivoModulo> adjuntos)
        {
            //string[] ems = { "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com",
            //               , "fbriolfo@rt-solutions.com.ar", "prueba.ionics@gmail.com", "facu.riolfo@gmail.com", "facu.riolfo@hotmail.com", "facundobarriosriolfo@hotmail.com"};
            


            foreach (MailDestinatario destinatario in mailDestinatarios)
            {
                try
                {
                    try
                    {
                        mail.Asunto = mail.Asunto.Split('(')[0].Trim();
                        string recipients = string.Join(",", destinatario.DestinatarioMail.Replace(";", ","));
                        var html = GetBodyHtml(HtmlEditorExtension.GetHtml("htmlEditor"));
                        mail.Cuerpo = html;
                        //var asun = destinatario.Destinatario.Split('[');
                        //mail.Asunto += " (" + asun[0] + ")";
                        MailMessage mm = new MailMessage(mail.Remitente, recipients, mail.Asunto, mail.Cuerpo);
                        AlternateView plainTextView = AlternateView.CreateAlternateViewFromString(mail.Cuerpo, null, MediaTypeNames.Text.Plain);
                        mm.AlternateViews.Add(plainTextView);
                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Cuerpo, null, MediaTypeNames.Text.Html);
                        foreach (string imgName in imageNames)
                        {
                            //LinkedResource sampleImage = new LinkedResource(MapPath(string.Format("~/Images/{0}.jpg", imgName)), MediaTypeNames.Image.Jpeg);
                            LinkedResource sampleImage = new LinkedResource(Server.MapPath(string.Format("~/Content/UploadImages/{0}.jpg", imgName)), MediaTypeNames.Image.Jpeg);
                            sampleImage.ContentId = imgName;
                            htmlView.LinkedResources.Add(sampleImage);

                        }
                        mm.AlternateViews.Add(htmlView);
                        mm.BodyEncoding = UTF8Encoding.UTF8;
                        mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                        mm.IsBodyHtml = true;
                        foreach (ArchivoModulo adjunto in adjuntos)
                        {
                            string fileName = Server.MapPath("~/Content/Files/") + adjunto.Path;
                            mm.Attachments.Add(new Attachment(fileName));
                        }


                        await SendEmailInBackgroundThread(mm);
                    }
                    catch (Exception ex)
                    {
                         SetMessage(ERROR, ex.Message);
                        var s = ex.Message;
                        
                    }

                }
                catch (Exception ex)
                {
                    SetMessage(ERROR, ex.Message);
                }
            }
        }

        public bool IsEmail(string email)
        {
            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }

        public async Task SendEmailInBackgroundThread(object mm)
        {
            //Thread bgThread = new Thread(new ParameterizedThreadStart(SendEmail));
            //bgThread.IsBackground = true;
            //bgThread.Start(mm);

            //Task beginSendEmailTask = new Task(SendEmail, mm);
            //beginSendEmailTask.Start();

            await SendEmail(mm);
        }

        public async Task SendEmail(object mm)
        {
            //Thread.Sleep(5000);
            MailMessage mailMessage = (MailMessage)mm;
            SmtpClient ss = new SmtpClient("smtp.gmail.com", 587);
            ss.EnableSsl = true;
            ss.Timeout = 100000;
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
            mailMessage.From = new MailAddress(mail, "Comunicación Ionics SA");
            //ss.SendAsync(mailMessage, mailMessage);
            await ss.SendMailAsync(mailMessage);
        }

        private async void AvisoMailErroneo(string cliente, string mails)
        {
            Mail mail = new Mail();
            mail.Remitente = ConfigurationManager.AppSettings["ADMIN.MAIL"];
            mail.NombreRemitente = "Sistema de Gestión Ionics SA (" + mail.Remitente + ")";
            mail.Asunto = "Error Mail Cliente- " + cliente;
            mail.Cuerpo = "Revisar los siguientes Mail: " + mails;
            MailMessage mm = new MailMessage(mail.Remitente, "crodriguez@ionics.com.ar", mail.Asunto, mail.Cuerpo);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mm.IsBodyHtml = true;

            await SendEmailInBackgroundThread(mm);
        }

        

        public ActionResult GetTags(string destinatarios)
        {
            List<KeyValuePair<string, string>> listEmails = new List<KeyValuePair<string, string>>();
            if (destinatarios != "" && destinatarios != null)
            {
                string[] emails = destinatarios.Split(',');

                foreach (string email in emails)
                {
                    if (email.ToLower() == "todos")
                    {
                        listEmails.Add(new KeyValuePair<string, string>("Todos", "Todos"));
                        continue;
                    }
                    string[] em = email.Split('/');
                    listEmails.Add(new KeyValuePair<string, string>(em[0] + "/" + em[1], em[1]));
                }
            }
            var serializedEmails = from email in listEmails select new { id = email.Key, name = email.Value };
            return Json(serializedEmails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddArchivoModulo()
        {
            ViewBag.TipoArchivoModulo = _ControllerName;

            return PartialView("_FormArchivo", new ArchivoModulo());
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
                SetMessage(ERROR, " No se pudo guardar el archivo: " + ex.Message);
            }

            return name;
        }

        public ActionResult View(int id, bool ac)
        {
            try
            {
                Session["IsAdmin"] = base.IsAdmin;

                Mail mail = mailRepository.Get(id, x => x.Usuario).First();
                List<MailDestinatario> mailDestinatarios = mailDestinatarioRepository.GetFiltered(x => x.Mail_Id == mail.Id).ToList();
                string[] mailsDest = mailDestinatarios.Where(x => x.Hilo == true && x.Destinatario.Contains(this.User.FirstName)).
                Select(x => x.DestinatarioMail).ToArray();

                List<string> mails = new List<string>();
                foreach (var destinatario in mailDestinatarios)
                {
                    mails.Add(destinatario.Destinatario);
                }

                ViewBag.Destinatarios = string.Join(", ", mails.ToArray());
                //ViewBag.Destinatarios = mails;
                string idString = id.ToString();

                mail.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                    && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

                ViewBag.ActionController = (bool)ac;

                Usuario currentUser = usuarioRepository.Get(this.User.UserID).FirstOrDefault();
                MailDestinatario mailDestinatario = mailDestinatarioRepository.GetFiltered(x => x.Mail_Id == mail.Id &&
                    x.Destinatario.Contains(this.User.FirstName)).FirstOrDefault();

                //AC es el nombre de la accion
                if (mailDestinatario != null && ac == true)
                {
                    mailDestinatario.Visto = true;
                    mailDestinatarioRepository.Modify(mailDestinatario, this.User.UserID);
                }

                GetMensajes();
                return View("View", mail);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, " " + ex.Message);
                return Index();
            }
        }

        public ActionResult Reenviar(int id)
        {
            try
            {
                Mail mail = mailRepository.Get(id).FirstOrDefault();
                string destinatarios;

                string[] listMailDest = mailDestinatarioRepository.GetFiltered(x => x.Mail_Id == mail.Id && x.Hilo == true).Select(x => x.DestinatarioMail).ToArray();

                List<string> mails = new List<string>();

                for (int i = 0; i < listMailDest.Length; i++)
                {
                    mails.Add(listMailDest[i]);
                }
                destinatarios = string.Join(", ", mails.ToArray());

                string idString = id.ToString();
                mail.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                    && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

                string texto = "<br><br><hr>" +
                    "<b>De: </b>" + mail.Remitente + "<br>" + "<b>Enviado: </b>" + mail.FechaActualizacion + "<br>" +
                    "<b>Para: </b>" + destinatarios + "<br>" + "<b>Asunto: </b>" + mail.Asunto + "<br><br>" + mail.Cuerpo;

                mail.Cuerpo = texto;
                ViewBag.SubtituloMensaje = "Reenviar Mensaje";

                return View("Create", mail);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, " " + ex.Message);
                return Index();
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Reenviar(Mail mail, string Destinatarios, string textEditor)
        {
            mail.Cuerpo = textEditor;
            mail.Asunto = "Comunicación interna CRM";
            if (Destinatarios == "")
                ModelState.AddModelError("ErrorDestinatario", "El Destinatario es requerido");

            if (!ModelState.IsValid)
            {
                if (mail.ArchivosModulo == null)
                    mail.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                ViewBag.Destinatarios = Destinatarios;
                ViewBag.SubtituloMensaje = "Reenviar Mensaje";
                return View("Create", mail);
            }

            try
            {
                Usuario currentUser = usuarioRepository.Get(this.User.UserID).First();
                mail.Remitente = currentUser.Email;
                mail.NombreRemitente = currentUser.UserName + " (" + currentUser.Email + ")";

                mailRepository.Add(mail, this.User.UserID);

                List<MailDestinatario> mailDestinatarios = new List<MailDestinatario>();
                string[] emails = Destinatarios.Split(',');
                foreach (string email in emails)
                {
                    string[] em = email.Split('/');
                    string[] emailsCliente = em[0].Trim().Split(';');

                    foreach (string emailCliente in emailsCliente)
                    {
                        MailDestinatario mailDest = new MailDestinatario();
                        mailDest.Mail_Id = mail.Id;
                        if (IsEmail(emailCliente.Trim()))
                            mailDest.DestinatarioMail = emailCliente.Trim();

                        mailDest.Destinatario = em[1];
                        mailDest.Hilo = true;

                        mailDestinatarioRepository.Add(mailDest, this.User.UserID);
                        mailDestinatarios.Add(mailDest);
                    }
                }

                if (mail.ArchivosModulo != null)
                {
                    foreach (ArchivoModulo newArchivo in mail.ArchivosModulo)
                    {
                        if (newArchivo.Deleted == false)
                        {
                            if (newArchivo.TipoArchivoModulo != null)
                            {
                                newArchivo.NombreTipoArchivoModulo = newArchivo.TipoArchivoModulo.Tipo;
                                CopyFileToTempFolder(newArchivo);
                                AltaArchivosModulo(newArchivo, mail.Id.ToString());
                            }
                            else
                            {
                                AltaArchivosModulo(newArchivo, mail.Id.ToString());
                            }
                        }
                    }
                }

                string idString = mail.Id.ToString();
                List<ArchivoModulo> archivosAdjuntos = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                    && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToList();

                SendEmails(mail, mailDestinatarios, archivosAdjuntos);
                SetMessage(SUCCESS, " El Mensaje ha sido enviado correctamente.");
            }
            catch (Exception ex)
            {
                if (mail.ArchivosModulo == null)
                    mail.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                ViewBag.Destinatarios = Destinatarios;
                ViewBag.SubtituloMensaje = "Reenviar Mensaje";
                SetMessage(ERROR, " " + ex.Message);
                return View("Create", mail);
            }

            return Index();
        }

        public ActionResult Responder(int id, bool todos)
        {
            try
            {
                Mail mail = mailRepository.Get(id).FirstOrDefault();
                string destinatarios;

                string[] listMailDest = mailDestinatarioRepository.GetFiltered(x => x.Mail_Id == mail.Id && x.Hilo == true).Select(x => x.DestinatarioMail).ToArray();
                string[] listDest = mailDestinatarioRepository.GetFiltered(x => x.Mail_Id == mail.Id && x.Hilo == true).Select(x => x.Destinatario).ToArray();
                List<MailDestinatario> mailDestinatarios = mailDestinatarioRepository.GetFiltered(x => x.Mail_Id == mail.Id && x.Hilo == true).ToList();

                List<string> mails = new List<string>();
                for (int i = 0; i < listMailDest.Length; i++)
                {
                    mails.Add(listMailDest[i]);
                }
                destinatarios = string.Join(", ", mails.ToArray());

                if (todos)
                {
                    List<string> mailsTag = new List<string>();

                    foreach (MailDestinatario mailDestinatario in mailDestinatarios)
                    {
                        if (mailsTag.Count > 0)
                        {
                            foreach (string mailTag in mailsTag.ToList())
                            {
                                string[] em = mailTag.Split('/');
                                if (!mailDestinatario.Destinatario.Equals(em[1]))
                                {
                                    mails = mailDestinatarios.Where(x => x.Destinatario.Equals(mailDestinatario.Destinatario)).Select(x => x.DestinatarioMail).ToList();
                                    string mails_S = string.Join(";", mails.ToArray());

                                    mailsTag.Add(mails_S + "/" + mailDestinatario.Destinatario);
                                }
                            }
                        }
                        else
                        {
                            mails = mailDestinatarios.Where(x => x.Destinatario.Equals(mailDestinatario.Destinatario)).Select(x => x.DestinatarioMail).ToList();
                            string mails_S = string.Join(";", mails.ToArray());

                            mailsTag.Add(mails_S + "/" + mailDestinatario.Destinatario);
                        }
                    }
                    Usuario currentUser = usuarioRepository.Get(this.User.UserID).First();
                    if (!currentUser.Email.Trim().Equals(mail.Remitente.Trim()))
                    {
                        mailsTag.Add(mail.Remitente + "/" + mail.NombreRemitente);
                    }

                    ViewBag.Destinatarios = string.Join(",", mailsTag.ToArray());
                }
                else
                {
                    string mailRemitente;
                    if (mail != null)
                        mailRemitente = mail.Remitente + "/" + mail.NombreRemitente;
                    else
                        mailRemitente = "";

                    ViewBag.Destinatarios = mailRemitente;
                }

                string idString = id.ToString();
                mail.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                    && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

                string texto = "<br><br><hr>" +
                    "<b>De: </b>" + mail.Remitente + "<br>" + "<b>Enviado: </b>" + mail.FechaActualizacion + "<br>" +
                    "<b>Para: </b>" + destinatarios + "<br>" + "<b>Asunto: </b>" + mail.Asunto + "<br><br>" + mail.Cuerpo;

                mail.Cuerpo = texto;
                ViewBag.SubtituloMensaje = "Responder Mensaje";

                return View("Create", mail);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, " " + ex.Message);
                return Index();
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Responder(Mail mail, string Destinatarios, string textEditor)
        {
            mail.Cuerpo = textEditor;
            mail.Asunto = "Comunicación interna CRM";
            if (Destinatarios == "")
                ModelState.AddModelError("ErrorDestinatario", "El Destinatario es requerido");

            if (!ModelState.IsValid)
            {
                if (mail.ArchivosModulo == null)
                    mail.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                ViewBag.Destinatarios = Destinatarios;
                ViewBag.SubtituloMensaje = "Responder Mensaje";
                return View("Create", mail);
            }

            try
            {
                Usuario currentUser = usuarioRepository.Get(this.User.UserID).First();
                mail.Remitente = currentUser.Email;
                mail.NombreRemitente = currentUser.UserName + " (" + currentUser.Email + ")";

                bool exists = mailRepository.GetFiltered(x => x.Id == mail.Id && x.Asunto.Equals(mail.Asunto) && x.idUsuario == this.User.UserID).Any();
                if (exists)
                {
                    mailRepository.Modify(mail, this.User.UserID);
                }
                else
                {
                    mailRepository.Add(mail, this.User.UserID);
                }

                List<MailDestinatario> mailDestinatarios = new List<MailDestinatario>();
                string[] emails = Destinatarios.Split(',');
                foreach (string email in emails)
                {
                    string[] em = email.Split('/');
                    string mailDesti = em[1];
                    List<MailDestinatario> mailsDest = mailDestinatarioRepository.GetFiltered(x => x.Mail_Id == mail.Id && x.Destinatario.Equals(mailDesti)).ToList();
                    if (mailsDest != null && mailsDest.Count > 0)
                    {
                        foreach (MailDestinatario mailDest in mailsDest)
                        {
                            mailDest.Visto = false;
                            mailDestinatarioRepository.Modify(mailDest, this.User.UserID);
                            mailDestinatarios.Add(mailDest);
                        }
                    }
                    else
                    {
                        string[] emailsCliente = em[0].Trim().Split(';');

                        foreach (string emailCliente in emailsCliente)
                        {
                            MailDestinatario mailDestinatario = new MailDestinatario();
                            mailDestinatario.Mail_Id = mail.Id;

                            if (IsEmail(emailCliente.Trim()))
                                mailDestinatario.DestinatarioMail = emailCliente.Trim();

                            mailDestinatario.Destinatario = em[1];
                            mailDestinatario.Hilo = true;

                            mailDestinatarioRepository.Add(mailDestinatario, this.User.UserID);
                            mailDestinatarios.Add(mailDestinatario);
                        }
                    }
                }
                List<MailDestinatario> allDestinatariosMail = mailDestinatarioRepository.GetFiltered(x => x.Mail_Id == mail.Id).ToList();

                foreach (MailDestinatario destinatarioBD in allDestinatariosMail)
                {
                    if (!mailDestinatarios.Where(x => x.Id == destinatarioBD.Id).Any())
                    {
                        destinatarioBD.Hilo = false;
                        mailDestinatarioRepository.Modify(destinatarioBD, this.User.UserID);
                    }
                }

                if (mail.ArchivosModulo != null)
                {
                    foreach (ArchivoModulo newArchivo in mail.ArchivosModulo)
                    {
                        if ((archivoRepository.GetFiltered(x => x.Id == newArchivo.Id).Any() == false) && newArchivo.Deleted != true)
                        {
                            AltaArchivosModulo(newArchivo, mail.Id.ToString());
                            SetMessage(SUCCESS, "Guardado.");
                        }

                        if (newArchivo.Deleted == true)
                        {
                            archivoRepository.RemoveFromDataBase(newArchivo, this.User.UserID);
                            SetMessage(SUCCESS, "Guardado.");
                        }
                    }
                }

                string idString = mail.Id.ToString();
                List<ArchivoModulo> archivosAdjuntos = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                    && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToList();

                SendEmails(mail, mailDestinatarios, archivosAdjuntos);
                SetMessage(SUCCESS, " El Mensaje ha sido enviado correctamente.");
            }
            catch (Exception ex)
            {
                if (mail.ArchivosModulo == null)
                    mail.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                ViewBag.Destinatarios = Destinatarios;
                ViewBag.SubtituloMensaje = "Responder Mensaje";
                SetMessage(ERROR, " " + ex.Message);
                return View("Create", mail);
            }

            return Index();
        }

        public ActionResult Delete(int? id, bool ac)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Mail mail = mailRepository.Get(id.Value).SingleOrDefault();
                if (mail == null)
                {
                    return HttpNotFound();
                }

                if (ac == false)
                {
                    mailRepository.Remove(mail, this.User.UserID);
                }
                else
                {
                    Usuario currentUser = usuarioRepository.Get(this.User.UserID).FirstOrDefault();
                    MailDestinatario mailDestinatario = mailDestinatarioRepository.GetFiltered(x => x.Mail_Id == id && x.DestinatarioMail.Equals(currentUser.Email)).FirstOrDefault();

                    mailDestinatarioRepository.Remove(mailDestinatario, this.User.UserID);
                }
            }
            catch (Exception)
            {
                SetMessage(ERROR, " Ocurrió un Error al momento de querer eliminar el mensaje.");
                return Index();
            }

            return RedirectToAction("Index");
        }

        private string GetBodyHtml(string htmlString)
        {
            Regex rgx = new Regex(@"<(img)\b[^>]*>", RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(htmlString);

            string img;
            imageNames = new List<string>();
            for (int i = 0, l = matches.Count; i < l; i++)
            {
                var m = matches[i];

                string imgName = GetImageName(matches[i].Value);

                imageNames.Add(imgName);
                img = string.Format("<img src=\"cid:{0}\">", imgName);
                htmlString = htmlString.Replace(matches[i].Value, img);
            }
            return htmlString;
        }

        public string GetImageName(string imgSource)
        {
            string src = XElement.Parse(imgSource).Attribute("src").Value;
            return Path.GetFileNameWithoutExtension(src);
        }

        public ActionResult HtmlEditorPartial()
        {
            return PartialView("_HtmlEditorPartial");
        }
        public ActionResult HtmlEditorPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("HtmlEditor", ComunicacionInternaControllerHtmlEditorSettings.ImageSelectorSettings);
            return null;
        }
        public ActionResult HtmlEditorPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("HtmlEditor", ComunicacionInternaControllerHtmlEditorSettings.ImageUploadValidationSettings, ComunicacionInternaControllerHtmlEditorSettings.ImageUploadDirectory);
            return null;
        }
    }
    public class ComunicacionInternaControllerHtmlEditorSettings
    {
        public const string ImageUploadDirectory = "~/Content/UploadImages/";
        public const string ImageSelectorThumbnailDirectory = "~/Content/Thumb/";

        public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

        static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings
        {
            get
            {
                if (imageSelectorSettings == null)
                {
                    imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
                    imageSelectorSettings.Enabled = true;
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "ComunicacionInterna", Action = "HtmlEditorPartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }
    }

}
