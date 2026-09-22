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

        string VIEW_CLOSE_PANEL_PARTIAL = "Partial/CloseReason/CloseReason_Panel_Partial";

        string VIEW_CLOSE_FORM_PARTIAL = "Partial/CloseReason/CloseReason_Form_Partial";

        public ActionResult CloseReason_PopupListOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            transferSummaryOutService = new TransferSummaryOutService();

            TransferSummaryOutCloseModel model = new TransferSummaryOutCloseModel() {
                Id = Id
            };
            
            return PartialView(VIEW_CLOSE_PANEL_PARTIAL, model);
        }


        public ActionResult PopupCloseReasonLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new TransferSummaryOutModel();

            return PartialView(VIEW_CLOSE_FORM_PARTIAL, model);
        }
    }
}