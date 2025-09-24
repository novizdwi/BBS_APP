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
using Models.Transaction.Perawatan;

namespace Controllers.Transaction
{
    public partial class PerawatanController : BaseController
    {

        string VIEW_TAB_ATTACHMENT = "Partial/Perawatan_Form_TabAttachment_List_Partial";

        public ActionResult TabAttachmentListPartial()
        {
            int userId = (int)Session["userId"];

            perawatanService = new PerawatanService();

            var Id = Convert.ToInt64(Request["cbId"]);

            var modelListAttachment = perawatanService.Perawatan_Attachments(Id);

            return PartialView(VIEW_TAB_ATTACHMENT, modelListAttachment);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult TabAttachmentEditModesDeletePartial(long detId)
        {
            int userId = (int)Session["userId"];

            perawatanService = new PerawatanService();

            var Id = Convert.ToInt64(Request["cbId"]);



            try
            {

                Perawatan_AttachmentModel model = new Perawatan_AttachmentModel();
                model.DetId = detId;
                perawatanService.Attachment_Delete(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorDesc = ex.Message;
            }


            var modelListAttachment = perawatanService.Perawatan_Attachments(Id);
            return PartialView(VIEW_TAB_ATTACHMENT, modelListAttachment);
        }
    }


}