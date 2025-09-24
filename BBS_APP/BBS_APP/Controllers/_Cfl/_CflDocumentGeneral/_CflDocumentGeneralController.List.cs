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
    public partial class _CflDocumentGeneralController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflDocumentGeneral_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflDocumentGeneral_Panel_List_Partial";

        public CflDocumentGeneral_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflDocumentGeneral_ParamModel();
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

            var cflDocumentGeneralParam = GetParam(Request);

            var viewModel = GetListModel(cflDocumentGeneralParam.Name);
            ProcessCustomBinding(userId, cflDocumentGeneralParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflDocumentGeneralParam = GetParam(Request);

            var viewModel = GetListModel(cflDocumentGeneralParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflDocumentGeneralParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflDocumentGeneralParam = GetParam(Request);

            var viewModel = GetListModel(cflDocumentGeneralParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflDocumentGeneralParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflDocumentGeneralParam = GetParam(Request);

            var viewModel = GetListModel(cflDocumentGeneralParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflDocumentGeneralParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflDocumentGeneralList" + name);
            if (viewModel == null)
            {
                viewModel = CflDocumentGeneral_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflDocumentGeneral_ParamModel cflDocumentGeneralParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflDocumentGeneral_Model.GetDataRowCount(args, userId, cflDocumentGeneralParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflDocumentGeneral_Model.GetData(args, userId, cflDocumentGeneralParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflDocumentGeneralParam = GetParam(Request);

            var viewModel = GetListModel(cflDocumentGeneralParam.Name);
            ProcessCustomBinding(userId, cflDocumentGeneralParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflDocumentGeneralParam);
        }

    }
}