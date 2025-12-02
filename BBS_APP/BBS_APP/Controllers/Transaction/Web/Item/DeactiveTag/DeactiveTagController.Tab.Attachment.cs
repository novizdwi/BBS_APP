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

using Models.Transaction.Web;
using Models._Utils; 
using DevExpress.Web.ASPxHtmlEditor.Internal;
using BBS_APP;
using Models.Transaction.Web.Item;

namespace Controllers.Transaction.Web.Item
{
    public partial class DeactiveTagController : BaseController
    {

        string VIEW_TAB_ATTACHMENT = "Partial/DeactiveTag_Form_TabAttachment_List_Partial";
        string moduleName = "DeactiveTag";

        public ActionResult TabAttachmentListPartial()
        {
            int userId = (int)Session["userId"];

            deactiveTagService = new DeactiveTagService();

            var Id = Convert.ToInt64(Request["cbId"]);

            var modelList = deactiveTagService.GetDeactiveTag_Attachments(Id);

            return PartialView(VIEW_TAB_ATTACHMENT, modelList);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult TabAttachmentEditModesDeletePartial(long detId)
        {
            int userId = (int)Session["userId"];

            deactiveTagService = new DeactiveTagService();

            var Id = Convert.ToInt64(Request["cbId"]);
            try
            {

                var model = deactiveTagService.GetDeactiveTag_Attachments_GetById(detId);
                if (model != null)
                {
                    CommonFileHelper.DeleteFile(moduleName, model.Guid + "_" + model.FileName);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorDesc = ex.Message;
            }


            var modelList = deactiveTagService.GetDeactiveTag_Attachments(Id);
            return PartialView(VIEW_TAB_ATTACHMENT, modelList);
        }
    }


}