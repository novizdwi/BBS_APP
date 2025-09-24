using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models;
using Models.Master.Ship;
namespace Controllers.Master
{
    public partial class ShipController : BaseController
    {
        string VIEW_DETAIL = "Ship";
        string VIEW_FORM_PARTIAL = "Partial/Ship_Form_Partial";
        string VIEW_LIST_PARTIAL = "Partial/Ship_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/Ship_Panel_List_Partial";

        ShipService shipService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }
        public ActionResult Detail(long Id = 0)
        {
            int userId = (int)Session["userId"];

            shipService = new ShipService();
            ShipModel shipModel;

            if (Id == 0)
            {
                ViewBag.initNew = true;

                shipModel = shipService.GetNewModel(userId);
                shipModel._FormMode = FormModeEnum.New;
            }
            else
            {
                shipService = new ShipService();
                shipModel = shipService.GetById(userId, Id);
                if (shipModel != null)
                {
                    shipModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return View(VIEW_DETAIL, shipModel);
        }
        public ActionResult DetailPartial(int Id = 0)
        {
            int userId = (int)Session["userId"];
            ShipModel shipModel;

            shipService = new ShipService();

            if (Id == 0)
            {
                shipModel = shipService.GetNewModel(userId);
                shipModel._FormMode = FormModeEnum.New;
            }
            else
            {
                shipModel = shipService.GetById(userId,Id);
                if (shipModel != null)
                {
                    shipModel._FormMode = FormModeEnum.Edit;
                }
                else
                {
                    throw new Exception("[VALIDATION]-Data not exists");
                }
            }

            return PartialView(VIEW_FORM_PARTIAL, shipModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Add([ModelBinder(typeof(DevExpressEditorsBinder))]  ShipModel shipModel, List<Ship_EngineModel> ShipEngine, List<Ship_NavEqModel> ShipNavEq, List<Ship_SafeEqModel> ShipSafeEq, List<Ship_AccoModel> ShipAcco, List<Ship_AnchorModel> ShipAnchor, List<Ship_OperationModel> ShipOperation, List<Ship_AttachmentModel> ShipAttachment)
        {
            int userId = (int)Session["userId"];

            shipModel._UserId = (int)Session["userId"];
            shipService = new ShipService();
            shipModel.ListEngines_ = ShipEngine;
            shipModel.ListNavEqs_ = ShipNavEq;
            shipModel.ListSafeEqs_ = ShipSafeEq;
            shipModel.ListAccos_ = ShipAcco;
            shipModel.ListAnchors_ = ShipAnchor;
            shipModel.ListOperations_ = ShipOperation;
            shipModel.ListAttachments_ = ShipAttachment;
            if (ModelState.IsValid)
            {
                long Id = 0;

                Id = shipService.Add(shipModel);
                shipModel = shipService.GetById(userId, Id);
                shipModel._FormMode = Models.FormModeEnum.Edit;
            }
            else
            {
                string message = GetErrorModel();
                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, shipModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  ShipModel shipModel, List<Ship_EngineModel> ShipEngine, List<Ship_NavEqModel> ShipNavEq, List<Ship_SafeEqModel> ShipSafeEq, List<Ship_AccoModel> ShipAcco, List<Ship_AnchorModel> ShipAnchor, List<Ship_OperationModel> ShipOperation, List<Ship_AttachmentModel> ShipAttachment)
        {
            int userId = (int)Session["userId"];

            shipModel._UserId = (int)Session["userId"];
            shipService = new ShipService();
            shipModel._FormMode = FormModeEnum.Edit;

            shipModel.ListEngines_ = ShipEngine;
            shipModel.ListNavEqs_ = ShipNavEq;
            shipModel.ListSafeEqs_ = ShipSafeEq;
            shipModel.ListAccos_ = ShipAcco;
            shipModel.ListAnchors_ = ShipAnchor;
            shipModel.ListOperations_ = ShipOperation;
            shipModel.ListAttachments_ = ShipAttachment;
            if (ModelState.IsValid)
            {
                shipService.Update(shipModel);
                shipModel = shipService.GetById(userId, shipModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, shipModel);
        }

    }
}