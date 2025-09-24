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
using Models.Master.Ship;

namespace Controllers.Master
{
    public partial class ShipController : BaseController
    {

        string VIEW_TAB_ATTACHMENT = "Partial/Ship_Form_TabAttachment_List_Partial";

        public ActionResult TabAttachmentListPartial()
        {
            int userId = (int)Session["userId"];

            shipService = new ShipService();

            var Id = Convert.ToInt64(Request["cbId"]);

            var modelListAttachment = shipService.Ship_Attachments(Id);

            return PartialView(VIEW_TAB_ATTACHMENT, modelListAttachment);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult TabAttachmentEditModesDeletePartial(long detId)
        {
            int userId = (int)Session["userId"];

            shipService = new ShipService();

            var Id = Convert.ToInt64(Request["cbId"]);



            try
            {

                Ship_AttachmentModel model = new Ship_AttachmentModel();
                model.DetId = detId;
                shipService.Detail_Delete(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorDesc = ex.Message;
            }


            var modelListAttachment = shipService.Ship_Attachments(Id);
            return PartialView(VIEW_TAB_ATTACHMENT, modelListAttachment);
        }
    }


}