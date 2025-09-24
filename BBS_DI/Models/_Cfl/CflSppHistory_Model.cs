using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Models;
using Models._Utils;
using System.Data;
using System.Dynamic;
using BBS_DI.Models._EF;
using Models._Ef;

namespace Models._Cfl
{
    public class CflSppHistory_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflSppHistory_View__
    {

        public long Id { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public string TransType { get; set; }

        public string Owner { get; set; }

        public string SppOwner { get; set; }

        public string ShippingType { get; set; }

        public Int32? DriverId { get; set; }

        public string DriverName { get; set; }

        public Int32? UnitId { get; set; }

        public string UnitName { get; set; }

        public string UnitPoliceNo { get; set; }

        public string TinggiTerra { get; set; }

        public decimal? UnitCapacity { get; set; }

        public string IsUseTangkiJalan { get; set; }

        public decimal? TangkiJalanCapacity { get; set; }

        public string IsNewRoute { get; set; }

        public long? OldSppRouteId { get; set; }

        public string OldSppRouteNo { get; set; }

        public string IsNewUnit { get; set; }

        public long? OldSppUnitId { get; set; }

        public string OldSppUnitNo { get; set; }

    }

    public class CflSppHistory_Model
    {

        public static string ssql = @"SELECT T0.""Id"",  T0.""TransNo"", T0.""TransDate"", CASE WHEN T0.""Owner"" = 'KGB' THEN 'SADPI' ELSE T0.""Owner"" END AS ""Owner"", 'SADPI' AS ""SppOwner"", T0.""TransType"" ,  T0.""ShippingType"", T0.""DriverId"", T0.""DriverName"", T0.""UnitId"" ,T0.""UnitName"", T0.""UnitPoliceNo"", T0.""TinggiTerra"", T0.""UnitCapacity"",CASE WHEN T0.""TangkiJalanType"" = 'Eksternal' THEN IFNULL(T0.""IsUseTangkiJalan"",'N') ELSE 'N' END AS ""IsUseTangkiJalan"", CASE WHEN T0.""TangkiJalanType"" = 'Eksternal' THEN T0.""TangkiJalanCapacity"" ELSE NULL END AS ""TangkiJalanCapacity"",
                                    T0.""IsNewRoute"",  T0.""OldLoadingOrderId"" AS ""OldSppRouteId"",  T0.""OldLoadingOrderNo"" AS ""OldSppRouteNo"", T0.""IsNewUnit"",  T0.""OldLoId"" AS ""OldSppUnitId"",  T0.""OldLoNo"" AS ""OldSppUnitNo""
                                    FROM ""{DbAppSadpI}"".""Tx_LoadingOrder"" T0
                                    WHERE T0.""Status"" = 'Posted' 
                                    AND  
			                        (
				                        EXISTS(
					                        SELECT T1_.""Id"" FROM ""{DbAppSadpI}"".""Tx_LoadingOrder_SalesOrder"" T1_ 
					                        WHERE T1_.""BaseOwner"" = {OwnerApp} AND T0.""Id"" = T1_.""Id""
				                        ) OR
				                        (
                                            --Harusnya ini cuma ke SADPII aja gak ke KGB (DML/SADPI yang pake TJ Eksternal) 
					                        {OwnerApp} <> 'KGB' AND
                                            --Standartnya
                                            T0.""TangkiJalanType"" = 'Eksternal' AND IFNULL(T0.""IsUseTangkiJalan"",'N') = 'Y'
                                            AND IFNULL(T0.""IsNewRoute"", 'N') != 'Y'
				                        )
			                        )
                                    UNION ALL
                                    SELECT T0.""Id"",  T0.""TransNo"", T0.""TransDate"", CASE WHEN T0.""Owner"" = 'KGB' THEN 'DML' ELSE T0.""Owner"" END AS ""Owner"", 'DML' AS ""SppOwner"", T0.""TransType"" ,  T0.""ShippingType"", T0.""DriverId"", T0.""DriverName"", T0.""UnitId"" ,T0.""UnitName"", T0.""UnitPoliceNo"", T0.""TinggiTerra"", T0.""UnitCapacity"",CASE WHEN T0.""TangkiJalanType"" = 'Eksternal' THEN IFNULL(T0.""IsUseTangkiJalan"",'N') ELSE 'N' END AS ""IsUseTangkiJalan"", CASE WHEN T0.""TangkiJalanType"" = 'Eksternal' THEN T0.""TangkiJalanCapacity"" ELSE NULL END AS ""TangkiJalanCapacity"",
                                    T0.""IsNewRoute"",  T0.""OldLoadingOrderId"" AS ""OldSppRouteId"",  T0.""OldLoadingOrderNo"" AS ""OldSppRouteNo"", T0.""IsNewUnit"",  T0.""OldLoId"" AS ""OldSppUnitId"",  T0.""OldLoNo"" AS ""OldSppUnitNo""
                                    FROM ""{DbAppDml}"".""Tx_LoadingOrder"" T0
                                    WHERE T0.""Status"" = 'Posted' 
                                    AND  
			                        (
				                        EXISTS(
					                        SELECT T1_.""Id"" FROM ""{DbAppDml}"".""Tx_LoadingOrder_SalesOrder"" T1_ 
					                        WHERE T1_.""BaseOwner"" = {OwnerApp} AND T0.""Id"" = T1_.""Id""
				                        ) OR
				                        (
                                             --Harusnya ini cuma ke SADPII aja gak ke KGB (DML/SADPI yang pake TJ Eksternal) 
					                        {OwnerApp} <> 'KGB' AND
                                            --Standartnya
					                        T0.""TangkiJalanType"" = 'Eksternal' AND IFNULL(T0.""IsUseTangkiJalan"",'N') = 'Y'
                                            AND IFNULL(T0.""IsNewRoute"", 'N') != 'Y'
				                        )
			                        )
                                    ";
                                   
                                  
       //Status = 0 means in stock
        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflSppHistory_ParamModel cflSppHistoryParam)
        {

            var Cfl_Sql = CflSppHistory_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbAppSadpI}", DbProvider.dbSadpIApp_Name);
            Cfl_Sql = Cfl_Sql.Replace("{DbAppDml}", DbProvider.dbDmlApp_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());
            string ownerApp = ConfigurationManager.AppSettings["OwnerApp"];
            Cfl_Sql = Cfl_Sql.Replace("{OwnerApp}", "'" + ownerApp + "'");

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflSppHistoryParam.SqlWhere != "")
            {
                sqlCriteria = cflSppHistoryParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflSppHistory_ParamModel cflSppHistoryParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflSppHistoryParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflSppHistory_View__> GetDataList(int userId, CflSppHistory_ParamModel cflSppHistoryParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {



            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }
            if (string.IsNullOrEmpty(sqlSort))
            {
                sqlSort = " ORDER BY T0.\"TransNo\" ASC ";
            }

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflSppHistoryParam.SqlWhere != "")
            {
                sqlCriteria = cflSppHistoryParam.SqlWhere + sqlCriteria;
            }

              

            var CflSppHistorys_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflSppHistorys_.Count == 0)
            {
                CflSppHistory_View__ item = new CflSppHistory_View__();
                CflSppHistorys_.Add(item);
            }


            return CflSppHistorys_;

        }

        public static List<CflSppHistory_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflSppHistory_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbAppSadpI}", DbProvider.dbSadpIApp_Name);
            Cfl_Sql = Cfl_Sql.Replace("{DbAppDml}", DbProvider.dbDmlApp_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());
            string ownerApp = ConfigurationManager.AppSettings["OwnerApp"];
            Cfl_Sql = Cfl_Sql.Replace("{OwnerApp}", "'" + ownerApp + "'");

            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }
            if (sqlSort == null)
            {
                sqlSort = "";
            }


            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            var items = DbProvider.dbApp.Database.SqlQuery<CflSppHistory_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflSppHistory_ParamModel cflSppHistoryParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List SppHistory";

            if (cflSppHistoryParam.Header != "")
            {
                settings.Name = "List SppHistory " + cflSppHistoryParam.Header;
            }

            settings.KeyFieldName = "SppHistoryCode";
            settings.Columns.Add("SppHistoryName");
            return settings;
        }


    }

}