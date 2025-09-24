using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.InventoryReceived;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class InventoryReceivedController : BaseController
    {
        string VIEW_DETAIL = "InventoryReceived";
        string VIEW_FORM_PARTIAL = "Partial/InventoryReceived_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/InventoryReceived_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/InventoryReceived_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/InventoryReceived_Item_Lookup_Partial";

        InventoryReceivedService inventoryReceivedService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            inventoryReceivedService = new InventoryReceivedService();
            InventoryReceivedModel inventoryReceivedModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                inventoryReceivedModel = inventoryReceivedService.GetNewModel(userId);
                inventoryReceivedModel._FormMode = FormModeEnum.New;
            }
            else
            {
                inventoryReceivedService = new InventoryReceivedService();
                inventoryReceivedModel = inventoryReceivedService.GetById(userId, Id);
                if (inventoryReceivedModel != null)
                {
                    inventoryReceivedModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, inventoryReceivedModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            InventoryReceivedModel inventoryReceivedModel;

            inventoryReceivedService = new InventoryReceivedService();

            if (Id == 0)
            {
                inventoryReceivedModel = inventoryReceivedService.GetNewModel(userId);
                inventoryReceivedModel._FormMode = FormModeEnum.New;
            }
            else
            {
                inventoryReceivedModel = inventoryReceivedService.GetById(userId, Id);
                if (inventoryReceivedModel != null)
                {
                    inventoryReceivedModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryReceivedModel);
        }
        public ActionResult DetailInventorySend(long Id = 0)
        {
            int userId = (int)Session["userId"];

            inventoryReceivedService = new InventoryReceivedService();
            InventoryReceivedModel inventoryReceivedModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                inventoryReceivedModel = inventoryReceivedService.GetNewModel(userId);
                inventoryReceivedModel._FormMode = FormModeEnum.New;
            }
            else
            {
                inventoryReceivedService = new InventoryReceivedService();
                inventoryReceivedModel = inventoryReceivedService.GetInventorySendById(userId, Id);
                if (inventoryReceivedModel != null)
                {
                    inventoryReceivedModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, inventoryReceivedModel);
        }
        public ActionResult DetailPartialInventorySend(int Id = 0)
        {
            int userId = (int)Session["userId"];
            InventoryReceivedModel inventoryReceivedModel;

            inventoryReceivedService = new InventoryReceivedService();

            if (Id == 0)
            {
                inventoryReceivedModel = inventoryReceivedService.GetNewModel(userId);
                inventoryReceivedModel._FormMode = FormModeEnum.New;
            }
            else
            {
                inventoryReceivedModel = inventoryReceivedService.GetInventorySendById(userId, Id);
                if (inventoryReceivedModel != null)
                {
                    inventoryReceivedModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryReceivedModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  InventoryReceivedModel inventoryReceivedModel, List<InventoryReceived_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            inventoryReceivedModel._UserId = (int)Session["userId"];
            inventoryReceivedService = new InventoryReceivedService();
            inventoryReceivedModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = inventoryReceivedService.Add(inventoryReceivedModel);
                inventoryReceivedModel = inventoryReceivedService.GetById(userId, Id);
                inventoryReceivedModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryReceivedModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  InventoryReceivedModel inventoryReceivedModel, List<InventoryReceived_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            inventoryReceivedModel._UserId = (int)Session["userId"];
            inventoryReceivedService = new InventoryReceivedService();
            inventoryReceivedModel._FormMode = FormModeEnum.Edit;
            inventoryReceivedModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                inventoryReceivedService.Update(inventoryReceivedModel);

                inventoryReceivedModel = inventoryReceivedService.GetById(userId, inventoryReceivedModel.Id);
                if (inventoryReceivedModel != null)
                {
                    inventoryReceivedModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    inventoryReceivedModel = inventoryReceivedService.GetNewModel(userId);
                    inventoryReceivedModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryReceivedModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))]  InventoryReceivedModel inventoryReceivedModel, List<InventoryReceived_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            inventoryReceivedModel._UserId = (int)Session["userId"];
            inventoryReceivedService = new InventoryReceivedService();
            inventoryReceivedModel._FormMode = FormModeEnum.Edit;
            inventoryReceivedModel.ListDetails_ = Details;
            if (ModelState.IsValid)
            {

                inventoryReceivedService.Update(inventoryReceivedModel);
                inventoryReceivedService.Post(userId, inventoryReceivedModel.Id);

                inventoryReceivedModel = inventoryReceivedService.GetById(userId, inventoryReceivedModel.Id);
                if (inventoryReceivedModel != null)
                {
                    inventoryReceivedModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    inventoryReceivedModel = inventoryReceivedService.GetNewModel(userId);
                    inventoryReceivedModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryReceivedModel);
        }


    }

}