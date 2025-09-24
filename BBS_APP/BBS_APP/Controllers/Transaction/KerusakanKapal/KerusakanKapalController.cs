using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.KerusakanKapal;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class KerusakanKapalController : BaseController
    {
        string VIEW_DETAIL = "KerusakanKapal";
        string VIEW_FORM_PARTIAL = "Partial/KerusakanKapal_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/KerusakanKapal_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/KerusakanKapal_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/KerusakanKapal_Item_Lookup_Partial";

        KerusakanKapalService kerusakanKapalService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            kerusakanKapalService = new KerusakanKapalService();
            KerusakanKapalModel kerusakanKapalModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                kerusakanKapalModel = kerusakanKapalService.GetNewModel(userId);
                kerusakanKapalModel._FormMode = FormModeEnum.New;
            }
            else
            {
                kerusakanKapalService = new KerusakanKapalService();
                kerusakanKapalModel = kerusakanKapalService.GetById(userId, Id);
                if (kerusakanKapalModel != null)
                {
                    kerusakanKapalModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, kerusakanKapalModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            KerusakanKapalModel kerusakanKapalModel;

            kerusakanKapalService = new KerusakanKapalService();

            if (Id == 0)
            {
                kerusakanKapalModel = kerusakanKapalService.GetNewModel(userId);
                kerusakanKapalModel._FormMode = FormModeEnum.New;
            }
            else
            {
                kerusakanKapalModel = kerusakanKapalService.GetById(userId, Id);
                if (kerusakanKapalModel != null)
                {
                    kerusakanKapalModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, kerusakanKapalModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  KerusakanKapalModel kerusakanKapalModel, List<KerusakanKapal_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            kerusakanKapalModel._UserId = (int)Session["userId"];
            kerusakanKapalModel._UserId = userId;
            kerusakanKapalService = new KerusakanKapalService();
            kerusakanKapalModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = kerusakanKapalService.Add(kerusakanKapalModel);
                kerusakanKapalModel = kerusakanKapalService.GetById(userId, Id);
                kerusakanKapalModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, kerusakanKapalModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  KerusakanKapalModel kerusakanKapalModel, List<KerusakanKapal_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            kerusakanKapalModel._UserId = (int)Session["userId"];
            kerusakanKapalService = new KerusakanKapalService();
            kerusakanKapalModel._FormMode = FormModeEnum.Edit;
            kerusakanKapalModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                kerusakanKapalService.Update(kerusakanKapalModel);

                kerusakanKapalModel = kerusakanKapalService.GetById(userId, kerusakanKapalModel.Id);
                if (kerusakanKapalModel != null)
                {
                    kerusakanKapalModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    kerusakanKapalModel = kerusakanKapalService.GetNewModel(userId);
                    kerusakanKapalModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, kerusakanKapalModel);
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult Post([ModelBinder(typeof(DevExpressEditorsBinder))] KerusakanKapalModel kerusakanKapalModel, List<KerusakanKapal_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            kerusakanKapalModel._UserId = (int)Session["userId"];
            kerusakanKapalService = new KerusakanKapalService();
            kerusakanKapalModel._FormMode = FormModeEnum.Edit;
            kerusakanKapalModel.ListDetails_ = Details;
            if (ModelState.IsValid)
            {

                kerusakanKapalService.Update(kerusakanKapalModel);
                kerusakanKapalService.Post(userId, kerusakanKapalModel.Id);

                kerusakanKapalModel = kerusakanKapalService.GetById(userId, kerusakanKapalModel.Id);
                if (kerusakanKapalModel != null)
                {
                    kerusakanKapalModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    kerusakanKapalModel = kerusakanKapalService.GetNewModel(userId);
                    kerusakanKapalModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, kerusakanKapalModel);
        }


    }

}