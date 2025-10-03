using Models;
using Models.Transaction.Web.Purchasing;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.Web.Purchasing
{
    public partial class PurchaseOrderController : BaseController
    {
        string VIEW_DETAIL = "PurchaseOrder";
        string VIEW_FORM_PARTIAL = "Partial/PurchaseOrder_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/PurchaseOrder_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/PurchaseOrder_Panel_List_Partial";


        PurchaseOrderService purchaseOrderService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            purchaseOrderService = new PurchaseOrderService();
            PurchaseOrderModel purchaseOrderModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                purchaseOrderModel = purchaseOrderService.GetNewModel(userId);
                purchaseOrderModel._FormMode = FormModeEnum.New;
            }
            else
            {
                purchaseOrderService = new PurchaseOrderService();
                purchaseOrderModel = purchaseOrderService.GetById(userId, Id);
                purchaseOrderModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, purchaseOrderModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            PurchaseOrderModel purchaseOrderModel;

            purchaseOrderService = new PurchaseOrderService();
            if (Id == 0)
            {
                purchaseOrderModel = purchaseOrderService.GetNewModel(userId);
                purchaseOrderModel._FormMode = FormModeEnum.New;
            }
            else
            {
                purchaseOrderModel = purchaseOrderService.GetById(userId, Id);
                if (purchaseOrderModel != null)
                {
                    purchaseOrderModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    purchaseOrderModel = purchaseOrderService.GetNewModel(userId);
                    purchaseOrderModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  PurchaseOrderModel purchaseOrderModel)
        //{
        //    int userId = (int)Session["userId"];

        //    purchaseOrderModel._UserId = (int)Session["userId"];
        //    purchaseOrderService = new PurchaseOrderService();

        //    if (ModelState.IsValid)
        //    {
        //        long Id = 0;

        //        Id = purchaseOrderService.Add(purchaseOrderModel);
        //        purchaseOrderModel = purchaseOrderService.GetById(userId, Id);
        //        purchaseOrderModel._FormMode = Models.FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        string message = GetErrorModel();
        //        throw new Exception(string.Format("[VALIDATION] {0}", message));
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        //}

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  PurchaseOrderModel purchaseOrderModel)
        {
            int userId = (int)Session["userId"];

            purchaseOrderModel._UserId = (int)Session["userId"];
            purchaseOrderService = new PurchaseOrderService();
            purchaseOrderModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            purchaseOrderService.Update(purchaseOrderModel);
            purchaseOrderModel = purchaseOrderService.GetById(userId, purchaseOrderModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  PurchaseOrderModel purchaseOrderModel)
        {
            int userId = (int)Session["userId"];

            purchaseOrderModel._UserId = (int)Session["userId"];
            purchaseOrderService = new PurchaseOrderService();
            purchaseOrderModel._FormMode = FormModeEnum.Edit;

            purchaseOrderService.Update(purchaseOrderModel);
            //purchaseOrderService.PostAPI(userId, purchaseOrderModel.Id);
            purchaseOrderService.Post(userId, purchaseOrderModel.Id);
            purchaseOrderModel = purchaseOrderService.GetById(userId, purchaseOrderModel.Id);

            if (purchaseOrderModel != null)
            {
                purchaseOrderModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                purchaseOrderModel = purchaseOrderService.GetNewModel(userId);
                purchaseOrderModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            PurchaseOrderModel purchaseOrderModel;

            purchaseOrderService = new PurchaseOrderService();
            purchaseOrderService.Cancel(userId, Id, CancelReason);

            purchaseOrderModel = purchaseOrderService.GetById(userId, Id);
            if (purchaseOrderModel != null)
            {
                purchaseOrderModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                purchaseOrderModel = purchaseOrderService.GetNewModel(userId);
                purchaseOrderModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, purchaseOrderModel);
        }
        
    }
}