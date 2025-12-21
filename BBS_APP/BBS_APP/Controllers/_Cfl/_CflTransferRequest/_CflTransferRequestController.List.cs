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

        public CflTransferRequest_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflTransferRequest_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "TransferSummaryIn")
            {
                cflParam.SqlWhere = string.Format(@"                 
                AND EXISTS(
                    SELECT 1
                    FROM ""Tx_TransferIn"" Ta
                    WHERE Ta.""Status"" = 'Posted'
                    AND T0.""Id"" = Ta.""BaseEntry""
                    AND COALESCE(Ta.""BaseEntry"", 0) != 0
                    AND NOT EXISTS(
                        SELECT 1
                        FROM ""Tx_TransferSummaryIn"" Tx
                        INNER JOIN ""Tx_TransferSummaryIn_Ref"" Ty ON Tx.""Id"" = Ty.""Id""
                        WHERE Ty.""BaseId"" = Ta.""Id""
                        AND Tx.""Status"" != 'Cancel'
                    )
               ) ");
            }

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request);

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request);

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request);

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request);

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

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflTransferRequest_Model.GetDataRowCount(args, userId, cflTransferRequestParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflTransferRequest_Model.GetData(args, userId, cflTransferRequestParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflTransferRequestParam = GetParam(Request);

            var viewModel = GetListModel(cflTransferRequestParam.Name);
            ProcessCustomBinding(userId, cflTransferRequestParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflTransferRequestParam);
        }

    }
}