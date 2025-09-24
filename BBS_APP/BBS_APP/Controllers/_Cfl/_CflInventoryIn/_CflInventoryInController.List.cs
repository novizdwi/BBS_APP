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
    public partial class _CflInventoryInController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflInventoryIn_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflInventoryIn_Panel_List_Partial";

        public CflInventoryIn_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflInventoryIn_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "InventoryReceipt")
            {
                cflParam.SqlWhere = string.Format(" AND T0.\"Status\"= 'Posted' AND T0.\"TransferType\" = 'DifferentWarehouse' " );
            }

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflInventoryInParam = GetParam(Request);

            var viewModel = GetListModel(cflInventoryInParam.Name);
            ProcessCustomBinding(userId, cflInventoryInParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflInventoryInParam = GetParam(Request);

            var viewModel = GetListModel(cflInventoryInParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflInventoryInParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflInventoryInParam = GetParam(Request);

            var viewModel = GetListModel(cflInventoryInParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflInventoryInParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflInventoryInParam = GetParam(Request);

            var viewModel = GetListModel(cflInventoryInParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflInventoryInParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflInventoryInList" + name);
            if (viewModel == null)
            {
                viewModel = CflInventoryIn_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflInventoryIn_ParamModel cflInventoryInParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflInventoryIn_Model.GetDataRowCount(args, userId, cflInventoryInParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflInventoryIn_Model.GetData(args, userId, cflInventoryInParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflInventoryInParam = GetParam(Request);

            var viewModel = GetListModel(cflInventoryInParam.Name);
            ProcessCustomBinding(userId, cflInventoryInParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflInventoryInParam);
        }

    }
}