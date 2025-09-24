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
using Models.Transaction.Kecelakaan;

namespace Controllers.Transaction
{
    public partial class KecelakaanController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/Kecelakaan_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            kecelakaanService = new KecelakaanService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListDetail = kecelakaanService.Kecelakaan_Details(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        

    }
}