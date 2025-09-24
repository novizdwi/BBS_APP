using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading;

using System.Net;

using Models._Utils;

using System.IO;

using System.Net;

using Models;
using Models.Transaction.CleaningTank;

namespace Controllers.Transaction
{
    public partial class CleaningTankController : BaseController
    {

        string VIEW_TAB_ATTACHMENT = "Partial/CleaningTank_Form_TabAttachment_List_Partial";

        public ActionResult TabAttachmentListPartial()
        {
            int userId = (int)Session["userId"];

            cleaningTankService = new CleaningTankService();

            var Id = Convert.ToInt64(Request["cbId"]);

            var modelListAttachment = cleaningTankService.CleaningTank_Attachments(Id);

            return PartialView(VIEW_TAB_ATTACHMENT, modelListAttachment);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult TabAttachmentEditModesDeletePartial(long detId)
        {
            int userId = (int)Session["userId"];

            cleaningTankService = new CleaningTankService();

            var Id = Convert.ToInt64(Request["cbId"]);



            try
            {

                CleaningTank_AttachmentModel model = new CleaningTank_AttachmentModel();
                model.DetId = detId;
                cleaningTankService.Attachment_Delete(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorDesc = ex.Message;
            }


            var modelListAttachment = cleaningTankService.CleaningTank_Attachments(Id);
            return PartialView(VIEW_TAB_ATTACHMENT, modelListAttachment);
        }
    }


}