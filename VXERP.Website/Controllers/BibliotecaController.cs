using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Website.DevExpressHelpers;
using CRM.Business.DAL;
using CRM.Business.Entities;

namespace CRM.Website.Controllers
{
    public class BibliotecaController : BaseController
    {
        
        public ActionResult Index()
        {
            if (UserContext.UserID == 1031 || UserContext.UserID == 1044)
                ViewBag.IsAdminDoc = true;
            else
                ViewBag.IsAdminDoc = false;

            return View();
        }


        [ValidateInput(false)]
        public ActionResult FileManagerPartial()
        {
            if (UserContext.UserID == 1031 || UserContext.UserID == 1044)
                ViewBag.IsAdminDoc = true;
            else
                ViewBag.IsAdminDoc = false;

            return PartialView("_FileManagerPartial", BibliotecaControllerFileManager1Settings.Model);
        }

        public FileStreamResult FileManagerPartialDownload()
        {
            return FileManagerExtension.DownloadFiles(BibliotecaControllerFileManager1Settings.DownloadSettings, BibliotecaControllerFileManager1Settings.Model);
        }
    }

    public class BibliotecaControllerFileManager1Settings
    {
        private static readonly ParametroRepository parametroRepository = new ParametroRepository();
        private static Parametro ubicacionServer = parametroRepository.GetFiltered(p => p.ParamName == "UbiDocVigeSC").FirstOrDefault();
        //public const string RootFolder = @"\\SQLSRV\Compartido\Sistema de Calidad\\"; 
        public static string RootFolder = ubicacionServer.ParamValue;
        public static string Model { get { return RootFolder; } }
        public static DevExpress.Web.Mvc.FileManagerSettings DownloadSettings
        {
            get
            {
                var settings = new DevExpress.Web.Mvc.FileManagerSettings { Name = "FileManager" };
                settings.SettingsEditing.AllowDownload = true;
                
                return settings;
            }
        }
    }


}
