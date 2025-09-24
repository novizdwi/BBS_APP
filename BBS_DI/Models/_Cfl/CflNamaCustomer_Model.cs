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
    public class CflNamaCustomer_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"
    }


    public class CflNamaCustomer_View__
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string City { get; set; }
    }

    public class CflNamaCustomer_Model
    {

        public static string ssql = @"SELECT T0.""CardCode"",
                                            T0.""CardName"",
                                            T1.""City""
                                        FROM ""{DbSap}"".""OCRD"" T0
                                        LEFT OUTER JOIN ""{DbSap}"".""CRD1"" T1 ON T0.""CardCode"" = T1.""CardCode""
                                        ORDER BY T0.""CardName"" ASC
                                    ";
        //WHERE T1.""AdresType"" = 'B'



        public static void SetBindingData(GridViewModel state, int userId, CflNamaCustomer_ParamModel cflParam)
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

        public static void GetData(GridViewCustomBindingGetDataArgs e, List<CflNamaCustomer_View__> dataList)
        {
            e.Data = dataList;
        }

        public static int GetRowCount(HANA_APP CONTEXT, int userId, CflNamaCustomer_ParamModel cflParam, string sqlCriteria)
        {

            var Cfl_Sql = CflNamaCustomer_Model.ssql;

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

        public static List<CflNamaCustomer_View__> GetDataList(HANA_APP CONTEXT, int userId, CflNamaCustomer_ParamModel cflNamaCustomerParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflNamaCustomerParam.SqlWhere != "")
            {
                sqlCriteria = cflNamaCustomerParam.SqlWhere + sqlCriteria;
            }



            var CflNamaCustomers_ = GetDataList(CONTEXT, userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflNamaCustomers_.Count == 0)
            {
                CflNamaCustomer_View__ item = new CflNamaCustomer_View__();
                CflNamaCustomers_.Add(item);
            }


            return CflNamaCustomers_;

        }

        public static List<CflNamaCustomer_View__> GetDataList(HANA_APP CONTEXT, int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflNamaCustomer_Model.ssql;

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

            var items = CONTEXT.Database.SqlQuery<CflNamaCustomer_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflNamaCustomer_ParamModel cflNamaCustomerParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Nama Customer";

            if (cflNamaCustomerParam.Header != "")
            {
                settings.Name = "List Nama Customer " + cflNamaCustomerParam.Header;
            }

            settings.KeyFieldName = "CardCode";
            settings.Columns.Add("CardName");
            return settings;
        }


    }


}