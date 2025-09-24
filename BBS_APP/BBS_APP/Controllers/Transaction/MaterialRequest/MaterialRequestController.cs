using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.MaterialRequest;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class MaterialRequestController : BaseController
    {
        string VIEW_DETAIL = "MaterialRequest";
        string VIEW_FORM_PARTIAL = "Partial/MaterialRequest_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/MaterialRequest_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/MaterialRequest_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/MaterialRequest_Item_Lookup_Partial";

        MaterialRequestService materialRequestService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            materialRequestService = new MaterialRequestService();
            MaterialRequestModel materialRequestModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                materialRequestModel = materialRequestService.GetNewModel(userId);
                materialRequestModel._FormMode = FormModeEnum.New;
            }
            else
            {
                materialRequestService = new MaterialRequestService();
                materialRequestModel = materialRequestService.GetById(userId, Id);
                if (materialRequestModel != null)
                {
                    materialRequestModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, materialRequestModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            MaterialRequestModel materialRequestModel;

            materialRequestService = new MaterialRequestService();

            if (Id == 0)
            {
                materialRequestModel = materialRequestService.GetNewModel(userId);
                materialRequestModel._FormMode = FormModeEnum.New;
            }
            else
            {
                materialRequestModel = materialRequestService.GetById(userId, Id);
                if (materialRequestModel != null)
                {
                    materialRequestModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, materialRequestModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  MaterialRequestModel materialRequestModel, List<MaterialRequest_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            materialRequestModel._UserId = (int)Session["userId"];
            materialRequestModel._UserId = userId;
            materialRequestService = new MaterialRequestService();
            materialRequestModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = materialRequestService.Add(materialRequestModel);
                materialRequestModel = materialRequestService.GetById(userId, Id);
                materialRequestModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, materialRequestModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  MaterialRequestModel materialRequestModel, List<MaterialRequest_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            materialRequestModel._UserId = (int)Session["userId"];
            materialRequestService = new MaterialRequestService();
            materialRequestModel._FormMode = FormModeEnum.Edit;
            materialRequestModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                materialRequestService.Update(materialRequestModel);

                materialRequestModel = materialRequestService.GetById(userId, materialRequestModel.Id);
                if (materialRequestModel != null)
                {
                    materialRequestModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    materialRequestModel = materialRequestService.GetNewModel(userId);
                    materialRequestModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, materialRequestModel);
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  MaterialRequestModel materialRequestModel, List<MaterialRequest_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            materialRequestModel._UserId = (int)Session["userId"];
            materialRequestService = new MaterialRequestService();
            materialRequestModel._FormMode = FormModeEnum.Edit;
            materialRequestModel.ListDetails_ = Details;
            if (ModelState.IsValid)
            {

                materialRequestService.Update(materialRequestModel);
                materialRequestService.Post(userId, materialRequestModel.Id);

                materialRequestModel = materialRequestService.GetById(userId, materialRequestModel.Id);
                if (materialRequestModel != null)
                {
                    materialRequestModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    materialRequestModel = materialRequestService.GetNewModel(userId);
                    materialRequestModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, materialRequestModel);
        }


    }

}