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
    public partial class _CflStockOpnameController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflStockOpname_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflStockOpname_Panel_List_Partial";

        public CflStockOpname_ParamModel GetParam(HttpRequestBase Request, int userId = 0)
        {
            var cflParam = new CflStockOpname_ParamModel();
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

            var cflStockOpnameParam = GetParam(Request, userId);

            var viewModel = GetListModel(cflStockOpnameParam.Name);
            ProcessCustomBinding(userId, cflStockOpnameParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflStockOpnameParam = GetParam(Request, userId);

            var viewModel = GetListModel(cflStockOpnameParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflStockOpnameParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflStockOpnameParam = GetParam(Request, userId);

            var viewModel = GetListModel(cflStockOpnameParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflStockOpnameParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflStockOpnameParam = GetParam(Request, userId);

            var viewModel = GetListModel(cflStockOpnameParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflStockOpnameParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflStockOpnameList" + name);
            if (viewModel == null)
            {
                viewModel = CflStockOpname_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflStockOpname_ParamModel cflStockOpnameParam, GridViewModel viewModel)
        {

            CflStockOpname_Model.SetBindingData(viewModel, userId, cflStockOpnameParam);

        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflStockOpnameParam = GetParam(Request, userId);

            var viewModel = GetListModel(cflStockOpnameParam.Name);
            ProcessCustomBinding(userId, cflStockOpnameParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflStockOpnameParam);
        }

    }
}