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


        StockSummaryOpnameService StockSummaryOpnameService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            StockSummaryOpnameService = new StockSummaryOpnameService();
            StockSummaryOpnameModel StockSummaryOpnameModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                StockSummaryOpnameModel = StockSummaryOpnameService.GetNewModel(userId);
                StockSummaryOpnameModel._FormMode = FormModeEnum.New;
            }
            else
            {
                StockSummaryOpnameService = new StockSummaryOpnameService();
                StockSummaryOpnameModel = StockSummaryOpnameService.GetById(userId, Id);
                StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, StockSummaryOpnameModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            StockSummaryOpnameModel StockSummaryOpnameModel;

            StockSummaryOpnameService = new StockSummaryOpnameService();
            if (Id == 0)
            {
                StockSummaryOpnameModel = StockSummaryOpnameService.GetNewModel(userId);
                StockSummaryOpnameModel._FormMode = FormModeEnum.New;
            }
            else
            {
                StockSummaryOpnameModel = StockSummaryOpnameService.GetById(userId, Id);
                if (StockSummaryOpnameModel != null)
                {
                    StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    StockSummaryOpnameModel = StockSummaryOpnameService.GetNewModel(userId);
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
            StockSummaryOpnameService = new StockSummaryOpnameService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = StockSummaryOpnameService.Add(StockSummaryOpnameModel);
                StockSummaryOpnameModel = StockSummaryOpnameService.GetById(userId, Id);
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
            StockSummaryOpnameService = new StockSummaryOpnameService();
            StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            StockSummaryOpnameService.Update(StockSummaryOpnameModel);
            StockSummaryOpnameModel = StockSummaryOpnameService.GetById(userId, StockSummaryOpnameModel.Id);
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
            StockSummaryOpnameService = new StockSummaryOpnameService();
            StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;

            //StockSummaryOpnameService.Update(StockSummaryOpnameModel, "Post");
            //StockSummaryOpnameService.PostAPI(userId, StockSummaryOpnameModel.Id);
            StockSummaryOpnameService.Post(userId, StockSummaryOpnameModel);
            StockSummaryOpnameModel = StockSummaryOpnameService.GetById(userId, StockSummaryOpnameModel.Id);

            if (StockSummaryOpnameModel != null)
            {
                StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                StockSummaryOpnameModel = StockSummaryOpnameService.GetNewModel(userId);
                StockSummaryOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            StockSummaryOpnameModel StockSummaryOpnameModel;

            StockSummaryOpnameService = new StockSummaryOpnameService();
            StockSummaryOpnameService.Cancel(userId, Id, CancelReason);

            StockSummaryOpnameModel = StockSummaryOpnameService.GetById(userId, Id);
            if (StockSummaryOpnameModel != null)
            {
                StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                StockSummaryOpnameModel = StockSummaryOpnameService.GetNewModel(userId);
                StockSummaryOpnameModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);
        }


        public ActionResult RefreshItem(StockSummaryOpnameModel StockSummaryOpnameModel)
        {
            int userId = (int)Session["userId"];

            StockSummaryOpnameService = new StockSummaryOpnameService();
            StockSummaryOpnameModel._FormMode = FormModeEnum.Edit;

            StockSummaryOpnameService.RefreshItem(userId, StockSummaryOpnameModel.Id);
            StockSummaryOpnameModel = StockSummaryOpnameService.GetById(userId, StockSummaryOpnameModel.Id);

            return PartialView(VIEW_FORM_PARTIAL, StockSummaryOpnameModel);

        }
    }
}