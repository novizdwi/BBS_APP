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
    public class CflTransferSummaryOut_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"
    }

    public class CflTransferSummaryOut_View__
    {
        public string Id { get; set; }
        public string TransNo { get; set; }

        public string TransDate { get; set; }

        public string FromWhsCode { get; set; }
        public string FromWhsName { get; set; }

        public string ToWhsCode { get; set; }
        public string ToWhsName { get; set; }

        public string TransitWhsCode { get; set; }
        public string TransitWhsName { get; set; }

        public string Comments { get; set; }
    }

    public class CflTransferSummaryOut_Model
    {
        public static string ssql = @"
            SELECT DISTINCT T0.""Id"", T0.""TransNo"", 
                T0.""TransDate"", T0.""FromWhsCode"", T0.""FromWhsName"",
                T0.""TransitWhsCode"", T0.""TransitWhsName"",
                T0.""ToWhsCode"", T0.""ToWhsName"", T0.""Comments""
                FROM ""Tx_TransferSummaryOut"" T0
                WHERE T0.""Status"" = 'Posted' AND IFNULL(T0.""DocEntry"",0) <> 0              
        ";
                
        public static void SetBindingData(GridViewModel state, int userId, CflTransferSummaryOut_ParamModel cflParam)
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



        public static void GetData(GridViewCustomBindingGetDataArgs e, List<CflTransferSummaryOut_View__> dataList)
        {

            e.Data = dataList;

        }

        public static int GetRowCount(HANA_APP CONTEXT, int userId, CflTransferSummaryOut_ParamModel cflParam, string sqlCriteria)
        {
            var Cfl_Sql = CflTransferSummaryOut_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
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

        public static List<CflTransferSummaryOut_View__> GetDataList(HANA_APP CONTEXT, int userId, CflTransferSummaryOut_ParamModel cflParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflTransferSummaryOut_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
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

            if (sqlSort == null)
            {
                sqlSort = "";
            }


            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            var items = CONTEXT.Database.SqlQuery<CflTransferSummaryOut_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }

        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }

        public static GridViewSettings CreateExportGridViewSettings(CflTransferSummaryOut_ParamModel cflTransferSummaryOutParam)
        {

            GridViewSettings settings = new GridViewSettings();

            settings.Name = "List Transfer Request";

            if (cflTransferSummaryOutParam.Header != "")
            {
                settings.Name = "List Transfer Request " + cflTransferSummaryOutParam.Header;
            }

            settings.KeyFieldName = "Tx_TransferSummaryOut___.Id";
            settings.Columns.Add("Tx_TransferSummaryOut___.TransNo");

            return settings;
        }


       


    }


}