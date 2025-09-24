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
    public class CflShipROB_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflShipROB_View__
    {

        public Int32 Id { get; set; }

        public string ShipCode { get; set; }

        public string ShipName { get; set; }

        public string ShipType { get; set; }

        public string Builder { get; set; }

        public string YearOfBuild { get; set; }

    }

    public class CflShipROB_Model
    {


        public static string ssql = @"SELECT A.""Id"", A.""ShipCode"", A.""ShipName"",  A.""ShipType"", A.""Builder"", A.""YearOfBuild"",B.""ShipSection""
                                    FROM  ""Tm_Ship"" A
                                    LEFT JOIN ""Tx_ShipInventory"" B ON B.""ShipId"" = A.""Id""
                                    WHERE 
                                    A.""Id""
                                    NOT IN
                                    (
	                                    SELECT X.""Id""
	                                    FROM
	                                    (
		                                    SELECT DISTINCT A.""Id"" AS ""Id"", A.""Id""  || B.""ShipSection"" AS ""CheckData""
		                                    FROM  ""Tm_Ship"" A
		                                    LEFT JOIN ""Tx_ShipInventory"" B ON B.""ShipId"" = A.""Id""
		                                    WHERE B.""ShipSection"" IN ('Deck','Engine')
	                                    )X
	                                    GROUP BY X.""Id""
	                                    HAVING COUNT(X.""Id"") > 1
                                    )";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflShipROB_ParamModel CflShipROBParam)
        {

            var Cfl_Sql = CflShipROB_Model.ssql;

            //Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (CflShipROBParam.SqlWhere != "")
            {
                sqlCriteria = CflShipROBParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflShipROB_ParamModel CflShipROBParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, CflShipROBParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflShipROB_View__> GetDataList(int userId, CflShipROB_ParamModel CflShipROBParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (CflShipROBParam.SqlWhere != "")
            {
                sqlCriteria = CflShipROBParam.SqlWhere + sqlCriteria;
            }

              

            var CflShipROBs_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflShipROBs_.Count == 0)
            {
                CflShipROB_View__ item = new CflShipROB_View__();
                CflShipROBs_.Add(item);
            }


            return CflShipROBs_;

        }

        public static List<CflShipROB_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflShipROB_Model.ssql;

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

            var items = DbProvider.dbApp.Database.SqlQuery<CflShipROB_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflShipROB_ParamModel CflShipROBParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Ship";

            if (CflShipROBParam.Header != "")
            {
                settings.Name = "List Ship " + CflShipROBParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("ShipName");
            return settings;
        }


    }

}