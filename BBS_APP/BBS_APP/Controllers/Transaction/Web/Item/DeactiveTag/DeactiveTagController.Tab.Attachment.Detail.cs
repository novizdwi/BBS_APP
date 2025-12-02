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
using Models._Utils;
using System.IO;
using BBS_DI.Models._EF;
using BBS_APP;
using Models.Transaction.Web.Item;

namespace Controllers.Transaction.Web.Item
{
    public partial class DeactiveTagController : BaseController
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

            //var UploadMultiFile = UploadControlExtension.GetUploadedFiles("UploadMultiFile", DeactiveTagUploadControlHelper.ValidationSettings, DeactiveTagUploadControlHelper.FileUploadComplete);
            var UploadMultiFile = UploadControlExtension.GetUploadedFiles("UploadMultiFile", CommonFileHelper.ValidationSettings, CommonFileHelper.FileUploadComplete);


            if (UploadMultiFile != null)
            {
                for (int i = 0; i < UploadMultiFile.Length; i++)
                {
                    if (!UploadMultiFile[i].IsValid)
                    {
                        return null;
                    }
                }

                deactiveTagService = new DeactiveTagService();

                List<DeactiveTag_AttachmentModel> listModel = new List<DeactiveTag_AttachmentModel>();

                for (int i = 0; i < UploadMultiFile.Length; i++)
                {
                    if (UploadMultiFile[i].FileBytes.Length > 0 && UploadMultiFile[i].IsValid)
                    {

                        var guid = Guid.NewGuid().ToString();

                        DeactiveTag_AttachmentModel model = new DeactiveTag_AttachmentModel();
                        model.Id = long.Parse(Id);
                        model.FileName = UploadMultiFile[i].FileName;
                        model.Guid = guid;
                        model._UserId = (int)Session["userId"];
                        model.FileIndex_ = i;
                        listModel.Add(model);
                    }
                }
                

                for (int i = 0; i < listModel.Count; i++)
                {
                    var fileNameSplit = listModel[i].FileName.Split('.');
                    string fileExt = fileNameSplit.Count() > 1 ? "." + fileNameSplit[1] : "";
                    string strFilename = CommonFileHelper.GetFilePath(listModel[i].Guid + "_" + listModel[i].FileName, moduleName, fileExt);

                    //string strFilename = DeactiveTagUploadControlHelper.GetFilePath(listModel[i].Guid + "_" + listModel[i].FileName);

                    UploadMultiFile[listModel[i].FileIndex_].SaveAs(strFilename);

                }

            }



            return null;
        }

        



    }

    
}