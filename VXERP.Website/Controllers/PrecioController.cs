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
    public class PrecioController : BaseController
    {
        ArchivoModuloRepository archivoRepository = new ArchivoModuloRepository();

        public vPrecios vPrecios = new vPrecios();

         [LogonAuthorize(Roles = "VIEW")]
        public ActionResult Index()
        {
            if (IsAdmin)
                vPrecios = new Business.Views.vPrecios();
            else
                vPrecios = new Business.Views.vPrecios(base.UserContext.RolesCliente.ToList());

            ClearTempFolder();
            return View(vPrecios);
        }


        [HttpPost]
        public ActionResult ExportXLS(GridViewExportedRowType exportRowType)
        {
            if (IsAdmin)
                vPrecios = new Business.Views.vPrecios();
            else
                vPrecios = new Business.Views.vPrecios(base.UserContext.RolesCliente.ToList());

            var setting = GridHelper.GetSettingExport(vPrecios.GetDynamicCollectionList(vPrecios.GetViewModel()), _ControllerName);
            setting.SettingsExport.ExportedRowType = exportRowType;
    
            // retornamos el excel al usuario
            return GridViewExtension.ExportToXls(setting, vPrecios.GetViewModel(), string.Format("{0}s_{1}.{2}", typeof(vPrecios).Name, DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls"));
        }

        public ActionResult GridViewAllPartial()
        {
            if (IsAdmin)
                vPrecios = new Business.Views.vPrecios();
            else
                vPrecios = new Business.Views.vPrecios(base.UserContext.RolesCliente.ToList());

            return PartialView("_GridForViews", vPrecios);
        }

         [LogonAuthorize(Roles = "VIEW")]
        public ActionResult View(string id)
        {
            if (IsAdmin)
                vPrecios = new Business.Views.vPrecios();
            else
                vPrecios = new Business.Views.vPrecios(base.UserContext.RolesCliente.ToList());

            try
            {
                DataTable datos = vPrecios.GetByID(id);
                vPrecios.Datos = datos;
                vPrecios.Id = id;

                string idString = id.ToString();
                ViewBag.Archivos = archivoRepository.GetFiltered(x => x.Entity_Id == idString && x.Estado == true
                                                                    && x.TipoArchivoModulo.Tipo.ToLower().Trim().Equals(_ControllerName.ToLower().Trim()), x => x.TipoArchivoModulo).ToArray();

                return View("FormViewFichaString", vPrecios);
            }
            catch (Exception ex)
            {
                SetMessage(ERROR, " ERROR: " + ex.Message);
                return View("Index", vPrecios);
            }
        }

        [HttpPost]
        public ActionResult View(string id, ArchivoModulo[] ArchivosModulo)
        {
            try
            {
                if (ArchivosModulo != null)
                {
                    foreach (ArchivoModulo archivoModulo in ArchivosModulo)
                    {
                        if ((archivoRepository.GetFiltered(x => x.Id == archivoModulo.Id).Any() == false) && archivoModulo.Deleted != true)
                        {
                            AltaArchivosModulo(archivoModulo, id);
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
