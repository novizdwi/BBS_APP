using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Models.Master.Item;


namespace Controllers.Master.Item
{
    public partial class ReactiveTagController : BaseController
    {

        string VIEW_TAB_CONTENT = "Partial/ReactiveTag_Form_TabDetail_List_Partial";
        
        public ActionResult TabDetailListPartial()
        {
            int userId = (int)Session["userId"];

            reactiveTagService = new ReactiveTagService();

            var Id = Convert.ToInt64(Request["cbId"]);
            List<ReactiveTag_ItemModel> modelList = reactiveTagService.GetReactiveTag_Items(Id);

            return PartialView(VIEW_TAB_CONTENT, modelList);
        }

      



    }
}