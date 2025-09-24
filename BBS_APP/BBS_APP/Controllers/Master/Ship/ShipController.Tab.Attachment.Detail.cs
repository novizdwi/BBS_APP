using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading;
using Models;

using System.Net;
using DevExpress.Web;
using Models._Utils;

using System.IO;

using Models._Ef;
using BBS_DI.Models._EF;
using Models.Master.Ship;

namespace Controllers.Master
{
    public partial class ShipController : BaseController
    {
        string VIEW_ATTACHMENT_PANEL_PARTIAL = "Partial/Attachment/Attachment_Panel_Partial";

        string VIEW_ATTACHMENT_FORM_PARTIAL = "Partial/Attachment/Attachment_Form_Partial";


        public ActionResult Attachment_PopupListLoadOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ViewBag.Id = Id;
            return PartialView(VIEW_ATTACHMENT_PANEL_PARTIAL);
        }

        public ActionResult Attachment_DetailPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            ViewBag.Id = Id;
            return PartialView(VIEW_ATTACHMENT_FORM_PARTIAL);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult Attachment_Upload()
        {
            int userId = (int)Session["userId"];

            var Id = Request["Id"];

            var UploadMultiFile = UploadControlExtension.GetUploadedFiles("UploadMultiFile", ShipUploadControlHelper.ValidationSettings, ShipUploadControlHelper.FileUploadComplete);


            if (UploadMultiFile != null)
            {
                for (int i = 0; i < UploadMultiFile.Length; i++)
                {
                    if (!UploadMultiFile[i].IsValid)
                    {
                        return null;
                    }
                }

                shipService = new ShipService();

                List<Ship_AttachmentModel> listModel = new List<Ship_AttachmentModel>();

                for (int i = 0; i < UploadMultiFile.Length; i++)
                {
                    if (UploadMultiFile[i].FileBytes.Length > 0 && UploadMultiFile[i].IsValid)
                    {

                        var guid = Guid.NewGuid().ToString();

                        Ship_AttachmentModel model = new Ship_AttachmentModel();
                        model.Id = long.Parse(Id);
                        model.FileName = UploadMultiFile[i].FileName;
                        model.Guid = guid;
                        model._UserId = (int)Session["userId"];
                        model.FileIndex_ = i;
                        listModel.Add(model);
                    }
                }

               shipService.Detail_Add(listModel);

                for (int i = 0; i < listModel.Count; i++)
                {
                    string strFilename = ShipUploadControlHelper.GetFilePath(listModel[i].Guid + "_" + listModel[i].FileName);

                    UploadMultiFile[listModel[i].FileIndex_].SaveAs(strFilename);

                }

            }



            return null;
        }


        public FileResult Attachment_Download(int DetId = 0)
        {
            int userId = (int)Session["userId"];


            using (var CONTEXT = new BBS_DI.Models._EF.HANA_APP())
            {
                Tm_Ship_Attachment model = CONTEXT.Tm_Ship_Attachment.Find(DetId);
                if (model != null)
                {
                    string fileName = model.FileName;
                    return File(ShipUploadControlHelper.GetFilePath(model.Guid + "_" + model.FileName), System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
                return null;
            }




        }



    }



    public class ShipUploadControlHelper
    {
        public const string UploadDirectory = "~/Content/Attachment/Ship/";

        public static UploadControlValidationSettings ValidationSettings = new UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 2097152
        };

        public static string GetFilePath(string FileName)
        {
            var resultFilePath = UploadDirectory + FileName;
            return HttpContext.Current.Request.MapPath(resultFilePath);
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
    }


}