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
using BBS_DI.Models._EF;

namespace Models.Setting.GeneralSetting
{

    #region Models


    public static class GeneralSettingGetList
    {
        public static DataTable GetItemList(string strType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetItemList(CONTEXT, strType);
            }
        }
        public static DataTable GetItemList(HANA_APP CONTEXT, string strType)
        {
            var ssql = "SELECT T0.\"ItmsGrpCod\" AS \"Code\", T0.\"ItmsGrpNam\" AS \"Name\" FROM \"" + DbProvider.dbSap_Name + "\".\"OITB\" T0";

            return GetDataTable(CONTEXT, ssql, strType);
        }
        public static DataTable GetDataTable(HANA_APP CONTEXT, string sql, params object[] parameters)
        {
            var s = EfIduHanaRsExtensionsApp.IduGetDataTable(CONTEXT, sql, parameters);

            return s;
        }
        public static DataTable GetDetailGroupItemCode(string docNum)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDetailGroupItemCode(CONTEXT, docNum);
            }
        }
        public static DataTable GetDetailGroupItemCode(HANA_APP CONTEXT, string docNum)
        {
            var ssql = "SELECT T0.\"ItmsGrpCod\" AS \"Code\", T0.\"ItmsGrpNam\" AS \"Name\" FROM \"" + DbProvider.dbSap_Name + "\".\"OITB\" T0 WHERE T0.\"ItmsGrpNam\" = '" + docNum + "'";
            //var ssql = "SELECT T0.\"DocNum\" AS \"Code\", T0.\"DocNum\" AS \"Name\" FROM \"" + DbProvider.dbSap_Name + "\".\"OINV\" T0 WHERE \"DocStatus\" = 'O'";

            return GetDataTable(CONTEXT, ssql, "");
        }

    }

    public class GeneralSettingModel
    {
        
        public int _UserId { get; set; }

        public int Id { get; set; }

        public string SegelItemCode { get; set; }

        public string SegelItemName { get; set; }

        //public string BiayaKirimItemCode { get; set; }

        //public string BiayaKirimItemName { get; set; }

        //public string SadpIICardCode { get; set; }

        //public string SadpIICardName { get; set; }

        public string SolarItemCode { get; set; }

        public string SolarItemName { get; set; }

        public string WarehouseTransitCode { get; set; }

        public string WarehouseTransitName { get; set; }

        //public string JasaAngkutCodeDarat { get; set; }

        //public string JasaAngkutNameDarat { get; set; }

        //public string JasaAngkutCodeLaut { get; set; }

        //public string JasaAngkutNameLaut { get; set; }
        
        public List<GeneralSetting_CoaModel> ListCoas_ = new List<GeneralSetting_CoaModel>();

        public List<GeneralSetting_ItemModel> ListItems_ = new List<GeneralSetting_ItemModel>();
        public GeneralSetting_Coas DetailCoas_ { get; set; }
        public GeneralSetting_Items DetailItems_ { get; set; }

    }

    public class GeneralSetting_CoaModel
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
        public string SortCode { get; set; }

        //[Required(ErrorMessage = "required")]
        public string Code { get; set; }

        //[Required(ErrorMessage = "required")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CoaCode { get; set; }

        //[Required(ErrorMessage = "required")]
        public string CoaName { get; set; }

    }
    public class GeneralSetting_ItemModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int? Id { get; set; }

        public int? DetId { get; set; }

        public string ItemGroupCode { get; set; }
        
        public string ItemGroupName { get; set; }
    }

    public class GeneralSetting_Coas
    {
        public List<long> deletedRowKeys { get; set; }
        public List<GeneralSetting_CoaModel> insertedRowValues { get; set; }
        public List<GeneralSetting_CoaModel> modifiedRowValues { get; set; }
    }
    public class GeneralSetting_Items
    {
        public List<int> deletedRowKeys { get; set; }
        public List<GeneralSetting_ItemModel> insertedRowValues { get; set; }
        public List<GeneralSetting_ItemModel> modifiedRowValues { get; set; }
    }


    #endregion

    #region Services

    public class GeneralSettingService
    {

        public void Update(GeneralSettingModel model)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {

                            Tm_GeneralSetting tm_GeneralSetting = CONTEXT.Database.SqlQuery<Tm_GeneralSetting>("SELECT TOP 1 * FROM \"Tm_GeneralSetting\" ").SingleOrDefault();
                            if (tm_GeneralSetting != null)
                            {
                                tm_GeneralSetting = CONTEXT.Tm_GeneralSetting.Find(tm_GeneralSetting.Id);

                                String keyValue;
                                keyValue = model.Id.ToString();

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", "Tm_GeneralSetting", "update", "Id", keyValue);

                                var exceptColumns = new string[] { "Id" };
                                CopyProperty.CopyProperties(model, tm_GeneralSetting, false, exceptColumns);

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                                tm_GeneralSetting.ModifiedDate = dtModified;
                                tm_GeneralSetting.ModifiedUser = model._UserId;
                                CONTEXT.SaveChanges();

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_GeneralSetting", "update", "Id", keyValue);

                            }
                            else
                            {
                                tm_GeneralSetting = new Tm_GeneralSetting();
                                CopyProperty.CopyProperties(model, tm_GeneralSetting, false);

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                tm_GeneralSetting.TransType = "GeneralSetting";
                                tm_GeneralSetting.CreatedDate = dtModified;
                                tm_GeneralSetting.CreatedUser = model._UserId;
                                tm_GeneralSetting.ModifiedDate = dtModified;
                                tm_GeneralSetting.ModifiedUser = model._UserId;

                                CONTEXT.Tm_GeneralSetting.Add(tm_GeneralSetting);
                                CONTEXT.SaveChanges();

                                String keyValue;
                                keyValue = tm_GeneralSetting.Id.ToString();
                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_GeneralSetting", "add", "Id", keyValue);
                            }

                            if (model.DetailCoas_ != null)
                            {
                                if (model.DetailCoas_.insertedRowValues != null)
                                {
                                    foreach (var coa in model.DetailCoas_.insertedRowValues)
                                    {
                                        Detail_Add(CONTEXT,coa, model._UserId);
                                    }
                                }


                                if (model.DetailCoas_.modifiedRowValues != null)
                                {
                                    foreach (var coa in model.DetailCoas_.modifiedRowValues)
                                    {
                                        Detail_Update(CONTEXT,coa, model._UserId);
                                    }
                                }

                                if (model.DetailCoas_.deletedRowKeys != null)
                                {
                                    foreach (var id in model.DetailCoas_.deletedRowKeys)
                                    {
                                        GeneralSetting_CoaModel coa = new GeneralSetting_CoaModel();
                                        coa.Id = id;
                                        Detail_Delete(CONTEXT,coa);
                                    }
                                }
                            }

                            if (model.DetailItems_ != null)
                            {
                                if (model.DetailItems_.insertedRowValues != null)
                                {
                                    foreach (var item in model.DetailItems_.insertedRowValues)
                                    {
                                        DetailItem_Add(CONTEXT, item, model.Id, model._UserId);
                                        //DetailItem_Add(CONTEXT, item, model._UserId);
                                    }
                                }


                                if (model.DetailItems_.modifiedRowValues != null)
                                {
                                    foreach (var item in model.DetailItems_.modifiedRowValues)
                                    {
                                        DetailItem_Update(CONTEXT, item, model._UserId);
                                    }
                                }

                                if (model.DetailItems_.deletedRowKeys != null)
                                {
                                    foreach (var id in model.DetailItems_.deletedRowKeys)
                                    {
                                        GeneralSetting_ItemModel item = new GeneralSetting_ItemModel();
                                        item.Id = Convert.ToInt32(id);
                                        DetailItem_Delete(CONTEXT, item);
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


        public GeneralSettingModel GetNewModel()
        {
            GeneralSettingModel model = new GeneralSettingModel();

            return model;
        }

        public GeneralSettingModel GetById(int id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, id);
            }
        }

        public GeneralSettingModel GetById(HANA_APP CONTEXT, int id = 0)
        {
            GeneralSettingModel model = null;

            Tm_GeneralSetting tm_GeneralSetting = CONTEXT.Database.SqlQuery<Tm_GeneralSetting>("SELECT TOP 1 * FROM \"Tm_GeneralSetting\" ").SingleOrDefault();

            if (tm_GeneralSetting != null)
            {
                model = new GeneralSettingModel();
                CopyProperty.CopyProperties(tm_GeneralSetting, model, false);

                model.ListCoas_ = this.GeneralSetting_Coas(CONTEXT);
                model.ListItems_ = this.GeneralSetting_Items(CONTEXT, id);
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
        public List<GeneralSetting_CoaModel> GeneralSetting_Coas(HANA_APP CONTEXT)
        {
            return CONTEXT.Database.SqlQuery<GeneralSetting_CoaModel>("SELECT T0.* FROM \"Tm_GeneralSetting_Coa\" T0 ").ToList();
        }

        //---------------------------------------
        //GeneralSetting_CoaModel
        //--------------------------------------- 
        public long Detail_Add(HANA_APP CONTEXT, GeneralSetting_CoaModel model, int UserId)
        {
            long Id = 0;

            if (model != null)
            {

                Tm_GeneralSetting_Coa tm_GeneralSetting_Coa = new Tm_GeneralSetting_Coa();

                CopyProperty.CopyProperties(model, tm_GeneralSetting_Coa, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tm_GeneralSetting_Coa.CreatedDate = dtModified;
                tm_GeneralSetting_Coa.CreatedUser = UserId;
                tm_GeneralSetting_Coa.ModifiedDate = dtModified;
                tm_GeneralSetting_Coa.ModifiedUser = UserId;

                CONTEXT.Tm_GeneralSetting_Coa.Add(tm_GeneralSetting_Coa);
                CONTEXT.SaveChanges();
                Id = tm_GeneralSetting_Coa.Id;

            }

            return Id;

        }

        public void Detail_Update(HANA_APP CONTEXT, GeneralSetting_CoaModel model, int UserId)
        {
            if (model != null)
            {

                Tm_GeneralSetting_Coa tm_GeneralSetting_Coa = CONTEXT.Tm_GeneralSetting_Coa.Find(model.Id);

                if (tm_GeneralSetting_Coa != null)
                {
                    var exceptColumns = new string[] { "Id", "SortCode", "Code", "Name" };
                    CopyProperty.CopyProperties(model, tm_GeneralSetting_Coa, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    tm_GeneralSetting_Coa.ModifiedDate = dtModified;
                    tm_GeneralSetting_Coa.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }
            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, GeneralSetting_CoaModel model)
        {
            if (model.Id != null)
            {
                if (model.Id != 0)
                {
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_GeneralSetting_Coa\"  WHERE \"Id\"=:p0", model.Id);

                    CONTEXT.SaveChanges();
                }
            }

        }

        //-------------------------------------
        //Detail  GeneralSetting_CoaModel
        //-------------------------------------
        public List<GeneralSetting_ItemModel> GeneralSetting_Items(HANA_APP CONTEXT, int id = 0)
        {
            string ssql = "SELECT * FROM \"Tm_GeneralSetting_Item\" T0 ORDER BY T0.\"DetId\" ";

            return CONTEXT.Database.SqlQuery<GeneralSetting_ItemModel>(ssql, id).ToList();
        }

        public int DetailItem_Add(HANA_APP CONTEXT, GeneralSetting_ItemModel model, int Id, int UserId)
        {
            int DetId = 0;

            if (model != null)
            {
                Tm_GeneralSetting_Item tm_GeneralSetting_Item = new Tm_GeneralSetting_Item();

                CopyProperty.CopyProperties(model, tm_GeneralSetting_Item, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tm_GeneralSetting_Item.CreatedDate = dtModified;
                tm_GeneralSetting_Item.CreatedUser = UserId;
                tm_GeneralSetting_Item.ModifiedDate = dtModified;
                tm_GeneralSetting_Item.ModifiedUser = UserId;

                CONTEXT.Tm_GeneralSetting_Item.Add(tm_GeneralSetting_Item);
                CONTEXT.SaveChanges();

            }

            return DetId;

        }
        public void DetailItem_Update(HANA_APP CONTEXT, GeneralSetting_ItemModel model, int UserId)
        {
            if (model != null)
            {
                Tm_GeneralSetting_Item Tm_GeneralSetting_Item = CONTEXT.Tm_GeneralSetting_Item.Find(model.DetId);

                if (Tm_GeneralSetting_Item != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tm_GeneralSetting_Item, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tm_GeneralSetting_Item.ModifiedDate = dtModified;
                    Tm_GeneralSetting_Item.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();
                }
            }

        }

        public void DetailItem_Delete(HANA_APP CONTEXT, GeneralSetting_ItemModel model)
        {
            if (model.Id != null)
            {
                if (model.Id != 0)
                {
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_GeneralSetting_Item\"  WHERE \"DetId\"=:p0", model.Id);
                    CONTEXT.SaveChanges();
                }
            }
        }

    }


    #endregion

}