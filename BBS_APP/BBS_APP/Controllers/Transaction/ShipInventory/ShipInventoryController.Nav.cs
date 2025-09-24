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
using Models.Transaction.ShipInventory;

namespace Controllers.Transaction
{
    public partial class ShipInventoryController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            ShipInventoryModel shipInventoryModel;
            shipInventoryService = new ShipInventoryService();

            shipInventoryModel = shipInventoryService.NavFirst(userId);
            if (shipInventoryModel != null)
            {
                shipInventoryModel._FormMode = FormModeEnum.Edit;
            }

            if (shipInventoryModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, shipInventoryModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ShipInventoryModel shipInventoryModel;
            shipInventoryService = new ShipInventoryService();

            shipInventoryModel = shipInventoryService.NavPrevious(userId, Id);
            if (shipInventoryModel != null)
            {
                shipInventoryModel._FormMode = FormModeEnum.Edit;
            }

            if (shipInventoryModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, shipInventoryModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            ShipInventoryModel shipInventoryModel;
            shipInventoryService = new ShipInventoryService();

            shipInventoryModel = shipInventoryService.NavNext(userId, Id);
            if (shipInventoryModel != null)
            {

                shipInventoryModel._FormMode = FormModeEnum.Edit;

            }

            if (shipInventoryModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, shipInventoryModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            ShipInventoryModel shipInventoryModel;
            shipInventoryService = new ShipInventoryService();

            shipInventoryModel = shipInventoryService.NavLast(userId);
            if (shipInventoryModel != null)
            {
                shipInventoryModel._FormMode = FormModeEnum.Edit;
            }

            if (shipInventoryModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, shipInventoryModel);
        }



    }
}