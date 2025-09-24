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
    public partial class _CflLoadingOrderController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflLoadingOrder_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflLoadingOrder_Panel_List_Partial";

        public CflLoadingOrder_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflLoadingOrder_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            //if (cflParam.Type == "Receipt")
            //{
            //    cflParam.SqlWhere = string.Format(" AND T0.\"Status\" = 'Delivered' ");
            //}

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflLoadingOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflLoadingOrderParam.Name);
            ProcessCustomBinding(userId, cflLoadingOrderParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflLoadingOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflLoadingOrderParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflLoadingOrderParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflLoadingOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflLoadingOrderParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflLoadingOrderParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflLoadingOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflLoadingOrderParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflLoadingOrderParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflLoadingOrderList" + name);
            if (viewModel == null)
            {
                viewModel = CflLoadingOrder_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflLoadingOrder_ParamModel cflLoadingOrderParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflLoadingOrder_Model.GetDataRowCount(args, userId, cflLoadingOrderParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflLoadingOrder_Model.GetData(args, userId, cflLoadingOrderParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflLoadingOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflLoadingOrderParam.Name);
            ProcessCustomBinding(userId, cflLoadingOrderParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflLoadingOrderParam);
        }

    }
}