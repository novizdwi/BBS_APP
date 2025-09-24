using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.CleaningTank;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class CleaningTankController : BaseController
    {
        string VIEW_DETAIL = "CleaningTank";
        string VIEW_FORM_PARTIAL = "Partial/CleaningTank_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/CleaningTank_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/CleaningTank_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/CleaningTank_Item_Lookup_Partial";

        CleaningTankService cleaningTankService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            cleaningTankService = new CleaningTankService();
            CleaningTankModel cleaningTankModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                cleaningTankModel = cleaningTankService.GetNewModel(userId);
                cleaningTankModel._FormMode = FormModeEnum.New;
            }
            else
            {
                cleaningTankService = new CleaningTankService();
                cleaningTankModel = cleaningTankService.GetById(userId, Id);
                if (cleaningTankModel != null)
                {
                    cleaningTankModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, cleaningTankModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            CleaningTankModel cleaningTankModel;

            cleaningTankService = new CleaningTankService();

            if (Id == 0)
            {
                cleaningTankModel = cleaningTankService.GetNewModel(userId);
                cleaningTankModel._FormMode = FormModeEnum.New;
            }
            else
            {
                cleaningTankModel = cleaningTankService.GetById(userId, Id);
                if (cleaningTankModel != null)
                {
                    cleaningTankModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, cleaningTankModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  CleaningTankModel cleaningTankModel, List<CleaningTank_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            cleaningTankModel._UserId = (int)Session["userId"];
            cleaningTankModel._UserId = userId;
            cleaningTankService = new CleaningTankService();
            cleaningTankModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = cleaningTankService.Add(cleaningTankModel);
                cleaningTankModel = cleaningTankService.GetById(userId, Id);
                cleaningTankModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, cleaningTankModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  CleaningTankModel cleaningTankModel, List<CleaningTank_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            cleaningTankModel._UserId = (int)Session["userId"];
            cleaningTankService = new CleaningTankService();
            cleaningTankModel._FormMode = FormModeEnum.Edit;
            cleaningTankModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                cleaningTankService.Update(cleaningTankModel);

                cleaningTankModel = cleaningTankService.GetById(userId, cleaningTankModel.Id);
                if (cleaningTankModel != null)
                {
                    cleaningTankModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    cleaningTankModel = cleaningTankService.GetNewModel(userId);
                    cleaningTankModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, cleaningTankModel);
        }
        
        

    }

}