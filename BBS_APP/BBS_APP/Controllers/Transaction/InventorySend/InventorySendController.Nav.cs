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
using Models.Transaction.InventorySend;

namespace Controllers.Transaction
{
    public partial class InventorySendController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            InventorySendModel inventorySendModel;
            inventorySendService = new InventorySendService();

            inventorySendModel = inventorySendService.NavFirst(userId);
            if (inventorySendModel != null)
            {
                inventorySendModel._FormMode = FormModeEnum.Edit;
            }

            if (inventorySendModel == null)
            {
                //InventorySendModel = InventorySendService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventorySendModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            InventorySendModel inventorySendModel;
            inventorySendService = new InventorySendService();

            inventorySendModel = inventorySendService.NavPrevious(userId, Id);
            if (inventorySendModel != null)
            {
                inventorySendModel._FormMode = FormModeEnum.Edit;
            }

            if (inventorySendModel == null)
            {
                //InventorySendModel = InventorySendService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventorySendModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            InventorySendModel inventorySendModel;
            inventorySendService = new InventorySendService();

            inventorySendModel = inventorySendService.NavNext(userId, Id);
            if (inventorySendModel != null)
            {

                inventorySendModel._FormMode = FormModeEnum.Edit;

            }

            if (inventorySendModel == null)
            {
                //InventorySendModel = InventorySendService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventorySendModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            InventorySendModel inventorySendModel;
            inventorySendService = new InventorySendService();

            inventorySendModel = inventorySendService.NavLast(userId);
            if (inventorySendModel != null)
            {
                inventorySendModel._FormMode = FormModeEnum.Edit;
            }

            if (inventorySendModel == null)
            {
                //InventorySendModel = InventorySendService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, inventorySendModel);
        }



    }
}