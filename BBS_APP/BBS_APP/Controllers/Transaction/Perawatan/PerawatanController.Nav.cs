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
using Models.Transaction.Perawatan;

namespace Controllers.Transaction
{
    public partial class PerawatanController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            PerawatanModel perawatanModel;
            perawatanService = new PerawatanService();

            perawatanModel = perawatanService.NavFirst(userId);
            if (perawatanModel != null)
            {
                perawatanModel._FormMode = FormModeEnum.Edit;
            }

            if (perawatanModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            PerawatanModel perawatanModel;
            perawatanService = new PerawatanService();

            perawatanModel = perawatanService.NavPrevious(userId, Id);
            if (perawatanModel != null)
            {
                perawatanModel._FormMode = FormModeEnum.Edit;
            }

            if (perawatanModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            PerawatanModel perawatanModel;
            perawatanService = new PerawatanService();

            perawatanModel = perawatanService.NavNext(userId, Id);
            if (perawatanModel != null)
            {

                perawatanModel._FormMode = FormModeEnum.Edit;

            }

            if (perawatanModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            PerawatanModel perawatanModel;
            perawatanService = new PerawatanService();

            perawatanModel = perawatanService.NavLast(userId);
            if (perawatanModel != null)
            {
                perawatanModel._FormMode = FormModeEnum.Edit;
            }

            if (perawatanModel == null)
            {
                //DocContentModel = DocContentService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanModel);
        }



    }
}