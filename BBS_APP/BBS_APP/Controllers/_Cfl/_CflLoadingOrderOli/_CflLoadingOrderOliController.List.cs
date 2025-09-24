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
    public partial class _CflLoadingOrderOliController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflLoadingOrderOli_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflLoadingOrderOli_Panel_List_Partial";

        public CflLoadingOrderOli_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflLoadingOrderOli_ParamModel();
            cflParam.Type = Request["hidden_CflType"];
            cflParam.Name = Request["hidden_CflName"];
            cflParam.Header = Request["hidden_CflHeader"];
            cflParam.SqlWhere = Request["hidden_CflSqlWhere"];

            //if (cflParam.Type == "Receipt")
            //{
            //    cflParam.SqlWhere = string.Format(" AND T0.\"Status\" = 'Delivered' ");
            //}

            cflParam.IsMulti = Request["hidden_CflIsMulti"];

            return cflParam;
        }

        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var cflLoadingOrderOliParam = GetParam(Request);

            var viewModel = GetListModel(cflLoadingOrderOliParam.Name);
            ProcessCustomBinding(userId, cflLoadingOrderOliParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflLoadingOrderOliParam = GetParam(Request);

            var viewModel = GetListModel(cflLoadingOrderOliParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflLoadingOrderOliParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflLoadingOrderOliParam = GetParam(Request);

            var viewModel = GetListModel(cflLoadingOrderOliParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflLoadingOrderOliParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflLoadingOrderOliParam = GetParam(Request);

            var viewModel = GetListModel(cflLoadingOrderOliParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflLoadingOrderOliParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflLoadingOrderOliList" + name);
            if (viewModel == null)
            {
                viewModel = CflLoadingOrderOli_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflLoadingOrderOli_ParamModel cflLoadingOrderOliParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflLoadingOrderOli_Model.GetDataRowCount(args, userId, cflLoadingOrderOliParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflLoadingOrderOli_Model.GetData(args, userId, cflLoadingOrderOliParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflLoadingOrderOliParam = GetParam(Request);

            var viewModel = GetListModel(cflLoadingOrderOliParam.Name);
            ProcessCustomBinding(userId, cflLoadingOrderOliParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflLoadingOrderOliParam);
        }

    }
}