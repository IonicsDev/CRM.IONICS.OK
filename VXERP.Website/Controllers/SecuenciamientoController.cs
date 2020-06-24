using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Business.Views;
using CRM.Website.DevExpressHelpers;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace CRM.Website.Controllers
{
    public class SecuenciamientoController : BaseController
    {
        ArchivoModuloRepository archivoRepository = new ArchivoModuloRepository();

        public vSecuenciamiento vSecuenciamiento = new vSecuenciamiento();

        public ActionResult Index()
        {
            //ClearTempFolder();
            return View(vSecuenciamiento);
        }


        //[HttpPost]
        //public ActionResult ExportXLS()
        //{

        //    var setting = GridHelper.GetSettingExport(vContactos.GetDynamicCollectionList(vContactos.GetViewModel()), _ControllerName);
    


        //    // retornamos el excel al usuario
        //    return GridViewExtension.ExportToXls(setting, vContactos.GetViewModel(), string.Format("{0}s_{1}.{2}", typeof(vContactos).Name, DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls"));
        //}

    }
}
