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
    public partial class _CflNoTtdController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflNoTtd_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflNoTtd_Panel_List_Partial";

        public CflNoTtd_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflNoTtd_ParamModel();
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

            var cflNoTtdParam = GetParam(Request);

            var viewModel = GetListModel(cflNoTtdParam.Name);
            ProcessCustomNoTtdding(userId, cflNoTtdParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflNoTtdParam = GetParam(Request);

            var viewModel = GetListModel(cflNoTtdParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomNoTtdding(userId, cflNoTtdParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflNoTtdParam = GetParam(Request);

            var viewModel = GetListModel(cflNoTtdParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomNoTtdding(userId, cflNoTtdParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflNoTtdParam = GetParam(Request);

            var viewModel = GetListModel(cflNoTtdParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomNoTtdding(userId, cflNoTtdParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflNoTtdList" + name);
            if (viewModel == null)
            {
                viewModel = CflNoTtd_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomNoTtdding(int userId, CflNoTtd_ParamModel cflParam, GridViewModel viewModel)
        {
            CflNoTtd_Model.SetBindingData(viewModel, userId, cflParam);



        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflNoTtdParam = GetParam(Request);

            var viewModel = GetListModel(cflNoTtdParam.Name);
            ProcessCustomNoTtdding(userId, cflNoTtdParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflNoTtdParam);
        }

    }
}