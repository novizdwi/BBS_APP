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
    public partial class _CflInventorySendController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflInventorySend_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflInventorySend_Panel_List_Partial";

        public CflInventorySend_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflInventorySend_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "InventoryReceipt")
            {
                cflParam.SqlWhere = string.Format(" AND T0.\"Status\"= 'Posted' AND T0.\"TransferType\" = 'DifferentWarehouse' ");
            }

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflInventorySendParam = GetParam(Request);

            var viewModel = GetListModel(cflInventorySendParam.Name);
            ProcessCustomBinding(userId, cflInventorySendParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflInventorySendParam = GetParam(Request);

            var viewModel = GetListModel(cflInventorySendParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflInventorySendParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflInventorySendParam = GetParam(Request);

            var viewModel = GetListModel(cflInventorySendParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflInventorySendParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflInventorySendParam = GetParam(Request);

            var viewModel = GetListModel(cflInventorySendParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflInventorySendParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflInventorySendList" + name);
            if (viewModel == null)
            {
                viewModel = CflInventorySend_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflInventorySend_ParamModel cflInventorySendParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflInventorySend_Model.GetDataRowCount(args, userId, cflInventorySendParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflInventorySend_Model.GetData(args, userId, cflInventorySendParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflInventorySendParam = GetParam(Request);

            var viewModel = GetListModel(cflInventorySendParam.Name);
            ProcessCustomBinding(userId, cflInventorySendParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflInventorySendParam);
        }

    }
}