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
    public class CflRefMR_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflRefMR_View__
    {

        public Int32 Id { get; set; }

        public string RefNum { get; set; }

        public string RefType { get; set; }
        

    }

    public class CflRefMR_Model
    {


        public static string ssql = @"SELECT * FROM(
                                    SELECT ""TransNo"" AS ""RefNum"", ""TransType"" AS ""RefType""
                                    FROM ""Tx_KerusakanKapal""
                                    UNION
                                    SELECT  ""TransNo"" AS ""RefNum"", ""TransType"" AS ""RefType""
                                    FROM ""Tx_Docking""
                                    UNION
                                    SELECT  ""TransNo"" AS ""RefNum"", ""TransType"" AS ""RefType""
                                    FROM ""Tx_Overhaul"")
                                    WHERE 1 = 1
                                    ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflRefMR_ParamModel cflRefMRParam)
        {

            var Cfl_Sql = CflRefMR_Model.ssql;

            //Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflRefMRParam.SqlWhere != "")
            {
                sqlCriteria = cflRefMRParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflRefMR_ParamModel cflRefMRParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflRefMRParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflRefMR_View__> GetDataList(int userId, CflRefMR_ParamModel cflRefMRParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflRefMRParam.SqlWhere != "")
            {
                sqlCriteria = cflRefMRParam.SqlWhere + sqlCriteria;
            }

              

            var CflRefMRs_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflRefMRs_.Count == 0)
            {
                CflRefMR_View__ item = new CflRefMR_View__();
                CflRefMRs_.Add(item);
            }


            return CflRefMRs_;

        }

        public static List<CflRefMR_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflRefMR_Model.ssql;

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

            var items = DbProvider.dbApp.Database.SqlQuery<CflRefMR_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflRefMR_ParamModel cflRefMRParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List RefMR";

            if (cflRefMRParam.Header != "")
            {
                settings.Name = "List RefMR " + cflRefMRParam.Header;
            }

            settings.KeyFieldName = "RefNum";
            settings.Columns.Add("RefNum");
            settings.Columns.Add("RefType");
            return settings;
        }


    }

}