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
    public class CflNoTtd_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"
    }


    public class CflNoTtd_View__
    {
        public string NoDokumen { get; set; }
        public DateTime TglDokumen { get; set; }
        public string MetodeKirim { get; set; }
        public string Pengiriman { get; set; }
        public string NamaCustomer { get; set; }
    }

    public class CflNoTtd_Model
    {

        public static string ssql = @"SELECT ""NoDokumen"", ""TglDokumen"", ""MetodeKirim"", ""Pengiriman"", ""NamaCustomer"" FROM ""Tx_TukarFakturSend"" WHERE ""Status"" = 'Posted'";



        public static void SetBindingData(GridViewModel state, int userId, CflNoTtd_ParamModel cflParam)
        {
            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(state);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(state);

            using (var CONTEXT = new HANA_APP())
            {
                var dataRowCount = GetRowCount(CONTEXT, userId, cflParam, sqlCriteria);
                var dataList = GetDataList(CONTEXT, userId, cflParam, sqlCriteria, sqlSort, state.Pager.PageIndex, state.Pager.PageSize);

                state.ProcessCustomBinding(
                  new GridViewCustomBindingGetDataRowCountHandler(args =>
                  {
                      GetDataRowCount(args, dataRowCount);
                  }),
                  new GridViewCustomBindingGetDataHandler(args =>
                  {
                      GetData(args, dataList);
                  })
              );
            }
        }

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int dataRowCount)
        {
            e.DataRowCount = dataRowCount;
        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, List<CflNoTtd_View__> dataList)
        {
            e.Data = dataList;
        }

        public static int GetRowCount(HANA_APP CONTEXT, int userId, CflNoTtd_ParamModel cflParam, string sqlCriteria)
        {

            var Cfl_Sql = CflNoTtd_Model.ssql;

            Cfl_Sql = Cfl_Sql.Replace("{DbSap}", DbProvider.dbSap_Name);
            Cfl_Sql = Cfl_Sql.Replace("{UserId}", userId.ToString());


            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }


            if (sqlCriteria != "")
            {
                sqlCriteria = " AND (" + sqlCriteria + ")";
            }

            if (cflParam.SqlWhere != "")
            {
                sqlCriteria = cflParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            return dataRowCount;
        }

        public static List<CflNoTtd_View__> GetDataList(HANA_APP CONTEXT, int userId, CflNoTtd_ParamModel cflNoTtdParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflNoTtdParam.SqlWhere != "")
            {
                sqlCriteria = cflNoTtdParam.SqlWhere + sqlCriteria;
            }



            var CflNoTtds_ = GetDataList(CONTEXT, userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflNoTtds_.Count == 0)
            {
                CflNoTtd_View__ item = new CflNoTtd_View__();
                CflNoTtds_.Add(item);
            }


            return CflNoTtds_;

        }

        public static List<CflNoTtd_View__> GetDataList(HANA_APP CONTEXT, int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflNoTtd_Model.ssql;

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

            var items = CONTEXT.Database.SqlQuery<CflNoTtd_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflNoTtd_ParamModel cflNoTtdParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List No TTD";

            if (cflNoTtdParam.Header != "")
            {
                settings.Name = "List No TTD " + cflNoTtdParam.Header;
            }

            settings.KeyFieldName = "NoDokumen";
            settings.Columns.Add("NoDokumen");
            return settings;
        }


    }


}