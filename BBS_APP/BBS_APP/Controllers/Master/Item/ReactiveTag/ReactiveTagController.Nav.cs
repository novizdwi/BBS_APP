using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading;
using Models; 

using System.Net;

using Models.Transaction;
using Models._Utils;
using Models.Master.Item;


namespace Controllers.Master.Item
{
    public partial class ReactiveTagController : BaseController
    {

        [HttpPost, ValidateInput(false)]
        public ActionResult NavFirst()
        {
            int userId = (int)Session["userId"];

            ReactiveTagModel reactiveTagModel;
            reactiveTagService = new ReactiveTagService();

            reactiveTagModel = reactiveTagService.NavFirst(userId);
            if (reactiveTagModel != null)
            {
                reactiveTagModel._FormMode = FormModeEnum.Edit;
            }

            if (reactiveTagModel == null)
            {
                //ReactiveTagModel = ReactiveTagService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, reactiveTagModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavPrevious(long Id = 0)
        {
            int userId = (int)Session["userId"];


            ReactiveTagModel reactiveTagModel;
            reactiveTagService = new ReactiveTagService();

            reactiveTagModel = reactiveTagService.NavPrevious(userId,Id);
            if (reactiveTagModel != null)
            {
                reactiveTagModel._FormMode = FormModeEnum.Edit;
            }

            if (reactiveTagModel == null)
            {
                //ReactiveTagModel = ReactiveTagService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, reactiveTagModel);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult NavNext(long Id = 0)
        {
            int userId = (int)Session["userId"];



            ReactiveTagModel reactiveTagModel;
            reactiveTagService = new ReactiveTagService();

            reactiveTagModel = reactiveTagService.NavNext(userId,Id);
            if (reactiveTagModel != null)
            {

                reactiveTagModel._FormMode = FormModeEnum.Edit;

            }

            if (reactiveTagModel == null)
            {
                //ReactiveTagModel = ReactiveTagService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, reactiveTagModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NavLast()
        {
            int userId = (int)Session["userId"];

            ReactiveTagModel reactiveTagModel;
            reactiveTagService = new ReactiveTagService();

            reactiveTagModel = reactiveTagService.NavLast(userId);
            if (reactiveTagModel != null)
            {
                reactiveTagModel._FormMode = FormModeEnum.Edit; 
            }

            if (reactiveTagModel == null)
            {
                //ReactiveTagModel = ReactiveTagService.GetNewModel(); 
                throw new Exception("[VALIDATION]-Data not exists");
            }

            return PartialView(VIEW_FORM_PARTIAL, reactiveTagModel);
        }



    }
}