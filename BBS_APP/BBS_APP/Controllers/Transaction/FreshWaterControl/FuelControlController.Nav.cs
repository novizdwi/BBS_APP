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
using Models.Transaction.FreshWaterControl;

namespace Controllers.Transaction
{
    public partial class FreshWaterControlController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            FreshWaterControlModel freshWaterControlModel;
            freshWaterControlService = new FreshWaterControlService();

            freshWaterControlModel = freshWaterControlService.NavFirst(userId);
            if (freshWaterControlModel != null)
            {
                freshWaterControlModel._FormMode = FormModeEnum.Edit;
            }

            if (freshWaterControlModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, freshWaterControlModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            FreshWaterControlModel freshWaterControlModel;
            freshWaterControlService = new FreshWaterControlService();

            freshWaterControlModel = freshWaterControlService.NavPrevious(userId, Id);
            if (freshWaterControlModel != null)
            {
                freshWaterControlModel._FormMode = FormModeEnum.Edit;
            }

            if (freshWaterControlModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, freshWaterControlModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            FreshWaterControlModel freshWaterControlModel;
            freshWaterControlService = new FreshWaterControlService();

            freshWaterControlModel = freshWaterControlService.NavNext(userId, Id);
            if (freshWaterControlModel != null)
            {

                freshWaterControlModel._FormMode = FormModeEnum.Edit;

            }

            if (freshWaterControlModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, freshWaterControlModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            FreshWaterControlModel freshWaterControlModel;
            freshWaterControlService = new FreshWaterControlService();

            freshWaterControlModel = freshWaterControlService.NavLast(userId);
            if (freshWaterControlModel != null)
            {
                freshWaterControlModel._FormMode = FormModeEnum.Edit;
            }

            if (freshWaterControlModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, freshWaterControlModel);
        }



    }
}