using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.FuelControl;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class FuelControlController : BaseController
    {
        string VIEW_DETAIL = "FuelControl";
        string VIEW_FORM_PARTIAL = "Partial/FuelControl_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/FuelControl_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/FuelControl_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/FuelControl_Item_Lookup_Partial";

        FuelControlService fuelControlService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            fuelControlService = new FuelControlService();
            FuelControlModel fuelControlModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                fuelControlModel = fuelControlService.GetNewModel(userId);
                fuelControlModel._FormMode = FormModeEnum.New;
            }
            else
            {
                fuelControlService = new FuelControlService();
                fuelControlModel = fuelControlService.GetById(userId, Id);
                if (fuelControlModel != null)
                {
                    fuelControlModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, fuelControlModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            FuelControlModel fuelControlModel;

            fuelControlService = new FuelControlService();

            if (Id == 0)
            {
                fuelControlModel = fuelControlService.GetNewModel(userId);
                fuelControlModel._FormMode = FormModeEnum.New;
            }
            else
            {
                fuelControlModel = fuelControlService.GetById(userId, Id);
                if (fuelControlModel != null)
                {
                    fuelControlModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, fuelControlModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  FuelControlModel fuelControlModel, List<FuelControl_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            fuelControlModel._UserId = (int)Session["userId"];
            fuelControlModel._UserId = userId;
            fuelControlService = new FuelControlService();
            fuelControlModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = fuelControlService.Add(fuelControlModel);
                fuelControlModel = fuelControlService.GetById(userId, Id);
                fuelControlModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, fuelControlModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  FuelControlModel fuelControlModel, List<FuelControl_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            fuelControlModel._UserId = (int)Session["userId"];
            fuelControlService = new FuelControlService();
            fuelControlModel._FormMode = FormModeEnum.Edit;
            fuelControlModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                fuelControlService.Update(fuelControlModel);

                fuelControlModel = fuelControlService.GetById(userId, fuelControlModel.Id);
                if (fuelControlModel != null)
                {
                    fuelControlModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    fuelControlModel = fuelControlService.GetNewModel(userId);
                    fuelControlModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, fuelControlModel);
        }
        
        

    }

}