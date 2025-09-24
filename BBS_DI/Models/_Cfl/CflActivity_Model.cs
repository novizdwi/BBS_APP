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
    public class CflActivity_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflActivity_View__
    {

        public string Id { get; set; }

        public string ActivityName { get; set; }

        public string IsActive { get; set; }

    }

    public class CflActivity_Model
    {


        public static string ssql = @"SELECT T0.*
                                        FROM ""Tm_Activity"" T0
                                        ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflActivity_ParamModel cflActivityParam)
        {

            var Cfl_Sql = CflActivity_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflActivityParam.SqlWhere != "")
            {
                sqlCriteria = cflActivityParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflActivity_ParamModel cflActivityParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflActivityParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflActivity_View__> GetDataList(int userId, CflActivity_ParamModel cflActivityParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflActivityParam.SqlWhere != "")
            {
                sqlCriteria = cflActivityParam.SqlWhere + sqlCriteria;
            }

              

            var CflActivitys_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflActivitys_.Count == 0)
            {
                CflActivity_View__ item = new CflActivity_View__();
                CflActivitys_.Add(item);
            }


            return CflActivitys_;

        }

        public static List<CflActivity_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflActivity_Model.ssql;

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

            var items = DbProvider.dbApp.Database.SqlQuery<CflActivity_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflActivity_ParamModel cflActivityParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Activity";

            if (cflActivityParam.Header != "")
            {
                settings.Name = "List Activity " + cflActivityParam.Header;
            }

            settings.KeyFieldName = "ActivityCode";
            settings.Columns.Add("ActivityName");
            return settings;
        }


    }

}