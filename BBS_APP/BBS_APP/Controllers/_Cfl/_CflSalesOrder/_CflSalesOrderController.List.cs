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
    public partial class _CflSalesOrderController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflSalesOrder_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflSalesOrder_Panel_List_Partial";

        public CflSalesOrder_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflSalesOrder_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "LoadingOrder")
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");

                cflParam.SqlWhere = string.Format(  " AND NOT EXISTS(SELECT T0_.\"DetId\" " +
                                                    " FROM \"Tx_LoadingOrder_SalesOrder\" T0_   " +
                                                    " WHERE T0_.\"Id\"={0} AND T0.\"DocEntry\" = T0_.\"BaseId\" )", hidden_CflDocId);
            }

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflSalesOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflSalesOrderParam.Name);
            ProcessCustomBinding(userId, cflSalesOrderParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflSalesOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflSalesOrderParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflSalesOrderParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflSalesOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflSalesOrderParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflSalesOrderParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflSalesOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflSalesOrderParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflSalesOrderParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflSalesOrderList" + name);
            if (viewModel == null)
            {
                viewModel = CflSalesOrder_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflSalesOrder_ParamModel cflParam, GridViewModel viewModel)
        {
            CflSalesOrder_Model.SetBindingData(viewModel, userId, cflParam);



        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflSalesOrderParam = GetParam(Request);

            var viewModel = GetListModel(cflSalesOrderParam.Name);
            ProcessCustomBinding(userId, cflSalesOrderParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflSalesOrderParam);
        }

    }
}