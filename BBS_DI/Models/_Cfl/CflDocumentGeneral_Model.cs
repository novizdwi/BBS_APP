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
    public class CflDocumentGeneral_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflDocumentGeneral_View__
    {

        public Int32 Id { get; set; }

        public string DocumentName { get; set; }

        public Int32? Type { get; set; }

        public string TypeName { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public DateTime? WarningDate { get; set; }

    }

    public class CflDocumentGeneral_Model
    {


        public static string ssql = @"SELECT T0.""Id"",  T0.""DocumentName"",  T0.""Type"", T0.""ExpiredDate"", T0.""WarningDate"",  T2.""DocumentName"" AS ""TypeName""
                                    FROM  ""Tm_DocGeneral"" T0
                                    LEFT JOIN ""Tm_DocContent"" T2 ON T2.""Type"" = 'General' AND T0.""Type"" = T2.""Id""
                                    WHERE T0.""IsActive"" = 'Y' 
                                    ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflDocumentGeneral_ParamModel cflDocumentGeneralParam)
        {

            var Cfl_Sql = CflDocumentGeneral_Model.ssql;

            //Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflDocumentGeneralParam.SqlWhere != "")
            {
                sqlCriteria = cflDocumentGeneralParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflDocumentGeneral_ParamModel cflDocumentGeneralParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflDocumentGeneralParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflDocumentGeneral_View__> GetDataList(int userId, CflDocumentGeneral_ParamModel cflDocumentGeneralParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflDocumentGeneralParam.SqlWhere != "")
            {
                sqlCriteria = cflDocumentGeneralParam.SqlWhere + sqlCriteria;
            }

              

            var CflDocumentGenerals_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflDocumentGenerals_.Count == 0)
            {
                CflDocumentGeneral_View__ item = new CflDocumentGeneral_View__();
                CflDocumentGenerals_.Add(item);
            }


            return CflDocumentGenerals_;

        }

        public static List<CflDocumentGeneral_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflDocumentGeneral_Model.ssql;

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

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            var items = DbProvider.dbApp.Database.SqlQuery<CflDocumentGeneral_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflDocumentGeneral_ParamModel cflDocumentGeneralParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List DocumentGeneral";

            if (cflDocumentGeneralParam.Header != "")
            {
                settings.Name = "List DocumentGeneral " + cflDocumentGeneralParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("DocumentGeneralName");
            return settings;
        }


    }

}