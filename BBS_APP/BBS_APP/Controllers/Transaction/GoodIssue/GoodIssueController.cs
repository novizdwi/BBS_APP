using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.GoodIssue;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class GoodIssueController : BaseController
    {
        string VIEW_DETAIL = "GoodIssue";
        string VIEW_FORM_PARTIAL = "Partial/GoodIssue_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/GoodIssue_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/GoodIssue_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/GoodIssue_Item_Lookup_Partial";

        GoodIssueService goodIssueService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            goodIssueService = new GoodIssueService();
            GoodIssueModel goodIssueModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                goodIssueModel = goodIssueService.GetNewModel(userId);
                goodIssueModel._FormMode = FormModeEnum.New;
            }
            else
            {
                goodIssueService = new GoodIssueService();
                goodIssueModel = goodIssueService.GetById(userId, Id);
                if (goodIssueModel != null)
                {
                    goodIssueModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, goodIssueModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            GoodIssueModel goodIssueModel;

            goodIssueService = new GoodIssueService();

            if (Id == 0)
            {
                goodIssueModel = goodIssueService.GetNewModel(userId);
                goodIssueModel._FormMode = FormModeEnum.New;
            }
            else
            {
                goodIssueModel = goodIssueService.GetById(userId, Id);
                if (goodIssueModel != null)
                {
                    goodIssueModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, goodIssueModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  GoodIssueModel goodIssueModel, List<GoodIssue_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            goodIssueModel._UserId = (int)Session["userId"];
            goodIssueService = new GoodIssueService();
            goodIssueModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = goodIssueService.Add(goodIssueModel);
                goodIssueModel = goodIssueService.GetById(userId, Id);
                goodIssueModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, goodIssueModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  GoodIssueModel goodIssueModel, List<GoodIssue_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            goodIssueModel._UserId = (int)Session["userId"];
            goodIssueService = new GoodIssueService();
            goodIssueModel._FormMode = FormModeEnum.Edit;
            goodIssueModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                goodIssueService.Update(goodIssueModel);

                goodIssueModel = goodIssueService.GetById(userId, goodIssueModel.Id);
                if (goodIssueModel != null)
                {
                    goodIssueModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    goodIssueModel = goodIssueService.GetNewModel(userId);
                    goodIssueModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, goodIssueModel);
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  GoodIssueModel goodIssueModel, List<GoodIssue_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            goodIssueModel._UserId = (int)Session["userId"];
            goodIssueService = new GoodIssueService();
            goodIssueModel._FormMode = FormModeEnum.Edit;
            goodIssueModel.ListDetails_ = Details;
            if (ModelState.IsValid)
            {

                goodIssueService.Update(goodIssueModel);
                goodIssueService.Post(userId, goodIssueModel.Id);

                goodIssueModel = goodIssueService.GetById(userId, goodIssueModel.Id);
                if (goodIssueModel != null)
                {
                    goodIssueModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    goodIssueModel = goodIssueService.GetNewModel(userId);
                    goodIssueModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, goodIssueModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id)
        {
            int userId = (int)Session["userId"];
            GoodIssueModel goodIssueModel;

            goodIssueService = new GoodIssueService();
            goodIssueService.Cancel(userId, Id);

            goodIssueModel = goodIssueService.GetById(userId, Id);
            if (goodIssueModel != null)
            {
                goodIssueModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                goodIssueModel = goodIssueService.GetNewModel(userId);
                goodIssueModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, goodIssueModel);
        }


    }

}