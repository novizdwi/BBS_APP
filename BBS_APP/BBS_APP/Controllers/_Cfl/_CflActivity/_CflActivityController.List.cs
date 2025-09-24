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
    public partial class _CflActivityController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflActivity_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflActivity_Panel_List_Partial";

        public CflActivity_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflActivity_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "Receipt")
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");

                cflParam.SqlWhere = string.Format(" AND T0.\"Status\" = 'Delivered' " +
                                                    " AND NOT EXISTS(SELECT T0_.\"Id\" " +
                                                    " FROM \"Tx_Receipt\" T0_   " +
                                                    " WHERE T0_.\"Id\"<>{0} AND T0_.\"Status\" = 'Posted' )", hidden_CflDocId);

            }

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflActivityParam = GetParam(Request);

            var viewModel = GetListModel(cflActivityParam.Name);
            ProcessCustomBinding(userId, cflActivityParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflActivityParam = GetParam(Request);

            var viewModel = GetListModel(cflActivityParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflActivityParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflActivityParam = GetParam(Request);

            var viewModel = GetListModel(cflActivityParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflActivityParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflActivityParam = GetParam(Request);

            var viewModel = GetListModel(cflActivityParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflActivityParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflActivityList" + name);
            if (viewModel == null)
            {
                viewModel = CflActivity_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflActivity_ParamModel cflActivityParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflActivity_Model.GetDataRowCount(args, userId, cflActivityParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflActivity_Model.GetData(args, userId, cflActivityParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflActivityParam = GetParam(Request);

            var viewModel = GetListModel(cflActivityParam.Name);
            ProcessCustomBinding(userId, cflActivityParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflActivityParam);
        }

    }
}