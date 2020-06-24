using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Business.Entities.BaseEntities;
using CRM.Website.Crosscutting;
using CRM.Website.DevExpressHelpers;
using CRM.Website.Security;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;
using System.Reflection;
using System.Web.UI;
using System.Collections;
using DevExpress.Web;
using CRM.Business.Views.BaseViews;
using CRM.Website.Controllers;
using CRM.Business.Views;
using System.Data;
using System.IO;

namespace CRM.Website.Controllers
{
   // <summary>
    /// Controlador generico
    /// TEntity indica la entidad de dominio de la BD
    /// TViewModel indica la entidad del view model de MVC
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [HandleError]
    [System.Web.Mvc.Authorize]
    public class GenericController<TEntity> : BaseController
        where TEntity : CRM.Business.Entities.BaseEntities.BaseEntity, new()
    {

        protected GridViewExportFormat GetExportFormat()
        {
            return GridViewExportHelper<TEntity>.ExportFormatsInfo.Keys
                .Where(k => Request.Params[k.ToString()] != null)
                .DefaultIfEmpty(GridViewExportFormat.None)
                .First();
        }

        [HttpPost]
        public virtual ActionResult ExportXLS(GridViewExportedRowType exportRowType)
        {
         
            var setting = GridHelper.GetSettingExport(typeof(TEntity),_ControllerName);
            var rep = new CRM.Common.RepositoryDomain.Repository<TEntity, Int32>(new Business.Contexts.ConfigurationContext());

            setting.SettingsExport.ExportedRowType = exportRowType;

            // retornamos el excel al usuario
            return GridViewExtension.ExportToXls(setting, rep.GetAll().ToList(), string.Format("{0}s_{1}.{2}", typeof(TEntity).Name, DateTime.Now.ToString("ddMMyyyy_HHmmss"), "xls"));
        }

    }
        
}
