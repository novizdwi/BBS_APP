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
using Models.Transaction.PerawatanEngine;

namespace Controllers.Transaction
{
    public partial class PerawatanEngineController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            PerawatanEngineModel perawatanEngineModel;
            perawatanEngineService = new PerawatanEngineService();

            perawatanEngineModel = perawatanEngineService.NavFirst(userId);
            if (perawatanEngineModel != null)
            {
                perawatanEngineModel._FormMode = FormModeEnum.Edit;
            }

            if (perawatanEngineModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanEngineModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            PerawatanEngineModel perawatanEngineModel;
            perawatanEngineService = new PerawatanEngineService();

            perawatanEngineModel = perawatanEngineService.NavPrevious(userId, Id);
            if (perawatanEngineModel != null)
            {
                perawatanEngineModel._FormMode = FormModeEnum.Edit;
            }

            if (perawatanEngineModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanEngineModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            PerawatanEngineModel perawatanEngineModel;
            perawatanEngineService = new PerawatanEngineService();

            perawatanEngineModel = perawatanEngineService.NavNext(userId, Id);
            if (perawatanEngineModel != null)
            {

                perawatanEngineModel._FormMode = FormModeEnum.Edit;

            }

            if (perawatanEngineModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanEngineModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            PerawatanEngineModel perawatanEngineModel;
            perawatanEngineService = new PerawatanEngineService();

            perawatanEngineModel = perawatanEngineService.NavLast(userId);
            if (perawatanEngineModel != null)
            {
                perawatanEngineModel._FormMode = FormModeEnum.Edit;
            }

            if (perawatanEngineModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanEngineModel);
        }



    }
}