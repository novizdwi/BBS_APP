using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.Overhaul;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class OverhaulController : BaseController
    {
        string VIEW_DETAIL = "Overhaul";
        string VIEW_FORM_PARTIAL = "Partial/Overhaul_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Overhaul_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Overhaul_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/Overhaul_Item_Lookup_Partial";

        OverhaulService overhaulService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            overhaulService = new OverhaulService();
            OverhaulModel overhaulModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                overhaulModel = overhaulService.GetNewModel(userId);
                overhaulModel._FormMode = FormModeEnum.New;
            }
            else
            {
                overhaulService = new OverhaulService();
                overhaulModel = overhaulService.GetById(userId, Id);
                if (overhaulModel != null)
                {
                    overhaulModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, overhaulModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            OverhaulModel overhaulModel;

            overhaulService = new OverhaulService();

            if (Id == 0)
            {
                overhaulModel = overhaulService.GetNewModel(userId);
                overhaulModel._FormMode = FormModeEnum.New;
            }
            else
            {
                overhaulModel = overhaulService.GetById(userId, Id);
                if (overhaulModel != null)
                {
                    overhaulModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, overhaulModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  OverhaulModel overhaulModel, List<Overhaul_MEKiriModel> MEKiris, List<Overhaul_MEKananModel> MEKanans, List<Overhaul_AEKiriModel> AEKiris, List<Overhaul_AEKananModel> AEKanans, List<Overhaul_GBKiriModel> GBKiris, List<Overhaul_GBKananModel> GBKanans)
        {
            int userId = (int)Session["userId"];

            overhaulModel._UserId = (int)Session["userId"];
            overhaulModel._UserId = userId;
            overhaulService = new OverhaulService();
            overhaulModel.ListMEKiris_ = MEKiris;
            overhaulModel.ListMEKanans_ = MEKanans;
            overhaulModel.ListAEKiris_ = AEKiris;
            overhaulModel.ListAEKanans_ = AEKanans;
            overhaulModel.ListGBKiris_ = GBKiris;
            overhaulModel.ListGBKanans_ = GBKanans;
            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = overhaulService.Add(overhaulModel);
                overhaulModel = overhaulService.GetById(userId, Id);
                overhaulModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, overhaulModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  OverhaulModel overhaulModel, List<Overhaul_MEKiriModel> MEKiris, List<Overhaul_MEKananModel> MEKanans, List<Overhaul_AEKiriModel> AEKiris, List<Overhaul_AEKananModel> AEKanans, List<Overhaul_GBKiriModel> GBKiris, List<Overhaul_GBKananModel> GBKanans)
        {
            int userId = (int)Session["userId"];

            overhaulModel._UserId = (int)Session["userId"];
            overhaulService = new OverhaulService();
            overhaulModel._FormMode = FormModeEnum.Edit;
            overhaulModel.ListMEKiris_ = MEKiris; overhaulModel.ListMEKiris_ = MEKiris;
            overhaulModel.ListMEKanans_ = MEKanans;
            overhaulModel.ListAEKiris_ = AEKiris;
            overhaulModel.ListAEKanans_ = AEKanans;
            overhaulModel.ListGBKiris_ = GBKiris;
            overhaulModel.ListGBKanans_ = GBKanans;
            if (ModelState.IsValid)
            {

                overhaulService.Update(overhaulModel);

                overhaulModel = overhaulService.GetById(userId, overhaulModel.Id);
                if (overhaulModel != null)
                {
                    overhaulModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    overhaulModel = overhaulService.GetNewModel(userId);
                    overhaulModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, overhaulModel);
        }
        
        

    }

}