using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Business.Entities.BaseEntities;
using CRM.Website.Crosscutting;
using CRM.Website.Security;
using DevExpress.Web.Mvc;
using CRM.Website.DevExpressHelpers;
using System.IO;
using System.Data;
using CRM.Business.Views;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Hosting;

namespace CRM.Website.Controllers
{
    public class BaseController : Controller
    {
        public string _ControllerName ;
        private string _ActionName;
        public const int ERROR     = 1;
        public const int SUCCESS = 2;
        public const int NOTICE = 2;
        public const string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

        public void SetMessage(int type, string msj)
        {
            ViewData["_TypeMessage"] = type;
            ViewData["_Message"] = msj;
        }
        protected virtual new ICustomPrincipal User
        {
            get { ICustomPrincipal user = AuthenticationFactory.CreateAuthentication().GetUser();
               // AppSession.SetUserID(User.UserID);
                //user.RolesEmpresa = AppSession.RolesEmpresa;
                //user.RolesCliente = AppSession.RolesCliente;
                return user;
            }
        }

        public virtual  IUserContext UserContext
        {
            get
            {
                try
                {
                    UserContext userContext = new UserContext();
                    userContext.UserID = User.UserID;
                    AppSession.SetUserID(User.UserID);
                    userContext.RolesEmpresa = AppSession.RolesEmpresa;
                    userContext.RolesCliente = AppSession.RolesCliente;
                    userContext.Modulos = AppSession.Modulos;
                    userContext.CurrentModulo = GetCurrenModulo();

                    //UserContext userContext = new UserContext();
                    //userContext.UserID = User.UserID;
                  
                    //userContext.RolesEmpresa = User.RolesEmpresa;
                    //userContext.CurrentModulo = GetCurrenModulo();

                    return userContext;
                }
                catch (Exception)
                {
                    UserContext userContext = new UserContext();
                    return userContext;
                }
            }
        }

        public virtual Boolean IsAdmin
        {
            get
            {
           
            bool isAdmin = false;
            if (UserContext.RolesEmpresa.Any(s => s.Rol_Id == 1))
            {
                Session["IsAdmin"] = true;
                isAdmin = true;
            }

           
                return isAdmin;
            }
        }

        public virtual Boolean IsCalidad
        {
            get
            {

                bool IsCalidad = false;
                if (UserContext.RolesEmpresa.Any(s => s.Rol_Id == 29))
                {
                    Session["IsCalidad"] = true;
                    IsCalidad = true;
                }


                return IsCalidad;
            }
        }

        public virtual Boolean IsOperador
        {
            get
            {

                bool IsOperador = false;
                if (UserContext.RolesEmpresa.Any(s => s.Rol_Id == 27))
                {
                    Session["IsOperador"] = true;
                    IsOperador = true;
                }


                return IsOperador;
            }
        }

        public virtual Boolean IsCliente
        {
            get
            {

                bool IsCliente = false;
                if (UserContext.RolesEmpresa.Any(s => s.Rol_Id == 25))
                {
                    Session["IsCliente"] = true;
                    IsCliente = true;
                }


                return IsCliente;
            }
        }

        public virtual Boolean IsProduccion
        {
            get
            {

                bool IsProduccion = false;
                if (UserContext.RolesEmpresa.Any(s => s.Rol_Id == 28))
                {
                    Session["IsOperador"] = true;
                    IsProduccion = true;
                }


                return IsProduccion;
            }
        }

        protected List<Usuario> GetUsuariosPermitidosByRolEmpresa()
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            UsuarioRolClienteRepository usuarioRolClienteRepository = new UsuarioRolClienteRepository();
            
            List<Usuario> listUsuarios = new List<Usuario>();
            List<string> allEmails = new List<string>();
            try
            {
                //if (IsAdmin)
                //    return usuarioRepository.GetFiltered(s => s.Estado == true).ToList();
                

                var thisUser = usuarioRepository.Get(User.UserID).FirstOrDefault();

                listUsuarios.Add(thisUser);

                //foreach(var rol in UserContext.RolesCliente)
                //{
                //    var listUsers = usuarioRolClienteRepository.GetFiltered(s => s.Cliente_Id == rol.Cliente_Id, i => i.UsuarioRol, m => m.UsuarioRol.Usuario)
                //        .Select(u => u.UsuarioRol.Usuario).Where(e => e.Estado == true);



                //    foreach (var user in listUsers)
                //    {
                //        if (!listUsuarios.Exists(s=> s.Id == user.Id))
                //            listUsuarios.Add(user);
                //    }

                //}

                var listUsers2 = usuarioRepository.GetFiltered(u => u.Estado == true, i => i.RolesEmpresa)
                        .Select(u => u).Where(u => u.RolesEmpresa.Any(r => r.Rol_Id != 25)).ToList();

                foreach (var user in listUsers2)
                {
                    if (!listUsuarios.Exists(s => s.Id == user.Id))
                        listUsuarios.Add(user);
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar usuario segun el perfil: " + ex.Message);
            }
            return listUsuarios;
        }

        protected List<KeyValuePair<string, string>> GetEmailsCliente()
        {
            List<KeyValuePair<string, string>> emailClientes = new List<KeyValuePair<string, string>>();
            vContactosSinFoto vContactosSinFoto = new vContactosSinFoto();

            if (!this.UserContext.RolesEmpresa.Any(x => x.Rol.RoleName.Trim().ToUpper().Equals("CLIENTE")))
            {
                
                emailClientes = (vContactosSinFoto.GetViewModel().AsEnumerable().
                    Where(c => c.Field<string>("Descripcion").Contains("Emails notificación de Sistema")).
                    Select(e => new KeyValuePair<string, string>(e.Field<string>("Cliente"), e.Field<string>("Email")))).ToList();

                emailClientes.Add(new KeyValuePair<string, string>("Todos", "Todos"));
            }

            return emailClientes;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            GetNotifications();
            GetMensajes();
            //GetEventualidades();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
          
            _ActionName = requestContext.RouteData.Values["action"].ToString();
            _ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.NameController = _ControllerName;
        }

        public ActionResult GetAuthentication()
        {
            if(this.User == null)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }

        //public Modulo GetCurrenModulo()
        //{
        //    try
        //    {

        //        if (_ControllerName == null)
        //            return null;

        //        ModuloRepository moduloRepository = new ModuloRepository();
        //        if (_ControllerName.Contains("Error"))
        //        {
        //            Modulo modulo = new Modulo();
        //            modulo.Id = -1;
        //            modulo.Descripcion = "Error";
        //            return modulo;
        //        }
        //        var result = moduloRepository.GetFiltered(o => o.URL != null && o.URL.ToUpper().StartsWith(_ControllerName.ToUpper()), p => p.Parent);

        //        var listResult = result.ToList();

        //        if (listResult.Count > 0)
        //        {
        //            foreach (var item in result.ToList())
        //            {
        //                if (item.URL.Split('/')[0].Equals(_ControllerName))
        //                    return item;
        //            }
        //        }
        //        else
        //        {
        //                throw new Exception("Error al buscar el Modulo actual en el controlador: No se encontró en la consulta");

        //        }
        //        return result.First();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al buscar el Modulo actual en el controlador:" + ex.Message);
        //    }


        //}

        public Modulo GetCurrenModulo()
        {
            try
            {

                if (_ControllerName == null)
                    return null;

                if (_ControllerName.Contains("Error"))
                {
                    Modulo modulo = new Modulo();
                    modulo.Id = -1;
                    modulo.Descripcion = "Error";
                    return modulo;
                }
                var result = AppSession.Modulos.Where(o => o.URL != null && o.URL.ToUpper().StartsWith(_ControllerName.ToUpper())).ToList();

            //    var listResult = result.ToList();

                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        if (item.URL.Split('/')[0].Equals(_ControllerName))
                            return item;
                    }
                }
                else
                {
                    throw new Exception("Error al buscar el Modulo actual en el controlador: No se encontró en la consulta");

                }
                return result.First();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el Modulo actual en el controlador:" + ex.Message);
            }


        }



        public void GetNotifications()
        {
            NovedadUsuariosRepository novedadUsuariosRepository = new NovedadUsuariosRepository();
            NovedadRepository novedadRepository = new NovedadRepository();
            List<NovedadUsuarios> novedadesUsuario = new List<NovedadUsuarios>();

            if (this.User != null)
            {
                novedadesUsuario = novedadUsuariosRepository.GetFiltered(x => x.Usuario_Id == this.User.UserID && x.Visto == false).ToList();
            }
            List<Novedad> notificaciones = new List<Novedad>();

            foreach (NovedadUsuarios novedadUsuario in novedadesUsuario)
            {
                Novedad novedad = novedadRepository.Get(novedadUsuario.Novedad_Id).FirstOrDefault();
                if (novedad != null && novedad.Estado == true)
                    notificaciones.Add(novedad);
            }

            ViewBag.Notificaciones = notificaciones;
        }

        public void GetEventualidades()
        {
            EventualidadUsuarioRepository eventualidadUsuarioRepository = new EventualidadUsuarioRepository();
            EventualidadRepository eventualidadRepository = new EventualidadRepository();
            List<EventualidadUsuario> eventualidadesUsuario = new List<EventualidadUsuario>();

            if (this.User != null)
            {
                eventualidadesUsuario = eventualidadUsuarioRepository.GetFiltered(x => x.Usuario_Id == this.User.UserID && x.Visto == false).ToList();
            }
            List<Eventualidad> eventualidades = new List<Eventualidad>();

            foreach (EventualidadUsuario eventUsuario in eventualidadesUsuario)
            {
                Eventualidad eventualidad = eventualidadRepository.Get(eventUsuario.Eventualidad_Id).FirstOrDefault();
                if (eventualidad != null && eventualidad.Estado == true)
                    eventualidades.Add(eventualidad);
            }

            ViewBag.Eventualidades = eventualidades;
        }

        public void GetNoConformidades()
        {
            NoConformidadRepository eventualidadRepository = new NoConformidadRepository();
            List<NoConformidad> eventualidades = new List<NoConformidad>();

            NoConformidad eventualidad = (NoConformidad)eventualidadRepository.GetFiltered(x => x.Estado == true);
           eventualidades.Add(eventualidad);

            ViewBag.Eventualidades = eventualidades;
        }

        public void GetMensajes()
        {
            MailDestinatarioRepository mailDestinatarioRepository = new MailDestinatarioRepository();
            MailRepository mailRepository = new MailRepository();
            List<MailDestinatario> mailDestinatarios = new List<MailDestinatario>();
            UsuarioRepository usuarioRepository = new UsuarioRepository();

            if (this.User != null)
            {
                Usuario currentUser = usuarioRepository.Get(this.User.UserID).FirstOrDefault();
                //mailDestinatarios = mailDestinatarioRepository.GetFiltered(x => x.DestinatarioMail == currentUser.Email.Replace(";", ",") && x.Visto == false).ToList();
                mailDestinatarios = mailDestinatarioRepository.GetFiltered(x =>
                (x.Destinatario.Contains(currentUser.NombreApellido)) && x.Visto == false).ToList();
            }
            List<Mail> mailsNoLeidos = new List<Mail>();

            foreach (MailDestinatario mailDestinatario in mailDestinatarios)
            {
                Mail mail = mailRepository.Get(mailDestinatario.Mail_Id).FirstOrDefault();
                if (mail != null && mail.Estado == true)
                    mailsNoLeidos.Add(mail);
            }

            ViewBag.MensajesNoLeidos = mailsNoLeidos;
        }

        public void AltaArchivosModulo(ArchivoModulo newArchivo, string entityId)
        {
            ArchivoModuloRepository objectRepository = new ArchivoModuloRepository();

            //Consigo el objeto TipoArchivoModulo
            TipoArchivoModuloRepository tipoArchivoModulo = new TipoArchivoModuloRepository();
            TipoArchivoModulo tipoArchivo = tipoArchivoModulo.GetFiltered(x => x.Tipo.ToLower().Trim().Equals(newArchivo.NombreTipoArchivoModulo.ToLower().Trim())).First();

            newArchivo.TipoArchivoModulo_Id = tipoArchivo.Id;

            //string oldFileName = Server.MapPath("~/Content/TempFiles/") + newArchivo.Path;
            string oldFileName = HostingEnvironment.MapPath("~/Content/TempFiles/") + newArchivo.Path;
            //if (newArchivo.Id != 0)
            //{
            //    oldFileName = Server.MapPath("~/Content/Files/") + newArchivo.Path;
            //}
            //else
            //{
            //    //obtengo la url del archivo temporal
            //    oldFileName = Server.MapPath("~/Content/TempFiles/") + newArchivo.Path;
            //}

            string extension = Path.GetExtension(newArchivo.Path);
            string newName = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString()
                                + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00")
                                + "_T" + newArchivo.TipoArchivoModulo_Id + extension;

            //string newFileName = Server.MapPath("~/Content/Files/") + newName;
            string newFileName = HostingEnvironment.MapPath("~/Content/Files/") + newName;

            // Muevo el archivo de la carpeta temporal a la carpeta donde se alojan los archivos permanentes
            System.IO.File.Move(oldFileName, newFileName);
            //Elimino el archivo de la carpeta temporal
            System.IO.File.Delete(oldFileName);

            newArchivo.Path = newName;

            newArchivo.Entity_Id = entityId;

            objectRepository.Add(newArchivo, this.User.UserID);
        }

        public void CopyFileToTempFolder(ArchivoModulo oldArchivo)
        {
            string savedFileName = Server.MapPath("~/Content/Files/") + oldArchivo.Path;
            string tempFileName = Server.MapPath("~/Content/TempFiles/") + oldArchivo.Path;

            try
            {
                System.IO.File.Copy(savedFileName, tempFileName);
            }
            catch (DirectoryNotFoundException)
            { }
        }

        public void ClearTempFolder()
        {
            try
            {
                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Content/TempFiles/"));

                foreach (string filePath in filePaths)
                    System.IO.File.Delete(filePath);
            }
            catch (DirectoryNotFoundException)
            {

            }
        }

        public DataTable GetClientesFromUsuario(int idUsuario, bool OnlyOne)
        {
            //RolEmpresaRepository rolEmpresaRepository = new RolEmpresaRepository();
            //UsuarioRolClienteRepository usuarioRolClienteRepository = new UsuarioRolClienteRepository();
            //vClientes vClientes = new Business.Views.vClientes();

            //List<RolEmpresa> rolesUsuario = rolEmpresaRepository.GetFiltered(x => x.Usuario_Id == idUsuario
            //                                    && x.Rol.Estado == true && x.Usuario.Estado == true, x => x.Rol).ToList();

            //bool isAdmin = false;
            //List<UsuarioRolCliente> userRolClient = new List<UsuarioRolCliente>();
            //foreach (RolEmpresa rol in rolesUsuario)
            //{
            //    List<UsuarioRolCliente> clientesRol = usuarioRolClienteRepository.GetFiltered(s => s.UsuarioRol.Rol_Id == rol.Rol_Id
            //                                            && s.UsuarioRol.Usuario_Id == idUsuario && s.Estado == true).ToList();

            //    if (rol.Rol.Id == (int)RolRepository.SystemROL.ADMIN)
            //        isAdmin = true;

            //    foreach (UsuarioRolCliente clienteRol in clientesRol)
            //    {
            //        userRolClient.Add(clienteRol);
            //    }
            //}

            //if (isAdmin)
            //    Session["IsAdmin"] = true;
            //else
            //    Session["IsAdmin"] = false;

            //foreach (var item in userRolClient)
            //{
            //    var listRow = (new vClientes()).GetViewModel().AsEnumerable()
            //            .Where(r => r.Field<int>("ID") == item.Cliente_Id).ToList();
            //    foreach (DataRow dr in listRow)
            //    {
            //        vClientes.Datos.Rows.Add(dr.ItemArray);
            //        if(OnlyOne == true)
            //            break;
            //    }
            //    if (OnlyOne == true)
            //        break;
            //}

            var vClientes = new vClientes();
            if (IsAdmin)
                vClientes = new Business.Views.vClientes();
            else
                vClientes = new Business.Views.vClientes(UserContext.RolesCliente.ToList());

            return vClientes.Datos;
        }

        public bool IsEmail(string email)
        {
            if (email != null) return System.Text.RegularExpressions.Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }

        

    }
}
