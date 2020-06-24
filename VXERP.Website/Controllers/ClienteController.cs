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
    public class ClienteController : BaseController
    {
        ArchivoModuloRepository archivoRepository = new ArchivoModuloRepository();

        public vClientes vClientes = new vClientes();

      
        public ActionResult Index()
        {
            if (IsAdmin)
                vClientes = new Business.Views.vClientes();
            else
                  vClientes = new Business.Views.vClientes(base.UserContext.RolesCliente.ToList());

            ClearTempFolder();
            return View(vClientes);
        }

        public ActionResult Refresh()
        {
            System.Web.HttpContext.Current.Session["RolesEmpresa"] = null;
            System.Web.HttpContext.Current.Session["UserRolesCliente"] = null;

            if (IsAdmin)
                vClientes = new Business.Views.vClientes();
            else
                vClientes = new Business.Views.vClientes(base.UserContext.RolesCliente.ToList());

            ClearTempFolder();

            return View("Index",vClientes);
        }

        [HttpPost]
        public ActionResult ExportXLS(GridViewExportedRowType exportRowType)
        {
            if (IsAdmin)
                vClientes = new Business.Views.vClientes();
            else
                vClientes = new Business.Views.vClientes(base.UserContext.RolesCliente.ToList());

            var setting = GridHelper.GetSettingExport(vClientes.GetDynamicCollectionList(vClientes.GetViewModel()), _ControllerName);
            setting.SettingsExport.ExportedRowType = exportRowType;
            //setting.SettingsExport.ExportSelectedRowsOnly = true;
            // retornamos el excel al usuario
            return GridViewExtension.ExportToXls(setting, vClientes.GetViewModel(), string.Format("{0}s_{1}.{2}", typeof(vClientes).Name, DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls"));
        }

        public ActionResult GridViewAllPartial()
        {
            if (IsAdmin)
                vClientes = new Business.Views.vClientes();
            else
                vClientes = new Business.Views.vClientes(base.UserContext.RolesCliente.ToList());
            return PartialView("_GridForViews", vClientes);
        }

       
       
        public ActionResult View(int id)
        {
            if (IsAdmin)
                vClientes = new Business.Views.vClientes();
            else
                vClientes = new Business.Views.vClientes(base.UserContext.RolesCliente.ToList());
            try
            {
               

                DataTable datos = vClientes.GetByID(id);
                vClientes.Datos = datos;
                vClientes.Id = id;

                string idString = id.ToString();
                ViewBag.Archivos = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                                                            && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

                return View("FormViewFicha", vClientes);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, " ERROR: " + ex.Message);
                return View("Index", vClientes);
            }
        }

        [HttpPost]
     
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
    }
}
