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
    public class CflNoArInvoice_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"
    }


    public class CflNoArInvoice_View__
    {
        public string DocNum { get; set; }

        public DateTime? TanggalInvoice { get; set; }

        public decimal? TotalInvoice { get; set; }

        public string NoInvoiceRevisi { get; set; }
    }

    public class CflNoArInvoice_Model
    {
        public static string ssql = @"SELECT T0.""DocNum"",
                                            T0.""DocDate"" AS ""TanggalInvoice"",
                                            T0.""DocTotal"" AS ""TotalInvoice"",
                                            T0.""NumAtCard"" AS ""NoInvoiceRevisi""
                                        FROM ""{DbSap}"".""OINV"" T0
                                        WHERE ""DocStatus"" = 'O' AND ""U_IDU_NoTFS"" IS NULL
                                        ORDER BY T0.""DocNum"" ASC
                                    ";
        public static void SetBindingData(GridViewModel state, int userId, CflNoArInvoice_ParamModel cflParam)
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

        public static void GetData(GridViewCustomBindingGetDataArgs e, List<CflNoArInvoice_View__> dataList)
        {
            e.Data = dataList;
        }

        public static int GetRowCount(HANA_APP CONTEXT, int userId, CflNoArInvoice_ParamModel cflParam, string sqlCriteria)
        {

            var Cfl_Sql = CflNoArInvoice_Model.ssql;

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

        public static List<CflNoArInvoice_View__> GetDataList(HANA_APP CONTEXT, int userId, CflNoArInvoice_ParamModel cflNoArInvoiceParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflNoArInvoiceParam.SqlWhere != "")
            {
                sqlCriteria = cflNoArInvoiceParam.SqlWhere + sqlCriteria;
            }



            var CflNoArInvoices_ = GetDataList(CONTEXT, userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflNoArInvoices_.Count == 0)
            {
                CflNoArInvoice_View__ item = new CflNoArInvoice_View__();
                CflNoArInvoices_.Add(item);
            }


            return CflNoArInvoices_;

        }

        public static List<CflNoArInvoice_View__> GetDataList(HANA_APP CONTEXT, int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflNoArInvoice_Model.ssql;

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

            var items = CONTEXT.Database.SqlQuery<CflNoArInvoice_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflNoArInvoice_ParamModel cflNoArInvoiceParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List No Ar Invoice";

            if (cflNoArInvoiceParam.Header != "")
            {
                settings.Name = "List No Ar Invoice " + cflNoArInvoiceParam.Header;
            }

            settings.KeyFieldName = "DocNum";
            settings.Columns.Add("DocNum");
            return settings;
        }


    }


}