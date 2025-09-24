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
    public class CflInventorySend_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }

    public class CflInventorySend_View__
    {
        public Int32 Id { get; set; }

        public DateTime? TransDate { get; set; }

        public string TransNo { get; set; }

        public string TransType { get; set; }

        public string Status { get; set; }

        public string FromWarehouseCode { get; set; }

        public string FromWarehouseName { get; set; }

        public string ToWarehouseCode { get; set; }

        public string ToWarehouseName { get; set; }

    }

    public class CflInventorySend_Model
    {
        public static string ssql = @"SELECT * 
                                    FROM  ""Tx_InventorySend"" T0
                                    WHERE ""Status"" = 'Posted' AND ""Received"" != 'Yes' ORDER BY ""TransDate"" DESC
                                    ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflInventorySend_ParamModel cflInventorySendParam)
        {

            var Cfl_Sql = CflInventorySend_Model.ssql;

            //Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflInventorySendParam.SqlWhere != "")
            {
                sqlCriteria = cflInventorySendParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflInventorySend_ParamModel cflInventorySendParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflInventorySendParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflInventorySend_View__> GetDataList(int userId, CflInventorySend_ParamModel cflInventorySendParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflInventorySendParam.SqlWhere != "")
            {
                sqlCriteria = cflInventorySendParam.SqlWhere + sqlCriteria;
            }



            var CflInventorySends_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflInventorySends_.Count == 0)
            {
                CflInventorySend_View__ item = new CflInventorySend_View__();
                CflInventorySends_.Add(item);
            }


            return CflInventorySends_;

        }

        public static List<CflInventorySend_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflInventorySend_Model.ssql;

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

            var items = DbProvider.dbApp.Database.SqlQuery<CflInventorySend_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflInventorySend_ParamModel cflInventorySendParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List InventorySend";

            if (cflInventorySendParam.Header != "")
            {
                settings.Name = "List InventorySend " + cflInventorySendParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("InventorySendName");
            return settings;
        }


    }

}