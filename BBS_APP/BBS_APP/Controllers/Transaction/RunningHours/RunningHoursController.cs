using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Transaction.RunningHours;
using Models._Cfl;
namespace Controllers.Transaction
{
    public partial class RunningHoursController : BaseController
    {
        string VIEW_DETAIL = "RunningHours";
        string VIEW_FORM_PARTIAL = "Partial/RunningHours_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/RunningHours_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/RunningHours_Panel_List_Partial";
        string VIEW_ITEM_LIST_PARTIAL = "Partial/RunningHours_Item_Lookup_Partial";

        RunningHoursService runningHoursService;
        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            runningHoursService = new RunningHoursService();
            RunningHoursModel runningHoursModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                runningHoursModel = runningHoursService.GetNewModel(userId);
                runningHoursModel._FormMode = FormModeEnum.New;
            }
            else
            {
                runningHoursService = new RunningHoursService();
                runningHoursModel = runningHoursService.GetById(userId, Id);
                if (runningHoursModel != null)
                {
                    runningHoursModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, runningHoursModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            RunningHoursModel runningHoursModel;

            runningHoursService = new RunningHoursService();

            if (Id == 0)
            {
                runningHoursModel = runningHoursService.GetNewModel(userId);
                runningHoursModel._FormMode = FormModeEnum.New;
            }
            else
            {
                runningHoursModel = runningHoursService.GetById(userId, Id);
                if (runningHoursModel != null)
                {
                    runningHoursModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, runningHoursModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  RunningHoursModel runningHoursModel, List<RunningHours_DailyModel> Dailys, List<RunningHours_TotalModel> Totals, List<RunningHours_RunPumpModel> RunPumps)
        {
            int userId = (int)Session["userId"];

            runningHoursModel._UserId = (int)Session["userId"];
            runningHoursModel._UserId = userId;
            runningHoursService = new RunningHoursService();
            runningHoursModel.ListDailys_ = Dailys;
            runningHoursModel.ListTotals_ = Totals;
            runningHoursModel.ListRunPumps_ = RunPumps;
            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = runningHoursService.Add(runningHoursModel);
                runningHoursModel = runningHoursService.GetById(userId, Id);
                runningHoursModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, runningHoursModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  RunningHoursModel runningHoursModel, List<RunningHours_DailyModel> Dailys, List<RunningHours_TotalModel> Totals, List<RunningHours_RunPumpModel> RunPumps)
        {
            int userId = (int)Session["userId"];

            runningHoursModel._UserId = (int)Session["userId"];
            runningHoursService = new RunningHoursService();
            runningHoursModel._FormMode = FormModeEnum.Edit;
            runningHoursModel.ListDailys_ = Dailys;
            runningHoursModel.ListTotals_ = Totals;
            runningHoursModel.ListRunPumps_ = RunPumps;
            if (ModelState.IsValid)
            {

                runningHoursService.Update(runningHoursModel);

                runningHoursModel = runningHoursService.GetById(userId, runningHoursModel.Id);
                if (runningHoursModel != null)
                {
                    runningHoursModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    runningHoursModel = runningHoursService.GetNewModel(userId);
                    runningHoursModel._FormMode = FormModeEnum.New;
                }
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, runningHoursModel);
        }
        
        

    }

}