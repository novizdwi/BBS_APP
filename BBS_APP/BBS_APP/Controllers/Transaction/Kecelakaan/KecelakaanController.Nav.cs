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
using Models.Transaction.Kecelakaan;

namespace Controllers.Transaction
{
    public partial class KecelakaanController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            KecelakaanModel kecelakaanModel;
            kecelakaanService = new KecelakaanService();

            kecelakaanModel = kecelakaanService.NavFirst(userId);
            if (kecelakaanModel != null)
            {
                kecelakaanModel._FormMode = FormModeEnum.Edit;
            }

            if (kecelakaanModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, kecelakaanModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            KecelakaanModel kecelakaanModel;
            kecelakaanService = new KecelakaanService();

            kecelakaanModel = kecelakaanService.NavPrevious(userId, Id);
            if (kecelakaanModel != null)
            {
                kecelakaanModel._FormMode = FormModeEnum.Edit;
            }

            if (kecelakaanModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, kecelakaanModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            KecelakaanModel kecelakaanModel;
            kecelakaanService = new KecelakaanService();

            kecelakaanModel = kecelakaanService.NavNext(userId, Id);
            if (kecelakaanModel != null)
            {

                kecelakaanModel._FormMode = FormModeEnum.Edit;
            
            }

            if (kecelakaanModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, kecelakaanModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            KecelakaanModel kecelakaanModel;
            kecelakaanService = new KecelakaanService();

            kecelakaanModel = kecelakaanService.NavLast(userId);
            if (kecelakaanModel != null)
            {
                kecelakaanModel._FormMode = FormModeEnum.Edit;
            }

            if (kecelakaanModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, kecelakaanModel);
        }



    }
}