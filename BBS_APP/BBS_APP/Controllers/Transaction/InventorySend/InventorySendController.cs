using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.InventorySend;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class InventorySendController : BaseController
    {
        string VIEW_DETAIL = "InventorySend";
        string VIEW_FORM_PARTIAL = "Partial/InventorySend_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/InventorySend_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/InventorySend_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/InventorySend_Item_Lookup_Partial";

        InventorySendService inventorySendService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            inventorySendService = new InventorySendService();
            InventorySendModel inventorySendModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                inventorySendModel = inventorySendService.GetNewModel(userId);
                inventorySendModel._FormMode = FormModeEnum.New;
            }
            else
            {
                inventorySendService = new InventorySendService();
                inventorySendModel = inventorySendService.GetById(userId, Id);
                if (inventorySendModel != null)
                {
                    inventorySendModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, inventorySendModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            InventorySendModel inventorySendModel;

            inventorySendService = new InventorySendService();

            if (Id == 0)
            {
                inventorySendModel = inventorySendService.GetNewModel(userId);
                inventorySendModel._FormMode = FormModeEnum.New;
            }
            else
            {
                inventorySendModel = inventorySendService.GetById(userId, Id);
                if (inventorySendModel != null)
                {
                    inventorySendModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, inventorySendModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  InventorySendModel inventorySendModel, List<InventorySend_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            inventorySendModel._UserId = (int)Session["userId"];
            inventorySendService = new InventorySendService();
            inventorySendModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = inventorySendService.Add(inventorySendModel);
                inventorySendModel = inventorySendService.GetById(userId, Id);
                inventorySendModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, inventorySendModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  InventorySendModel inventorySendModel, List<InventorySend_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            inventorySendModel._UserId = (int)Session["userId"];
            inventorySendService = new InventorySendService();
            inventorySendModel._FormMode = FormModeEnum.Edit;
            inventorySendModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                inventorySendService.Update(inventorySendModel);

                inventorySendModel = inventorySendService.GetById(userId, inventorySendModel.Id);
                if (inventorySendModel != null)
                {
                    inventorySendModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    inventorySendModel = inventorySendService.GetNewModel(userId);
                    inventorySendModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, inventorySendModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  InventorySendModel inventorySendModel, List<InventorySend_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            inventorySendModel._UserId = (int)Session["userId"];
            inventorySendService = new InventorySendService();
            inventorySendModel._FormMode = FormModeEnum.Edit;
            inventorySendModel.ListDetails_ = Details;
            if (ModelState.IsValid)
            {

                inventorySendService.Update(inventorySendModel);
                inventorySendService.Post(userId, inventorySendModel.Id);

                inventorySendModel = inventorySendService.GetById(userId, inventorySendModel.Id);
                if (inventorySendModel != null)
                {
                    inventorySendModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    inventorySendModel = inventorySendService.GetNewModel(userId);
                    inventorySendModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, inventorySendModel);
        }


    }

}