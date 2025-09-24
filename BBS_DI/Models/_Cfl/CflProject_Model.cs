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
    public class CflProject_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflProject_View__
    {
        public string PrjName { get; set; }

        public string PrjCode { get; set; }
        

    }

    public class CflProject_Model
    {


        public static string ssql = @"SELECT T0.""PrjName"",  T0.""PrjCode""
                                    FROM  ""{DbSap}"".""OPRJ"" T0
                                    LEFT JOIN ""{DbApp}"".""Tm_Ship"" T1 ON T0.""PrjCode"" = T1.""ShipCode""
                                    WHERE T1.""Id"" IS NULL AND T0.""Active"" = 'Y'
                                    ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflProject_ParamModel cflProjectParam)
        {

            var Cfl_Sql = CflProject_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{DbApp}", DbProvider.dbApp_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflProjectParam.SqlWhere != "")
            {
                sqlCriteria = cflProjectParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflProject_ParamModel cflProjectParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflProjectParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflProject_View__> GetDataList(int userId, CflProject_ParamModel cflProjectParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflProjectParam.SqlWhere != "")
            {
                sqlCriteria = cflProjectParam.SqlWhere + sqlCriteria;
            }

              

            var CflProjects_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflProjects_.Count == 0)
            {
                CflProject_View__ item = new CflProject_View__();
                CflProjects_.Add(item);
            }


            return CflProjects_;

        }

        public static List<CflProject_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflProject_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{DbApp}", DbProvider.dbApp_Name);
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

            var items = DbProvider.dbApp.Database.SqlQuery<CflProject_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflProject_ParamModel cflProjectParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Project";

            if (cflProjectParam.Header != "")
            {
                settings.Name = "List Project " + cflProjectParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("PrjName");
            return settings;
        }


    }

}