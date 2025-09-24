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

        string VIEW_PROGRESS_PANEL_PARTIAL = "Partial/BatchDetail/BatchDetail_Panel_Partial";

        string VIEW_PROGRESS_FORM_PARTIAL = "Partial/BatchDetail/BatchDetail_Form_Partial";


        InventoryInBatchDetailService inventoryInBatchDetailService;

        //public ActionResult BatchDetail_PopupListOnDemandPartialAdd(long id = 0)
        //{
        //    int userId = (int)Session["userId"];
        //    inventoryInBatchDetailService = new InventoryInBatchDetailService();
        //    InventoryInBatchDetailModel inventoryInBatchDetailModel;
        //    if (id == 0)
        //    {
        //        inventoryInBatchDetailModel = inventoryInBatchDetailService.GetNewModel(userId);
        //    }
        //    else
        //    {
        //        inventoryInBatchDetailModel = inventoryInBatchDetailService.GetByIdNewBatchDetail(id);
        //        inventoryInBatchDetailModel = null;
        //        inventoryInBatchDetailModel = inventoryInBatchDetailService.GetNewModel(userId);

        //    }

        //    return PartialView(VIEW_PROGRESS_PANEL_PARTIAL, inventoryInBatchDetailModel);
        //}

        public ActionResult BatchDetail_PopupListOnDemandPartial(long id = 0)
        {
            int userId = (int)Session["userId"];
            inventoryInBatchDetailService = new InventoryInBatchDetailService();
            InventoryIn_DetailModel model;
            //if (id == 0)
            //{
            //    model = inventoryInBatchDetailService.GetNewModel(userId);
            //}
            //else
            //{
                model = inventoryInBatchDetailService.GetById(userId, id);

            //}

            return PartialView(VIEW_PROGRESS_PANEL_PARTIAL, model);
        }
        public ActionResult AddBatchDetail([ModelBinder(typeof(DevExpressEditorsBinder))]  InventoryIn_DetailModel inventoryIn_DetailModel, List<InventoryInBatchDetailModel> BatchDetails)
        {
            int userId = (int)Session["userId"];

            inventoryIn_DetailModel._UserId = (int)Session["userId"];
            inventoryInBatchDetailService = new InventoryInBatchDetailService();
            inventoryIn_DetailModel.ListBatchDetails_ = BatchDetails;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = inventoryInBatchDetailService.Add(inventoryIn_DetailModel);
                inventoryIn_DetailModel = inventoryInBatchDetailService.GetById(userId, Id);
                inventoryIn_DetailModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, inventoryIn_DetailModel);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteBatchDetail(int Id = 0)
        {

            inventoryInBatchDetailService = new InventoryInBatchDetailService();

            if (Id != 0)
            {
                inventoryInBatchDetailService = new InventoryInBatchDetailService();
                inventoryInBatchDetailService.Delete(Id);

            }

            InventoryIn_DetailModel inventoryIn_DetailModel = new InventoryIn_DetailModel();
            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, inventoryIn_DetailModel);
        }

    }
}