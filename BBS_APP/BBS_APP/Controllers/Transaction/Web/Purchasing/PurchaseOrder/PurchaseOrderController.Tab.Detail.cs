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

using Models.Transaction.Web.Purchasing;

namespace Controllers.Transaction.Web.Purchasing
{
    public partial class PurchaseOrderController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/PurchaseOrder_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            purchaseOrderService = new PurchaseOrderService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListDetail = purchaseOrderService.PurchaseOrder_Items(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}