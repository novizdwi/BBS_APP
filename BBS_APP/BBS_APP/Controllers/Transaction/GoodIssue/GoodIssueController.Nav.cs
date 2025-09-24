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

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            GoodIssueModel goodIssueModel;
            goodIssueService = new GoodIssueService();

            goodIssueModel = goodIssueService.NavFirst(userId);
            if (goodIssueModel != null)
            {
                goodIssueModel._FormMode = FormModeEnum.Edit;
            }

            if (goodIssueModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, goodIssueModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            GoodIssueModel goodIssueModel;
            goodIssueService = new GoodIssueService();

            goodIssueModel = goodIssueService.NavPrevious(userId, Id);
            if (goodIssueModel != null)
            {
                goodIssueModel._FormMode = FormModeEnum.Edit;
            }

            if (goodIssueModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, goodIssueModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            GoodIssueModel goodIssueModel;
            goodIssueService = new GoodIssueService();

            goodIssueModel = goodIssueService.NavNext(userId, Id);
            if (goodIssueModel != null)
            {

                goodIssueModel._FormMode = FormModeEnum.Edit;

            }

            if (goodIssueModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, goodIssueModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            GoodIssueModel goodIssueModel;
            goodIssueService = new GoodIssueService();

            goodIssueModel = goodIssueService.NavLast(userId);
            if (goodIssueModel != null)
            {
                goodIssueModel._FormMode = FormModeEnum.Edit;
            }

            if (goodIssueModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, goodIssueModel);
        }



    }
}