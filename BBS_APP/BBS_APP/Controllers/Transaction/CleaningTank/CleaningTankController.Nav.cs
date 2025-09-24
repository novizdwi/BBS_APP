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
using Models.Transaction.CleaningTank;

namespace Controllers.Transaction
{
    public partial class CleaningTankController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            CleaningTankModel cleaningTankModel;
            cleaningTankService = new CleaningTankService();

            cleaningTankModel = cleaningTankService.NavFirst(userId);
            if (cleaningTankModel != null)
            {
                cleaningTankModel._FormMode = FormModeEnum.Edit;
            }

            if (cleaningTankModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, cleaningTankModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            CleaningTankModel cleaningTankModel;
            cleaningTankService = new CleaningTankService();

            cleaningTankModel = cleaningTankService.NavPrevious(userId, Id);
            if (cleaningTankModel != null)
            {
                cleaningTankModel._FormMode = FormModeEnum.Edit;
            }

            if (cleaningTankModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, cleaningTankModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            CleaningTankModel cleaningTankModel;
            cleaningTankService = new CleaningTankService();

            cleaningTankModel = cleaningTankService.NavNext(userId, Id);
            if (cleaningTankModel != null)
            {

                cleaningTankModel._FormMode = FormModeEnum.Edit;

            }

            if (cleaningTankModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, cleaningTankModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            CleaningTankModel cleaningTankModel;
            cleaningTankService = new CleaningTankService();

            cleaningTankModel = cleaningTankService.NavLast(userId);
            if (cleaningTankModel != null)
            {
                cleaningTankModel._FormMode = FormModeEnum.Edit;
            }

            if (cleaningTankModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, cleaningTankModel);
        }



    }
}