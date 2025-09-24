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
    public class CflTongkang_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflTongkang_View__
    {

        public Int32 Id { get; set; }

        public string ShipCode { get; set; }

        public string ShipName { get; set; }

        public string ShipType { get; set; }

        public string Builder { get; set; }

        public string YearOfBuild { get; set; }

    }

    public class CflTongkang_Model
    {


        public static string ssql = @"SELECT T0.""Id"", T0.""ShipCode"", T0.""ShipName"",  T0.""ShipType"", T0.""Builder"", T0.""YearOfBuild""
                                    FROM  ""Tm_Ship"" T0
                                    WHERE  T0.""ShipType"" = 'Barge'
                                    ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflTongkang_ParamModel CflTongkangParam)
        {

            var Cfl_Sql = CflTongkang_Model.ssql;

            //Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (CflTongkangParam.SqlWhere != "")
            {
                sqlCriteria = CflTongkangParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflTongkang_ParamModel CflTongkangParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, CflTongkangParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflTongkang_View__> GetDataList(int userId, CflTongkang_ParamModel CflTongkangParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (CflTongkangParam.SqlWhere != "")
            {
                sqlCriteria = CflTongkangParam.SqlWhere + sqlCriteria;
            }

              

            var CflTongkangs_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflTongkangs_.Count == 0)
            {
                CflTongkang_View__ item = new CflTongkang_View__();
                CflTongkangs_.Add(item);
            }


            return CflTongkangs_;

        }

        public static List<CflTongkang_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflTongkang_Model.ssql;

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

            var items = DbProvider.dbApp.Database.SqlQuery<CflTongkang_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflTongkang_ParamModel CflTongkangParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Ship";

            if (CflTongkangParam.Header != "")
            {
                settings.Name = "List Ship " + CflTongkangParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("ShipName");
            return settings;
        }


    }

}