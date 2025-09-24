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
    public class CflSegel_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflSegel_View__
    {

        public long Id { get; set; }

        public long DetId { get; set; }

        public string SegelNo { get; set; }

        //public string BinCode { get; set; }

    }

    public class CflSegel_Model
    {


        public static string ssql = @"SELECT T0.""Id"",  T1.""DetId"" ,  T1.""SegelNo""
                                    FROM ""{DbSegel}"".""Tx_SegelIn"" T0
                                    INNER JOIN ""{DbSegel}"".""Tx_SegelIn_Number"" T1 ON T0.""Id"" = T1.""Id""
                                    WHERE T0.""Status"" = 'Posted' AND IFNULL(T1.""IsOut"", 'N') = 'N'
                                    ";
                                  
                                    //Status = 0 means in stock
        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflSegel_ParamModel cflSegelParam)
        {

            var Cfl_Sql = CflSegel_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSegel}", DbProvider.dbSegel_Name);
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

            if (cflSegelParam.SqlWhere != "")
            {
                sqlCriteria = cflSegelParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflSegel_ParamModel cflSegelParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflSegelParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflSegel_View__> GetDataList(int userId, CflSegel_ParamModel cflSegelParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {



            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }
            if (string.IsNullOrEmpty(sqlSort))
            {
                sqlSort = " ORDER BY T0.\"SegelNo\" ASC ";
            }

            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflSegelParam.SqlWhere != "")
            {
                sqlCriteria = cflSegelParam.SqlWhere + sqlCriteria;
            }

              

            var CflSegels_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflSegels_.Count == 0)
            {
                CflSegel_View__ item = new CflSegel_View__();
                CflSegels_.Add(item);
            }


            return CflSegels_;

        }

        public static List<CflSegel_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflSegel_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSegel}", DbProvider.dbSegel_Name);
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

            var items = DbProvider.dbApp.Database.SqlQuery<CflSegel_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflSegel_ParamModel cflSegelParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Segel";

            if (cflSegelParam.Header != "")
            {
                settings.Name = "List Segel " + cflSegelParam.Header;
            }

            settings.KeyFieldName = "SegelCode";
            settings.Columns.Add("SegelName");
            return settings;
        }


    }

}