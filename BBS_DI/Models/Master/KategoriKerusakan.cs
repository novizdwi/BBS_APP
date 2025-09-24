using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Transactions;
using Models._Utils;
using Models._Ef;
using MJL_DI.Models._EF;

namespace Models.Master.KategoriKerusakan
{

    #region Models


    //public static class GeneralSettingGetList
    //{
    //    public static DataTable GetItemList(string strType)
    //    {
    //        using (var CONTEXT = new HANA_APP())
    //        {
    //            return GetItemList(CONTEXT, strType);
    //        }
    //    }
    //    public static DataTable GetItemList(HANA_APP CONTEXT, string strType)
    //    {
    //        var ssql = "SELECT T0.\"ItmsGrpCod\" AS \"Code\", T0.\"ItmsGrpNam\" AS \"Name\" FROM \"" + DbProvider.dbSap_Name + "\".\"OITB\" T0";

    //        return GetDataTable(CONTEXT, ssql, strType);
    //    }
    //    public static DataTable GetDataTable(HANA_APP CONTEXT, string sql, params object[] parameters)
    //    {
    //        var s = EfIduHanaRsExtensionsApp.IduGetDataTable(CONTEXT, sql, parameters);

    //        return s;
    //    }
    //    public static DataTable GetDetailGroupItemCode(string docNum)
    //    {
    //        using (var CONTEXT = new HANA_APP())
    //        {
    //            return GetDetailGroupItemCode(CONTEXT, docNum);
    //        }
    //    }
    //    public static DataTable GetDetailGroupItemCode(HANA_APP CONTEXT, string docNum)
    //    {
    //        var ssql = "SELECT T0.\"ItmsGrpCod\" AS \"Code\", T0.\"ItmsGrpNam\" AS \"Name\" FROM \"" + DbProvider.dbSap_Name + "\".\"OITB\" T0 WHERE T0.\"ItmsGrpNam\" = '" + docNum + "'";
    //        //var ssql = "SELECT T0.\"DocNum\" AS \"Code\", T0.\"DocNum\" AS \"Name\" FROM \"" + DbProvider.dbSap_Name + "\".\"OINV\" T0 WHERE \"DocStatus\" = 'O'";

    //        return GetDataTable(CONTEXT, ssql, "");
    //    }

    //}

    public class KategoriKerusakanModel
    {
        
        public int _UserId { get; set; }

        public int Id { get; set; }

        
        
        public List<KategoriKerusakan_Model> Lists_ = new List<KategoriKerusakan_Model>();
        
        public KategoriKerusakan_s Details_ { get; set; }

    }

    public class KategoriKerusakan_Model
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long Id { get; set; }

        //[Required(ErrorMessage = "required")]
        public string Kategori { get; set; }

        //[Required(ErrorMessage = "required")]
        public string Remark { get; set; }
        

    }
    

    public class KategoriKerusakan_s
    {
        public List<long> deletedRowKeys { get; set; }
        public List<KategoriKerusakan_Model> insertedRowValues { get; set; }
        public List<KategoriKerusakan_Model> modifiedRowValues { get; set; }
    }
    


    #endregion

    #region Services

    public class KategoriKerusakanService
    {

        public void Update(KategoriKerusakanModel model)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            

                            if (model.Details_ != null)
                            {
                                if (model.Details_.insertedRowValues != null)
                                {
                                    foreach (var detail in model.Details_.insertedRowValues)
                                    {
                                        Detail_Add(CONTEXT, detail, model._UserId);
                                    }
                                }


                                if (model.Details_.modifiedRowValues != null)
                                {
                                    foreach (var detail in model.Details_.modifiedRowValues)
                                    {
                                        Detail_Update(CONTEXT, detail, model._UserId);
                                    }
                                }

                                if (model.Details_.deletedRowKeys != null)
                                {
                                    foreach (var id in model.Details_.deletedRowKeys)
                                    {
                                        KategoriKerusakan_Model detail = new KategoriKerusakan_Model();
                                        detail.Id = id;
                                        Detail_Delete(CONTEXT, detail);
                                    }
                                }
                            }
                            
                            CONTEXT_TRANS.Commit();
                        }

                        catch (Exception ex)
                        {
                            CONTEXT_TRANS.Rollback();

                            string errorMassage;
                            if (ex.Message.Substring(12) == "[VALIDATION]")
                            {
                                errorMassage = ex.Message;
                            }
                            else
                            {
                                errorMassage = string.Format("[VALIDATION] {0} ", ex.Message);
                            }

                            throw new Exception(errorMassage);
                        }
                    }
                }
            }


        }


        public KategoriKerusakanModel GetNewModel()
        {
            KategoriKerusakanModel model = new KategoriKerusakanModel();

            return model;
        }

        public KategoriKerusakanModel GetById(int id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, id);
            }
        }

        public KategoriKerusakanModel GetById(HANA_APP CONTEXT, int id = 0)
        {
            KategoriKerusakanModel model = null;

            Tm_KerusakanKapal_Kategori tm_KerusakanKapal_Kategori = CONTEXT.Database.SqlQuery<Tm_KerusakanKapal_Kategori>("SELECT TOP 1 * FROM \"Tm_KerusakanKapal_Kategori\" ").SingleOrDefault();

            if (tm_KerusakanKapal_Kategori != null)
            {

                //model.Lists_ = CONTEXT.Database.SqlQuery<KategoriKerusakan_Model>("SELECT TOP 1 * FROM \"Tm_KerusakanKapal_Kategori\" ").ToList();
                model.Lists_ = KategoriKerusakan_s(CONTEXT);
            }
            else
            {
                model = GetNewModel();
            }

            return model;
        }

        //-------------------------------------
        //Detail  GeneralSetting_CoaModel
        //-------------------------------------
        public List<KategoriKerusakan_Model> KategoriKerusakan_s(HANA_APP CONTEXT)
        {
            return CONTEXT.Database.SqlQuery<KategoriKerusakan_Model>("SELECT T0.* FROM \"Tm_KerusakanKapal_Kategori\" T0 ").ToList();
        }

        //---------------------------------------
        //GeneralSetting_CoaModel
        //--------------------------------------- 
        public long Detail_Add(HANA_APP CONTEXT, KategoriKerusakan_Model model, int UserId)
        {
            long Id = 0;

            if (model != null)
            {

                Tm_KerusakanKapal_Kategori tm_KerusakanKapal_Kategori = new Tm_KerusakanKapal_Kategori();

                CopyProperty.CopyProperties(model, tm_KerusakanKapal_Kategori, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tm_KerusakanKapal_Kategori.CreatedDate = dtModified;
                tm_KerusakanKapal_Kategori.CreatedUser = UserId;
                tm_KerusakanKapal_Kategori.ModifiedDate = dtModified;
                tm_KerusakanKapal_Kategori.ModifiedUser = UserId;

                CONTEXT.Tm_KerusakanKapal_Kategori.Add(tm_KerusakanKapal_Kategori);
                CONTEXT.SaveChanges();
                Id = tm_KerusakanKapal_Kategori.Id;

            }

            return Id;

        }

        public void Detail_Update(HANA_APP CONTEXT, KategoriKerusakan_Model model, int UserId)
        {
            if (model != null)
            {

                Tm_KerusakanKapal_Kategori tm_KerusakanKapal_Kategori = CONTEXT.Tm_KerusakanKapal_Kategori.Find(model.Id);

                if (tm_KerusakanKapal_Kategori != null)
                {
                    var exceptColumns = new string[] { "Id", "SortCode", "Code", "Name" };
                    CopyProperty.CopyProperties(model, tm_KerusakanKapal_Kategori, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    tm_KerusakanKapal_Kategori.ModifiedDate = dtModified;
                    tm_KerusakanKapal_Kategori.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }
            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, KategoriKerusakan_Model model)
        {
            if (model.Id != null)
            {
                if (model.Id != 0)
                {
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_KerusakanKapal_Kategori\"  WHERE \"Id\"=:p0", model.Id);

                    CONTEXT.SaveChanges();
                }
            }

        }

        //-------------------------------------
        //Detail  GeneralSetting_CoaModel
        //-------------------------------------
        

    }


    #endregion

}