using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Transaction.StockOpname;

namespace Controllers.Transaction.StockOpname
{
    public partial class StockSummaryOpnameController : BaseController
    {
        string VIEW_DETAIL = "StockSummaryOpname";
        string VIEW_FORM_PARTIAL = "Partial/StockSummaryOpname_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/StockSummaryOpname_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/StockSummaryOpname_Panel_List_Partial";


        StockSummaryOpnameService stockSummaryOpnameService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            stockSummaryOpnameService = new StockSummaryOpnameService();
            StockSummaryOpnameModel stockSummaryOpnameModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                stockSummaryOpnameModel = stockSummaryOpnameService.GetNewModel(userId);
                stockSummaryOpnameModel._FormMode = FormModeEnum.New;
            }
            else
            {
                stockSummaryOpnameService = new StockSummaryOpnameService();
                stockSummaryOpnameModel = stockSummaryOpnameService.GetById(userId, Id);
                stockSummaryOpnameModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, stockSummaryOpnameModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            StockSummaryOpnameModel StockSummaryOpnameModel;

            stockSummaryOpnameService = new StockSummaryOpnameService();
            if (Id == 0)
            {
                StockSummaryOpnameModel = stockSummaryOpnameService.GetNewModel(userId);
                StockSummaryOpnameModel._FormMode = FormModeEnum.New;
            }
            else
            {
                StockSummaryOpnameModel = stockSummaryOpnameService.GetById(userId, Id);
                if (StockSummaryOpnameModel != null)
                {
                    StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    StockSummaryOpnameModel = stockSummaryOpnameService.GetNewModel(userId);
                    StockSummaryOpnameModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  StockSummaryOpnameModel StockSummaryOpnameModel)
        {
            int userId = (int)Session["userId"];

            StockSummaryOpnameModel._UserId = (int)Session["userId"];
            stockSummaryOpnameService = new StockSummaryOpnameService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = stockSummaryOpnameService.Add(StockSummaryOpnameModel);
                StockSummaryOpnameModel = stockSummaryOpnameService.GetById(userId, Id);
                StockSummaryOpnameModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  StockSummaryOpnameModel StockSummaryOpnameModel)
        {
            int userId = (int)Session["userId"];

            StockSummaryOpnameModel._UserId = (int)Session["userId"];
            stockSummaryOpnameService = new StockSummaryOpnameService();
            StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            stockSummaryOpnameService.Update(StockSummaryOpnameModel);
            StockSummaryOpnameModel = stockSummaryOpnameService.GetById(userId, StockSummaryOpnameModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  StockSummaryOpnameModel StockSummaryOpnameModel)
        {
            int userId = (int)Session["userId"];

            StockSummaryOpnameModel._UserId = (int)Session["userId"];
            stockSummaryOpnameService = new StockSummaryOpnameService();
            StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;

            //StockSummaryOpnameService.Update(StockSummaryOpnameModel, "Post");
            //StockSummaryOpnameService.PostAPI(userId, StockSummaryOpnameModel.Id);
            stockSummaryOpnameService.Post(userId, StockSummaryOpnameModel);
            StockSummaryOpnameModel = stockSummaryOpnameService.GetById(userId, StockSummaryOpnameModel.Id);

            if (StockSummaryOpnameModel != null)
            {
                StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                StockSummaryOpnameModel = stockSummaryOpnameService.GetNewModel(userId);
                StockSummaryOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            StockSummaryOpnameModel StockSummaryOpnameModel;

            stockSummaryOpnameService = new StockSummaryOpnameService();
            stockSummaryOpnameService.Cancel(userId, Id, CancelReason);

            StockSummaryOpnameModel = stockSummaryOpnameService.GetById(userId, Id);
            if (StockSummaryOpnameModel != null)
            {
                StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                StockSummaryOpnameModel = stockSummaryOpnameService.GetNewModel(userId);
                StockSummaryOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);
        }


        public ActionResult RefreshItem(StockSummaryOpnameModel StockSummaryOpnameModel)
        {
            int userId = (int)Session["userId"];

            stockSummaryOpnameService = new StockSummaryOpnameService();
            StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;

            stockSummaryOpnameService.RefreshItem(userId, StockSummaryOpnameModel.Id);
            StockSummaryOpnameModel = stockSummaryOpnameService.GetById(userId, StockSummaryOpnameModel.Id);

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);

        }
    }
}