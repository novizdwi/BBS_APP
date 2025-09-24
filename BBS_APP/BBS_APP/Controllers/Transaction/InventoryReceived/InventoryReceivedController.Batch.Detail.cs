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

        string VIEW_PROGRESS_PANEL_PARTIAL = "Partial/BatchDetail/BatchDetail_Panel_Partial";

        string VIEW_PROGRESS_FORM_PARTIAL = "Partial/BatchDetail/BatchDetail_Form_Partial";


        InventoryReceivedBatchDetailService inventoryReceivedBatchDetailService;

        //public ActionResult BatchDetail_PopupListOnDemandPartialAdd(long id = 0)
        //{
        //    int userId = (int)Session["userId"];
        //    inventoryReceivedBatchDetailService = new InventoryReceivedBatchDetailService();
        //    InventoryReceivedBatchDetailModel inventoryReceivedBatchDetailModel;
        //    if (id == 0)
        //    {
        //        inventoryReceivedBatchDetailModel = inventoryReceivedBatchDetailService.GetNewModel(userId);
        //    }
        //    else
        //    {
        //        inventoryReceivedBatchDetailModel = inventoryReceivedBatchDetailService.GetByIdNewBatchDetail(id);
        //        inventoryReceivedBatchDetailModel = null;
        //        inventoryReceivedBatchDetailModel = inventoryReceivedBatchDetailService.GetNewModel(userId);

        //    }

        //    return PartialView(VIEW_PROGRESS_PANEL_PARTIAL, inventoryReceivedBatchDetailModel);
        //}

        public ActionResult BatchDetail_PopupListOnDemandPartial(long id = 0)
        {
            int userId = (int)Session["userId"];
            inventoryReceivedBatchDetailService = new InventoryReceivedBatchDetailService();
            InventoryReceived_DetailModel model;
            //if (id == 0)
            //{
            //    model = inventoryReceivedBatchDetailService.GetNewModel(userId);
            //}
            //else
            //{
                model = inventoryReceivedBatchDetailService.GetById(userId, id);

            //}

            return PartialView(VIEW_PROGRESS_PANEL_PARTIAL, model);
        }
        public ActionResult AddBatchDetail([ModelBinder(typeof(DevExpressEditorsBinder))]  InventoryReceived_DetailModel inventoryReceived_DetailModel, List<InventoryReceivedBatchDetailModel> BatchDetails)
        {
            int userId = (int)Session["userId"];

            inventoryReceived_DetailModel._UserId = (int)Session["userId"];
            inventoryReceivedBatchDetailService = new InventoryReceivedBatchDetailService();
            inventoryReceived_DetailModel.ListBatchDetails_ = BatchDetails;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = inventoryReceivedBatchDetailService.Add(inventoryReceived_DetailModel);
                inventoryReceived_DetailModel = inventoryReceivedBatchDetailService.GetById(userId, Id);
                inventoryReceived_DetailModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, inventoryReceived_DetailModel);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteBatchDetail(int Id = 0)
        {

            inventoryReceivedBatchDetailService = new InventoryReceivedBatchDetailService();

            if (Id != 0)
            {
                inventoryReceivedBatchDetailService = new InventoryReceivedBatchDetailService();
                inventoryReceivedBatchDetailService.Delete(Id);

            }

            InventoryReceived_DetailModel inventoryReceived_DetailModel = new InventoryReceived_DetailModel();
            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, inventoryReceived_DetailModel);
        }

    }
}