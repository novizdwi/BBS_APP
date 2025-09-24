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
using BBS_DI.Models._EF;
using Models._Ef;

using System.Data.Entity;
using System.Threading.Tasks;


namespace Models.Master.DocGeneral
{

    public class View___
    {
        public long Id { get; set; }

        public string DocumentName { get; set; }

        public string Type { get; set; }

        public string TypeName { get; set; }

        public string Remarks { get; set; }

        public string IsActive { get; set; }

    }

    public class DocGeneral__List_Model
    {
        static string ViewSql = "SELECT T0.*, T1.\"DocumentName\" \"TypeName\" FROM \"Tm_DocGeneral\" T0 " +
                                 "LEFT JOIN \"Tm_DocContent\" T1 ON T0.\"Type\" = T1.\"Id\" ";

        public static void SetBindingData(GridViewModel state, int userId)
        {
            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(state);
            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(state);

            using (var CONTEXT = new HANA_APP())
            {
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DocGeneral");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    if (string.IsNullOrEmpty(sqlCriteria))
                    {
                        sqlCriteria = formAuthorizeSqlWhere;
                    }
                    else
                    {
                        sqlCriteria = sqlCriteria + " AND " + formAuthorizeSqlWhere;
                    }

                }

                var dataRowCount = GetRowCount(CONTEXT, userId, sqlCriteria);
                var dataList = GetDataList(CONTEXT, userId, sqlCriteria, sqlSort, state.Pager.PageIndex, state.Pager.PageSize);

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

        public static void GetData(GridViewCustomBindingGetDataArgs e, List<View___> dataList)
        {
            e.Data = dataList;
        }

        public static int GetRowCount(HANA_APP CONTEXT, int userId, string sqlCriteria)
        {

            if (sqlCriteria == null)
            {
                sqlCriteria = "";
            }
            if (sqlCriteria != "")
            {
                sqlCriteria = " AND ( " + sqlCriteria + " )";
            }



            int dataRowCount = 0;
            string ssql = " ";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + ViewSql + ") T0 WHERE 1=1 " + sqlCriteria;

            ssql = string.Format(ssql, DbProvider.dbSap_Name);

            dataRowCount = CONTEXT.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            return dataRowCount;
        }


        public static List<View___> GetDataList(HANA_APP CONTEXT, int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {
            //var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(userId, "DocGeneral", "Tm_DocGeneral___");


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


            if (sqlSort == "")
            {
                sqlSort = " ORDER BY \"DocumentName\" ASC ";
            }

            var views = new List<View___>();

            string ssqlSap = "";
            ssqlSap = "SELECT T0.* FROM (" + ViewSql + ") T0 WHERE 1=1 " + sqlCriteria;
            string ssql = string.Format(ssqlSap, DbProvider.dbSap_Name);
            string ssqlLimit = string.Format(" LIMIT {0} OFFSET {1} ", PageSize, (PageIndex) * PageSize);

            views = CONTEXT.Database.SqlQuery<View___>(ssql + sqlSort + ssqlLimit).ToList();

            if (views.Count == 0)
            {
                View___ view = new View___();
                views.Add(view);
            }

            return views;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();


            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings()
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List DocGeneral";

            settings.KeyFieldName = "Id";
            settings.Columns.Add("Id").Visible = false;
            settings.Columns.Add("DocGeneralNo", "DocGeneral No");
            

            return settings;
        }


    }


}