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
    public class CflShip_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflShip_View__
    {

        public Int32 Id { get; set; }

        public string ShipCode { get; set; }

        public string ShipName { get; set; }

        public string ShipType { get; set; }

        public string Builder { get; set; }

        public string YearOfBuild { get; set; }

    }

    public class CflShip_Model
    {


        public static string ssql = @"SELECT T0.""Id"", T0.""ShipCode"", T0.""ShipName"",  T0.""ShipType"", T0.""Builder"", T0.""YearOfBuild""
                                    FROM  ""Tm_Ship"" T0
                                    WHERE 1 = 1
                                    ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflShip_ParamModel cflShipParam)
        {

            var Cfl_Sql = CflShip_Model.ssql;

            //Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflShipParam.SqlWhere != "")
            {
                sqlCriteria = cflShipParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflShip_ParamModel cflShipParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflShipParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflShip_View__> GetDataList(int userId, CflShip_ParamModel cflShipParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflShipParam.SqlWhere != "")
            {
                sqlCriteria = cflShipParam.SqlWhere + sqlCriteria;
            }

              

            var CflShips_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflShips_.Count == 0)
            {
                CflShip_View__ item = new CflShip_View__();
                CflShips_.Add(item);
            }


            return CflShips_;

        }

        public static List<CflShip_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflShip_Model.ssql;

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

            var items = DbProvider.dbApp.Database.SqlQuery<CflShip_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflShip_ParamModel cflShipParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Ship";

            if (cflShipParam.Header != "")
            {
                settings.Name = "List Ship " + cflShipParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("ShipName");
            return settings;
        }


    }

}