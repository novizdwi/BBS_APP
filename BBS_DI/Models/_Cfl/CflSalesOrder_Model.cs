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
using System.Configuration;

namespace Models._Cfl
{
    public class CflSalesOrder_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"
    }


    public class CflSalesOrder_View__
    {
        public int DocEntry { get; set; }
        public string DocNum { get; set; }
        public DateTime? DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string JenisSO { get; set; }
        public string ShippingType { get; set; }
        public string Destination { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? OpenQty { get; set; }
        public string WhsCode { get; set; }

    }

    public class CflSalesOrder_Model
    {

        public static string ssql = @"
                                        SELECT * FROM
                                        (
                                        SELECT 
                                            T0.""DocEntry"",
                                            CONCAT(SUBSTRING_REGEXPR( '[^-]+' IN T1.""SeriesName"" FROM 1 OCCURRENCE 1),CONCAT('-', CAST(T0.""DocNum"" AS VARCHAR))) AS ""DocNum"",
                                            T0.""DocDate"",
                                            T0.""CardCode"",
                                            T0.""CardName"",
                                            T0.""U_JenisSO"" AS ""JenisSO"",
                                            T0.""U_ShippingType"" AS ""ShippingType"", 
                                            T0.""U_Area"" AS ""Destination"",
                                            T2.""ItemCode"",
                                            T3.""ItemName"" AS ""ItemName"",
                                            SUM(T2.""Quantity"") AS ""Quantity"",
                                            SUM(T2.""OpenQty"") AS ""OpenQty"", T2.""WhsCode""
                                        FROM ""{DbSap}"".""ORDR"" T0
                                        INNER JOIN ""{DbSap}"".""NNM1"" T1 ON T1.""Series"" = T0.""Series"" AND T1.""ObjectCode"" = T0.""ObjType""
                                        INNER JOIN ""{DbSap}"".""RDR1"" T2 ON T2.""DocEntry"" = T0.""DocEntry""
                                        INNER JOIN ""{DbSap}"".""OITM"" T3 ON T3.""ItemCode"" = T2.""ItemCode""  AND T3.""ItmsGrpCod"" = {GroupCode}
                                        WHERE  
                                        T0.""DocStatus"" ='O' 
                                        AND UPPER(T0.""U_JenisSO"") NOT IN ('TRANSPORTIR GROUP') 
                                        AND IFNULL(T0.""U_CashKeras"", 'N') = 'N'
                                        AND T2.""OpenQty"" > 0
                                        GROUP BY T0.""DocEntry"", T1.""SeriesName"", T2.""ItemCode"", T3.""ItemName"",T0.""DocNum"", T0.""DocDate"", T0.""CardCode"", 
		                                T0.""CardName"", T0.""U_JenisSO"", T0.""U_ShippingType"", T0.""U_Area"", T2.""ItemCode"", T2.""Dscription"", T2.""WhsCode""
                                        UNION ALL
                                        SELECT 
                                            T0.""DocEntry"",
                                            CONCAT(SUBSTRING_REGEXPR( '[^-]+' IN T1.""SeriesName"" FROM 1 OCCURRENCE 1),CONCAT('-', CAST(T0.""DocNum"" AS VARCHAR))) AS ""DocNum"",
                                            T0.""DocDate"",
                                            T0.""CardCode"",
                                            T0.""CardName"",
                                            T0.""U_JenisSO"" AS ""JenisSO"",
                                            T0.""U_ShippingType"" AS ""ShippingType"", 
                                            T0.""U_Area"" AS ""Destination"",
                                            T2.""ItemCode"",
                                            T3.""ItemName"" AS ""ItemName"",
                                            SUM(T2.""Quantity"") AS ""Quantity"",
                                            SUM(IFNULL(T2.""U_OpenQtyBBM"", T2.""Quantity"")) AS ""OpenQty"", T2.""WhsCode""
                                        FROM ""{DbSap}"".""ORDR"" T0
                                        INNER JOIN ""{DbSap}"".""NNM1"" T1 ON T1.""Series"" = T0.""Series"" AND T1.""ObjectCode"" = T0.""ObjType""
                                        INNER JOIN ""{DbSap}"".""RDR1"" T2 ON T2.""DocEntry"" = T0.""DocEntry""
                                        INNER JOIN ""{DbSap}"".""OITM"" T3 ON T3.""ItemCode"" = T2.""ItemCode""  AND T3.""ItmsGrpCod"" = {GroupCode}
                                        WHERE  
                                        T0.""DocStatus"" ='C' 
                                        AND UPPER(T0.""U_JenisSO"") NOT IN ('TRANSPORTIR GROUP') 
                                        AND IFNULL(T0.""U_CashKeras"", 'N') = 'Y'
                                        AND IFNULL(T2.""U_OpenQtyBBM"", T2.""Quantity"") > 0
                                        GROUP BY T0.""DocEntry"", T1.""SeriesName"", T2.""ItemCode"", T3.""ItemName"",T0.""DocNum"", T0.""DocDate"", T0.""CardCode"", 
		                                T0.""CardName"", T0.""U_JenisSO"", T0.""U_ShippingType"", T0.""U_Area"", T2.""ItemCode"", T2.""Dscription"", T2.""WhsCode""
                                        ) T0
                                        ORDER BY T0.""DocDate"" ASC
                                    ";

        public static void SetBindingData(GridViewModel state, int userId, CflSalesOrder_ParamModel cflParam)
        {
            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(state);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(state);

            using (var CONTEXT = new HANA_APP())
            {
                var dataRowCount = GetRowCount(CONTEXT, userId, cflParam, sqlCriteria);
                var dataList = GetDataList(CONTEXT, userId, cflParam, sqlCriteria, sqlSort, state.Pager.PageIndex, state.Pager.PageSize);

                state.ProcessCustomBinding(
                  new GridViewCustomBindingGetDataRowCountHandler(args =>
                  {
                      GetDataRowCount(args, dataRowCount);
                  }),
                  new GridViewCustomBindingGetDataHandler(args =>
                  {
                      GetData(args, dataList);
                  })
              );
            }
        }

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int dataRowCount)
        {
            e.DataRowCount = dataRowCount;
        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, List<CflSalesOrder_View__> dataList)
        {
            e.Data = dataList;
        }

        public static int GetRowCount(HANA_APP CONTEXT, int userId, CflSalesOrder_ParamModel cflParam, string sqlCriteria)
        {

            var Cfl_Sql = CflSalesOrder_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            string groupSadp = ConfigurationManager.AppSettings["GroupCode"];
            Cfl_Sql = Cfl_Sql.Replace("{GroupCode}", groupSadp);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());


            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }


            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflParam.SqlWhere != "")
            {
                sqlCriteria = cflParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            return dataRowCount;
        }

        public static List<CflSalesOrder_View__> GetDataList(HANA_APP CONTEXT, int userId, CflSalesOrder_ParamModel cflSalesOrderParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflSalesOrderParam.SqlWhere != "")
            {
                sqlCriteria = cflSalesOrderParam.SqlWhere + sqlCriteria;
            }



            var CflSalesOrders_ = GetDataList(CONTEXT, userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflSalesOrders_.Count == 0)
            {
                CflSalesOrder_View__ item = new CflSalesOrder_View__();
                CflSalesOrders_.Add(item);
            }


            return CflSalesOrders_;

        }

        public static List<CflSalesOrder_View__> GetDataList(HANA_APP CONTEXT, int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflSalesOrder_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            string groupSadp = ConfigurationManager.AppSettings["GroupCode"];
            Cfl_Sql = Cfl_Sql.Replace("{GroupCode}", groupSadp);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());



            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }

            if (string.IsNullOrEmpty(sqlSort))
            {
                sqlSort = " ORDER BY T0.\"DocDate\" DESC ";
            }


            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            var items = CONTEXT.Database.SqlQuery<CflSalesOrder_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflSalesOrder_ParamModel cflSalesOrderParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List SalesOrder";

            if (cflSalesOrderParam.Header != "")
            {
                settings.Name = "List SalesOrder " + cflSalesOrderParam.Header;
            }

            settings.KeyFieldName = "CardCode";
            settings.Columns.Add("CardCode");
            settings.Columns.Add("CardName");
            settings.Columns.Add("Address");
            settings.Columns.Add("Phone1");
            settings.Columns.Add("Phone2");
            settings.Columns.Add("Cellular");
            return settings;
        }


    }


}