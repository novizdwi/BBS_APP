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
    public partial class _CflNoArInvoiceController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflNoArInvoice_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflNoArInvoice_Panel_List_Partial";

        public CflNoArInvoice_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflNoArInvoice_ParamModel();
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

            var cflNoArInvoiceParam = GetParam(Request);

            var viewModel = GetListModel(cflNoArInvoiceParam.Name);
            ProcessCustomNoArInvoiceding(userId, cflNoArInvoiceParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflNoArInvoiceParam = GetParam(Request);

            var viewModel = GetListModel(cflNoArInvoiceParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomNoArInvoiceding(userId, cflNoArInvoiceParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflNoArInvoiceParam = GetParam(Request);

            var viewModel = GetListModel(cflNoArInvoiceParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomNoArInvoiceding(userId, cflNoArInvoiceParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflNoArInvoiceParam = GetParam(Request);

            var viewModel = GetListModel(cflNoArInvoiceParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomNoArInvoiceding(userId, cflNoArInvoiceParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflNoArInvoiceList" + name);
            if (viewModel == null)
            {
                viewModel = CflNoArInvoice_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomNoArInvoiceding(int userId, CflNoArInvoice_ParamModel cflParam, GridViewModel viewModel)
        {
            CflNoArInvoice_Model.SetBindingData(viewModel, userId, cflParam);



        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflNoArInvoiceParam = GetParam(Request);

            var viewModel = GetListModel(cflNoArInvoiceParam.Name);
            ProcessCustomNoArInvoiceding(userId, cflNoArInvoiceParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflNoArInvoiceParam);
        }

    }
}