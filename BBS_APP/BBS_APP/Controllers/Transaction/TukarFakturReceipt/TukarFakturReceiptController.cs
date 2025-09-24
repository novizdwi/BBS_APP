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
using Models.Transaction.TukarFakturReceipt;


namespace Controllers.Transaction
{
    public partial class TukarFakturReceiptController : BaseController
    {

        string VIEW_DETAIL = "TukarFakturReceipt";
        string VIEW_FORM_PARTIAL = "Partial/TukarFakturReceipt_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/TukarFakturReceipt_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/TukarFakturReceipt_Panel_List_Partial";

        TukarFakturReceiptService tukarFakturReceiptService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }


        public ActionResult Detail(long Id = 0, string CopyFromForm = "", int CopyFromId = 0)
        {
            int userId = (int)Session["userId"];

            tukarFakturReceiptService = new TukarFakturReceiptService();
            TukarFakturReceiptModel tukarFakturReceiptModel;


            if (Id == 0)
            {
                ViewBag.initNew = true;

                tukarFakturReceiptModel = tukarFakturReceiptService.GetNewModel(userId);
                tukarFakturReceiptModel._FormMode = FormModeEnum.New;
            }
            else
            {
                tukarFakturReceiptService = new TukarFakturReceiptService();
                tukarFakturReceiptModel = tukarFakturReceiptService.GetById(userId, Id);
                if (tukarFakturReceiptModel != null)
                {
                    tukarFakturReceiptModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, tukarFakturReceiptModel);
        }


        public ActionResult DetailPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            TukarFakturReceiptModel tukarFakturReceiptModel;

            tukarFakturReceiptService = new TukarFakturReceiptService();

            if (Id == 0)
            {

                ViewBag.initNew = true;

                tukarFakturReceiptModel = tukarFakturReceiptService.GetNewModel(userId);
                tukarFakturReceiptModel._FormMode = FormModeEnum.New;

            }
            else
            {
                tukarFakturReceiptModel = tukarFakturReceiptService.GetById(userId, Id);
                if (tukarFakturReceiptModel != null)
                {
                    tukarFakturReceiptModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }


            return PartialView(VIEW_FORM_PARTIAL, tukarFakturReceiptModel);
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  TukarFakturReceiptModel tukarFakturReceiptModel)
        {
            int userId = (int)Session["userId"];

            tukarFakturReceiptModel._UserId = (int)Session["userId"];
            tukarFakturReceiptService = new TukarFakturReceiptService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = tukarFakturReceiptService.Add(tukarFakturReceiptModel);
                tukarFakturReceiptModel = tukarFakturReceiptService.GetById(userId, Id);
                tukarFakturReceiptModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturReceiptModel);
        }


        //[HttpPost, ValidateInput(false)]
        //public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  TukarFakturReceiptModel tukarFakturReceiptModel)
        //{
        //    int userId = (int)Session["userId"];

        //    tukarFakturReceiptModel._UserId = (int)Session["userId"];
        //    tukarFakturReceiptService = new TukarFakturReceiptService();
        //    tukarFakturReceiptModel._FormMode = FormModeEnum.Edit;

        //    if (ModelState.IsValid)
        //    {

        //        tukarFakturReceiptService.Update(tukarFakturReceiptModel);

        //        tukarFakturReceiptModel = tukarFakturReceiptService.GetById(userId, tukarFakturReceiptModel.Id);
        //        if (tukarFakturReceiptModel != null)
        //        {
        //            tukarFakturReceiptModel._FormMode = FormModeEnum.Edit;
        //        }
        //        else
        //        {
        //            tukarFakturReceiptModel = tukarFakturReceiptService.GetNewModel(userId);
        //            tukarFakturReceiptModel._FormMode = FormModeEnum.New;
        //        }
        //    }
        //    else
        //    {
        //        string message = GetErrorModel();

        //        throw new Exception(string.Format("[VALIDATION] {0}", message));
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, tukarFakturReceiptModel);
        //}

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  TukarFakturReceiptModel tukarFakturReceiptModel)
        {
            int userId = (int)Session["userId"];

            tukarFakturReceiptModel._UserId = (int)Session["userId"];
            tukarFakturReceiptService = new TukarFakturReceiptService();
            tukarFakturReceiptModel._FormMode = FormModeEnum.Edit;


            if (ModelState.IsValid)
            {
                tukarFakturReceiptService.Update(tukarFakturReceiptModel);
                tukarFakturReceiptModel = tukarFakturReceiptService.GetById(userId, tukarFakturReceiptModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturReceiptModel);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult Post(long Id)
        {
            int userId = (int)Session["userId"];
            TukarFakturReceiptModel tukarFakturReceiptModel;

            tukarFakturReceiptService = new TukarFakturReceiptService();
            tukarFakturReceiptService.Post(userId, Id);
            tukarFakturReceiptService.Invoice(userId, Id);

            tukarFakturReceiptModel = tukarFakturReceiptService.GetById(userId, Id);
            if (tukarFakturReceiptModel != null)
            {
                tukarFakturReceiptModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                tukarFakturReceiptModel = tukarFakturReceiptService.GetNewModel(userId);
                tukarFakturReceiptModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturReceiptModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string cancelReason)
        {
            int userId = (int)Session["userId"];
            TukarFakturReceiptModel tukarFakturReceiptModel;

            tukarFakturReceiptService = new TukarFakturReceiptService();
            tukarFakturReceiptService.Cancel(userId, Id, cancelReason);

            tukarFakturReceiptModel = tukarFakturReceiptService.GetById(userId, Id);
            if (tukarFakturReceiptModel != null)
            {
                tukarFakturReceiptModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                tukarFakturReceiptModel = tukarFakturReceiptService.GetNewModel(userId);
                tukarFakturReceiptModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturReceiptModel);
        }


    }
}