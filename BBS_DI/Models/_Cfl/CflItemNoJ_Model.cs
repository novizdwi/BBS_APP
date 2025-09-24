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
    public class CflItemNoJ_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflItemNoJ_View__
    {

        public string ItemCode { get; set; }

        public string ItemName { get; set; }
        
        public string IUoMEntry { get; set; }

        public string UomCode { get; set; }

        public Decimal? OnHand { get; set; }

        public string ManBtchNum { get; set; }
    }

    public class CflItemNoJ_Model
    {


        public static string ssql = @"SELECT T0.""ItemCode"",  T0.""ItemName"" ,  T0.""ItemType"" , T0.""IUoMEntry"" , T2.""UomCode"", T1.""ItmsGrpNam"", T0.""ManBtchNum"", T0.""OnHand""
                                    FROM  ""{DbSap}"".""OITM"" T0
                                    LEFT OUTER JOIN ""{DbSap}"".""OITB"" T1 ON T0.""ItmsGrpCod"" = T1.""ItmsGrpCod""
                                    LEFT JOIN  ""{DbSap}"".""OUOM"" T2 ON T0.""IUoMEntry"" = T2.""UomEntry""
                                    WHERE LEFT(T0.""ItemCode"",1) !='J' AND T0.""frozenFor"" = 'N' ORDER BY T0.""ItemCode"" ASC
                                    ";


        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflItemNoJ_ParamModel cflItemNoJParam)
        {

            var Cfl_Sql = CflItemNoJ_Model.ssql;

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

            if (cflItemNoJParam.SqlWhere != "")
            {
                sqlCriteria = cflItemNoJParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = @"SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflItemNoJ_ParamModel CflItemNoJParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, CflItemNoJParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflItemNoJ_View__> GetDataList(int userId, CflItemNoJ_ParamModel CflItemNoJParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (CflItemNoJParam.SqlWhere != "")
            {
                sqlCriteria = CflItemNoJParam.SqlWhere + sqlCriteria;
            }

              

            var CflItemNoJs_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflItemNoJs_.Count == 0)
            {
                CflItemNoJ_View__ item = new CflItemNoJ_View__();
                CflItemNoJs_.Add(item);
            }


            return CflItemNoJs_;

        }

        public static List<CflItemNoJ_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflItemNoJ_Model.ssql;

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

            var items = DbProvider.dbApp.Database.SqlQuery<CflItemNoJ_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflItemNoJ_ParamModel CflItemNoJParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Item";

            if (CflItemNoJParam.Header != "")
            {
                settings.Name = "List Item " + CflItemNoJParam.Header;
            }

            settings.KeyFieldName = "ItemCode";
            settings.Columns.Add("ItemName");
            return settings;
        }


    }

}