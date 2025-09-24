using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.PerawatanEngine;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class PerawatanEngineController : BaseController
    {
        string VIEW_DETAIL = "PerawatanEngine";
        string VIEW_FORM_PARTIAL = "Partial/PerawatanEngine_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/PerawatanEngine_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/PerawatanEngine_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/PerawatanEngine_Item_Lookup_Partial";

        PerawatanEngineService perawatanEngineService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            perawatanEngineService = new PerawatanEngineService();
            PerawatanEngineModel perawatanEngineModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                perawatanEngineModel = perawatanEngineService.GetNewModel(userId);
                perawatanEngineModel._FormMode = FormModeEnum.New;
            }
            else
            {
                perawatanEngineService = new PerawatanEngineService();
                perawatanEngineModel = perawatanEngineService.GetById(userId, Id);
                if (perawatanEngineModel != null)
                {
                    perawatanEngineModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, perawatanEngineModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            PerawatanEngineModel perawatanEngineModel;

            perawatanEngineService = new PerawatanEngineService();

            if (Id == 0)
            {
                perawatanEngineModel = perawatanEngineService.GetNewModel(userId);
                perawatanEngineModel._FormMode = FormModeEnum.New;
            }
            else
            {
                perawatanEngineModel = perawatanEngineService.GetById(userId, Id);
                if (perawatanEngineModel != null)
                {
                    perawatanEngineModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanEngineModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  PerawatanEngineModel perawatanEngineModel, List<PerawatanEngine_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            perawatanEngineModel._UserId = (int)Session["userId"];
            perawatanEngineModel._UserId = userId;
            perawatanEngineService = new PerawatanEngineService();
            perawatanEngineModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = perawatanEngineService.Add(perawatanEngineModel);
                perawatanEngineModel = perawatanEngineService.GetById(userId, Id);
                perawatanEngineModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanEngineModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  PerawatanEngineModel perawatanEngineModel, List<PerawatanEngine_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            perawatanEngineModel._UserId = (int)Session["userId"];
            perawatanEngineService = new PerawatanEngineService();
            perawatanEngineModel._FormMode = FormModeEnum.Edit;
            perawatanEngineModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                perawatanEngineService.Update(perawatanEngineModel);

                perawatanEngineModel = perawatanEngineService.GetById(userId, perawatanEngineModel.Id);
                if (perawatanEngineModel != null)
                {
                    perawatanEngineModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    perawatanEngineModel = perawatanEngineService.GetNewModel(userId);
                    perawatanEngineModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanEngineModel);
        }
        
        

    }

}