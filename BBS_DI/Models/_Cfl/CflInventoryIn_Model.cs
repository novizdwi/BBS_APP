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
    public class CflInventoryIn_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }

    public class CflInventoryIn_View__
    {
        public Int32 Id { get; set; }

        public DateTime? TransDate { get; set; }

        public string TransNo { get; set; }

        public string TransType { get; set; }

        public string Status { get; set; }

        public string ShippingType { get; set; }

        public int? UnitId { get; set; }

        public string UnitName { get; set; }

        public int? DriverId { get; set; }

        public string DriverName { get; set; }
        
        public string WarehouseCodeFrom { get; set; }

        public string WarehouseNameFrom { get; set; }

        public string WarehouseCodeTo { get; set; }

        public string WarehouseNameTo { get; set; }

        public Int32? BinAbsEntry { get; set; }
        
        public string BinCode { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string TransferType { get; set; }

        public decimal? QtyTransfer { get; set; }

        public decimal? QtyAfterMF { get; set; }

        public decimal? QtyReceipt { get; set; }

        public string Remarks { get; set; }

        public string DONo { get; set; }

    }

    public class CflInventoryIn_Model
    {
        public static string ssql = @"SELECT * 
                                    FROM  ""Tx_InventoryIn"" T0
                                    WHERE 1=1
                                    ORDER BY ""DONo"" DESC
                                    ";

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflInventoryIn_ParamModel cflInventoryInParam)
        {

            var Cfl_Sql = CflInventoryIn_Model.ssql;

            //Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflInventoryInParam.SqlWhere != "")
            {
                sqlCriteria = cflInventoryInParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflInventoryIn_ParamModel cflInventoryInParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflInventoryInParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflInventoryIn_View__> GetDataList(int userId, CflInventoryIn_ParamModel cflInventoryInParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflInventoryInParam.SqlWhere != "")
            {
                sqlCriteria = cflInventoryInParam.SqlWhere + sqlCriteria;
            }

              

            var CflInventoryIns_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflInventoryIns_.Count == 0)
            {
                CflInventoryIn_View__ item = new CflInventoryIn_View__();
                CflInventoryIns_.Add(item);
            }


            return CflInventoryIns_;

        }

        public static List<CflInventoryIn_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflInventoryIn_Model.ssql;

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
            //    sqlCriteria = " AND (" + sqlCriteria + ")";
            //}

            string ssql = "";
            ssql = "SELECT T0.* FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            var items = DbProvider.dbApp.Database.SqlQuery<CflInventoryIn_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflInventoryIn_ParamModel cflInventoryInParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List InventoryIn";

            if (cflInventoryInParam.Header != "")
            {
                settings.Name = "List InventoryIn " + cflInventoryInParam.Header;
            }

            settings.KeyFieldName = "Id";
            settings.Columns.Add("InventoryInName");
            return settings;
        }


    }

}