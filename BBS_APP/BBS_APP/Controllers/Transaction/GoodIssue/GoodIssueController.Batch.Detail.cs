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
using Models.Transaction.GoodIssue;

namespace Controllers.Transaction
{
    public partial class GoodIssueController : BaseController
    {

        string VIEW_PROGRESS_PANEL_PARTIAL = "Partial/BatchDetail/BatchDetail_Panel_Partial";

        string VIEW_PROGRESS_FORM_PARTIAL = "Partial/BatchDetail/BatchDetail_Form_Partial";


        GoodIssueBatchDetailService goodIssueBatchDetailService;

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
            goodIssueBatchDetailService = new GoodIssueBatchDetailService();
            GoodIssue_DetailModel model;
            //if (id == 0)
            //{
            //    model = inventoryInBatchDetailService.GetNewModel(userId);
            //}
            //else
            //{
                model = goodIssueBatchDetailService.GetById(userId, id);

            //}

            return PartialView(VIEW_PROGRESS_PANEL_PARTIAL, model);
        }
        public ActionResult AddBatchDetail([ModelBinder(typeof(DevExpressEditorsBinder))]  GoodIssue_DetailModel goodIssue_DetailModel, List<GoodIssueBatchDetailModel> BatchDetails)
        {
            int userId = (int)Session["userId"];

            goodIssue_DetailModel._UserId = (int)Session["userId"];
            goodIssueBatchDetailService = new GoodIssueBatchDetailService();
            goodIssue_DetailModel.ListBatchDetails_ = BatchDetails;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = goodIssueBatchDetailService.Add(goodIssue_DetailModel);
                goodIssue_DetailModel = goodIssueBatchDetailService.GetById(userId, Id);
                goodIssue_DetailModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, goodIssue_DetailModel);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteBatchDetail(int Id = 0)
        {

            goodIssueBatchDetailService = new GoodIssueBatchDetailService();

            if (Id != 0)
            {
                goodIssueBatchDetailService = new GoodIssueBatchDetailService();
                goodIssueBatchDetailService.Delete(Id);

            }

            GoodIssue_DetailModel goodIssue_DetailModel = new GoodIssue_DetailModel();
            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, goodIssue_DetailModel);
        }

    }
}