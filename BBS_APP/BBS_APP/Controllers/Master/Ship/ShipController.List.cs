using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.IO;
using System.Threading;


using System.Net;

using Models;
using Models.Master.Ship;

namespace Controllers.Master
{
    public partial class ShipController : BaseController
    {
        public ActionResult ListPartial()
        {
            int userId = (int)Session["userId"];

            var viewModel = GetListModel();
            ProcessCustomBinding(userId, viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            int userId = (int)Session["userId"];

            var viewModel = GetListModel();
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(userId, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering
       
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            int userId = (int)Session["userId"];

            var viewModel = GetListModel();
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(userId, viewModel);
           
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            int userId = (int)Session["userId"];

            var viewModel = GetListModel();
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(userId, viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

      

        static GridViewModel GetListModel()
        {
            var viewModel = GridViewExtension.GetViewModel("gvShipList");
            if (viewModel == null)
            {
                viewModel = Ship__List_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(int userId, GridViewModel viewModel)
        {
            Ship__List_Model.SetBindingData(viewModel, userId); 
        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            int userId = (int)Session["userId"];

            var viewModel = GetListModel();
            ProcessCustomBinding(userId, viewModel);

            return PartialView(VIEW_PANEL_LIST_PARTIAL, viewModel);
        } 

    }
}