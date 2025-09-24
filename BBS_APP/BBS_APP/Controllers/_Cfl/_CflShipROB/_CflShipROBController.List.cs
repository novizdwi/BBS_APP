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
using Models._Cfl;


namespace Controllers._Cfl
{
    public partial class _CflShipROBController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflShipROB_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflShipROB_Panel_List_Partial";

        public CflShipROB_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflShipROB_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var CflShipROBParam = GetParam(Request);

            var viewModel = GetListModel(CflShipROBParam.Name);
            ProcessCustomBinding(userId, CflShipROBParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var CflShipROBParam = GetParam(Request);

            var viewModel = GetListModel(CflShipROBParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, CflShipROBParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var CflShipROBParam = GetParam(Request);

            var viewModel = GetListModel(CflShipROBParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, CflShipROBParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var CflShipROBParam = GetParam(Request);

            var viewModel = GetListModel(CflShipROBParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, CflShipROBParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflShipROBList" + name);
            if (viewModel == null)
            {
                viewModel = CflDocumentGeneral_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflShipROB_ParamModel CflShipROBParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflShipROB_Model.GetDataRowCount(args, userId, CflShipROBParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflShipROB_Model.GetData(args, userId, CflShipROBParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var CflShipROBParam = GetParam(Request);

            var viewModel = GetListModel(CflShipROBParam.Name);
            ProcessCustomBinding(userId, CflShipROBParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, CflShipROBParam);
        }

    }
}