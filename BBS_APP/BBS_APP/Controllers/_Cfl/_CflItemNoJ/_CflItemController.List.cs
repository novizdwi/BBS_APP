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
    public partial class _CflItemNoJController : BaseController
    {
        string VIEW_LIST_PARTIAL = "Partial/_CflItemNoJ_List_Partial";
        string VIEW_PANEL_LIST_PARTIAL = "Partial/_CflItemNoJ_Panel_List_Partial";

        public CflItemNoJ_ParamModel GetParam(HttpRequestBase Request)
        {
            var cflParam = new CflItemNoJ_ParamModel();
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

            var cflItemNoJParam = GetParam(Request);

            var viewModel = GetListModel(cflItemNoJParam.Name);
            ProcessCustomBinding(userId, cflItemNoJParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var cflItemNoJParam = GetParam(Request);

            var viewModel = GetListModel(cflItemNoJParam.Name);
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, cflItemNoJParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering 
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var cflItemNoJParam = GetParam(Request);

            var viewModel = GetListModel(cflItemNoJParam.Name);
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, cflItemNoJParam, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var cflItemNoJParam = GetParam(Request);

            var viewModel = GetListModel(cflItemNoJParam.Name);
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, cflItemNoJParam, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }



        static GridViewModel GetListModel(string name)
        {
            var viewModel = GridViewExtension.GetViewModel("gvCflItemNoJList" + name);
            if (viewModel == null)
            {
                viewModel = CflItem_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, CflItemNoJ_ParamModel cflItemNoJParam, GridViewModel viewModel)
        {

            viewModel.ProcessCustomBinding(
              new GridViewCustomBindingGetDataRowCountHandler(args =>
              {
                  CflItemNoJ_Model.GetDataRowCount(args, userId, cflItemNoJParam);
              }),
              new GridViewCustomBindingGetDataHandler(args =>
              {
                  CflItemNoJ_Model.GetData(args, userId, cflItemNoJParam);
              })
          );


        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var cflItemNoJParam = GetParam(Request);

            var viewModel = GetListModel(cflItemNoJParam.Name);
            ProcessCustomBinding(userId, cflItemNoJParam, viewModel);

            ViewBag.viewModel = viewModel;

            return PartialView(VIEW_PANEL_LIST_PARTIAL, cflItemNoJParam);
        }

    }
}