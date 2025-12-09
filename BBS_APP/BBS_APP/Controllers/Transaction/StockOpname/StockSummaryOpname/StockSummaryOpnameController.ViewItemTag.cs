using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Transaction.StockOpname;

namespace Controllers.Transaction.StockOpname
{
    public partial class StockSummaryOpnameController : BaseController
    {

        string VIEW_ITEMTAG_PANEL_PARTIAL = "Partial/ItemTag/ItemTag_Panel_Partial";
        string VIEW_ITEMTAG_FORM_PARTIAL = "Partial/ItemTag/ItemTag_Form_Partial";

        public ActionResult ViewItemTag_PopupListOnDemandPartial(long id = 0, long detId = 0)
        {
            int userId = (int)Session["userId"];

            StockSummaryOpnameService = new StockSummaryOpnameService();
            var model = new StockSummaryOpnameItemTagView___();
            if(id != 0 && detId != 0)
            {
                model = StockSummaryOpnameService.GetItemTags(id, detId);
            }
            return PartialView(VIEW_ITEMTAG_PANEL_PARTIAL, model);
        }


        public ActionResult PopupItemTagLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];
            var model = new StockSummaryOpnameModel();

            return PartialView(VIEW_ITEMTAG_FORM_PARTIAL, model);
        }
    }

}