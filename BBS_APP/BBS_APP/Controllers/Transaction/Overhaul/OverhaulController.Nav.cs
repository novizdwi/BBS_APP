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
using Models.Transaction.RunningHours;

namespace Controllers.Transaction
{
    public partial class RunningHoursController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            RunningHoursModel runningHoursModel;
            runningHoursService = new RunningHoursService();

            runningHoursModel = runningHoursService.NavFirst(userId);
            if (runningHoursModel != null)
            {
                runningHoursModel._FormMode = FormModeEnum.Edit;
            }

            if (runningHoursModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, runningHoursModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            RunningHoursModel runningHoursModel;
            runningHoursService = new RunningHoursService();

            runningHoursModel = runningHoursService.NavPrevious(userId, Id);
            if (runningHoursModel != null)
            {
                runningHoursModel._FormMode = FormModeEnum.Edit;
            }

            if (runningHoursModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, runningHoursModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            RunningHoursModel runningHoursModel;
            runningHoursService = new RunningHoursService();

            runningHoursModel = runningHoursService.NavNext(userId, Id);
            if (runningHoursModel != null)
            {

                runningHoursModel._FormMode = FormModeEnum.Edit;

            }

            if (runningHoursModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, runningHoursModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            RunningHoursModel runningHoursModel;
            runningHoursService = new RunningHoursService();

            runningHoursModel = runningHoursService.NavLast(userId);
            if (runningHoursModel != null)
            {
                runningHoursModel._FormMode = FormModeEnum.Edit;
            }

            if (runningHoursModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, runningHoursModel);
        }



    }
}