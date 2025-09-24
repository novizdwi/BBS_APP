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
    public partial class _CflPurchaseOrderOliController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflPurchaseOrderOli_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflPurchaseOrderOli_Panel_List_Partial";

        public CflPurchaseOrderOli_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflPurchaseOrderOli_ParamModel();
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

            var cflPurchaseOrderOliParam = GetParam(Request);

            var viewModel = GetListModel(cflPurchaseOrderOliParam.Name);
            ProcessCustomPurchaseOrderOliding(userId, cflPurchaseOrderOliParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflPurchaseOrderOliParam = GetParam(Request);

            var viewModel = GetListModel(cflPurchaseOrderOliParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomPurchaseOrderOliding(userId, cflPurchaseOrderOliParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflPurchaseOrderOliParam = GetParam(Request);

            var viewModel = GetListModel(cflPurchaseOrderOliParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomPurchaseOrderOliding(userId, cflPurchaseOrderOliParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflPurchaseOrderOliParam = GetParam(Request);

            var viewModel = GetListModel(cflPurchaseOrderOliParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomPurchaseOrderOliding(userId, cflPurchaseOrderOliParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflPurchaseOrderOliList" + name);
            if (viewModel == null)
            {
                viewModel = CflPurchaseOrderOli_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomPurchaseOrderOliding(int userId, CflPurchaseOrderOli_ParamModel cflParam, GridViewModel viewModel)
        {
            CflPurchaseOrderOli_Model.SetBindingData(viewModel, userId, cflParam);



        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflPurchaseOrderOliParam = GetParam(Request);

            var viewModel = GetListModel(cflPurchaseOrderOliParam.Name);
            ProcessCustomPurchaseOrderOliding(userId, cflPurchaseOrderOliParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflPurchaseOrderOliParam);
        }

    }
}