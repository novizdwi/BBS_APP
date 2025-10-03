using Models;
using Models.Transaction.Web.Purchasing;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Controllers.Transaction.Web.Purchasing
{
    public partial class GoodsReceiptPOController : BaseController
    {
        string VIEW_DETAIL = "GoodsReceiptPO";
        string VIEW_FORM_PARTIAL = "Partial/GoodsReceiptPO_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/GoodsReceiptPO_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/GoodsReceiptPO_Panel_List_Partial";


        GoodsReceiptPOService goodsReceiptPOService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            goodsReceiptPOService = new GoodsReceiptPOService();
            GoodsReceiptPOModel goodsReceiptPOModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                goodsReceiptPOModel = goodsReceiptPOService.GetNewModel(userId);
                goodsReceiptPOModel._FormMode = FormModeEnum.New;
            }
            else
            {
                goodsReceiptPOService = new GoodsReceiptPOService();
                goodsReceiptPOModel = goodsReceiptPOService.GetById(userId, Id);
                goodsReceiptPOModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, goodsReceiptPOModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            GoodsReceiptPOModel goodsReceiptPOModel;

            goodsReceiptPOService = new GoodsReceiptPOService();
            if (Id == 0)
            {
                goodsReceiptPOModel = goodsReceiptPOService.GetNewModel(userId);
                goodsReceiptPOModel._FormMode = FormModeEnum.New;
            }
            else
            {
                goodsReceiptPOModel = goodsReceiptPOService.GetById(userId, Id);
                if (goodsReceiptPOModel != null)
                {
                    goodsReceiptPOModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    goodsReceiptPOModel = goodsReceiptPOService.GetNewModel(userId);
                    goodsReceiptPOModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPOModel);
        }

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  GoodsReceiptPOModel goodsReceiptPOModel)
        //{
        //    int userId = (int)Session["userId"];

        //    goodsReceiptPOModel._UserId = (int)Session["userId"];
        //    goodsReceiptPOService = new GoodsReceiptPOService();

        //    if (ModelState.IsValid)
        //    {
        //        long Id = 0;

        //        Id = goodsReceiptPOService.Add(goodsReceiptPOModel);
        //        goodsReceiptPOModel = goodsReceiptPOService.GetById(userId, Id);
        //        goodsReceiptPOModel._FormMode = Models.FormModeEnum.Edit;
        //    }
        //    else
        //    {
        //        string message = GetErrorModel();
        //        throw new Exception(string.Format("[VALIDATION] {0}", message));
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPOModel);
        //}

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  GoodsReceiptPOModel goodsReceiptPOModel)
        {
            int userId = (int)Session["userId"];

            goodsReceiptPOModel._UserId = (int)Session["userId"];
            goodsReceiptPOService = new GoodsReceiptPOService();
            goodsReceiptPOModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            goodsReceiptPOService.Update(goodsReceiptPOModel);
            goodsReceiptPOModel = goodsReceiptPOService.GetById(userId, goodsReceiptPOModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPOModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  GoodsReceiptPOModel goodsReceiptPOModel)
        {
            int userId = (int)Session["userId"];

            goodsReceiptPOModel._UserId = (int)Session["userId"];
            goodsReceiptPOService = new GoodsReceiptPOService();
            goodsReceiptPOModel._FormMode = FormModeEnum.Edit;

            goodsReceiptPOService.Update(goodsReceiptPOModel);
            //goodsReceiptPOService.PostAPI(userId, goodsReceiptPOModel.Id);
            goodsReceiptPOService.Post(userId, goodsReceiptPOModel.Id);
            goodsReceiptPOModel = goodsReceiptPOService.GetById(userId, goodsReceiptPOModel.Id);

            if (goodsReceiptPOModel != null)
            {
                goodsReceiptPOModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                goodsReceiptPOModel = goodsReceiptPOService.GetNewModel(userId);
                goodsReceiptPOModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPOModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            GoodsReceiptPOModel goodsReceiptPOModel;

            goodsReceiptPOService = new GoodsReceiptPOService();
            goodsReceiptPOService.Cancel(userId, Id, CancelReason);

            goodsReceiptPOModel = goodsReceiptPOService.GetById(userId, Id);
            if (goodsReceiptPOModel != null)
            {
                goodsReceiptPOModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                goodsReceiptPOModel = goodsReceiptPOService.GetNewModel(userId);
                goodsReceiptPOModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, goodsReceiptPOModel);
        }

    }
}