using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web;
using CRM.Website.DevExpressHelpers;

namespace CRM.Website.DevExpressHelpers
{
    public class FileManagerHelper 
    {
        //static DevExpress.Web.Demos.ArtsFileSystemProvider artsFileSystemProvider;

        static HttpContext Context { get { return HttpContext.Current; } }

        private static readonly string masterrepositorio = @"\\10.0.0.10\\crm\\Sistema de Calidad\\";
        public static readonly object FileManagerFolder = masterrepositorio;//"~/Content/FileManager";
        public static readonly object RootFolder = masterrepositorio; //HttpContext.Current.Server.MapPath(@"~/Content/Files");
        public static readonly object VirtualScrollingRootFolder = masterrepositorio; //~/Content/FileManager/VirtualScrolling/Files";
        public static readonly object DocumentsRootFolder = masterrepositorio;
        public static readonly object ImagesRootFolder = masterrepositorio;
        public static readonly string[] AllowedFileExtensions = new string[] {
            ".jpg", ".jpeg", ".gif", ".rtf", ".txt", ".avi", ".png", ".mp3", ".xml", ".doc", ".pdf"
        };
        public static readonly string[] VirtualScrollingAllowedFileExtensions = new string[] {
            ".jpg", ".jpeg", ".gif", ".rtf", ".txt", ".png", ".doc", ".pdf", ".xls", ".xlsx"
        };
        public static readonly string[] DocumentsAllowedFileExtensions = new string[] {
            ".jpg", ".jpeg", ".gif", ".rtf", ".txt", ".png", ".doc", ".docx", ".pdf", ".xls", ".xlsx"
        };
        public static FileManagerFeaturesOptions FeaturesDemoOptions
        {
            get
            {
                if (Context.Session["FeaturesDemoOptions"] == null)
                    Context.Session["FeaturesDemoOptions"] = new FileManagerFeaturesOptions();

                var options = (FileManagerFeaturesOptions)Context.Session["FeaturesDemoOptions"];
                return options;
            }
            set { Context.Session["FeaturesDemoOptions"] = value; }
        }
        public static FileManagerSubfolderSearchingOptions SubfolderSearchingDemoOptions
        {
            get
            {
                if (Context.Session["SubfolderSearchingDemoOptions"] == null)
                    Context.Session["SubfolderSearchingDemoOptions"] = new FileManagerSubfolderSearchingOptions();
                return (FileManagerSubfolderSearchingOptions)Context.Session["SubfolderSearchingDemoOptions"];
            }
            set { Context.Session["SubfolderSearchingDemoOptions"] = value; }
        }
        public static FileListView VirtualScrollingViewMode
        {
            get
            {
                if (Context.Session["VirtualScrollingViewMode"] == null)
                    return FileListView.Thumbnails;
                return (FileListView)Context.Session["VirtualScrollingViewMode"];
            }
            set { Context.Session["VirtualScrollingViewMode"] = value; }
        }
        public static int VirtualScrollingPageSize
        {
            get
            {
                if (Context.Session["VirtualScrollingPageSize"] == null)
                    return 300;
                return (int)Context.Session["VirtualScrollingPageSize"];
            }
            set { Context.Session["VirtualScrollingPageSize"] = value; }
        }
        public static DevExpress.Web.Mvc.FileManagerSettings CreateFileManagerDownloadSettings()
        {
            return CreateFileManagerDownloadSettingsCore(FeaturesDemoOptions.SettingsEditing);
        }
        public static DevExpress.Web.Mvc.FileManagerSettings CreateMultipleFilesSelectionDownloadSettings()
        {
            var editingSettings = new DevExpress.Web.FileManagerSettingsEditing(null)
            {
                AllowDownload = true
            };
            return CreateFileManagerDownloadSettingsCore(editingSettings);
        }
        public static DevExpress.Web.Mvc.FileManagerSettings CreateFileManagerGeneralDownloadSettings()
        {
            FileManagerSettingsEditing editingSettings = CreateFileManagerEditingSettings();
            return CreateFileManagerDownloadSettingsCore(editingSettings);
        }
        static DevExpress.Web.Mvc.FileManagerSettings CreateFileManagerDownloadSettingsCore(FileManagerSettingsEditing editingSettings)
        {
            var settings = new DevExpress.Web.Mvc.FileManagerSettings();
            settings.SettingsEditing.Assign(editingSettings);
            settings.Name = "fileManager";
            return settings;
        }
        public static FileManagerSettingsEditing CreateFileManagerEditingSettings()
        {
            return new FileManagerSettingsEditing(null)
            {
                AllowCreate = true,
                AllowMove = true,
                AllowDelete = true,
                AllowRename = true,
                AllowCopy = true,
                AllowDownload = true
            };
        }
        //public static DevExpress.Web.Demos.ArtsFileSystemProvider ArtsFileSystemProvider
        //{
        //    get
        //    {
        //        if (artsFileSystemProvider == null)
        //            artsFileSystemProvider = new DevExpress.Web.Demos.ArtsFileSystemProvider(string.Empty);
        //        return artsFileSystemProvider;
        //    }
        //}
        public static CustomFileSystemProvider CreateCustomFileSystemProvider()
        {
            return new CustomFileSystemProvider((string)DocumentsRootFolder);
        }
        //public static void ValidateSiteEdit(FileManagerActionEventArgsBase e)
        //{
        //    e.Cancel = DevExpress.Web.Demos.Utils.IsSiteMode;
        //    e.ErrorText = DevExpress.Web.Demos.Utils.GetReadOnlyMessageText();
        //}
        public static List<SelectListItem> GetSecurityRoles()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = "Default User", Value = SecurityRole.DefaultUser.ToString(), Selected = true },
                new SelectListItem() { Text = "Document Manager", Value = SecurityRole.DocumentManager.ToString() },
                new SelectListItem() { Text = "Media Moderator", Value = SecurityRole.MediaModerator.ToString() },
                new SelectListItem() { Text = "Administrator", Value = SecurityRole.Administrator.ToString() }
            };
        }
        public static List<SelectListItem> GetFileListViewModes()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = FileListView.Thumbnails.ToString(), Value = FileListView.Thumbnails.ToString(), Selected = true },
                new SelectListItem() { Text = FileListView.Details.ToString(), Value = FileListView.Details.ToString() }
            };
        }
        public static List<SelectListItem> GetFileListPageSizes()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = "50", Value = "50" },
                new SelectListItem() { Text = "100", Value = "100" },
                new SelectListItem() { Text = "300", Value = "300", Selected = true },
                new SelectListItem() { Text = "500", Value = "500" }
            };
        }
    }

    public enum SecurityRole { DefaultUser, DocumentManager, MediaModerator, Administrator }
}
