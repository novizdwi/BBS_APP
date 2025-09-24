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
    public partial class _CflTongkangController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflTongkang_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflTongkang_Panel_List_Partial";

        public CflTongkang_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflTongkang_ParamModel();
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

            var CflTongkangParam = GetParam(Request);

            var viewModel = GetListModel(CflTongkangParam.Name);
            ProcessCustomBinding(userId, CflTongkangParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var CflTongkangParam = GetParam(Request);

            var viewModel = GetListModel(CflTongkangParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, CflTongkangParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var CflTongkangParam = GetParam(Request);

            var viewModel = GetListModel(CflTongkangParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, CflTongkangParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var CflTongkangParam = GetParam(Request);

            var viewModel = GetListModel(CflTongkangParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, CflTongkangParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflTongkangList" + name);
            if (viewModel == null)
            {
                viewModel = CflDocumentGeneral_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflTongkang_ParamModel CflTongkangParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflTongkang_Model.GetDataRowCount(args, userId, CflTongkangParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflTongkang_Model.GetData(args, userId, CflTongkangParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var CflTongkangParam = GetParam(Request);

            var viewModel = GetListModel(CflTongkangParam.Name);
            ProcessCustomBinding(userId, CflTongkangParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, CflTongkangParam);
        }

    }
}