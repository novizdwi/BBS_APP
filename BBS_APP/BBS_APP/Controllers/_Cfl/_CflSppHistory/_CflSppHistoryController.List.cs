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
    public partial class _CflSppHistoryController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflSppHistory_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflSppHistory_Panel_List_Partial";

        public CflSppHistory_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflSppHistory_ParamModel();
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

            var cflSppHistoryParam = GetParam(Request);

            var viewModel = GetListModel(cflSppHistoryParam.Name);
            ProcessCustomBinding(userId, cflSppHistoryParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflSppHistoryParam = GetParam(Request);

            var viewModel = GetListModel(cflSppHistoryParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflSppHistoryParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflSppHistoryParam = GetParam(Request);

            var viewModel = GetListModel(cflSppHistoryParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflSppHistoryParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflSppHistoryParam = GetParam(Request);

            var viewModel = GetListModel(cflSppHistoryParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflSppHistoryParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflSppHistoryList" + name);
            if (viewModel == null)
            {
                viewModel = CflSppHistory_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflSppHistory_ParamModel cflSppHistoryParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflSppHistory_Model.GetDataRowCount(args, userId, cflSppHistoryParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflSppHistory_Model.GetData(args, userId, cflSppHistoryParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflSppHistoryParam = GetParam(Request);

            var viewModel = GetListModel(cflSppHistoryParam.Name);
            ProcessCustomBinding(userId, cflSppHistoryParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflSppHistoryParam);
        }

    }
}