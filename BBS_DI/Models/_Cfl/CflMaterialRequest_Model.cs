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
    public class CflMaterialRequest_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflMaterialRequest_View__
    {

        public Int32 Id { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public int? ShipId { get; set; }

        public string ShipName { get; set; }

        public string ShipSection { get; set; }

        public string WarehouseCode { get; set; }

        public string WarehouseName { get; set; }

        public int? RequesterId { get; set; }

        public string RequesterUserName { get; set; }

        public string Status { get; set; }

        public string ReqType { get; set; }

    }

    public class CflMaterialRequest_Model
    {


        public static string ssql = @"SELECT T0.""Id"",
                                    T0.""TransNo"", 
                                    T0.""TransDate"", 
                                    T0.""ShipId"",
                                    T0.""ShipName"", 
                                    T0.""ShipSection"", 
                                    T0.""WarehouseCode"",
                                    T0.""WarehouseName"",
                                    T2.""Id"" AS ""RequesterId"",
                                    T2.""UserName"" AS ""RequesterUserName"",
                                    T0.""Status"",
                                    T1.""Name"" AS ""ReqType""
                                    FROM  ""Tx_MaterialRequest"" T0
                                    LEFT JOIN ""Ts_List"" T1 ON T0.""ReqType"" = T1.""Code"" AND T1.""Type"" = 'MaterialRequestReqType'
                                    LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                                    WHERE ""Status"" = 'Open' ORDER BY T0.""TransDate"" DESC
                                    ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflMaterialRequest_ParamModel cflMaterialRequestParam)
        {

            var Cfl_Sql = CflMaterialRequest_Model.ssql;

            //Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflMaterialRequestParam.SqlWhere != "")
            {
                sqlCriteria = cflMaterialRequestParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflMaterialRequest_ParamModel cflMaterialRequestParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflMaterialRequestParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflMaterialRequest_View__> GetDataList(int userId, CflMaterialRequest_ParamModel cflMaterialRequestParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflMaterialRequestParam.SqlWhere != "")
            {
                sqlCriteria = cflMaterialRequestParam.SqlWhere + sqlCriteria;
            }

              

            var CflMaterialRequests_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflMaterialRequests_.Count == 0)
            {
                CflMaterialRequest_View__ item = new CflMaterialRequest_View__();
                CflMaterialRequests_.Add(item);
            }


            return CflMaterialRequests_;

        }

        public static List<CflMaterialRequest_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflMaterialRequest_Model.ssql;

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
            //    sqlCriteria = sqlCriteria;
            //}

            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            var items = DbProvider.dbApp.Database.SqlQuery<CflMaterialRequest_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflMaterialRequest_ParamModel cflMaterialRequestParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List MaterialRequest";

            if (cflMaterialRequestParam.Header != "")
            {
                settings.Name = "List MaterialRequest " + cflMaterialRequestParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("TransDate", "Request Date", MVCxGridViewColumnType.DateEdit).PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            settings.Columns.Add("TransNo");
            return settings;
        }


    }

}