using Models;
using Models.Transaction.Web.Purchasing;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.Web.Purchasing
{
    public partial class PurchaseOrderController : BaseController
    {

        string VIEW_ITEMTAG_PANEL_PARTIAL = "Partial/ItemTag/ItemTag_Panel_Partial";
        string VIEW_ITEMTAG_FORM_PARTIAL = "Partial/ItemTag/ItemTag_Form_Partial";

        public ActionResult ViewItemTag_PopupListOnDemandPartial(long id = 0, long detId = 0)
        {
            int userId = (int)Session["userId"];

            purchaseOrderService = new PurchaseOrderService();
            var model = new PurchaseOrderItemTagView___();
            if(id != 0 && detId != 0)
            {
                model = purchaseOrderService.GetItemTags(id, detId);
            }
            return PartialView(VIEW_ITEMTAG_PANEL_PARTIAL, model);
        }


        public ActionResult PopupItemTagLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new PurchaseOrderModel();

            return PartialView(VIEW_ITEMTAG_FORM_PARTIAL, model);
        }
    }

}