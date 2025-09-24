using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.Kecelakaan;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class KecelakaanController : BaseController
    {
        string VIEW_DETAIL = "Kecelakaan";
        string VIEW_FORM_PARTIAL = "Partial/Kecelakaan_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Kecelakaan_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Kecelakaan_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/Kecelakaan_Item_Lookup_Partial";

        KecelakaanService kecelakaanService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            kecelakaanService = new KecelakaanService();
            KecelakaanModel kecelakaanModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                kecelakaanModel = kecelakaanService.GetNewModel(userId);
                kecelakaanModel._FormMode = FormModeEnum.New;
            }
            else
            {
                kecelakaanService = new KecelakaanService();
                kecelakaanModel = kecelakaanService.GetById(userId, Id);
                if (kecelakaanModel != null)
                {
                    kecelakaanModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, kecelakaanModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            KecelakaanModel kecelakaanModel;

            kecelakaanService = new KecelakaanService();

            if (Id == 0)
            {
                kecelakaanModel = kecelakaanService.GetNewModel(userId);
                kecelakaanModel._FormMode = FormModeEnum.New;
            }
            else
            {
                kecelakaanModel = kecelakaanService.GetById(userId, Id);
                if (kecelakaanModel != null)
                {
                    kecelakaanModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, kecelakaanModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  KecelakaanModel kecelakaanModel, List<Kecelakaan_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            kecelakaanModel._UserId = (int)Session["userId"];
            kecelakaanModel._UserId = userId;
            kecelakaanService = new KecelakaanService();
            kecelakaanModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = kecelakaanService.Add(kecelakaanModel);
                kecelakaanModel = kecelakaanService.GetById(userId, Id);
                kecelakaanModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, kecelakaanModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  KecelakaanModel kecelakaanModel, List<Kecelakaan_DetailModel> Details)
        {
            int userId = (int)Session["userId"];

            kecelakaanModel._UserId = (int)Session["userId"];
            kecelakaanService = new KecelakaanService();
            kecelakaanModel._FormMode = FormModeEnum.Edit;
            kecelakaanModel.ListDetails_ = Details;

            if (ModelState.IsValid)
            {

                kecelakaanService.Update(kecelakaanModel);

                kecelakaanModel = kecelakaanService.GetById(userId, kecelakaanModel.Id);
                if (kecelakaanModel != null)
                {
                    kecelakaanModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    kecelakaanModel = kecelakaanService.GetNewModel(userId);
                    kecelakaanModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, kecelakaanModel);
        }
        
        

    }

}