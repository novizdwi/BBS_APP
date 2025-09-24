﻿using System;
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
using Models.Transaction.Docking;

namespace Controllers.Transaction
{
    public partial class DockingController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/Docking_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            dockingService = new DockingService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListDetail = dockingService.Docking_Details(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}