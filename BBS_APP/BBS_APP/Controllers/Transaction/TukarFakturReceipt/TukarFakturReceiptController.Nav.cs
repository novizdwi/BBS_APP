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

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            TukarFakturReceiptModel tukarFakturReceiptModel;
            tukarFakturReceiptService = new TukarFakturReceiptService();

            tukarFakturReceiptModel = tukarFakturReceiptService.NavFirst(userId);
            if (tukarFakturReceiptModel != null)
            {
                tukarFakturReceiptModel._FormMode = FormModeEnum.Edit;
            }

            if (tukarFakturReceiptModel == null)
            {
                //TukarFakturReceiptModel = TukarFakturReceiptService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturReceiptModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            TukarFakturReceiptModel tukarFakturReceiptModel;
            tukarFakturReceiptService = new TukarFakturReceiptService();

            tukarFakturReceiptModel = tukarFakturReceiptService.NavPrevious(userId, Id);
            if (tukarFakturReceiptModel != null)
            {
                tukarFakturReceiptModel._FormMode = FormModeEnum.Edit;
            }

            if (tukarFakturReceiptModel == null)
            {
                //TukarFakturReceiptModel = TukarFakturReceiptService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturReceiptModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            TukarFakturReceiptModel tukarFakturReceiptModel;
            tukarFakturReceiptService = new TukarFakturReceiptService();

            tukarFakturReceiptModel = tukarFakturReceiptService.NavNext(userId, Id);
            if (tukarFakturReceiptModel != null)
            {

                tukarFakturReceiptModel._FormMode = FormModeEnum.Edit;

            }

            if (tukarFakturReceiptModel == null)
            {
                //TukarFakturReceiptModel = TukarFakturReceiptService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturReceiptModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            TukarFakturReceiptModel tukarFakturReceiptModel;
            tukarFakturReceiptService = new TukarFakturReceiptService();

            tukarFakturReceiptModel = tukarFakturReceiptService.NavLast(userId);
            if (tukarFakturReceiptModel != null)
            {
                tukarFakturReceiptModel._FormMode = FormModeEnum.Edit;
            }

            if (tukarFakturReceiptModel == null)
            {
                //TukarFakturReceiptModel = TukarFakturReceiptService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturReceiptModel);
        }



    }
}