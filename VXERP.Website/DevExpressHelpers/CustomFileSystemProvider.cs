using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CRM.Website.DevExpressHelpers
{
    public class CustomFileSystemProvider : PhysicalFileSystemProvider
    {
        readonly int DefaultRecentFilesNumber = 20;

        readonly string[] FavoriteFileNames = new string[] {
            "Bill payment.xlsx",
            "Design.rtf",
            "Requests\\Materials.xlsx",
            "Requests\\Instruments.xlsx",
            "Projects\\Components.xlsx",
            "Projects\\Documentation.pdf"
        };

        readonly string[] RTFExtensions = new string[] { ".rtf", ".doc", ".docx", ".txt" };
        readonly string[] SheetExtensions = new string[] { ".xlsx", ".xls" };
        readonly string[] ImageExtensions = new string[] { ".png", ".gif", ".jpg", ".jpeg", ".ico", ".bmp" };
        readonly string[] PDFExtensions = new string[] { ".pdf" };

        public CustomFileSystemProvider(string rootFolder) : base(rootFolder) { }

        public override void GetFilteredItems(FileManagerGetFilteredItemsArgs args)
        {
            switch (args.FileListCustomFilter)
            {
                case "Recent":
                    args.Items = ExecuteFilterByRecent(args.Folder);
                    break;
                case "RTFDocs":
                    args.Items = ExecuteFilterByExtension(RTFExtensions, args.Folder);
                    break;
                case "Sheets":
                    args.Items = ExecuteFilterByExtension(SheetExtensions, args.Folder);
                    break;
                case "Images":
                    args.Items = ExecuteFilterByExtension(ImageExtensions, args.Folder);
                    break;
                case "PDFs":
                    args.Items = ExecuteFilterByExtension(PDFExtensions, args.Folder);
                    break;
                case "Favorites":
                    args.Items = GetFavoriteFiles();
                    break;
                default:
                    base.GetFilteredItems(args);
                    return;
            }
            if (args.Items != null && !string.IsNullOrEmpty(args.FilterBoxText))
                args.Items = args.Items.Where(item => item.Name.ToLower().IndexOf(args.FilterBoxText) > -1);
        }
        IEnumerable<FileManagerItem> ExecuteFilterByExtension(string[] extensions, FileManagerFolder folder)
        {
            DirectoryInfo dir = GetDirectoryInfo(folder);
            return dir.GetFiles("*", SearchOption.AllDirectories).
                Where(file => extensions.Contains(file.Extension)).
                Select(f => CreateFileManagerItem(f));
        }
        IEnumerable<FileManagerItem> ExecuteFilterByRecent(FileManagerFolder folder)
        {
            DirectoryInfo dir = GetDirectoryInfo(folder);
            return dir.GetFileSystemInfos("*", SearchOption.AllDirectories).
                OrderByDescending(item => item.LastWriteTime).
                Take(DefaultRecentFilesNumber).
                Select(i => CreateFileManagerItem(i));
        }
        IEnumerable<FileManagerFile> GetFavoriteFiles()
        {
            return FavoriteFileNames.Select(relativeName => new FileManagerFile(this, relativeName));
        }
        DirectoryInfo GetDirectoryInfo(FileManagerFolder folder)
        {
            return new DirectoryInfo(Path.Combine(GetResolvedRootFolderPath(), folder.RelativeName));
        }
        FileManagerItem CreateFileManagerItem(FileSystemInfo info)
        {
            string rootFullPath = Path.GetFullPath(GetResolvedRootFolderPath());
            string relativeName = info.FullName.Replace(rootFullPath, "").TrimStart('\\');
            if (info is DirectoryInfo)
                return new FileManagerFolder(this, relativeName);
            return new FileManagerFile(this, relativeName);
        }
    }
}