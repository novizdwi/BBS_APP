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

using Models.Setting.MasterSetting;
using Models._Utils;

namespace Controllers.Master
{

    public partial class MasterSettingController : BaseController
    {

        string VIEW_DETAIL = "MasterSetting";
        string VIEW_FORM_PARTIAL = "Partial/MasterSetting_Form_Partial";

        MasterSettingService masterSettingService;

        public ActionResult Index()
        {
            return RedirectToAction("Detail");
        }

        public ActionResult Detail(int Id = 0)
        {


            MasterSettingModel masterSettingModel;

            masterSettingService = new MasterSettingService();
            masterSettingModel = masterSettingService.GetById(Id);


            return View(VIEW_DETAIL, masterSettingModel);
        }


        public ActionResult DetailPartial(int Id = 0)
        {

            MasterSettingModel masterSettingModel;

            masterSettingService = new MasterSettingService();


            masterSettingModel = masterSettingService.GetById(Id);


            return PartialView(VIEW_FORM_PARTIAL, masterSettingModel);
        }

        public ActionResult ItemGroupName(string docNum = "")
        {
            var abc = MasterSettingGetList.GetDetailGroupItemCode(docNum);
            if (abc != null)
            {
                var response = new MyCustomResponse
                {
                    ItemGroupCode = abc.Rows[0]["Code"].ToString(),
                    ItemGroupName = abc.Rows[0]["Name"].ToString()
                };
                return this.Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var response = new MyCustomResponse
                {
                    ItemGroupCode = "",
                    ItemGroupName = ""
                };
                return this.Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public class MyCustomResponse
        {
            public string ItemGroupCode { get; set; }
            public string ItemGroupName { get; set; }
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  MasterSettingModel masterSettingModel)
        {
            masterSettingModel._UserId = (int)Session["userId"];
            masterSettingService = new MasterSettingService();


            if (ModelState.IsValid)
            {
                masterSettingService.Update(masterSettingModel);
                masterSettingModel = masterSettingService.GetById(masterSettingModel.Id);
            }
            else
            {
                string message = GetErrorModel();

                throw new Exception(string.Format("[VALIDATION] {0}", message));
            }

            return PartialView(VIEW_FORM_PARTIAL, masterSettingModel);
        }


    }
}

//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using DevExpress.Web.Mvc;
//using System.IO;
//using System.Threading;

//using DevExpress.Web.ASPxUploadControl;

//using System.Net;

//using Models;
//using Models.Setting.GeneralSetting;

//namespace Controllers.Setting
//{

//    public partial class GeneralSettingController : BaseController
//    {

//        string VIEW_DETAIL = "GeneralSetting";
//        string VIEW_FORM_PARTIAL = "Partial/GeneralSetting_Form_Partial";

//        GeneralSettingService generalSettingService;

//        public ActionResult Index()
//        {
//            return RedirectToAction("Detail");
//        }

//        public ActionResult Detail(int Id = 0)
//        {


//            GeneralSettingModel generalSettingModel;

//            generalSettingService = new GeneralSettingService();
//            generalSettingModel = generalSettingService.GetById(Id);

//            return View(VIEW_DETAIL, generalSettingModel);
//        }


//        public ActionResult DetailPartial(int Id = 0)
//        {

//            GeneralSettingModel generalSettingModel;

//            generalSettingService = new GeneralSettingService();


//            generalSettingModel = generalSettingService.GetById(Id);


//            return PartialView(VIEW_FORM_PARTIAL, generalSettingModel);
//        }



//        [HttpPost, ValidateInput(false)]
//        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))]  GeneralSettingModel generalSettingModel)
//        {
//            generalSettingModel._UserId = (int)Session["userId"];
//            generalSettingService = new GeneralSettingService();


//            if (ModelState.IsValid)
//            {
//                generalSettingService.Update(generalSettingModel);
//                generalSettingModel = generalSettingService.GetById(generalSettingModel.Id);
//            }
//            else
//            {
//                string message = GetErrorModel();

//                throw new Exception(string.Format("[VALIDATION] {0}", message));
//            } 

//            return PartialView(VIEW_FORM_PARTIAL, generalSettingModel);
//        }


//    }
//}