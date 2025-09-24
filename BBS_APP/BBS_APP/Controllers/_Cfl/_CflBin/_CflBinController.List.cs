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
    public partial class _CflBinController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflBin_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflBin_Panel_List_Partial";

        public CflBin_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflBin_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            if (cflParam.Type == "Fpfs")
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");

                cflParam.SqlWhere = string.Format(" AND T0.\"WhsCode\" = (SELECT T0_.\"WarehouseCode\" FROM \"Tx_Fpfs\" T0_ WHERE T0_.\"Id\"={0} ) ", hidden_CflDocId);
            }
            else if (cflParam.Type == "BinFrom" || cflParam.Type == "BinTo") //InventorySend
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");

                var hidden_CflWarehouseCodeFrom = (string)Request["hidden_CflWarehouseCodeFrom"];
                hidden_CflWarehouseCodeFrom = hidden_CflWarehouseCodeFrom.Replace("'", "''");

                cflParam.SqlWhere = string.Format(" AND T0.\"WhsCode\" = '{0}' " , hidden_CflWarehouseCodeFrom);
            }
            else if (cflParam.Type == "InventoryReceipt") //InventorySend
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");

                var hidden_CflWarehouseCodeTo = (string)Request["hidden_CflWarehouseCodeTo"];
                hidden_CflWarehouseCodeTo = hidden_CflWarehouseCodeTo.Replace("'", "''");

                cflParam.SqlWhere = string.Format(" AND T0.\"WhsCode\" = '{0}' ", hidden_CflWarehouseCodeTo);
            }
            else if (cflParam.Type == "FillingShed" || cflParam.Type == "TangkiJalan" ) //LoCustomer -- LoadingOrder
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");

                var hidden_CflWarehouseCode = (string)Request["hidden_CflWarehouseCode"];
                hidden_CflWarehouseCode = hidden_CflWarehouseCode.Replace("'", "''");

                cflParam.SqlWhere = string.Format(" AND T0.\"WhsCode\" = '{0}' ", hidden_CflWarehouseCode);
            }
            else if (cflParam.Type == "AdjustmentOut" || cflParam.Type == "AdjustmentIn") //LoCustomer -- LoadingOrder
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");

                var hidden_CflWarehouseCode = (string)Request["hidden_CflWarehouseCode"];
                hidden_CflWarehouseCode = hidden_CflWarehouseCode.Replace("'", "''");

                cflParam.SqlWhere = string.Format(" AND T0.\"WhsCode\" = '{0}' ", hidden_CflWarehouseCode);
            }
            else if (cflParam.Type == "InventoryIn")
            {
                var hidden_CflDocId = (string)Request["hidden_CflDocId"];
                hidden_CflDocId = hidden_CflDocId.Replace("'", "''");

                cflParam.SqlWhere = string.Format(" AND T0.\"WhsCode\" = (SELECT T0_.\"WarehouseCode\" FROM \"Tx_InventoryIn\" T0_ WHERE T0_.\"Id\"={0} ) ", hidden_CflDocId);
            }

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflBinParam = GetParam(Request);

            var viewModel = GetListModel(cflBinParam.Name);
            ProcessCustomBinding(userId, cflBinParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflBinParam = GetParam(Request);

            var viewModel = GetListModel(cflBinParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflBinParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflBinParam = GetParam(Request);

            var viewModel = GetListModel(cflBinParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflBinParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflBinParam = GetParam(Request);

            var viewModel = GetListModel(cflBinParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflBinParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflBinList" + name);
            if (viewModel == null)
            {
                viewModel = CflBin_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflBin_ParamModel cflParam, GridViewModel viewModel)
        {
            CflBin_Model.SetBindingData(viewModel, userId, cflParam);



        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflBinParam = GetParam(Request);

            var viewModel = GetListModel(cflBinParam.Name);
            ProcessCustomBinding(userId, cflBinParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflBinParam);
        }

    }
}