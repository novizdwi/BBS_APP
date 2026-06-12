using Models;
using Models.Transaction.Purchasing;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Master.Item;


namespace Controllers.Master.Item
{
    public partial class ReactiveTagController : BaseController
    {
        string VIEW_DETAIL = "ReactiveTag";
        string VIEW_FORM_PARTIAL = "Partial/ReactiveTag_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/ReactiveTag_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/ReactiveTag_Panel_List_Partial";


        ReactiveTagService reactiveTagService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];


            reactiveTagService = new ReactiveTagService();
            ReactiveTagModel reactiveTagModel;
            if (Id == 0)
            {
                ViewBag.initNew = true;
                reactiveTagModel = reactiveTagService.GetNewModel(userId);
                reactiveTagModel._FormMode = FormModeEnum.New;
            }
            else
            {
                reactiveTagService = new ReactiveTagService();
                reactiveTagModel = reactiveTagService.GetById(userId, Id);
                reactiveTagModel._FormMode = FormModeEnum.Edit;
            }

            return View(VIEW_DETAIL, reactiveTagModel);
        }

        public ActionResult DetailPartial(long Id = 0, string copyFromForm = "", long copyFromId = 0)
        {
            int userId = (int)Session["userId"];


            ReactiveTagModel reactiveTagModel;

            reactiveTagService = new ReactiveTagService();
            if (Id == 0)
            {
                reactiveTagModel = reactiveTagService.GetNewModel(userId);
                reactiveTagModel._FormMode = FormModeEnum.New;
            }
            else
            {
                reactiveTagModel = reactiveTagService.GetById(userId, Id);
                if (reactiveTagModel != null)
                {
                    reactiveTagModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    reactiveTagModel = reactiveTagService.GetNewModel(userId);
                    reactiveTagModel._FormMode = FormModeEnum.New;
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, reactiveTagModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  ReactiveTagModel ReactiveTagModel)
        {
            int userId = (int)Session["userId"];

            ReactiveTagModel._UserId = (int)Session["userId"];
            reactiveTagService = new ReactiveTagService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = reactiveTagService.Add(ReactiveTagModel);
                ReactiveTagModel = reactiveTagService.GetById(userId, Id);
                ReactiveTagModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, ReactiveTagModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  ReactiveTagModel reactiveTagModel)
        {
            int userId = (int)Session["userId"];

            reactiveTagModel._UserId = (int)Session["userId"];
            reactiveTagService = new ReactiveTagService();
            reactiveTagModel._FormMode = FormModeEnum.Edit;



            //if (ModelState.IsValid)
            //{
            reactiveTagService.Update(reactiveTagModel);
            reactiveTagModel = reactiveTagService.GetById(userId, reactiveTagModel.Id);
            //}
            //else
            //{
            //    string message = GetErrorModel();

            //    throw new Exception(string.Format("[VALIDATION] {0}", message));
            //}

            return PartialView(VIEW_FORM_PARTIAL, reactiveTagModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  ReactiveTagModel reactiveTagModel)
        {
            int userId = (int)Session["userId"];

            reactiveTagModel._UserId = (int)Session["userId"];
            reactiveTagService = new ReactiveTagService();
            reactiveTagModel._FormMode = FormModeEnum.Edit;

            //reactiveTagService.Update(reactiveTagModel);
            //reactiveTagService.PostAPI(userId, reactiveTagModel.Id);
            reactiveTagService.Post(userId, reactiveTagModel.Id);
            reactiveTagModel = reactiveTagService.GetById(userId, reactiveTagModel.Id);

            if (reactiveTagModel != null)
            {
                reactiveTagModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                reactiveTagModel = reactiveTagService.GetNewModel(userId);
                reactiveTagModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, reactiveTagModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string CancelReason = "")
        {
            int userId = (int)Session["userId"];

            ReactiveTagModel reactiveTagModel;

            reactiveTagService = new ReactiveTagService();
            reactiveTagService.Cancel(userId, Id, CancelReason);

            reactiveTagModel = reactiveTagService.GetById(userId, Id);
            if (reactiveTagModel != null)
            {
                reactiveTagModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                reactiveTagModel = reactiveTagService.GetNewModel(userId);
                reactiveTagModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, reactiveTagModel);
        }

        public ContentResult ChooseItem(long Id, String[] Data)
        {
            int userId = (int)Session["userId"];

            reactiveTagService = new ReactiveTagService();
            var result = reactiveTagService.ChooseItem(userId, Id, Data);


            return Content(result.ToString());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RequestApproval(long id, int templateId, string approvalMessage = "")
        {
            int userId = (int)Session["userId"];

            ReactiveTagModel reactiveTagModel;

            reactiveTagService = new ReactiveTagService();
            reactiveTagService.RequestApproval(userId, id, templateId, approvalMessage);

            reactiveTagModel = reactiveTagService.GetById(userId, id);
            if (reactiveTagModel != null)
            {
                reactiveTagModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                reactiveTagModel = reactiveTagService.GetNewModel(userId);
                reactiveTagModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, reactiveTagModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Approve(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            ReactiveTagModel reactiveTagModel;

            reactiveTagService = new ReactiveTagService();
            reactiveTagService.Approve(userId, Id, ApprovalMessage);

            reactiveTagModel = reactiveTagService.GetById(userId, Id);
            if (reactiveTagModel != null)
            {
                reactiveTagModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                reactiveTagModel = reactiveTagService.GetNewModel(userId);
                reactiveTagModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, reactiveTagModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Reject(long Id, string ApprovalMessage = "")
        {
            int userId = (int)Session["userId"];

            ReactiveTagModel reactiveTagModel;

            reactiveTagService = new ReactiveTagService();
            reactiveTagService.Authorize(userId, Id, "Reject", ApprovalMessage);

            reactiveTagModel = reactiveTagService.GetById(userId, Id);
            if (reactiveTagModel != null)
            {
                reactiveTagModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                reactiveTagModel = reactiveTagService.GetNewModel(userId);
                reactiveTagModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, reactiveTagModel);
        }


    }
}