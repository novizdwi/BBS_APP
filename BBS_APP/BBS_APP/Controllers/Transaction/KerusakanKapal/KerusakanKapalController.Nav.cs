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
using Models.Transaction.KerusakanKapal;

namespace Controllers.Transaction
{
    public partial class KerusakanKapalController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            KerusakanKapalModel kerusakanKapalModel;
            kerusakanKapalService = new KerusakanKapalService();

            kerusakanKapalModel = kerusakanKapalService.NavFirst(userId);
            if (kerusakanKapalModel != null)
            {
                kerusakanKapalModel._FormMode = FormModeEnum.Edit;
            }

            if (kerusakanKapalModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, kerusakanKapalModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            KerusakanKapalModel kerusakanKapalModel;
            kerusakanKapalService = new KerusakanKapalService();

            kerusakanKapalModel = kerusakanKapalService.NavPrevious(userId, Id);
            if (kerusakanKapalModel != null)
            {
                kerusakanKapalModel._FormMode = FormModeEnum.Edit;
            }

            if (kerusakanKapalModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, kerusakanKapalModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            KerusakanKapalModel kerusakanKapalModel;
            kerusakanKapalService = new KerusakanKapalService();

            kerusakanKapalModel = kerusakanKapalService.NavNext(userId, Id);
            if (kerusakanKapalModel != null)
            {

                kerusakanKapalModel._FormMode = FormModeEnum.Edit;

            }

            if (kerusakanKapalModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, kerusakanKapalModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            KerusakanKapalModel kerusakanKapalModel;
            kerusakanKapalService = new KerusakanKapalService();

            kerusakanKapalModel = kerusakanKapalService.NavLast(userId);
            if (kerusakanKapalModel != null)
            {
                kerusakanKapalModel._FormMode = FormModeEnum.Edit;
            }

            if (kerusakanKapalModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, kerusakanKapalModel);
        }



    }
}