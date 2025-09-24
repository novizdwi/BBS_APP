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
    public partial class InventroyInController : BaseController
    {

        string VIEW_PROGRESS_PANEL_PARTIAL = "Partial/BatchDetail/BatchDetail_Panel_Partial";

        string VIEW_PROGRESS_FORM_PARTIAL = "Partial/BatchDetail/BatchDetail_Form_Partial";


        InventoryInBatchDetailService labelingPurchaseOrderBatchDetailService;

        public ActionResult BatchDetail_PopupListOnDemandPartialAdd(long id = 0, long docEntry = 0)
        {
            int userId = (int)Session["userId"];
            labelingPurchaseOrderBatchDetailService = new LabelingPurchaseOrderBatchDetailService();
            LabelingPurchaseOrderBatchDetailModel model;
            if (id == 0)
            {
                model = labelingPurchaseOrderBatchDetailService.GetNewModel(userId);
            }
            else
            {
                model = labelingPurchaseOrderBatchDetailService.GetByIdNewBatchDetail(id, docEntry);
                //model = null;
                //model = labelingPurchaseOrderBatchDetailService.GetNewModel(userId);

            }

            return PartialView(VIEW_PROGRESS_PANEL_PARTIAL, model);
        }

        public ActionResult BatchDetail_PopupListOnDemandPartial(long id = 0)
        {
            int userId = (int)Session["userId"];
            labelingPurchaseOrderBatchDetailService = new LabelingPurchaseOrderBatchDetailService();
            LabelingPurchaseOrderBatchDetailModel model;
            if (id == 0)
            {
                model = labelingPurchaseOrderBatchDetailService.GetNewModel(userId);
            }
            else
            {
                model = labelingPurchaseOrderBatchDetailService.GetById(id);

            }

            return PartialView(VIEW_PROGRESS_PANEL_PARTIAL, model);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult AddBatchDetail([ModelBinder(typeof(DevExpressEditorsBinder))]  LabelingPurchaseOrderBatchDetailModel labelingPurchaseOrderBatchDetailModel)
        {

            int userId = (int)Session["userId"];

            labelingPurchaseOrderBatchDetailModel._UserId = (int)Session["userId"];
            labelingPurchaseOrderBatchDetailService = new LabelingPurchaseOrderBatchDetailService();



            if (ModelState.IsValid)
            {
                if (labelingPurchaseOrderBatchDetailModel.BatchDetail_DetId == 0) //Save
                {
                    long lineNum = labelingPurchaseOrderBatchDetailService.Add(labelingPurchaseOrderBatchDetailModel);

                    labelingPurchaseOrderBatchDetailModel = labelingPurchaseOrderBatchDetailService.GetById(lineNum);
                    if (labelingPurchaseOrderBatchDetailModel != null)
                    {
                        labelingPurchaseOrderBatchDetailModel._FormMode = FormModeEnum.Edit;
                    }
                    else
                    {
                        labelingPurchaseOrderBatchDetailModel = labelingPurchaseOrderBatchDetailService.GetNewModel(userId);
                        labelingPurchaseOrderBatchDetailModel._FormMode = FormModeEnum.New;
                    }
                }
                else //Update
                {
                    labelingPurchaseOrderBatchDetailService.Update(labelingPurchaseOrderBatchDetailModel);
                    labelingPurchaseOrderBatchDetailModel = labelingPurchaseOrderBatchDetailService.GetById(labelingPurchaseOrderBatchDetailModel.BatchDetail_DetId);

                    if (labelingPurchaseOrderBatchDetailModel != null)
                    {
                        labelingPurchaseOrderBatchDetailModel._FormMode = FormModeEnum.Edit;
                    }
                    else
                    {
                        labelingPurchaseOrderBatchDetailModel = labelingPurchaseOrderBatchDetailService.GetNewModel(userId);
                        labelingPurchaseOrderBatchDetailModel._FormMode = FormModeEnum.New;
                    }
                }

            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, labelingPurchaseOrderBatchDetailModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteBatchDetail(long Id = 0)
        {

            labelingPurchaseOrderBatchDetailService = new LabelingPurchaseOrderBatchDetailService();

            if (Id > 0)
            {
                labelingPurchaseOrderBatchDetailService = new LabelingPurchaseOrderBatchDetailService();
                labelingPurchaseOrderBatchDetailService.DeleteBatchDetail(Id);

            }

            LabelingPurchaseOrderBatchDetailModel labelingPurchaseOrderBatchDetailModel = new LabelingPurchaseOrderBatchDetailModel();
            return PartialView(VIEW_PROGRESS_FORM_PARTIAL, labelingPurchaseOrderBatchDetailModel);
        }

    }
}