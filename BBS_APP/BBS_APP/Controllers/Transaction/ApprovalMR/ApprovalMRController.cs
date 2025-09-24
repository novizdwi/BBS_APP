using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.ApprovalMR;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class ApprovalMRController : BaseController
    {
        string VIEW_DETAIL = "ApprovalMR";
        string VIEW_FORM_PARTIAL = "Partial/ApprovalMR_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/ApprovalMR_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/ApprovalMR_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/ApprovalMR_Item_Lookup_Partial";

        ApprovalMRService approvalMRService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            approvalMRService = new ApprovalMRService();
            ApprovalMRModel approvalMRModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                approvalMRModel = approvalMRService.GetNewModel(userId);
                approvalMRModel._FormMode = FormModeEnum.New;
            }
            else
            {
                approvalMRService = new ApprovalMRService();
                approvalMRModel = approvalMRService.GetById(userId, Id);
                if (approvalMRModel != null)
                {
                    approvalMRModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, approvalMRModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            ApprovalMRModel approvalMRModel;

            approvalMRService = new ApprovalMRService();

            if (Id == 0)
            {
                approvalMRModel = approvalMRService.GetNewModel(userId);
                approvalMRModel._FormMode = FormModeEnum.New;
            }
            else
            {
                approvalMRModel = approvalMRService.GetById(userId, Id);
                if (approvalMRModel != null)
                {
                    approvalMRModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalMRModel);
        }
        public ActionResult DetailMR(long Id = 0)
        {
            int userId = (int)Session["userId"];

            approvalMRService = new ApprovalMRService();
            ApprovalMRModel approvalMRModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                approvalMRModel = approvalMRService.GetNewModel(userId);
                approvalMRModel._FormMode = FormModeEnum.New;
            }
            else
            {
                approvalMRService = new ApprovalMRService();
                approvalMRModel = approvalMRService.GetByIdMR(userId, Id);
                if (approvalMRModel != null)
                {
                    approvalMRModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, approvalMRModel);
        }
        public ActionResult DetailPartialMR(int Id = 0)
        {
            int userId = (int)Session["userId"];
            ApprovalMRModel approvalMRModel;

            approvalMRService = new ApprovalMRService();

            if (Id == 0)
            {
                approvalMRModel = approvalMRService.GetNewModel(userId);
                approvalMRModel._FormMode = FormModeEnum.New;
            }
            else
            {
                approvalMRModel = approvalMRService.GetByIdMR(userId, Id);
                if (approvalMRModel != null)
                {
                    approvalMRModel._FormMode = FormModeEnum.New;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalMRModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  ApprovalMRModel approvalMRModel, List<ApprovalMR_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            approvalMRModel._UserId = (int)Session["userId"];
            approvalMRService = new ApprovalMRService();
            approvalMRModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = approvalMRService.Add(approvalMRModel);
                approvalMRModel = approvalMRService.GetById(userId, Id);
                approvalMRModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalMRModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  ApprovalMRModel approvalMRModel, List<ApprovalMR_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            approvalMRModel._UserId = (int)Session["userId"];
            approvalMRService = new ApprovalMRService();
            approvalMRModel._FormMode = FormModeEnum.Edit;
            approvalMRModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                approvalMRService.Update(approvalMRModel);

                approvalMRModel = approvalMRService.GetById(userId, approvalMRModel.Id);
                if (approvalMRModel != null)
                {
                    approvalMRModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    approvalMRModel = approvalMRService.GetNewModel(userId);
                    approvalMRModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalMRModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  ApprovalMRModel approvalMRModel, List<ApprovalMR_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            approvalMRModel._UserId = (int)Session["userId"];
            approvalMRService = new ApprovalMRService();
            approvalMRModel._FormMode = FormModeEnum.Edit;
            approvalMRModel.ListDetails_ = Details;
            if (ModelState.IsValid)
            {
                
                approvalMRService.Post(userId, approvalMRModel.Id);

                approvalMRModel = approvalMRService.GetById(userId, approvalMRModel.Id);
                if (approvalMRModel != null)
                    {
                        approvalMRModel._FormMode = FormModeEnum.Edit;
                    }
                else
                    {
                        approvalMRModel = approvalMRService.GetNewModel(userId);
                        approvalMRModel._FormMode = FormModeEnum.New;
                    }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalMRModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, long BaseId)
        {
            int userId = (int)Session["userId"];
            ApprovalMRModel approvalMRModel;

            approvalMRService = new ApprovalMRService();
            approvalMRService.Cancel(userId, Id, BaseId);

            approvalMRModel = approvalMRService.GetById(userId, Id);
            if (approvalMRModel != null)
            {
                approvalMRModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                approvalMRModel = approvalMRService.GetNewModel(userId);
                approvalMRModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalMRModel);
        }
    }
        
}