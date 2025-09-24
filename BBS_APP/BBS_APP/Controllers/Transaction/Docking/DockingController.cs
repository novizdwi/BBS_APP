using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.Docking;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class DockingController : BaseController
    {
        string VIEW_DETAIL = "Docking";
        string VIEW_FORM_PARTIAL = "Partial/Docking_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Docking_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Docking_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/Docking_Item_Lookup_Partial";

        DockingService dockingService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            dockingService = new DockingService();
            DockingModel dockingModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                dockingModel = dockingService.GetNewModel(userId);
                dockingModel._FormMode = FormModeEnum.New;
            }
            else
            {
                dockingService = new DockingService();
                dockingModel = dockingService.GetById(userId, Id);
                if (dockingModel != null)
                {
                    dockingModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, dockingModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            DockingModel dockingModel;

            dockingService = new DockingService();

            if (Id == 0)
            {
                dockingModel = dockingService.GetNewModel(userId);
                dockingModel._FormMode = FormModeEnum.New;
            }
            else
            {
                dockingModel = dockingService.GetById(userId, Id);
                if (dockingModel != null)
                {
                    dockingModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, dockingModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  DockingModel dockingModel, List<Docking_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            dockingModel._UserId = (int)Session["userId"];
            dockingModel._UserId = userId;
            dockingService = new DockingService();
            dockingModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = dockingService.Add(dockingModel);
                dockingModel = dockingService.GetById(userId, Id);
                dockingModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, dockingModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  DockingModel dockingModel, List<Docking_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            dockingModel._UserId = (int)Session["userId"];
            dockingService = new DockingService();
            dockingModel._FormMode = FormModeEnum.Edit;
            dockingModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                dockingService.Update(dockingModel);

                dockingModel = dockingService.GetById(userId, dockingModel.Id);
                if (dockingModel != null)
                {
                    dockingModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    dockingModel = dockingService.GetNewModel(userId);
                    dockingModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, dockingModel);
        }
        
        

    }

}