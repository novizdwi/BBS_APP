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

using Models.Transaction.Web.Inventory;

namespace Controllers.Transaction.Web.Inventory
{
    public partial class StockSummaryOpnameController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/StockSummaryOpname_Form_TabDetail_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            StockSummaryOpnameService = new StockSummaryOpnameService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListDetail = StockSummaryOpnameService.StockSummaryOpname_Details(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}