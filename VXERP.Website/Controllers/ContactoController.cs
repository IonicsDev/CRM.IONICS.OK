using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Business.Views;
using CRM.Website.DevExpressHelpers;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CRM.Website.Security.Infrastructure;

namespace CRM.Website.Controllers
{
    public class ContactoController : BaseController
    {
        ArchivoModuloRepository archivoRepository = new ArchivoModuloRepository();

        public vContactos vContactos = new vContactos();
        public vContactosSinFoto vContactosSinFoto = new vContactosSinFoto();
        public vClientes vClientes = new vClientes();
        public vGuardarContacto vGuardarContacto = new vGuardarContacto();
        //public vEliminarContacto vEliminarContacto = new vEliminarContacto();
        ArchivoModuloRepository archivoModuloRepository = new ArchivoModuloRepository();
        public vDesactivarContacto vDesactivarContacto = new vDesactivarContacto();
        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult Index()
        {
            ClearTempFolder();

            if  (base.UserContext.RolesEmpresa.Any(s => s.Rol_Id == 1) && base.UserContext.RolesCliente.Count == 0)
            {
                vContactosSinFoto = vContactosSinFoto.GetAll();
            }
            else{

                 vContactosSinFoto = vContactosSinFoto.GetByUserRol(base.UserContext.RolesCliente.ToList());
            }
           

            vContactosSinFoto.Datos.Columns.Add("CodCliente", typeof(GridViewDataHyperLinkColumn)).SetOrdinal(1);

            foreach (DataRow row in vContactosSinFoto.Datos.Rows)
            {
                if (row["CodigoCliente"].ToString() != "")
                {
                    int idCliente = Convert.ToInt32(row["CodigoCliente"].ToString());
                    GridViewDataHyperLinkColumn link = new GridViewDataHyperLinkColumn();
                    link.FieldName = row["Cliente"].ToString();
                    row["CodCliente"] = link;
                }
            }
         
            return View("Index", vContactosSinFoto);
        }

        [HttpPost]
        public ActionResult ExportXLS(GridViewExportedRowType exportRowType)
        {
        
           vContactosSinFoto = vContactosSinFoto.GetByUserRol(base.UserContext.RolesCliente.ToList());
          

            var setting = GridHelper.GetSettingExport(vContactosSinFoto.GetDynamicCollectionList(vContactosSinFoto.Datos), _ControllerName);
            setting.SettingsExport.ExportedRowType = exportRowType;

            // retornamos el excel al usuario
            return GridViewExtension.ExportToXls(setting, vContactosSinFoto.Datos, string.Format("{0}s_{1}.{2}", typeof(vContactos).Name, DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls"));
        }

        public ActionResult GridViewAllPartial()
        {
            vContactosSinFoto = vContactosSinFoto.GetByUserRol(base.UserContext.RolesCliente.ToList());


            vContactosSinFoto.Datos.Columns.Add("CodCliente", typeof(GridViewDataHyperLinkColumn)).SetOrdinal(1);

            foreach (DataRow row in vContactosSinFoto.Datos.Rows)
            {
                if (row["CodigoCliente"].ToString() != "")
                {
                    int idCliente = Convert.ToInt32(row["CodigoCliente"].ToString());
                    GridViewDataHyperLinkColumn link = new GridViewDataHyperLinkColumn();
                    link.FieldName = row["Cliente"].ToString();
                    row["CodCliente"] = link;
                }
            }
           
            return PartialView("_GridForViews", vContactosSinFoto);
        }

        [LogonAuthorize(Roles = "VIEW")]
        public ActionResult View(int id)
        {
            try
            {
                DataTable datos = vContactos.GetByID(id);
                vContactos.Datos = datos;
                vContactos.Id = id;

                if (vContactos.Datos.Rows.Count == 0)
                    throw new Exception(" El Contacto no existe.");

                if(!base.IsAdmin)
                if(!vContactos.HasPerm(vContactos.Datos,base.UserContext.RolesCliente.ToList()))
                    throw new Exception(" No tiene permiso para ver este contacto");

                string idString = id.ToString();
                ViewBag.Archivos = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                                                            && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

                return View("FormViewFicha", vContactos);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                return Index();
            }
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult View(int id, ArchivoModulo[] ArchivosModulo)
        {
            try
            {
                if (ArchivosModulo != null)
                {
                    foreach (ArchivoModulo archivoModulo in ArchivosModulo)
                    {
                        if ((archivoRepository.GetFiltered(x => x.Id == archivoModulo.Id).Any() == false) && archivoModulo.Deleted != true)
                        {
                            AltaArchivosModulo(archivoModulo, id.ToString());
                            SetMessage(SUCCESS, "Guardado.");
                        }

                        if (archivoModulo.Deleted == true)
                        {
                            archivoRepository.Remove(archivoModulo, this.User.UserID);
                            SetMessage(SUCCESS, "Guardado.");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, " Error al guardar: " + ex.Message);
            }

            return View(id);
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

        [Authorize(Roles = "EDIT")]
        public ActionResult Create()
        {
            ViewBag.Clientes = GetListadoClientes();

            Contacto contacto = new Contacto();
            contacto.ArchivosModulo = new List<ArchivoModulo>().ToArray();
            return View(contacto);
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Create(Contacto contacto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = GetListadoClientes();

                if (contacto.ArchivosModulo == null)
                    contacto.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                if (contacto.Foto != null)
                {
                    if (contacto.Foto.ContentType.ToLower() != "image/jpg" && contacto.Foto.ContentType.ToLower() != "image/jpeg" &&
                                contacto.Foto.ContentType.ToLower() != "image/pjpeg" && contacto.Foto.ContentType.ToLower() != "image/gif" &&
                                contacto.Foto.ContentType.ToLower() != "image/x-png" && contacto.Foto.ContentType.ToLower() != "image/png")
                    {
                        ModelState.AddModelError("ErrorImage", "El archivo seleccionado no es una imagen");
                    }
                    else
                    {
                        MemoryStream target = new MemoryStream();
                        contacto.Foto.InputStream.CopyTo(target);
                        contacto.FotoByte = target.ToArray();
                    }
                }
               // return View(contacto);
            }

            try
            {
                DataTable dtResult = vGuardarContacto.GuardarContacto(contacto, this.User.UserName);

                if (dtResult != null)
                {
                    int idContacto = Convert.ToInt32(dtResult.Rows[0].ItemArray[0].ToString());
                    contacto.Id = idContacto;

                    if (contacto.ArchivosModulo != null)
                    {
                        foreach (ArchivoModulo newArchivo in contacto.ArchivosModulo)
                        {
                            if (newArchivo.Deleted == false)
                            {
                                AltaArchivosModulo(newArchivo, contacto.Id.ToString());
                            }
                        }
                    }
                }

                SetMessage(SUCCESS, " Guardado.");
            }
            catch (Exception ex)
            {
                if (contacto.ArchivosModulo == null)
                    contacto.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                ViewBag.Clientes = GetListadoClientes();
                SetMessage(ERROR, " " + ex.Message);
                return View(contacto);
            }

            return Index();
        }

        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(int id)
        {
            try
            {
                DataTable datos = vContactos.GetByID(id);
                vContactos.Datos = datos;
                vContactos.Id = id;

                if (vContactos.Datos.Rows.Count == 0)
                    throw new Exception(" El Contacto no existe.");


                if(!base.IsAdmin)
                if (!vContactos.HasPerm(vContactos.Datos, base.UserContext.RolesCliente.ToList()))
                    throw new Exception(" No tiene permiso para ver este contacto");

                string[] descripcion = vContactos.Datos.Rows[0]["Descripcion"].ToString().Trim().Split(' ');

                string apellido = "";
                for (int i = 0; i < descripcion.Length; i++)
                {
                    if (i > 0)
                        apellido += " " + descripcion[i].ToString();
                }

                Contacto contacto = new Contacto();
                contacto.Id = vContactos.Id;
                contacto.CodigoCliente = Convert.ToInt32(vContactos.Datos.Rows[0]["CodigoCliente"].ToString());
                contacto.Nombre = descripcion[0].ToString();
                contacto.Apellido = apellido.Trim().ToString();
                contacto.Cargo = vContactos.Datos.Rows[0]["Cargo"].ToString();
                contacto.Telefono = vContactos.Datos.Rows[0]["Telefono"].ToString();
                contacto.Movil = vContactos.Datos.Rows[0]["Movil"].ToString();
                if (vContactos.Datos.Rows[0]["Interno"].ToString() != "" && vContactos.Datos.Rows[0]["Interno"].ToString() != null)
                    contacto.Interno = Convert.ToInt32(vContactos.Datos.Rows[0]["Interno"].ToString());
                contacto.Email = vContactos.Datos.Rows[0]["Email"].ToString();
                if (vContactos.Datos.Rows[0]["DNI"].ToString() != "" && vContactos.Datos.Rows[0]["DNI"].ToString() != null)
                    contacto.Dni = Convert.ToInt32(vContactos.Datos.Rows[0]["DNI"].ToString());
                if (vContactos.Datos.Rows[0]["Foto"].ToString() != "")
                    contacto.FotoByte = (byte[])vContactos.Datos.Rows[0]["Foto"];

                string idString = id.ToString();
                contacto.ArchivosModulo = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                                                            && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

                ViewBag.Clientes = GetListadoClientes();

                return View("Edit", contacto);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
                return Index();
            }
        }

        [HttpPost]
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Edit(Contacto contacto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = GetListadoClientes();

                if (contacto.ArchivosModulo == null)
                    contacto.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                if (contacto.Foto != null)
                {
                    if (contacto.Foto.ContentType.ToLower() != "image/jpg" && contacto.Foto.ContentType.ToLower() != "image/jpeg" &&
                                contacto.Foto.ContentType.ToLower() != "image/pjpeg" && contacto.Foto.ContentType.ToLower() != "image/gif" &&
                                contacto.Foto.ContentType.ToLower() != "image/x-png" && contacto.Foto.ContentType.ToLower() != "image/png")
                    {
                        ModelState.AddModelError("ErrorImage", "El archivo seleccionado no es una imagen");
                    }
                    else
                    {
                        MemoryStream target = new MemoryStream();
                        contacto.Foto.InputStream.CopyTo(target);
                        contacto.FotoByte = target.ToArray();
                    }
                }
               // return View(contacto);
            }

            try
            {
                DataTable datos = vContactos.GetByID(contacto.Id);
                vContactos.Datos = datos;
                if(vContactos.Datos.Rows[0]["Foto"].ToString() != string.Empty)
                  contacto.FotoByte = (byte[])vContactos.Datos.Rows[0]["Foto"];

                DataTable dtResult = vGuardarContacto.GuardarContacto(contacto, this.User.UserName);

                if (dtResult != null)
                {
                    if (contacto.ArchivosModulo != null)
                    {
                        foreach (ArchivoModulo archivoModulo in contacto.ArchivosModulo)
                        {
                            if ((archivoModuloRepository.GetFiltered(x => x.Id == archivoModulo.Id).Any() == false) && archivoModulo.Deleted != true)
                            {
                                AltaArchivosModulo(archivoModulo, this.User.UserID.ToString());
                            }

                            if (archivoModulo.Deleted == true)
                            {
                                archivoModuloRepository.Remove(archivoModulo, this.User.UserID);
                            }
                        }
                    }
                }

                SetMessage(SUCCESS, " Guardado.");
            }
            catch (Exception ex)
            {
                if (contacto.ArchivosModulo == null)
                    contacto.ArchivosModulo = new List<ArchivoModulo>().ToArray();

                ViewBag.Clientes = GetListadoClientes();
                SetMessage(ERROR, " " + ex.Message);
                return View(contacto);
            }

            return Index();
        }

        public List<KeyValuePair<int, string>> GetListadoClientes()
        {
            DataTable dtClientes =  new DataTable();
            if (!base.IsAdmin)
                dtClientes = vClientes.GetByUserRol(base.UserContext.RolesCliente.ToList()).Datos;
            else
                dtClientes = vClientes.GetViewModel();

            List<KeyValuePair<int, string>> lsClientes = new List<KeyValuePair<int, string>>();

            foreach (DataRow row in dtClientes.Rows)
            {
                int idCliente = Convert.ToInt32(row["ID"].ToString());
                string desCliente = row["Descripcion"].ToString();
                lsClientes.Add(new KeyValuePair<int, string>(idCliente, desCliente));
            }

            return lsClientes.ToList();
        }

      
        [LogonAuthorize(Roles = "EDIT")]
        public ActionResult Delete(int id)
        {
            try
            {
                DataTable datos = vContactos.GetByID(id);
                vContactos.Datos = datos;
                vContactos.Id = id;

                if (vContactos.Datos.Rows.Count == 0)
                    throw new Exception(" El Contacto no existe.");

                if(!base.IsAdmin)
                if (!vContactos.HasPerm(vContactos.Datos, base.UserContext.RolesCliente.ToList()))
                    throw new Exception(" No tiene permiso para ver este contacto");

                //vEliminarContacto.EliminarContacto(id);
                vDesactivarContacto.DesactivarContacto(id);
                SetMessage(SUCCESS," El Contacto se desactivó correctamente.");
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, ex.Message);
            }

            return Index();
        }
    }
}
