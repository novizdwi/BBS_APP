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
using Newtonsoft.Json;

namespace Controllers.Transaction
{
    public partial class TukarFakturSendController : BaseController
    {

        string VIEW_DETAIL = "TukarFakturSend";
        string VIEW_FORM_PARTIAL = "Partial/TukarFakturSend_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/TukarFakturSend_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/TukarFakturSend_Panel_List_Partial";

        TukarFakturSendService tukarFakturSentService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }


        public ActionResult Detail(long Id = 0, string CopyFromForm = "", int CopyFromId = 0)
        {
            int userId = (int)Session["userId"];

            tukarFakturSentService = new TukarFakturSendService();
            TukarFakturSendModel tukarFakturSentModel;


            if (Id == 0)
            {
                ViewBag.initNew = true;

                tukarFakturSentModel = tukarFakturSentService.GetNewModel(userId);
                tukarFakturSentModel._FormMode = FormModeEnum.New;
            }
            else
            {
                tukarFakturSentService = new TukarFakturSendService();
                tukarFakturSentModel = tukarFakturSentService.GetById(userId, Id);
                if (tukarFakturSentModel != null)
                {
                    tukarFakturSentModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, tukarFakturSentModel);
        }


        public ActionResult DetailPartial(long Id = 0)
        {
            int userId = (int)Session["userId"];

            TukarFakturSendModel tukarFakturSentModel;

            tukarFakturSentService = new TukarFakturSendService();

            if (Id == 0)
            {

                ViewBag.initNew = true;

                tukarFakturSentModel = tukarFakturSentService.GetNewModel(userId);
                tukarFakturSentModel._FormMode = FormModeEnum.New;

            }
            else
            {
                tukarFakturSentModel = tukarFakturSentService.GetById(userId, Id);
                if (tukarFakturSentModel != null)
                {
                    tukarFakturSentModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }


            return PartialView(VIEW_FORM_PARTIAL, tukarFakturSentModel);
        }

        public ActionResult NoArInvoice(long docNum = 0)
        {
            var abc = TukarFakturSendGetList.GetDetailNoArInvoice(docNum);
            if (abc != null)
            {
                var response = new MyCustomResponse
                {
                    TanggalInvoice = string.Format("{0:yyyy-MM-dd}", abc.Rows[0]["TanggalInvoice"]),
                    TotalInvoice = Convert.ToInt32(abc.Rows[0]["TotalInvoice"]),
                    NoInvoiceRevisi = abc.Rows[0]["NoInvoiceRevisi"].ToString()
                };
                return this.Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var response = new MyCustomResponse
                {
                    TanggalInvoice = string.Format("{0:yyyy-MM-dd}", DateTime.Now),
                    TotalInvoice = 0,
                    NoInvoiceRevisi = ""
                };
                return this.Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public class MyCustomResponse
        {
            public string TanggalInvoice { get; set; }
            public long TotalInvoice { get; set; }
            public string NoInvoiceRevisi { get; set; }
        }




        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  TukarFakturSendModel tukarFakturSentModel)
        {
            int userId = (int)Session["userId"];

            tukarFakturSentModel._UserId = (int)Session["userId"];
            tukarFakturSentService = new TukarFakturSendService();

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = tukarFakturSentService.Add(tukarFakturSentModel);
                tukarFakturSentModel = tukarFakturSentService.GetById(userId, Id);
                tukarFakturSentModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturSentModel);
        }


        //[HttpPost, ValidateInput(false)]
        //public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  TukarFakturSendModel tukarFakturSentModel)
        //{
        //    int userId = (int)Session["userId"];

        //    tukarFakturSentModel._UserId = (int)Session["userId"];
        //    tukarFakturSentService = new TukarFakturSendService();
        //    tukarFakturSentModel._FormMode = FormModeEnum.Edit;

        //    if (ModelState.IsValid)
        //    {

        //        tukarFakturSentService.Update(tukarFakturSentModel);

        //        tukarFakturSentModel = tukarFakturSentService.GetById(userId, tukarFakturSentModel.Id);
        //        if (tukarFakturSentModel != null)
        //        {
        //            tukarFakturSentModel._FormMode = FormModeEnum.Edit;
        //        }
        //        else
        //        {
        //            tukarFakturSentModel = tukarFakturSentService.GetNewModel(userId);
        //            tukarFakturSentModel._FormMode = FormModeEnum.New;
        //        }
        //    }
        //    else
        //    {
        //        string message = GetErrorModel();

        //        throw new Exception(string.Format("[VALIDATION] {0}", message));
        //    }

        //    return PartialView(VIEW_FORM_PARTIAL, tukarFakturSentModel);
        //}

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  TukarFakturSendModel tukarFakturSentModel)
        {
            int userId = (int)Session["userId"];

            tukarFakturSentModel._UserId = (int)Session["userId"];
            tukarFakturSentService = new TukarFakturSendService();
            tukarFakturSentModel._FormMode = FormModeEnum.Edit;


            if (ModelState.IsValid)
            {
                tukarFakturSentService.Update(tukarFakturSentModel);
                tukarFakturSentModel = tukarFakturSentService.GetById(userId, tukarFakturSentModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturSentModel);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult Post(long Id)
        {
            int userId = (int)Session["userId"];
            TukarFakturSendModel tukarFakturSentModel;

            tukarFakturSentService = new TukarFakturSendService();
            tukarFakturSentService.Post(userId, Id);

            tukarFakturSentModel = tukarFakturSentService.GetById(userId, Id);
            if (tukarFakturSentModel != null)
            {
                tukarFakturSentModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                tukarFakturSentModel = tukarFakturSentService.GetNewModel(userId);
                tukarFakturSentModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturSentModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Cancel(long Id, string cancelReason)
        {
            int userId = (int)Session["userId"];
            TukarFakturSendModel tukarFakturSentModel;

            tukarFakturSentService = new TukarFakturSendService();
            tukarFakturSentService.Cancel(userId, Id, cancelReason);

            tukarFakturSentModel = tukarFakturSentService.GetById(userId, Id);
            if (tukarFakturSentModel != null)
            {
                tukarFakturSentModel._FormMode = FormModeEnum.Edit;
            }
            else
            {
                tukarFakturSentModel = tukarFakturSentService.GetNewModel(userId);
                tukarFakturSentModel._FormMode = FormModeEnum.New;
            }

            return PartialView(VIEW_FORM_PARTIAL, tukarFakturSentModel);
        }


    }
}