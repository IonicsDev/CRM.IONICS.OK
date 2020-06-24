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
using CRM.Website.Models;
using CRM.Website.Security;
using CRM.Website.Security.Infrastructure;
using DevExpress.Web;
using CRM.Website.Crosscutting;
using CRM.Business.Views;

namespace CRM.Website.Controllers
{
    public class UsuarioController : GenericController<Usuario>
    {
      
        private UsuarioRepository usuarioRepository = new UsuarioRepository();
        private UsuarioRolClienteRepository usuarioRolClienteRepository = new UsuarioRolClienteRepository();

      
        [HttpGet]
        [AllowAnonymous()]
        public ActionResult Login()
        {        
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel usuarioViewModel)
        {
           
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario();
                usuario.UserName = usuarioViewModel.UserName;
                usuario.Password = usuarioViewModel.Password;
                usuario.Recordarme = usuarioViewModel.Recordarme;

                if (usuarioRepository.IsValid(usuario))
                {
                    var user = usuarioRepository.GetUserByUserName(usuario.UserName);

                    if (user.RolesEmpresa.Any(i => i.Rol_Id == 25))
                    {
                        if (!usuarioRepository.IsClienteActivo(usuario))
                        {
                            //vClientes cliente = new vClientes();
                            //string filtro = string.Format("Cuit = '{0}'", user.UserName);
                            //var dtcliente = cliente.GetByFilter(filtro);
                            //string rs = dtcliente.Rows[0][1].ToString();
                            string aviso = string.Format(" El Cliente {0} se encuentra inactivo, solicite su activacion", user.NombreApellido);
                            ModelState.AddModelError("", aviso);
                        }
                        else if (user.CambiarPass != true)
                        {
                            var authenticationService = AuthenticationFactory.CreateAuthentication();

                            var serializeModel = new CustomPrincipalSerializeModel();
                            serializeModel.UserID = user.Id;
                            serializeModel.FirstName = user.NombreApellido;
                            serializeModel.UserName = user.UserName;


                            var serializer = new JavaScriptSerializer();
                            var userData = serializer.Serialize(serializeModel);

                            authenticationService.Login(user.UserName, user.Password, usuario.Recordarme, userData);

                            user.FechaUltimoAcceso = DateTime.Now;
                            usuarioRepository.Modify(user, user.Id);
                            AppSession.Init_Session(user.Id);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return UsuarioCambioPassword(usuario);
                        }
                    }
                    else if (user.CambiarPass != true)
                    {
                        var authenticationService = AuthenticationFactory.CreateAuthentication();

                        var serializeModel = new CustomPrincipalSerializeModel();
                        serializeModel.UserID = user.Id;
                        serializeModel.FirstName = user.NombreApellido;
                        serializeModel.UserName = user.UserName;
                       
                     
                        var serializer = new JavaScriptSerializer();
                        var userData = serializer.Serialize(serializeModel);

                        authenticationService.Login(user.UserName, user.Password, usuario.Recordarme, userData);

                        user.FechaUltimoAcceso = DateTime.Now;
                        usuarioRepository.Modify(user, user.Id);
                        AppSession.Init_Session(user.Id);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return UsuarioCambioPassword(usuario);
                    }
                }
                else
                {
                   ModelState.AddModelError("", "Error al validar datos ingresados, intente nuevamente");
                }
              
            }
            return View(usuarioViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult UsuarioCambioPassword(Usuario usuario)
        {
            ChangePasswordModel changePasswordModel = new ChangePasswordModel();
            changePasswordModel.UserName = usuario.UserName;

            ViewBag.FirstTime = true;

            return View("UsuarioCambioPassword", changePasswordModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult UsuarioCambioPassword([Bind(Include = "UserName,Password,NuevaPassword,NuevaPasswordAgain")]ChangePasswordModel changeUsuario)
        {
            ViewBag.FirstTime = null;
            if (ModelState.IsValid)
            {
                Usuario user = new Usuario();
                user.UserName = changeUsuario.UserName;
                user.Password = changeUsuario.Password;

                if (usuarioRepository.IsValid(user))
                {
                    if (changeUsuario.NuevaPassword.Trim().Equals(changeUsuario.NuevaPasswordAgain.Trim()))
                    {
                        if (changeUsuario.NuevaPassword.Length >= 4)
                        {
                            var usuario = usuarioRepository.GetUserByUserName(changeUsuario.UserName);

                            usuario.Password = changeUsuario.NuevaPassword;
                            usuario.FechaUltimoAcceso = DateTime.Now;
                            usuario.CambiarPass = false;
                            
                            usuarioRepository.Modify(usuario, user.Id);

                            var authenticationService = AuthenticationFactory.CreateAuthentication();

                            var serializeModel = new CustomPrincipalSerializeModel();
                            serializeModel.UserID = usuario.Id;
                            serializeModel.FirstName = usuario.NombreApellido;
                            serializeModel.UserName = usuario.UserName;

                            var serializer = new JavaScriptSerializer();
                            var userData = serializer.Serialize(serializeModel);

                            authenticationService.Login(usuario.UserName, usuario.Password, usuario.Recordarme, userData);

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "La nueva Password debe contener más de 3 caracteres");
                        }
                    }
                    else 
                    {
                        ModelState.AddModelError("", "Las Password no coinciden");
                    }
                }
                else
                { 
                   ModelState.AddModelError("", "Error al validar datos ingresados, intente nuevamente");
                }
            }
            else 
            {
                changeUsuario.Password = null;
                changeUsuario.NuevaPassword = null;
                changeUsuario.NuevaPasswordAgain = null;
            }

            return View("UsuarioCambioPassword", changeUsuario);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            AuthenticationFactory.CreateAuthentication().Logout();
            return RedirectToAction("Login", "Usuario");
        }
       
        public ActionResult GridViewAllPartial()
        {
            return PartialView("_GridViewAllPartial", usuarioRepository.GetAll().Where(s => !s.Email.Contains("ionics.com.ar")).ToList());
        }
        
        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult Index()
        {
            ClearTempFolder();

            List<Usuario> listUsuarios = new List<Usuario>();
            IQueryable<Usuario> usuarios;

            if (base.UserContext.RolesEmpresa.Any(s => s.Rol_Id == 1))
            {
                usuarios = usuarioRepository.GetAll();
            }
            else
            {
                usuarios = usuarioRepository.GetFiltered(s => s.RolesEmpresa.Any(a => a.Rol_Id == 25), i => i.RolesEmpresa);

                usuarios = usuarios.Where(s => !s.Email.Contains("ionics.com.ar"));

            }


            ViewBag.ObjectModel = usuarios;

            if (usuarios.Count() > 0)
            {
                return View("Index", usuarios.ToList());
            }
            else
            {
                return View("Index", listUsuarios);
            }
           
        }

        public ActionResult UsuariosPartial()
        {
            return PartialView("_GridParcial", usuarioRepository.GetAll().ToList());
        }

        [HttpGet]
        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = usuarioRepository.Get(id.Value).SingleOrDefault();

            if (usuario == null)
            {
                return HttpNotFound();
            }
            string idString = id.ToString();
            ArchivoModuloRepository archivoRepository = new ArchivoModuloRepository();
            usuario.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                                                        && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

            return View(usuario);
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            Usuario newUsuario = new Usuario();
            newUsuario.ArchivosModulo = new List<ArchivoModulo>().ToArray();
            return View(newUsuario);
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
                SetMessage(ERROR, "No se pudo guardar el archivo: " + ex.Message);
            }

            return name;
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NombreApellido,UserName,Password,ArchivosModulo,CambiarPass,FechaUltimoAcceso,Email")] Usuario usuario)
        {
            try
            {
                if (usuario.ArchivosModulo == null)
                    usuario.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                if (ModelState.IsValid)
                {
                    if (usuarioRepository.GetFiltered(x => x.UserName.ToLower().Trim().Equals(usuario.UserName.ToLower().Trim())).Any())
                    {
                        ModelState.AddModelError(string.Empty, "El Nombre de Usuario ya ha sido utilizado!");
                        return View(usuario);
                    }
                    else
                    {
                        if (usuario.Password.Length >= 4)
                        {
                            usuarioRepository.Add(usuario, this.User.UserID);

                            if (usuario.ArchivosModulo != null)
                            {
                                foreach (ArchivoModulo newArchivo in usuario.ArchivosModulo)
                                {
                                    if (newArchivo.Deleted == false)
                                    {
                                        AltaArchivosModulo(newArchivo, usuario.Id.ToString());
                                    }
                                }
                            }
                            SetMessage(SUCCESS, "Guardado.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "La Password debe contener más de 3 caracteres");
                            return View(usuario);
                        }
                    }
                }
                else
                {
                    return View(usuario);
                }
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, "Error al guardar usuario :" + ex.Message);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return View(usuario);
            }

            return Index();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (User.UserID != 1157) { 
            if (!IsAdmin)
            {
                if (id.Value != User.UserID)
                {
                       SetMessage(ERROR, " Error al intentar modificar un usuario sin permisos.");
                      ModelState.AddModelError("", "No tiene permisis para editar usuarios");
                      return RedirectToAction("Index", "Home");
                }

            }
            }
            Usuario usuario = usuarioRepository.Get(id.Value).SingleOrDefault();

            if (User.UserID == 1157 && usuario.Email.Contains("@ionics.com.ar"))
            {
                SetMessage(ERROR, " Error al intentar modificar un usuario sin permisos.");
                ModelState.AddModelError("", "No tiene permisis para editar usuarios");
                return RedirectToAction("Index", "Home");
            }

            if (usuario == null)
            {
                return HttpNotFound();
            }
            string idString = id.ToString();
            ArchivoModuloRepository archivoRepository = new ArchivoModuloRepository();
            usuario.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true 
                                                                        && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NombreApellido,UserName,Password,Estado,FechaCreacion,idUsuario,ArchivosModulo,CambiarPass,FechaUltimoAcceso,Email")]Usuario usuario)
        {
            try
            {
                if (!IsAdmin && User.UserID!= 1157) 
                {
                    if (usuario.Id != User.UserID)
                    {
                        SetMessage(ERROR, " Error al intentar modificar un usuario sin permisos.");
                        ModelState.AddModelError("", "No tiene permisis para editar usuarios");
                        return RedirectToAction("Index", "Home");
                    }

                }

                if (usuario.ArchivosModulo == null)
                    usuario.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                if (ModelState.IsValid)
                {
                    if (usuarioRepository.GetFiltered(x => x.UserName.ToLower().Trim().Equals(usuario.UserName.ToLower().Trim()) && x.Id != usuario.Id).Any())
                    {
                        ModelState.AddModelError(string.Empty, "El Nombre de Usuario ya ha sido utilizado!");
                        return View(usuario);
                    }
                    else
                    {
                        if (usuario.Password.Length >= 4)
                        {
                            string idString = usuario.Id.ToString();
                            ArchivoModuloRepository archivoModuloRepository = new ArchivoModuloRepository(this.UserContext);
                            List<ArchivoModulo> list = archivoModuloRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true).ToList();

                            if (usuario.ArchivosModulo != null)
                            {
                                foreach (ArchivoModulo archivoModulo in usuario.ArchivosModulo)
                                {
                                    if ((archivoModuloRepository.GetFiltered(x => x.Id == archivoModulo.Id).Any() == false) && archivoModulo.Deleted != true)
                                    {
                                        AltaArchivosModulo(archivoModulo, usuario.Id.ToString());
                                    }

                                    if (archivoModulo.Deleted == true)
                                    {
                                        archivoModuloRepository.Remove(archivoModulo, this.User.UserID);
                                    }
                                }
                            }

                            usuarioRepository.Modify(usuario, this.User.UserID);
                            SetMessage(SUCCESS, "Guardado.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "La Password debe contener más de 4 caracteres");
                            return View(usuario);
                        }
                    }
                }
                else
                {
                    return View(usuario);
                }

                TipoArchivoModuloRepository tipoArchivoModulo = new TipoArchivoModuloRepository();
                TipoArchivoModulo tipoArchivo = tipoArchivoModulo.GetFiltered(x => x.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim())).First();

                if (usuario.ArchivosModulo != null)
                {
                    foreach (ArchivoModulo archivo in usuario.ArchivosModulo)
                    {
                        archivo.TipoArchivoModulo = tipoArchivo;
                    } 
                }
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, " Error al guardar usuario :" + ex.Message);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return View(usuario);
            }
            if(!IsAdmin)
                return RedirectToAction("Index", "Home");

            return Index();
        }

        [HttpGet]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Usuario usuario = usuarioRepository.Get(id.Value).SingleOrDefault();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            usuarioRepository.Remove(usuario, this.User.UserID);

            return RedirectToAction("Index");
        }
                
    }
}
