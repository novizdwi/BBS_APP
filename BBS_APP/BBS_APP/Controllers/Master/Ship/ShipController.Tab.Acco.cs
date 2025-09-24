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

        string VIEW_TAB_ACCO_COMPONENT = "Partial/Ship_Form_TabAcco_List_Partial";

        public ActionResult TabAccoListPartial()
        {
            int userId = (int)Session["userId"];

            shipService = new ShipService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListAcco = shipService.Ship_Accos(Id);

            return PartialView(VIEW_TAB_ACCO_COMPONENT, modelListAcco);
        }
        

    }
}