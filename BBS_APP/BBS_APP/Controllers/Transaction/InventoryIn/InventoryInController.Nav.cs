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

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            InventoryInModel inventoryInModel;
            inventoryInService = new InventoryInService();

            inventoryInModel = inventoryInService.NavFirst(userId);
            if (inventoryInModel != null)
            {
                inventoryInModel._FormMode = FormModeEnum.Edit;
            }

            if (inventoryInModel == null)
            {
                //InventoryInModel = InventoryInService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryInModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            InventoryInModel inventoryInModel;
            inventoryInService = new InventoryInService();

            inventoryInModel = inventoryInService.NavPrevious(userId, Id);
            if (inventoryInModel != null)
            {
                inventoryInModel._FormMode = FormModeEnum.Edit;
            }

            if (inventoryInModel == null)
            {
                //InventoryInModel = InventoryInService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryInModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            InventoryInModel inventoryInModel;
            inventoryInService = new InventoryInService();

            inventoryInModel = inventoryInService.NavNext(userId, Id);
            if (inventoryInModel != null)
            {

                inventoryInModel._FormMode = FormModeEnum.Edit;

            }

            if (inventoryInModel == null)
            {
                //InventoryInModel = InventoryInService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryInModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            InventoryInModel inventoryInModel;
            inventoryInService = new InventoryInService();

            inventoryInModel = inventoryInService.NavLast(userId);
            if (inventoryInModel != null)
            {
                inventoryInModel._FormMode = FormModeEnum.Edit;
            }

            if (inventoryInModel == null)
            {
                //InventoryInModel = InventoryInService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryInModel);
        }



    }
}