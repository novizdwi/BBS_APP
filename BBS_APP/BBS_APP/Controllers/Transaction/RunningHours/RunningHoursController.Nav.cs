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
using Models.Transaction.Overhaul;

namespace Controllers.Transaction
{
    public partial class OverhaulController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            OverhaulModel overhaulModel;
            overhaulService = new OverhaulService();

            overhaulModel = overhaulService.NavFirst(userId);
            if (overhaulModel != null)
            {
                overhaulModel._FormMode = FormModeEnum.Edit;
            }

            if (overhaulModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, overhaulModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            OverhaulModel overhaulModel;
            overhaulService = new OverhaulService();

            overhaulModel = overhaulService.NavPrevious(userId, Id);
            if (overhaulModel != null)
            {
                overhaulModel._FormMode = FormModeEnum.Edit;
            }

            if (overhaulModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, overhaulModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            OverhaulModel overhaulModel;
            overhaulService = new OverhaulService();

            overhaulModel = overhaulService.NavNext(userId, Id);
            if (overhaulModel != null)
            {

                overhaulModel._FormMode = FormModeEnum.Edit;

            }

            if (overhaulModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, overhaulModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            OverhaulModel overhaulModel;
            overhaulService = new OverhaulService();

            overhaulModel = overhaulService.NavLast(userId);
            if (overhaulModel != null)
            {
                overhaulModel._FormMode = FormModeEnum.Edit;
            }

            if (overhaulModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, overhaulModel);
        }



    }
}