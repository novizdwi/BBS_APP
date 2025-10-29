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
    public partial class _CflTransferSummaryOutController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflTransferSummaryOut_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflTransferSummaryOut_Panel_List_Partial";

        public CflTransferSummaryOut_ParamModel GetParam(HttpRequestBase Request, int userId = 0)
        {
            var cflParam = new CflTransferSummaryOut_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];
            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            if (cflParam.Type == "TransferSummaryIn")
            {

                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");

                cflParam.SqlWhere = string.Format(" AND " +
                                                " T0.\"Id\" NOT IN (SELECT T0_.\"BaseEntry\" FROM \"Tx_TransferSummaryIn\" T0_ WHERE T0_.\"Status\"='Cancel' )" +
                                                " ", hidden_CflDocId);
              }

            return cflParam;
        }


        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflTransferSummaryOutParam = GetParam(Request, userId);  

            var viewModel = GetListModel(cflTransferSummaryOutParam.Name);
            ProcessCustomBinding(userId, cflTransferSummaryOutParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflTransferSummaryOutParam = GetParam(Request, userId);  

            var viewModel = GetListModel(cflTransferSummaryOutParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflTransferSummaryOutParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflTransferSummaryOutParam = GetParam(Request, userId);  

            var viewModel = GetListModel(cflTransferSummaryOutParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflTransferSummaryOutParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflTransferSummaryOutParam = GetParam(Request, userId);  

            var viewModel = GetListModel(cflTransferSummaryOutParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflTransferSummaryOutParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflTransferSummaryOutList" + name);
            if (viewModel == null)
            {
                viewModel = CflTransferSummaryOut_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflTransferSummaryOut_ParamModel cflTransferSummaryOutParam, GridViewModel viewModel)
        { 

            CflTransferSummaryOut_Model.SetBindingData(viewModel, userId, cflTransferSummaryOutParam ); 

        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflTransferSummaryOutParam = GetParam(Request, userId);  

            var viewModel = GetListModel(cflTransferSummaryOutParam.Name);
            ProcessCustomBinding(userId, cflTransferSummaryOutParam, viewModel);

            ViewBag.viewModel = viewModel; 

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflTransferSummaryOutParam);
        }

    }
}