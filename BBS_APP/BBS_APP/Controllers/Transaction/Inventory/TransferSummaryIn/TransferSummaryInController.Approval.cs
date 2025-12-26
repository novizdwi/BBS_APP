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
    public partial class TransferSummaryInController : BaseController
    {

        string VIEW_APPROVAL_PROGRESS_PANEL_PARTIAL = "Partial/Approval/Approval_Panel_Partial";

        string VIEW_APPROVAL_PROGRESS_FORM_PARTIAL = "Partial/Approval/Approval_Form_Partial";

        public ActionResult Approval_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            transferSummaryInService = new TransferSummaryInService();

            TransferSummaryInModel model;

            if (Id != 0)
            {
                model = transferSummaryInService.GetById(userId, Id);
            }
            else
            {
                model = transferSummaryInService.GetNewModel(userId);
            }

            return PartialView(VIEW_APPROVAL_PROGRESS_PANEL_PARTIAL, model);
        }


        public ActionResult PopupApprovalLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new TransferSummaryInModel();

            return PartialView(VIEW_APPROVAL_PROGRESS_FORM_PARTIAL, model);
        }
    }
}