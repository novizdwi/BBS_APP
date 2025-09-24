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
    //string CardCode;
    public class CflPurchaseOrder_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"
        //public string CardCode { get; set; }
        
    }


    public class CflPurchaseOrder_View__
    {
        public Int32 DocEntry { get; set; }
        public Int32 LineNum { get; set; }
        public string DocNum { get; set; }
        public DateTime? DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? OpenQuantity { get; set; }
        public string SoDocNum { get; set; }
    }

    public class CflPurchaseOrder_Model
    {
       // public string CardCode { get; set; }

        //public static string ssql = @"SELECT T0.""DocEntry"",
        //                                    T1.""LineNum"",
        //                                    T0.""DocNum"",
        //                                    T0.""DocDate"",
        //                                    T0.""CardCode"",
        //                                    T0.""CardName"",
        //                                    T1.""ItemCode"",
        //                                    T1.""Dscription"" AS ""ItemName"",
        //                                    T1.""WhsCode"",
        //                                    T2.""WhsName"",
        //                                    T1.""Quantity"",
        //                                    T1.""OpenQty"" AS ""OpenQuantity"",
        //                                    T4.""ItmsGrpNam"",
        //                                    T3.""ItmsGrpCod""
        //                                FROM ""{DbSap}"".""OPOR"" T0
        //                                INNER JOIN ""{DbSap}"".""POR1"" T1 ON T0.""DocEntry"" = T1.""DocEntry""
        //                                INNER JOIN ""{DbSap}"".""OWHS"" T2 ON T1.""WhsCode"" = T2.""WhsCode""
        //                                INNER JOIN ""{DbSap}"".""OITM"" T3 ON T1.""ItemCode"" = T3.""ItemCode""
        //                                INNER JOIN ""{DbSap}"".""OITB"" T4 ON T3.""ItmsGrpCod"" = T4.""ItmsGrpCod""
        //                                WHERE T1.""LineStatus"" = 'O' AND IFNULL(T1.""OpenQty"",0) > 0 AND T0.""DocStatus"" = 'O' 
        //                                ORDER BY T0.""DocDate"" ASC
        //                            ";
        public static string ssql = @"SELECT DISTINCT T0.""DocEntry"",
                                                    T0.""DocNum"",
                                                    T0.""DocDate"",
                                                    T0.""CardCode"",
                                                    T0.""CardName""FROM ""{DbSap}"".""OPOR"" T0
                                        INNER JOIN ""{DbSap}"".""POR1"" T1 ON T0.""DocEntry"" = T1.""DocEntry""
                                        INNER JOIN ""{DbSap}"".""OWHS"" T2 ON T1.""WhsCode"" = T2.""WhsCode""
                                        INNER JOIN ""{DbSap}"".""OITM"" T3 ON T1.""ItemCode"" = T3.""ItemCode""
                                        INNER JOIN ""{DbSap}"".""OITB"" T4 ON T3.""ItmsGrpCod"" = T4.""ItmsGrpCod""
                                        WHERE T1.""LineStatus"" = 'O' AND IFNULL(T1.""OpenQty"",0) > 0 AND T0.""DocStatus"" = 'O'
                                        ORDER BY T0.""DocDate"" DESC
                                        ";




        public static void SetBindingData(GridViewModel state, int userId, CflPurchaseOrder_ParamModel cflParam)
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

        public static void GetData(GridViewCustomBindingGetDataArgs e, List<CflPurchaseOrder_View__> dataList)
        {
            e.Data = dataList;
        }

        public static int GetRowCount(HANA_APP CONTEXT, int userId, CflPurchaseOrder_ParamModel cflParam, string sqlCriteria)
        {

            var Cfl_Sql = CflPurchaseOrder_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            //Cfl_Sql = Cfl_Sql.Replace(":p0", cflParam.CardCode);

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

        public static List<CflPurchaseOrder_View__> GetDataList(HANA_APP CONTEXT, int userId, CflPurchaseOrder_ParamModel cflPurchaseOrderParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflPurchaseOrderParam.SqlWhere != "")
            {
                sqlCriteria = cflPurchaseOrderParam.SqlWhere + sqlCriteria;
            }



            var CflPurchaseOrders_ = GetDataList(CONTEXT, userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflPurchaseOrders_.Count == 0)
            {
                CflPurchaseOrder_View__ item = new CflPurchaseOrder_View__();
                CflPurchaseOrders_.Add(item);
            }


            return CflPurchaseOrders_;

        }

        public static List<CflPurchaseOrder_View__> GetDataList(HANA_APP CONTEXT, int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflPurchaseOrder_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());



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

            var items = CONTEXT.Database.SqlQuery<CflPurchaseOrder_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflPurchaseOrder_ParamModel cflPurchaseOrderParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List PurchaseOrder";

            if (cflPurchaseOrderParam.Header != "")
            {
                settings.Name = "List PurchaseOrder " + cflPurchaseOrderParam.Header;
            }

            settings.KeyFieldName = "WhsCode";
            settings.Columns.Add("WhsName");
            return settings;
        }


    }


}