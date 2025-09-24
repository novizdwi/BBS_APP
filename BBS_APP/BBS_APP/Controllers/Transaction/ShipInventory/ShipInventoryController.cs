using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.ShipInventory;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class ShipInventoryController : BaseController
    {
        string VIEW_DETAIL = "ShipInventory";
        string VIEW_FORM_PARTIAL = "Partial/ShipInventory_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/ShipInventory_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/ShipInventory_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/ShipInventory_Item_Lookup_Partial";

        ShipInventoryService shipInventoryService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            shipInventoryService = new ShipInventoryService();
            ShipInventoryModel shipInventoryModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                shipInventoryModel = shipInventoryService.GetNewModel(userId);
                shipInventoryModel._FormMode = FormModeEnum.New;
            }
            else
            {
                shipInventoryService = new ShipInventoryService();
                shipInventoryModel = shipInventoryService.GetById(userId, Id);
                if (shipInventoryModel != null)
                {
                    shipInventoryModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, shipInventoryModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            ShipInventoryModel shipInventoryModel;

            shipInventoryService = new ShipInventoryService();

            if (Id == 0)
            {
                shipInventoryModel = shipInventoryService.GetNewModel(userId);
                shipInventoryModel._FormMode = FormModeEnum.New;
            }
            else
            {
                shipInventoryModel = shipInventoryService.GetById(userId, Id);
                if (shipInventoryModel != null)
                {
                    shipInventoryModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, shipInventoryModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  ShipInventoryModel shipInventoryModel, List<ShipInventory_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            shipInventoryModel._UserId = (int)Session["userId"];
            shipInventoryModel._UserId = userId;
            shipInventoryService = new ShipInventoryService();
            shipInventoryModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = shipInventoryService.Add(shipInventoryModel);
                shipInventoryModel = shipInventoryService.GetById(userId, Id);
                shipInventoryModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, shipInventoryModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  ShipInventoryModel shipInventoryModel, List<ShipInventory_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            shipInventoryModel._UserId = (int)Session["userId"];
            shipInventoryService = new ShipInventoryService();
            shipInventoryModel._FormMode = FormModeEnum.Edit;
            shipInventoryModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                shipInventoryService.Update(shipInventoryModel);

                shipInventoryModel = shipInventoryService.GetById(userId, shipInventoryModel.Id);
                if (shipInventoryModel != null)
                {
                    shipInventoryModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    shipInventoryModel = shipInventoryService.GetNewModel(userId);
                    shipInventoryModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, shipInventoryModel);
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))] ShipInventoryModel shipInventoryModel, List<ShipInventory_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            shipInventoryModel._UserId = (int)Session["userId"];
            shipInventoryService = new ShipInventoryService();
            shipInventoryModel._FormMode = FormModeEnum.Edit;
            shipInventoryModel.ListDetails_ = Details;
            if (ModelState.IsValid)
            {

                shipInventoryService.Update(shipInventoryModel);
                shipInventoryService.Post(userId, shipInventoryModel.Id);

                shipInventoryModel = shipInventoryService.GetById(userId, shipInventoryModel.Id);
                if (shipInventoryModel != null)
                {
                    shipInventoryModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    shipInventoryModel = shipInventoryService.GetNewModel(userId);
                    shipInventoryModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, shipInventoryModel);
        }


    }

}