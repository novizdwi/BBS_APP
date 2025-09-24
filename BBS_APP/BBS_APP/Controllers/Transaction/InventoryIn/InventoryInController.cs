using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.IO;
using System.Threading;



using System.Net;

using Models;
using Models.Transaction.InventoryIn;


namespace Controllers.Transaction
{
    public partial class InventoryInController : BaseController
    {

        string VIEW_DETAIL = "InventoryIn";
        string VIEW_FORM_PARTIAL = "Partial/InventoryIn_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/InventoryIn_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/InventoryIn_Panel_List_Partial";

        InventoryInService inventoryInService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }


        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            inventoryInService = new InventoryInService();
            InventoryInModel inventoryInModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                inventoryInModel = inventoryInService.GetNewModel(userId);
                inventoryInModel._FormMode = FormModeEnum.New;
            }
            else
            {
                inventoryInService = new InventoryInService();
                inventoryInModel = inventoryInService.GetById(userId, Id);
                if (inventoryInModel != null)
                {
                    inventoryInModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, inventoryInModel);
        }


        public ActionResult DetailPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            inventoryInService = new InventoryInService();
            InventoryInModel inventoryInModel;

            if (Id == 0)
            {

                ViewBag.initNew = true;

                inventoryInModel = inventoryInService.GetNewModel(userId);
                inventoryInModel._FormMode = FormModeEnum.New;

            }
            else
            {
                inventoryInModel = inventoryInService.GetById(userId, Id);
                if (inventoryInModel != null)
                {
                    inventoryInModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }


            return PartialView(VIEW_FORM_PARTIAL, inventoryInModel);
        }
        public ActionResult DetailPartialPO(long Id = 0)
        {
            int userId = (int)Session["userId"];
            InventoryInModel inventoryInModel;

            inventoryInService = new InventoryInService();

            if (Id == 0)
            {
                inventoryInModel = inventoryInService.GetNewModel(userId);
                inventoryInModel._FormMode = FormModeEnum.New;
            }
            else
            {
                inventoryInModel = inventoryInService.GetPoByIdS(userId, Id);
                if (inventoryInModel != null)
                {
                    inventoryInModel._FormMode = FormModeEnum.New;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryInModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ChoosePurchaseOrder(int[] data, long Id)
        {
            int userId = (int)Session["userId"];



            InventoryInModel inventoryInModel;
            inventoryInService = new InventoryInService();

            inventoryInModel = inventoryInService.GetPoByIdM(userId, data, Id);
            if (inventoryInModel != null)
            {

                inventoryInModel._FormMode = FormModeEnum.Edit;

            }

            if (inventoryInModel == null)
            {
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryInModel);
        }
       



        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  InventoryInModel inventoryInModel, List<InventoryIn_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            inventoryInModel._UserId = (int)Session["userId"];
            inventoryInService = new InventoryInService();
            inventoryInModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = inventoryInService.Add(inventoryInModel);
                inventoryInModel = inventoryInService.GetById(userId, Id);
                inventoryInModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryInModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  InventoryInModel inventoryInModel, List<InventoryIn_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            inventoryInModel._UserId = (int)Session["userId"];
            inventoryInService = new InventoryInService();
            inventoryInModel._FormMode = FormModeEnum.Edit;
            inventoryInModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                inventoryInService.Update(inventoryInModel);

                inventoryInModel = inventoryInService.GetById(userId, inventoryInModel.Id);
                if (inventoryInModel != null)
                {
                    inventoryInModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    inventoryInModel = inventoryInService.GetNewModel(userId);
                    inventoryInModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Post(long Id)
        {
            int userId = (int)Session["userId"];
            InventoryInModel inventoryInModel;

            inventoryInService = new InventoryInService();
            inventoryInService.Post(userId, Id);

            inventoryInModel = inventoryInService.GetById(userId, Id);
            if (inventoryInModel != null)
            {
                inventoryInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                inventoryInModel = inventoryInService.GetNewModel(userId);
                inventoryInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryInModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id)
        {
            int userId = (int)Session["userId"];
            InventoryInModel inventoryInModel;

            inventoryInService = new InventoryInService();
            inventoryInService.Cancel(userId, Id);

            inventoryInModel = inventoryInService.GetById(userId, Id);
            if (inventoryInModel != null)
            {
                inventoryInModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                inventoryInModel = inventoryInService.GetNewModel(userId);
                inventoryInModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryInModel);
        }

    }
}