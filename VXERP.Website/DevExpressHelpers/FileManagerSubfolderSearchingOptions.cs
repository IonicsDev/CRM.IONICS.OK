using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Website.DevExpressHelpers
{
    public class FileManagerSubfolderSearchingOptions
    {
        public FileManagerSubfolderSearchingOptions()
        {
            SearchFilesInSubfolders = true;
            SearchResultView = FileManagerFilteredFileListViewMode.Auto;
            FileListView = DevExpress.Web.FileListView.Thumbnails;
        }

        public bool SearchFilesInSubfolders { get; set; }
        public FileManagerFilteredFileListViewMode SearchResultView { get; set; }
        public FileListView FileListView { get; set; }
    }
}