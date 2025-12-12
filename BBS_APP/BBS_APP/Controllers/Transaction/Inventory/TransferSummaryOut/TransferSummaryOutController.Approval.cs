using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.IO;
using System.Threading;

using System.Net;

using Models;
using Models.Transaction.Inventory;


namespace Controllers.Transaction.Inventory
{
    public partial class TransferSummaryOutController : BaseController
    {

        string VIEW_APPROVAL_PROGRESS_PANEL_PARTIAL = "Partial/Approval/Approval_Panel_Partial";

        string VIEW_APPROVAL_PROGRESS_FORM_PARTIAL = "Partial/Approval/Approval_Form_Partial";

        public ActionResult Approval_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            transferSummaryOutService = new TransferSummaryOutService();

            TransferSummaryOutModel model;

            if (Id != 0)
            {
                model = transferSummaryOutService.GetById(userId, Id);
            }
            else
            {
                model = transferSummaryOutService.GetNewModel(userId);
            }

            return PartialView(VIEW_APPROVAL_PROGRESS_PANEL_PARTIAL, model);
        }


        public ActionResult PopupApprovalLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new TransferSummaryOutModel();

            return PartialView(VIEW_APPROVAL_PROGRESS_FORM_PARTIAL, model);
        }
    }
}