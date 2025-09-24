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
using Models.Master.Ship;

namespace Controllers.Master
{
    public partial class ShipController : BaseController
    {

        string VIEW_TAB_NAVEQ_COMPONENT = "Partial/Ship_Form_TabNavEq_List_Partial";

        public ActionResult TabNavEqListPartial()
        {
            int userId = (int)Session["userId"];

            shipService = new ShipService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListNavEq = shipService.Ship_NavEqs(Id);

            return PartialView(VIEW_TAB_NAVEQ_COMPONENT, modelListNavEq);
        }
        

    }
}