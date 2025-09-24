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
    public partial class _CflSppHistoryOliController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflSppHistoryOli_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflSppHistoryOli_Panel_List_Partial";

        public CflSppHistoryOli_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflSppHistoryOli_ParamModel();
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

            var cflSppHistoryOliParam = GetParam(Request);

            var viewModel = GetListModel(cflSppHistoryOliParam.Name);
            ProcessCustomBinding(userId, cflSppHistoryOliParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflSppHistoryOliParam = GetParam(Request);

            var viewModel = GetListModel(cflSppHistoryOliParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflSppHistoryOliParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflSppHistoryOliParam = GetParam(Request);

            var viewModel = GetListModel(cflSppHistoryOliParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflSppHistoryOliParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflSppHistoryOliParam = GetParam(Request);

            var viewModel = GetListModel(cflSppHistoryOliParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflSppHistoryOliParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflSppHistoryOliList" + name);
            if (viewModel == null)
            {
                viewModel = CflSppHistoryOli_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflSppHistoryOli_ParamModel cflSppHistoryOliParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflSppHistoryOli_Model.GetDataRowCount(args, userId, cflSppHistoryOliParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflSppHistoryOli_Model.GetData(args, userId, cflSppHistoryOliParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflSppHistoryOliParam = GetParam(Request);

            var viewModel = GetListModel(cflSppHistoryOliParam.Name);
            ProcessCustomBinding(userId, cflSppHistoryOliParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflSppHistoryOliParam);
        }

    }
}