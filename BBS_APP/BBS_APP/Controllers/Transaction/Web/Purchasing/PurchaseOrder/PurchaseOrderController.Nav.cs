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
using Models.Transaction.Web.Purchasing;

namespace Controllers.Transaction.Web.Purchasing
{
    public partial class PurchaseOrderController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            PurchaseOrderModel purchaseOrderModel;
            purchaseOrderService = new PurchaseOrderService();

            purchaseOrderModel = purchaseOrderService.NavFirst(userId);
            if (purchaseOrderModel != null)
            {
                purchaseOrderModel._FormMode = FormModeEnum.Edit;
            }

            if (purchaseOrderModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            PurchaseOrderModel purchaseOrderModel;
            purchaseOrderService = new PurchaseOrderService();

            purchaseOrderModel = purchaseOrderService.NavPrevious(userId, Id);
            if (purchaseOrderModel != null)
            {
                purchaseOrderModel._FormMode = FormModeEnum.Edit;
            }

            if (purchaseOrderModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            PurchaseOrderModel purchaseOrderModel;
            purchaseOrderService = new PurchaseOrderService();

            purchaseOrderModel = purchaseOrderService.NavNext(userId, Id);
            if (purchaseOrderModel != null)
            {

                purchaseOrderModel._FormMode = FormModeEnum.Edit;

            }

            if (purchaseOrderModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            PurchaseOrderModel purchaseOrderModel;
            purchaseOrderService = new PurchaseOrderService();

            purchaseOrderModel = purchaseOrderService.NavLast(userId);
            if (purchaseOrderModel != null)
            {
                purchaseOrderModel._FormMode = FormModeEnum.Edit;
            }

            if (purchaseOrderModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }



    }
}