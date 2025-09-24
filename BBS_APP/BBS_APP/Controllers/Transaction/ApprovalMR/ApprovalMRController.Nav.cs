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
using Models.Transaction.ApprovalMR;

namespace Controllers.Transaction
{
    public partial class ApprovalMRController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            ApprovalMRModel approvalMRModel;
            approvalMRService = new ApprovalMRService();

            approvalMRModel = approvalMRService.NavFirst(userId);
            if (approvalMRModel != null)
            {
                approvalMRModel._FormMode = FormModeEnum.Edit;
            }

            if (approvalMRModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalMRModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ApprovalMRModel approvalMRModel;
            approvalMRService = new ApprovalMRService();

            approvalMRModel = approvalMRService.NavPrevious(userId, Id);
            if (approvalMRModel != null)
            {
                approvalMRModel._FormMode = FormModeEnum.Edit;
            }

            if (approvalMRModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalMRModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            ApprovalMRModel approvalMRModel;
            approvalMRService = new ApprovalMRService();

            approvalMRModel = approvalMRService.NavNext(userId, Id);
            if (approvalMRModel != null)
            {

                approvalMRModel._FormMode = FormModeEnum.Edit;

            }

            if (approvalMRModel == null)
            {
                 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalMRModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            ApprovalMRModel approvalMRModel;
            approvalMRService = new ApprovalMRService();

            approvalMRModel = approvalMRService.NavLast(userId);
            if (approvalMRModel != null)
            {
                approvalMRModel._FormMode = FormModeEnum.Edit;
            }

            if (approvalMRModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, approvalMRModel);
        }



    }
}