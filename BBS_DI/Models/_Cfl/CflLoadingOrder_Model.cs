using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Models;
using Models._Utils;
using System.Data;
using System.Dynamic;
using BBS_DI.Models._EF;
using Models._Ef;

namespace Models._Cfl
{
    public class CflLoadingOrder_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflLoadingOrder_View__
    {

        public long Id { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public string WarehouseCode { get; set; }

        public string ShippingType { get; set; }

        public string Owner { get; set; }

        public int? UnitId { get; set; }

        public string UnitName { get; set; }

        public string UnitPoliceNo { get; set; }

        public decimal? UnitCapacity { get; set; }

        public int? DriverId { get; set; }

        public string DriverName { get; set; }

        public string Status { get; set; }

        public decimal? QtyDelivery { get; set; }
    }

    public class CflLoadingOrder_Model
    {
        public static string ssql = @"
        SELECT *
        FROM (
	        SELECT T0.""Id"",  T0.""TransNo"", T0.""TransDate"", T0.""WarehouseCode""
            ,  T0.""ShippingType"", T0.""Owner"", T0.""UnitId"", T0.""UnitName"", T0.""UnitPoliceNo"", T0.""UnitCapacity""
            , T0.""DriverId"", T0.""DriverName""
            , T0.""Status""
            , (SELECT SUM(IFNULL(T1.""QtyDeliver"",0)) FROM ""Tx_LoadingOrder_SalesOrder"" T1 WHERE T1.""Id"" = T0.""Id"") 
            AS ""QtyDelivery""
	        FROM  ""Tx_LoadingOrder"" T0
	        LEFT OUTER JOIN ""Tx_Receipt"" T2 ON T0.""Id"" = T2.""SuratJalanId"" AND T0.""TransNo"" = T2.""SuratJalanNo"" AND T2.""Status"" <> 'Cancel'
	        WHERE T2.""Id"" IS NULL
        ) T0
        WHERE 1=1
                                    ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflLoadingOrder_ParamModel cflLoadingOrderParam)
        {

            var Cfl_Sql = CflLoadingOrder_Model.ssql;

            //Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflLoadingOrderParam.SqlWhere != "")
            {
                sqlCriteria = cflLoadingOrderParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflLoadingOrder_ParamModel cflLoadingOrderParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflLoadingOrderParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflLoadingOrder_View__> GetDataList(int userId, CflLoadingOrder_ParamModel cflLoadingOrderParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {
            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }
            if (sqlSort == null)
            {
                sqlSort = "";
            }

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflLoadingOrderParam.SqlWhere != "")
            {
                sqlCriteria = cflLoadingOrderParam.SqlWhere + sqlCriteria;
            }

              

            var CflLoadingOrders_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflLoadingOrders_.Count == 0)
            {
                CflLoadingOrder_View__ item = new CflLoadingOrder_View__();
                CflLoadingOrders_.Add(item);
            }


            return CflLoadingOrders_;

        }

        public static List<CflLoadingOrder_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflLoadingOrder_Model.ssql;

            //Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());



            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }
            if (sqlSort == null)
            {
                sqlSort = "";
            }

            //if (sqlCriteria != "")
            //{
            //    sqlCriteria = " AND (" + sqlCriteria + ")";
            //}

            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            var items = DbProvider.dbApp.Database.SqlQuery<CflLoadingOrder_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflLoadingOrder_ParamModel cflLoadingOrderParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List LoadingOrder";

            if (cflLoadingOrderParam.Header != "")
            {
                settings.Name = "List LoadingOrder " + cflLoadingOrderParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("LoadingOrderName");
            return settings;
        }


    }

}