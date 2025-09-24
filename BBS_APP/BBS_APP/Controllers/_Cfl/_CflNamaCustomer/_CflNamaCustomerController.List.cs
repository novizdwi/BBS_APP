﻿using System;
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
    public partial class _CflNamaCustomerController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflNamaCustomer_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflNamaCustomer_Panel_List_Partial";

        public CflNamaCustomer_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflNamaCustomer_ParamModel();
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

            var cflNamaCustomerParam = GetParam(Request);

            var viewModel = GetListModel(cflNamaCustomerParam.Name);
            ProcessCustomNamaCustomerding(userId, cflNamaCustomerParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflNamaCustomerParam = GetParam(Request);

            var viewModel = GetListModel(cflNamaCustomerParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomNamaCustomerding(userId, cflNamaCustomerParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflNamaCustomerParam = GetParam(Request);

            var viewModel = GetListModel(cflNamaCustomerParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomNamaCustomerding(userId, cflNamaCustomerParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflNamaCustomerParam = GetParam(Request);

            var viewModel = GetListModel(cflNamaCustomerParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomNamaCustomerding(userId, cflNamaCustomerParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflNamaCustomerList" + name);
            if (viewModel == null)
            {
                viewModel = CflNamaCustomer_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomNamaCustomerding(int userId, CflNamaCustomer_ParamModel cflParam, GridViewModel viewModel)
        {
            CflNamaCustomer_Model.SetBindingData(viewModel, userId, cflParam);



        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflNamaCustomerParam = GetParam(Request);

            var viewModel = GetListModel(cflNamaCustomerParam.Name);
            ProcessCustomNamaCustomerding(userId, cflNamaCustomerParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflNamaCustomerParam);
        }

    }
}