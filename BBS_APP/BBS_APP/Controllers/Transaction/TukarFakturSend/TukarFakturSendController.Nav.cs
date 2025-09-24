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
using Models.Transaction.TukarFakturSend;

namespace Controllers.Transaction
{
    public partial class TukarFakturSendController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            TukarFakturSendModel tukarFakturSentModel;
            tukarFakturSentService = new TukarFakturSendService();

            tukarFakturSentModel = tukarFakturSentService.NavFirst(userId);
            if (tukarFakturSentModel != null)
            {
                tukarFakturSentModel._FormMode = FormModeEnum.Edit;
            }

            if (tukarFakturSentModel == null)
            {
                //TukarFakturSendModel = TukarFakturSendService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturSentModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            TukarFakturSendModel tukarFakturSentModel;
            tukarFakturSentService = new TukarFakturSendService();

            tukarFakturSentModel = tukarFakturSentService.NavPrevious(userId, Id);
            if (tukarFakturSentModel != null)
            {
                tukarFakturSentModel._FormMode = FormModeEnum.Edit;
            }

            if (tukarFakturSentModel == null)
            {
                //TukarFakturSendModel = TukarFakturSendService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturSentModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            TukarFakturSendModel tukarFakturSentModel;
            tukarFakturSentService = new TukarFakturSendService();

            tukarFakturSentModel = tukarFakturSentService.NavNext(userId, Id);
            if (tukarFakturSentModel != null)
            {

                tukarFakturSentModel._FormMode = FormModeEnum.Edit;

            }

            if (tukarFakturSentModel == null)
            {
                //TukarFakturSendModel = TukarFakturSendService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturSentModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            TukarFakturSendModel tukarFakturSentModel;
            tukarFakturSentService = new TukarFakturSendService();

            tukarFakturSentModel = tukarFakturSentService.NavLast(userId);
            if (tukarFakturSentModel != null)
            {
                tukarFakturSentModel._FormMode = FormModeEnum.Edit;
            }

            if (tukarFakturSentModel == null)
            {
                //TukarFakturSendModel = TukarFakturSendService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturSentModel);
        }



    }
}