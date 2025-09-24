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
using Models.Transaction.Docking;

namespace Controllers.Transaction
{
    public partial class DockingController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            DockingModel dockingModel;
            dockingService = new DockingService();

            dockingModel = dockingService.NavFirst(userId);
            if (dockingModel != null)
            {
                dockingModel._FormMode = FormModeEnum.Edit;
            }

            if (dockingModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, dockingModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            DockingModel dockingModel;
            dockingService = new DockingService();

            dockingModel = dockingService.NavPrevious(userId, Id);
            if (dockingModel != null)
            {
                dockingModel._FormMode = FormModeEnum.Edit;
            }

            if (dockingModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, dockingModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            DockingModel dockingModel;
            dockingService = new DockingService();

            dockingModel = dockingService.NavNext(userId, Id);
            if (dockingModel != null)
            {

                dockingModel._FormMode = FormModeEnum.Edit;

            }

            if (dockingModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, dockingModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            DockingModel dockingModel;
            dockingService = new DockingService();

            dockingModel = dockingService.NavLast(userId);
            if (dockingModel != null)
            {
                dockingModel._FormMode = FormModeEnum.Edit;
            }

            if (dockingModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, dockingModel);
        }



    }
}