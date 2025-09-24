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
using Models.Transaction.ShipInventory;

namespace Controllers.Transaction
{
    public partial class ShipInventoryController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/ShipInventory_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            shipInventoryService = new ShipInventoryService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListDetail = shipInventoryService.ShipInventory_Details(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}