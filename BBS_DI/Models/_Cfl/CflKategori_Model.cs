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
    public class CflKategori_ParamModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string SqlWhere { get; set; }
        public string IsMulti { get; set; }//"Y","N"

    }


    public class CflKategori_View__
    {

        public string KategoriId { get; set; }

        public string Kategori { get; set; }
        
        public string Remark { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }

    public class CflKategori_Model
    {


        public static string ssql = @"SELECT T0.""Id"" AS ""KategoriId"",  T0.""Kategori"" ,  T0.""Remark"" , T0.""CreatedDate"" , T0.""ModifiedDate""
                                    FROM  ""Tm_KerusakanKapal_Kategori"" T0 ORDER BY T0.""CreatedDate"" DESC
                                    ";


        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e, int userId, CflKategori_ParamModel cflKategoriParam)
        {

            var Cfl_Sql = CflKategori_Model.ssql;

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

            if (cflKategoriParam.SqlWhere != "")
            {
                sqlCriteria = cflKategoriParam.SqlWhere + sqlCriteria;
            }

            int dataRowCount;
            string ssql = "";
            ssql = "SELECT COUNT(*) AS IDU FROM (" + Cfl_Sql + ") T0  WHERE 1=1 " + sqlCriteria;
            dataRowCount = DbProvider.dbApp.Database.SqlQuery<int>(ssql).FirstOrDefault<int>();

            e.DataRowCount = dataRowCount;

        }

        public static void GetData(GridViewCustomBindingGetDataArgs e, int userId, CflKategori_ParamModel cflKategoriParam)
        {

            string sqlCriteria = GetSqlFromGridViewModelState.getHanaCriteria(e.State);

            string sqlSort = GetSqlFromGridViewModelState.getHanaSort(e.State);

            e.Data = GetDataList(userId, cflKategoriParam, sqlCriteria, sqlSort, e.State.Pager.PageIndex, e.State.Pager.PageSize);

        }

        public static List<CflKategori_View__> GetDataList(int userId, CflKategori_ParamModel cflKategoriParam, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
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

            if (cflKategoriParam.SqlWhere != "")
            {
                sqlCriteria = cflKategoriParam.SqlWhere + sqlCriteria;
            }

              

            var CflKategoris_ = GetDataList(userId, sqlCriteria, sqlSort, PageIndex, PageSize);

            if (CflKategoris_.Count == 0)
            {
                CflKategori_View__ Kategori = new CflKategori_View__();
                CflKategoris_.Add(Kategori);
            }


            return CflKategoris_;

        }

        public static List<CflKategori_View__> GetDataList(int userId, string sqlCriteria, string sqlSort, int PageIndex, int PageSize)
        {

            var Cfl_Sql = CflKategori_Model.ssql;

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

            var Kategoris = DbProvider.dbApp.Database.SqlQuery<CflKategori_View__>(ssql + sqlSort + ssqlLimit).ToList();
            
            return Kategoris;

        }


        public static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();

            return viewModel;
        }


        public static GridViewSettings CreateExportGridViewSettings(CflKategori_ParamModel cflKategoriParam)
        {

            GridViewSettings settings = new GridViewSettings();
            settings.Name = "List Kategori";

            if (cflKategoriParam.Header != "")
            {
                settings.Name = "List Kategori " + cflKategoriParam.Header;
            }

            settings.KeyFieldName = "KategoriId";
            settings.Columns.Add("Kategori");
            return settings;
        }


    }

}