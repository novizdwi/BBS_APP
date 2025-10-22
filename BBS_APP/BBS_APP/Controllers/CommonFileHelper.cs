using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;


namespace BBS_APP
{
    public class CommonFileHelper
    {
        protected static string[] docExtensions = new string[] { ".xls", ".xlsx", ".doc", ".docx", ".pdf", ".txt", ".csv" };
        protected static string[] imgExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" };
        protected static string[] mailExtensions = new string[] { ".msg" };
        protected static string[] rptExtensions = new string[] {".rpt"};

        protected static string GetUploadDirectory()
        {
            string _BaseDirectory = Directory.GetDirectoryRoot(HostingEnvironment.MapPath("~/"));
            //string _BaseDirectory = new DirectoryInfo(HostingEnvironment.MapPath("~/")).Parent.Parent.FullName;
            var path = Path.Combine(_BaseDirectory, ConfigurationManager.AppSettings["UploadDirectory"], ConfigurationManager.AppSettings["WebName"], "Attachment");            
            return path;
        }
        protected static string GetRptDirectory()
        {
            string _BaseDirectory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/")).Parent.Parent.Parent.FullName;
            return Path.Combine(_BaseDirectory, ConfigurationManager.AppSettings["UploadDirectory"], ConfigurationManager.AppSettings["WebName"], "Temp");
        }

        public static UploadControlValidationSettings RptValidationSettings = new UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".rpt" },
            MaxFileSize = 2097152
        };

        public static DevExpress.Web.UploadControlValidationSettings ValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".xls", ".xlsx", ".doc", ".docx", ".pdf", ".txt", ".csv", ".jpg", ".jpeg", ".jpe", ".gif", ".png", ".msg" },
            //MaxFileSize = 6291456
            MaxFileSize = 2097152
        };

        public static string GetFilePath(string FileName, string ModuleName, string extension)
        {
            string uploadDirectory = GetUploadDirectory();
            string fileTypePath = "";
            //if (Array.IndexOf(docExtensions, extension.ToLower()) > -1)
            //{
            //    fileTypePath = "docs";
            //}
            //}
            //else if (Array.IndexOf(imgExtensions, extension.ToLower()) > -1)
            //{
            //    fileTypePath = "img";
            //}

            Directory.CreateDirectory(Path.Combine(uploadDirectory, ModuleName, fileTypePath));
            var resultFilePath = Path.Combine(uploadDirectory, ModuleName, fileTypePath, FileName);
            return resultFilePath;
        }
        public static void FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                e.CallbackData = "mantap";
            }
            else
            {
                e.CallbackData = "";
            }

        }

        public static void DeleteFile(string ModuleName, string fileName)
        {
            var fileNameSplit = fileName.Split('.');
            string extension = fileNameSplit.Count() > 1 ? "." + fileNameSplit[1] : "";
            string fileTypePath = "";

            //if (Array.IndexOf(docExtensions, extension) > -1)
            //{
            //    fileTypePath = "docs";
            //}
            //else if (Array.IndexOf(imgExtensions, extension) > -1)
            //{
            //    fileTypePath = "img";
            //}

            string uploadDirectory = GetUploadDirectory();
            var resultFilePath = Path.Combine(uploadDirectory, ModuleName, fileTypePath, fileName);
            if (System.IO.File.Exists(resultFilePath))
                System.IO.File.Delete(resultFilePath);
        }
    }
}