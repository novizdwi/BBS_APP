using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using Models._Ef;
using BBS_DI.Models._EF;
using Models._Utils;
using System.Linq;

namespace Models._Alert
{


    public class Alert_View__
    {
        public Tp_UserAlert Tp_UserAlert___ { get; set; }
        public string TransNo { get; set; }
    }

    public static class Alert_Model
    {


        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId)
        {


            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);



            //if (sqlCriteria != "")
            //{
            //    sqlCriteria = " AND ( " + sqlCriteria + " )";
            //}

            //var ssql = PetaPoco.Sql.Builder
            //    .Append("SELECT TOP 24 COUNT(*) AS IDU ")
            //    .Append("FROM Tp_UserAlert Tp_UserAlert___ ")
            //    .Append("WHERE Tp_UserAlert___.UserId = @0 ", userId)
            //    .Append(sqlCriteria);
            //int dataRowCount;
            //dataRowCount = dbApp.<int>(ssql);
            //e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId)
        {


            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);


            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<Alert_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            List<Alert_View__> Alerts_ = new List<Alert_View__>();

            //if (sqlCriteria == null)
            //{
            //    sqlCriteria = "";
            //}
            //if (sqlSort == null)
            //{
            //    sqlSort = "";
            //}

            //if (sqlCriteria != "")
            //{
            //    sqlCriteria = " AND (" + sqlCriteria + ")";
            //}

            //if (sqlSort == "")
            //{
            //    sqlSort = "ORDER BY Tp_UserAlert___.Id DESC  ";
            //}
            //var ssql = PetaPoco.Sql.Builder
            //    .Append("SELECT TOP 24 Tp_UserAlert___.*")
            //    .Append("FROM Tp_UserAlert Tp_UserAlert___ ")
            //    .Append("WHERE Tp_UserAlert___.UserId = @0 ", userId)
            //    .Append(sqlCriteria)
            //    .Append(sqlSort ,
            //    new
            //    {
            //        start = (PageIndex) * PageSize,
            //        pageSize = PageSize
            //    }
            //);


            //List<Alert_View__> Alerts_;


            //Alerts_ = DbProvider.dbApp.FetchManyToOne<Alert_View__, Tp_UserAlert>(m => m.GetHashCode(), ssql);

            //if (Alerts_.Count == 0)
            //{
            //    Alert_View__ item = new Alert_View__();
            //    Alerts_.Add(item);
            //}



            return Alerts_;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings()
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List User Alert";

            settings.KeyFieldName = "Tp_UserAlert___.Id";
            settings.Columns.Add("Tp_UserAlert___.Id").Visible = false;
            settings.Columns.Add("Tm_Alert___.Id").Visible = false;
            settings.Columns.Add("Tm_Alert___.AlertName");
            settings.Columns.Add("Tp_UserAlert___.LastShowTime");

            return settings;
        }

        public static string AjaxAlert(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return AjaxAlert(CONTEXT, userId);
            }
        }

        public static string AjaxAlert(HANA_APP CONTEXT, int userId)
        {
            var ssql = @"SELECT T0.""Id"" FROM ""Tp_UserAlert"" T0 WHERE T0.""UserId"" = :p0 AND ""IsShow"" ='Y' ";
            int? Id = CONTEXT.Database.SqlQuery<int?>(ssql, userId).FirstOrDefault();

            if ((Id.HasValue ? Id.Value : 0) == 0)
            {
                return "N";
            }
            else
            {
                return "Y";
            }

        }

        public static string CheckUnread(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return CheckUnread(CONTEXT, userId);
            }
        }

        public static string CheckUnread(HANA_APP CONTEXT, int userId)
        {
            return CONTEXT.Database.SqlQuery<string>(
            @" SELECT COUNT(Tx.""countAlert"") 
                FROM(
                SELECT TOP 11 1 AS ""CountAlert""
                FROM ""Tp_UserAlert"" T0
                WHERE T0.""UserId"" = 1
                    AND(T0.""IsRead"" = 'N' OR T0.""IsRead"" IS NULL)
                )Tx
            ", userId).FirstOrDefault();
        }

        public static void AjaxAlertNonActive(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    //DbProvider.dbApp.Execute("UPDATE T0 SET T0.""IsShow""='N' FROM ""Tp_UserAlert"" T0 WHERE T0.""UserId"" = :p0", userId);  

                }
            }

        }




    }


}