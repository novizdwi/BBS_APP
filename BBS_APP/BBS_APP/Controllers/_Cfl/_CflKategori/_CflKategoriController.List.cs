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
    public partial class _CflKategoriController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflKategori_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflKategori_Panel_List_Partial";

        public CflKategori_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflKategori_ParamModel();
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

            var cflKategoriParam = GetParam(Request);

            var viewModel = GetListModel(cflKategoriParam.Name);
            ProcessCustomBinding(userId, cflKategoriParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflKategoriParam = GetParam(Request);

            var viewModel = GetListModel(cflKategoriParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflKategoriParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflKategoriParam = GetParam(Request);

            var viewModel = GetListModel(cflKategoriParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflKategoriParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflKategoriParam = GetParam(Request);

            var viewModel = GetListModel(cflKategoriParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflKategoriParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflKategoriList" + name);
            if (viewModel == null)
            {
                viewModel = CflKategori_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflKategori_ParamModel cflKategoriParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflKategori_Model.GetDataRowCount(args, userId, cflKategoriParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflKategori_Model.GetData(args, userId, cflKategoriParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflKategoriParam = GetParam(Request);

            var viewModel = GetListModel(cflKategoriParam.Name);
            ProcessCustomBinding(userId, cflKategoriParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflKategoriParam);
        }

    }
}