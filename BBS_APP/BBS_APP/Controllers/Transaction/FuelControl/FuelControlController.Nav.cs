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
using Models.Transaction.FuelControl;

namespace Controllers.Transaction
{
    public partial class FuelControlController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            FuelControlModel fuelControlModel;
            fuelControlService = new FuelControlService();

            fuelControlModel = fuelControlService.NavFirst(userId);
            if (fuelControlModel != null)
            {
                fuelControlModel._FormMode = FormModeEnum.Edit;
            }

            if (fuelControlModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, fuelControlModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            FuelControlModel fuelControlModel;
            fuelControlService = new FuelControlService();

            fuelControlModel = fuelControlService.NavPrevious(userId, Id);
            if (fuelControlModel != null)
            {
                fuelControlModel._FormMode = FormModeEnum.Edit;
            }

            if (fuelControlModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, fuelControlModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            FuelControlModel fuelControlModel;
            fuelControlService = new FuelControlService();

            fuelControlModel = fuelControlService.NavNext(userId, Id);
            if (fuelControlModel != null)
            {

                fuelControlModel._FormMode = FormModeEnum.Edit;

            }

            if (fuelControlModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, fuelControlModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            FuelControlModel fuelControlModel;
            fuelControlService = new FuelControlService();

            fuelControlModel = fuelControlService.NavLast(userId);
            if (fuelControlModel != null)
            {
                fuelControlModel._FormMode = FormModeEnum.Edit;
            }

            if (fuelControlModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, fuelControlModel);
        }



    }
}