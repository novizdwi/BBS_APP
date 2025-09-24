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
    public partial class _CflSegelController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflSegel_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflSegel_Panel_List_Partial";

        public CflSegel_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflSegel_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "LoadingOrder")
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");

                cflParam.SqlWhere = string.Format(" " +
                                                    " AND NOT EXISTS(SELECT T0_.\"DetId\" " +
                                                    " FROM \"Tx_LoadingOrder_Segel\" T0_   " +
                                                    " WHERE T0_.\"Id\"={0}  AND T0.\"SegelNo\" = T0_.\"SegelNo\" )", hidden_CflDocId);

            }

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflSegelParam = GetParam(Request);

            var viewModel = GetListModel(cflSegelParam.Name);
            ProcessCustomBinding(userId, cflSegelParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflSegelParam = GetParam(Request);

            var viewModel = GetListModel(cflSegelParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflSegelParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflSegelParam = GetParam(Request);

            var viewModel = GetListModel(cflSegelParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflSegelParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflSegelParam = GetParam(Request);

            var viewModel = GetListModel(cflSegelParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflSegelParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflSegelList" + name);
            if (viewModel == null)
            {
                viewModel = CflSegel_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflSegel_ParamModel cflSegelParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflSegel_Model.GetDataRowCount(args, userId, cflSegelParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflSegel_Model.GetData(args, userId, cflSegelParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflSegelParam = GetParam(Request);

            var viewModel = GetListModel(cflSegelParam.Name);
            ProcessCustomBinding(userId, cflSegelParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflSegelParam);
        }

    }
}