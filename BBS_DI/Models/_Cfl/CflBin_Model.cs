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
    public class CflBin_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"
    }


    public class CflBin_View__
    {
        public int BinAbsEntry { get; set; }
        public string BinCode { get; set; }
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public string Descr { get; set; }
    }

    public class CflBin_Model
    {
        //Nanti Acc Gak boleh tampil di operasional
        public static string ssql = @"SELECT 
                                            T0.""AbsEntry"" AS ""BinAbsEntry"",
                                            T0.""BinCode"",
                                            T1.""WhsCode"",
                                            T1.""WhsName"",
                                            T0.""Descr"" 
                                        FROM ""{DbSap}"".""OBIN"" T0
                                        LEFT JOIN ""{DbSap}"".""OWHS"" T1 ON T0.""WhsCode"" = T1.""WhsCode""    
                                        WHERE T0.""SysBin"" = 'N'
                                        AND T0.""Disabled"" = 'N'
                                        ORDER BY T0.""WhsCode"" ASC
                                    ";



        public static void SetBindingData(GridViewModel state, int userId, CflBin_ParamModel cflParam)
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

        public static void GetData(GridViewCustomBindingGetDataArgs e, List<CflBin_View__> dataList)
        {
            e.Data = dataList;
        }

        public static int GetRowCount(HANA_APP CONTEXT, int userId, CflBin_ParamModel cflParam, string sqlCriteria)
        {

            var Cfl_Sql = CflBin_Model.ssql;

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

        public static List<CflBin_View__> GetDataList(HANA_APP CONTEXT, int userId, CflBin_ParamModel cflBinParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflBinParam.SqlWhere != "")
            {
                sqlCriteria = cflBinParam.SqlWhere + sqlCriteria;
            }



            var CflBins_ = GetDataList(CONTEXT, userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflBins_.Count == 0)
            {
                CflBin_View__ item = new CflBin_View__();
                CflBins_.Add(item);
            }


            return CflBins_;

        }

        public static List<CflBin_View__> GetDataList(HANA_APP CONTEXT, int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflBin_Model.ssql;

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

            var items = CONTEXT.Database.SqlQuery<CflBin_View__>(ssql + sqlSort + ssqlLimit).ToList();

            return items;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflBin_ParamModel cflBinParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Bin";

            if (cflBinParam.Header != "")
            {
                settings.Name = "List Bin " + cflBinParam.Header;
            }

            settings.KeyFieldName = "CardCode";
            settings.Columns.Add("CardCode");
            settings.Columns.Add("CardName");
            settings.Columns.Add("Address");
            settings.Columns.Add("Phone1");
            settings.Columns.Add("Phone2");
            settings.Columns.Add("Cellular");
            return settings;
        }


    }


}