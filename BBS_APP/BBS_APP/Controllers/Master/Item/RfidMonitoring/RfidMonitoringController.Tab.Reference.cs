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
using Models.Master.Item.RfidMonitoring;

namespace Controllers.Master.Item
{
    public partial class RfidMonitoringController : BaseController
    {

        string VIEW_TAB_COMPONENT = "Partial/RfidMonitoring_Form_TabReference_List_Partial";

        public ActionResult TabTransListPartial()
        {
            int userId = (int)Session["userId"];
            DateTime filterDate = DateTime.Now.AddMonths(-1);
            if (!string.IsNullOrEmpty(Request["cbFilterDate"]))
            {
                filterDate = Convert.ToDateTime((Request["cbFilterDate"]).ToString() ); 

            }
            rfidMonitoringService = new RfidMonitoringService();
            string itemCode = (Request["cbItemCode"]).ToString();
            string whsCode = (Request["cbWhsCode"]).ToString();
            string tagId = (Request["cbTagId"]).ToString();
            string status = (Request["cbStatus"]).ToString();

            var modelList = rfidMonitoringService.RfidMonitoring_GetReferences(userId, filterDate, itemCode, whsCode, tagId, status);

            return PartialView(VIEW_TAB_COMPONENT, modelList);
        }




    }
}