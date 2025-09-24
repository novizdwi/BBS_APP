using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.Perawatan;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class PerawatanController : BaseController
    {
        string VIEW_DETAIL = "Perawatan";
        string VIEW_FORM_PARTIAL = "Partial/Perawatan_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Perawatan_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Perawatan_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/Perawatan_Item_Lookup_Partial";

        PerawatanService perawatanService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            perawatanService = new PerawatanService();
            PerawatanModel perawatanModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                perawatanModel = perawatanService.GetNewModel(userId);
                perawatanModel._FormMode = FormModeEnum.New;
            }
            else
            {
                perawatanService = new PerawatanService();
                perawatanModel = perawatanService.GetById(userId, Id);
                if (perawatanModel != null)
                {
                    perawatanModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, perawatanModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            PerawatanModel perawatanModel;

            perawatanService = new PerawatanService();

            if (Id == 0)
            {
                perawatanModel = perawatanService.GetNewModel(userId);
                perawatanModel._FormMode = FormModeEnum.New;
            }
            else
            {
                perawatanModel = perawatanService.GetById(userId, Id);
                if (perawatanModel != null)
                {
                    perawatanModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  PerawatanModel perawatanModel, List<Perawatan_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            perawatanModel._UserId = (int)Session["userId"];
            perawatanModel._UserId = userId;
            perawatanService = new PerawatanService();
            perawatanModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = perawatanService.Add(perawatanModel);
                perawatanModel = perawatanService.GetById(userId, Id);
                perawatanModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  PerawatanModel perawatanModel, List<Perawatan_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            perawatanModel._UserId = (int)Session["userId"];
            perawatanService = new PerawatanService();
            perawatanModel._FormMode = FormModeEnum.Edit;
            perawatanModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                perawatanService.Update(perawatanModel);

                perawatanModel = perawatanService.GetById(userId, perawatanModel.Id);
                if (perawatanModel != null)
                {
                    perawatanModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    perawatanModel = perawatanService.GetNewModel(userId);
                    perawatanModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, perawatanModel);
        }
        
        

    }

}