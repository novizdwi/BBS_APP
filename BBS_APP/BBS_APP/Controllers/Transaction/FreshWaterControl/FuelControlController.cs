using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.FreshWaterControl;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class FreshWaterControlController : BaseController
    {
        string VIEW_DETAIL = "FreshWaterControl";
        string VIEW_FORM_PARTIAL = "Partial/FreshWaterControl_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/FreshWaterControl_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/FreshWaterControl_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/FreshWaterControl_Item_Lookup_Partial";

        FreshWaterControlService freshWaterControlService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            freshWaterControlService = new FreshWaterControlService();
            FreshWaterControlModel freshWaterControlModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                freshWaterControlModel = freshWaterControlService.GetNewModel(userId);
                freshWaterControlModel._FormMode = FormModeEnum.New;
            }
            else
            {
                freshWaterControlService = new FreshWaterControlService();
                freshWaterControlModel = freshWaterControlService.GetById(userId, Id);
                if (freshWaterControlModel != null)
                {
                    freshWaterControlModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, freshWaterControlModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            FreshWaterControlModel freshWaterControlModel;

            freshWaterControlService = new FreshWaterControlService();

            if (Id == 0)
            {
                freshWaterControlModel = freshWaterControlService.GetNewModel(userId);
                freshWaterControlModel._FormMode = FormModeEnum.New;
            }
            else
            {
                freshWaterControlModel = freshWaterControlService.GetById(userId, Id);
                if (freshWaterControlModel != null)
                {
                    freshWaterControlModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, freshWaterControlModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  FreshWaterControlModel freshWaterControlModel, List<FreshWaterControl_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            freshWaterControlModel._UserId = (int)Session["userId"];
            freshWaterControlModel._UserId = userId;
            freshWaterControlService = new FreshWaterControlService();
            freshWaterControlModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = freshWaterControlService.Add(freshWaterControlModel);
                freshWaterControlModel = freshWaterControlService.GetById(userId, Id);
                freshWaterControlModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, freshWaterControlModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  FreshWaterControlModel freshWaterControlModel, List<FreshWaterControl_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            freshWaterControlModel._UserId = (int)Session["userId"];
            freshWaterControlService = new FreshWaterControlService();
            freshWaterControlModel._FormMode = FormModeEnum.Edit;
            freshWaterControlModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                freshWaterControlService.Update(freshWaterControlModel);

                freshWaterControlModel = freshWaterControlService.GetById(userId, freshWaterControlModel.Id);
                if (freshWaterControlModel != null)
                {
                    freshWaterControlModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    freshWaterControlModel = freshWaterControlService.GetNewModel(userId);
                    freshWaterControlModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, freshWaterControlModel);
        }
        
        

    }

}