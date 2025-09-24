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
using Models.Transaction.InventoryReceived;

namespace Controllers.Transaction
{
    public partial class InventoryReceivedController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            InventoryReceivedModel inventoryReceivedModel;
            inventoryReceivedService = new InventoryReceivedService();

            inventoryReceivedModel = inventoryReceivedService.NavFirst(userId);
            if (inventoryReceivedModel != null)
            {
                inventoryReceivedModel._FormMode = FormModeEnum.Edit;
            }

            if (inventoryReceivedModel == null)
            {
                //InventoryReceivedModel = InventoryReceivedService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryReceivedModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            InventoryReceivedModel inventoryReceivedModel;
            inventoryReceivedService = new InventoryReceivedService();

            inventoryReceivedModel = inventoryReceivedService.NavPrevious(userId, Id);
            if (inventoryReceivedModel != null)
            {
                inventoryReceivedModel._FormMode = FormModeEnum.Edit;
            }

            if (inventoryReceivedModel == null)
            {
                //InventoryReceivedModel = InventoryReceivedService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryReceivedModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            InventoryReceivedModel inventoryReceivedModel;
            inventoryReceivedService = new InventoryReceivedService();

            inventoryReceivedModel = inventoryReceivedService.NavNext(userId, Id);
            if (inventoryReceivedModel != null)
            {

                inventoryReceivedModel._FormMode = FormModeEnum.Edit;

            }

            if (inventoryReceivedModel == null)
            {
                //InventoryReceivedModel = InventoryReceivedService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryReceivedModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            InventoryReceivedModel inventoryReceivedModel;
            inventoryReceivedService = new InventoryReceivedService();

            inventoryReceivedModel = inventoryReceivedService.NavLast(userId);
            if (inventoryReceivedModel != null)
            {
                inventoryReceivedModel._FormMode = FormModeEnum.Edit;
            }

            if (inventoryReceivedModel == null)
            {
                //InventoryReceivedModel = InventoryReceivedService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventoryReceivedModel);
        }



    }
}