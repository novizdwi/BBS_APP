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

        string VIEW_TAB_Ref_COMPONENT = "Partial/StockSummaryOpname_Form_TabRef_Partial";

        public ActionResult TabRefListPartial()
        {
            int userId = (int)Session["userId"];

            StockSummaryOpnameService = new StockSummaryOpnameService();

            var Id = Convert.ToInt64(Request["cbId"]);

            var modelListRef = StockSummaryOpnameService.StockSummaryOpname_Refs(Id);

            return PartialView(VIEW_TAB_Ref_COMPONENT, modelListRef);
        }
        

    }
}