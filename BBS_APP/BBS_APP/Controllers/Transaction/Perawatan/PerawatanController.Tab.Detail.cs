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
using Models.Transaction.Perawatan;

namespace Controllers.Transaction
{
    public partial class PerawatanController : BaseController
    {

        string VIEW_TAB_DETAIL_COMPONENT = "Partial/Perawatan_Form_TabDetail_List_Partial";

        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            perawatanService = new PerawatanService();

            var Id = Convert.ToInt64(Request["cbId"]);


            var modelListDetail = perawatanService.Perawatan_Details(Id);

            return PartialView(VIEW_TAB_DETAIL_COMPONENT, modelListDetail);
        }
        public ActionResult GetItemsByBagian(string bagian)
        {
            // Menggunakan logika bisnis Anda, ambil data "Item" berdasarkan "Bagian"
            var items = Models._Utils.GeneralGetList.GetItemsByBagian(bagian);

            // Mengembalikan data dalam format yang sesuai (misalnya, JSON)
            return Content(items.ToString());
        }

    }
}