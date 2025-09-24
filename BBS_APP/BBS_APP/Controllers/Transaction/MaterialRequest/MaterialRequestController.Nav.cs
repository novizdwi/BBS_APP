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
using Models.Transaction.MaterialRequest;

namespace Controllers.Transaction
{
    public partial class MaterialRequestController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            MaterialRequestModel materialRequestModel;
            materialRequestService = new MaterialRequestService();

            materialRequestModel = materialRequestService.NavFirst(userId);
            if (materialRequestModel != null)
            {
                materialRequestModel._FormMode = FormModeEnum.Edit;
            }

            if (materialRequestModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, materialRequestModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            MaterialRequestModel materialRequestModel;
            materialRequestService = new MaterialRequestService();

            materialRequestModel = materialRequestService.NavPrevious(userId, Id);
            if (materialRequestModel != null)
            {
                materialRequestModel._FormMode = FormModeEnum.Edit;
            }

            if (materialRequestModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, materialRequestModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            MaterialRequestModel materialRequestModel;
            materialRequestService = new MaterialRequestService();

            materialRequestModel = materialRequestService.NavNext(userId, Id);
            if (materialRequestModel != null)
            {

                materialRequestModel._FormMode = FormModeEnum.Edit;

            }

            if (materialRequestModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, materialRequestModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            MaterialRequestModel materialRequestModel;
            materialRequestService = new MaterialRequestService();

            materialRequestModel = materialRequestService.NavLast(userId);
            if (materialRequestModel != null)
            {
                materialRequestModel._FormMode = FormModeEnum.Edit;
            }

            if (materialRequestModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, materialRequestModel);
        }



    }
}