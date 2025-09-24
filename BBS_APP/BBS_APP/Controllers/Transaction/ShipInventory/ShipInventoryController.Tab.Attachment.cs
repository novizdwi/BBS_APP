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
using Models.Transaction.ShipInventory;

namespace Controllers.Transaction
{
    public partial class ShipInventoryController : BaseController
    {

        string VIEW_TAB_ATTACHMENT = "Partial/ShipInventory_Form_TabAttachment_List_Partial";

        public ActionResult TabAttachmentListPartial()
        {
            int userId = (int)Session["userId"];

            shipInventoryService = new ShipInventoryService();

            var Id = Convert.ToInt64(Request["cbId"]);

            var modelListAttachment = shipInventoryService.ShipInventory_Attachments(Id);

            return PartialView(VIEW_TAB_ATTACHMENT, modelListAttachment);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult TabAttachmentEditModesDeletePartial(long detId)
        {
            int userId = (int)Session["userId"];

            materialRequestService = new MaterialRequestService();

            var Id = Convert.ToInt64(Request["cbId"]);



            try
            {

                MaterialRequest_AttachmentModel model = new MaterialRequest_AttachmentModel();
                model.DetId = detId;
                materialRequestService.Attachment_Delete(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorDesc = ex.Message;
            }


            var modelListAttachment = materialRequestService.MaterialRequest_Attachments(Id);
            return PartialView(VIEW_TAB_ATTACHMENT, modelListAttachment);
        }
    }


}