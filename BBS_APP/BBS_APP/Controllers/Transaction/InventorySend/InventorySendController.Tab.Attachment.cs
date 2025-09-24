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
using Models.Transaction.InventorySend;

namespace Controllers.Transaction
{
    public partial class InventorySendController : BaseController
    {

        string VIEW_TAB_ATTACHMENT = "Partial/InventorySend_Form_TabAttachment_List_Partial";

        //public ActionResult TabAttachmentListPartial()
        //{
        //    int userId = (int)Session["userId"];

        //    inventorySendService = new InventorySendService();

        //    var Id = Convert.ToInt64(Request["cbId"]);

        //    var modelList = inventorySendService.InventorySend_Attachments(Id);

        //    return PartialView(VIEW_TAB_ATTACHMENT, modelList);
        //}


        //[HttpPost, ValidateInput(false)]
        //public ActionResult TabAttachmentEditModesDeletePartial(long detId)
        //{
        //    int userId = (int)Session["userId"];

        //    inventorySendService = new InventorySendService();

        //    var Id = Convert.ToInt64(Request["cbId"]);



        //    try
        //    {

        //        InventorySend_AttachmentModel model = new InventorySend_AttachmentModel();
        //        model.DetId = detId;
        //        inventorySendService.Detail_Delete(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorDesc = ex.Message;
        //    }


        //    var modelList = inventorySendService.InventorySend_Attachments(Id);
        //    return PartialView(VIEW_TAB_ATTACHMENT, modelList);
        //}
    }


}