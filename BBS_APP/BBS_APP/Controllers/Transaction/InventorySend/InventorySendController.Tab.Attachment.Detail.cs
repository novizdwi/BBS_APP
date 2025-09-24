using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading;
using Models;

using System.Net;

using Models._Utils;

using System.IO;

using Models._Ef;
using BBS_DI.Models._EF;

using Models;
using Models.Transaction.InventorySend;

namespace Controllers.Transaction
{
    public partial class InventorySendController : BaseController
    {
        string VIEW_ATTACHMENT_PANEL_PARTIAL = "Partial/Attachment/Attachment_Panel_Partial";

        string VIEW_ATTACHMENT_FORM_PARTIAL = "Partial/Attachment/Attachment_Form_Partial";


        public ActionResult Attachment_PopupListLoadOnDemandPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ViewBag.Id = Id;
            return PartialView(VIEW_ATTACHMENT_PANEL_PARTIAL);
        }

        public ActionResult Attachment_DetailPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            ViewBag.Id = Id;
            return PartialView(VIEW_ATTACHMENT_FORM_PARTIAL);
        }



       

        


    }



    

}