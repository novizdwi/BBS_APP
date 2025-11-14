using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using Models;

using Models._Utils;
using Models._Ef;
using Models._Cfl;
using BBS_DI.Models._EF;
using System.Linq;

namespace Models._Cfl
{
    public class CflStockOpname_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"
    }

    public class CflStockOpname_View__
    {
        public string Id { get; set; }

        public int? RequestId { get; set; }
        public string RequestNo { get; set; }

        public DateTime? TransDate { get; set; }

        public string WhsCode { get; set; }
        public string WhsName { get; set; }

        public string Comments { get; set; }
    }

    public class CflStockOpname_Model
    {
        public static string ssql = @"
            SELECT DISTINCT T0.""RequestId"" AS ""Id"", T0.""RequestId"", T0.""RequestNo"", T0.""TransDate"", T0.""Comments"", T0.""WhsCode"", T1.""WhsName""
                FROM ""Tx_StockOpname"" T0 " +
            @"LEFT JOIN """+ DbProvider.dbSap_Name + @""".""OWHS"" T1 ON T0.""WhsCode"" = T1.""WhsCode""
            WHERE T0.""Status"" = 'Posted'
                AND NOT EXISTS(
                    SELECT T1.""Id""
                    FROM ""Tx_StockSummaryOpname"" T1
                    WHERE T0.""RequestId"" = T1.""RequestId"" 
                    AND T1.""Status"" NOT IN ('Cancel')
                )
        ";
                
        public static void SetBindingData(GridViewModel state, int userId, CflStockOpname_ParamModel cflParam)
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



        public static void GetData(GridViewCustomBindingGetDataArgs e, List<CflStockOpname_View__> dataList)
        {

            e.Data = dataList;

        }

        public static int GetRowCount(HANA_APP CONTEXT, int userId, CflStockOpname_ParamModel cflParam, string sqlCriteria)
        {
            var Cfl_Sql = CflStockOpname_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            if (sqlCriteria == null || string.IsNullOrWhiteSpace(sqlCriteria))
            {
                sqlCriteria = "";
            }

            if (!string.IsNullOrWhiteSpace(sqlCriteria))
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

        public static List<CflStockOpname_View__> GetDataList(HANA_APP CONTEXT, int userId, CflStockOpname_ParamModel cflBpParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflStockOpname_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            if (sqlCriteria == null || string.IsNullOrWhiteSpace(sqlCriteria))
            {
                sqlCriteria = "";
            }

            if (!string.IsNullOrWhiteSpace(sqlCriteria))
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }
            if (sqlSort == null)
            {
                sqlSort = "";
            }


            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"RequestId\" DESC ";
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            var items = CONTEXT.Database.SqlQuery<CflStockOpname_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }

        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }

        public static GridViewSettings CreateExportGridViewSettings(CflStockOpname_ParamModel cflStockOpnameParam)
        {

            GridViewSettings settings = new GridViewSettings();

            settings.Name = "List Purchase Order";

            if (cflStockOpnameParam.Header != "")
            {
                settings.Name = "List Purchase Order " + cflStockOpnameParam.Header;
            }

            settings.KeyFieldName = "Tx_StockOpname___.Id";
            settings.Columns.Add("Tx_StockOpname___.TransNo");

            return settings;
        }


       


    }


}