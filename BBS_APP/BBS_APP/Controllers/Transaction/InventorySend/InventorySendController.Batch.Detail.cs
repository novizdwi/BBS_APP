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

        string VIEW_PROGRESS_PANEL_PARTIAL = "Partial/BatchDetail/BatchDetail_Panel_Partial";

        string VIEW_PROGRESS_FORM_PARTIAL = "Partial/BatchDetail/BatchDetail_Form_Partial";


        InventorySendBatchDetailService inventorySendBatchDetailService;

        //public ActionResult BatchDetail_PopupListOnDemandPartialAdd(long id = 0)
        //{
        //    int userId = (int)Session["userId"];
        //    inventorySendBatchDetailService = new InventorySendBatchDetailService();
        //    InventorySendBatchDetailModel inventorySendBatchDetailModel;
        //    if (id == 0)
        //    {
        //        inventorySendBatchDetailModel = inventorySendBatchDetailService.GetNewModel(userId);
        //    }
        //    else
        //    {
        //        inventorySendBatchDetailModel = inventorySendBatchDetailService.GetByIdNewBatchDetail(id);
        //        inventorySendBatchDetailModel = null;
        //        inventorySendBatchDetailModel = inventorySendBatchDetailService.GetNewModel(userId);

        //    }

        //    return PartialView(VIEW_PROGRESS_PANEL_PARTIAL, inventorySendBatchDetailModel);
        //}

        public ActionResult BatchDetail_PopupListOnDemandPartial(long id = 0)
        {
            int userId = (int)Session["userId"];
            inventorySendBatchDetailService = new InventorySendBatchDetailService();
            InventorySend_DetailModel model;
            //if (id == 0)
            //{
            //    model = inventorySendBatchDetailService.GetNewModel(userId);
            //}
            //else
            //{
                model = inventorySendBatchDetailService.GetById(userId, id);

            //}

            return PartialView(VIEW_PROGRESS_PANEL_PARTIAL, model);
        }
        public ActionResult AddBatchDetail([ModelBinder(typeof(DevExpressEditorsBinder))]  InventorySend_DetailModel inventorySend_DetailModel, List<InventorySendBatchDetailModel> BatchDetails)
        {
            int userId = (int)Session["userId"];

            inventorySend_DetailModel._UserId = (int)Session["userId"];
            inventorySendBatchDetailService = new InventorySendBatchDetailService();
            inventorySend_DetailModel.ListBatchDetails_ = BatchDetails;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = inventorySendBatchDetailService.Add(inventorySend_DetailModel);
                inventorySend_DetailModel = inventorySendBatchDetailService.GetById(userId, Id);
                inventorySend_DetailModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, inventorySend_DetailModel);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteBatchDetail(int Id = 0)
        {

            inventorySendBatchDetailService = new InventorySendBatchDetailService();

            if (Id != 0)
            {
                inventorySendBatchDetailService = new InventorySendBatchDetailService();
                inventorySendBatchDetailService.Delete(Id);

            }

            InventorySend_DetailModel inventorySend_DetailModel = new InventorySend_DetailModel();
            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, inventorySend_DetailModel);
        }

    }
}