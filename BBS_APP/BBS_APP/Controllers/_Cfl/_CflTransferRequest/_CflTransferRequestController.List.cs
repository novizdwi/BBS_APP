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
    public partial class _CflTransferRequestController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflTransferRequest_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflTransferRequest_Panel_List_Partial";

        public CflTransferRequest_ParamModel GetParam(HttpRequestBase Request, int userId = 0)
        {
            var cflParam = new CflTransferRequest_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];
            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            if (cflParam.Type == "TransferSummaryOut")
            {

                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");

                cflParam.SqlWhere = string.Format(" AND " +
                                                " T0.\"Id\" NOT IN (SELECT T0_.\"BaseEntry\" FROM \"Tx_TransferSummaryOut\" T0_ WHERE T0_.\"Status\"<>'Cancel' ) " +
                                                " ", hidden_CflDocId);


                //var hidden_CflSlpCode = (string)Request["hidden_CflSlpCode"];
                //hidden_CflSlpCode = hidden_CflSlpCode.Replace("'", "''");

                //cflParam.SqlWhere = string.Format(" AND Tx_Delivery___.SalesEmployeeId='{0}' ", hidden_CflSlpCode);
                //var hidden_CflBpCode = (string)Request["hidden_CflBpCode"];
                //hidden_CflBpCode = hidden_CflBpCode.Replace("'", "''");

                //cflParam.SqlWhere = string.Format(" AND Tx_TransferRequest___.BpCode='{0}' ", hidden_CflBpCode);

            }

            return cflParam;
        }


        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request, userId);  

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request, userId);  

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request, userId);  

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request, userId);  

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflTransferRequestList" + name);
            if (viewModel == null)
            {
                viewModel = CflTransferRequest_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflTransferRequest_ParamModel cflTransferRequestParam, GridViewModel viewModel)
        { 

            CflTransferRequest_Model.SetBindingData(viewModel, userId, cflTransferRequestParam ); 

        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request, userId);  

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);

            ViewBag.viewModel = viewModel; 

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflTransferRequestParam);
        }

    }
}