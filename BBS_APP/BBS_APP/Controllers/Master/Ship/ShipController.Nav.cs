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
using Models.Master.Ship;

namespace Controllers.Master
{
    public partial class ShipController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            ShipModel shipModel;
            shipService = new ShipService();

            shipModel = shipService.NavFirst(userId);
            if (shipModel != null)
            {
                shipModel._FormMode = FormModeEnum.Edit;
            }

            if (shipModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, shipModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ShipModel shipModel;
            shipService = new ShipService();

            shipModel = shipService.NavPrevious(userId, Id);
            if (shipModel != null)
            {
                shipModel._FormMode = FormModeEnum.Edit;
            }

            if (shipModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, shipModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            ShipModel shipModel;
            shipService = new ShipService();

            shipModel = shipService.NavNext(userId, Id);
            if (shipModel != null)
            {

                shipModel._FormMode = FormModeEnum.Edit;

            }

            if (shipModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, shipModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            ShipModel shipModel;
            shipService = new ShipService();

            shipModel = shipService.NavLast(userId);
            if (shipModel != null)
            {
                shipModel._FormMode = FormModeEnum.Edit;
            }

            if (shipModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, shipModel);
        }



    }
}