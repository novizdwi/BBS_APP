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

namespace Models.Setting.MasterSettingEngine
{

    #region Models


    public static class MasterSettingEngineGetList
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

    public class MasterSettingEngineModel
    {
        
        public int _UserId { get; set; }

        public long Id { get; set; }

        public string Desc { get; set; }
        
        public List<MasterSettingEngine_BagianModel> ListBagians_ = new List<MasterSettingEngine_BagianModel>();
        public List<MasterSettingEngine_ItemModel> ListItems_ = new List<MasterSettingEngine_ItemModel>();
        public List<MasterSettingEngine_SubItemModel> ListSubItems_ = new List<MasterSettingEngine_SubItemModel>();
        public MasterSettingEngine_Bagians DetailBagians_ { get; set; }
        public MasterSettingEngine_Items DetailItems_ { get; set; }
        public MasterSettingEngine_SubItems DetailSubItems_ { get; set; }
    }

    public class MasterSettingEngine_BagianModel
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
        public string Bagian { get; set; }

        //[Required(ErrorMessage = "required")]
        public string Description { get; set; }
    }
    public class MasterSettingEngine_ItemModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long? Id { get; set; }

        public string Bagian { get; set; }
        //[Required(ErrorMessage = "required")]
        public string Item { get; set; }

        //[Required(ErrorMessage = "required")]
        public string Description { get; set; }
    }

    public class MasterSettingEngine_SubItemModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long? Id { get; set; }

        public string Bagian { get; set; }
        //[Required(ErrorMessage = "required")]
        public string Item { get; set; }

        public string SubItem { get; set; }

        //[Required(ErrorMessage = "required")]
        public string Description { get; set; }
    }

    public class MasterSettingEngine_Bagians
    {
        public List<long> deletedRowKeys { get; set; }
        public List<MasterSettingEngine_BagianModel> insertedRowValues { get; set; }
        public List<MasterSettingEngine_BagianModel> modifiedRowValues { get; set; }
    }
    public class MasterSettingEngine_Items
    {
        public List<int> deletedRowKeys { get; set; }
        public List<MasterSettingEngine_ItemModel> insertedRowValues { get; set; }
        public List<MasterSettingEngine_ItemModel> modifiedRowValues { get; set; }
    }
    public class MasterSettingEngine_SubItems
    {
        public List<int> deletedRowKeys { get; set; }
        public List<MasterSettingEngine_SubItemModel> insertedRowValues { get; set; }
        public List<MasterSettingEngine_SubItemModel> modifiedRowValues { get; set; }
    }

    #endregion

    #region Services

    public class MasterSettingEngineService
    {

        public void Update(MasterSettingEngineModel model)
        {
            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {

                            Tm_MasterSetting tm_MasterSetting = CONTEXT.Database.SqlQuery<Tm_MasterSetting>("SELECT TOP 1 * FROM \"Tm_MasterSetting\" ").SingleOrDefault();
                            if (tm_MasterSetting != null)
                            {
                                tm_MasterSetting = CONTEXT.Tm_MasterSetting.Find(tm_MasterSetting.Id);

                                String keyValue;
                                keyValue = model.Id.ToString();

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", "Tm_MasterSetting", "update", "Id", keyValue);

                                var exceptColumns = new string[] { "Id" };
                                CopyProperty.CopyProperties(model, tm_MasterSetting, false, exceptColumns);

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                
                                CONTEXT.SaveChanges();

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_MasterSetting", "update", "Id", keyValue);

                            }
                            else
                            {
                                tm_MasterSetting = new Tm_MasterSetting();
                                CopyProperty.CopyProperties(model, tm_MasterSetting, false);

                                //DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                tm_MasterSetting.Desc = "MasterSetting";

                                CONTEXT.Tm_MasterSetting.Add(tm_MasterSetting);
                                CONTEXT.SaveChanges();

                                String keyValue;
                                keyValue = tm_MasterSetting.Id.ToString();
                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_MasterSetting", "add", "Id", keyValue);
                            }

                            if (model.DetailBagians_ != null)
                            {
                                if (model.DetailBagians_.insertedRowValues != null)
                                {
                                    foreach (var Bagian in model.DetailBagians_.insertedRowValues)
                                    {
                                        Detail_Add(CONTEXT,Bagian, model._UserId);
                                    }
                                }


                                if (model.DetailBagians_.modifiedRowValues != null)
                                {
                                    foreach (var Bagian in model.DetailBagians_.modifiedRowValues)
                                    {
                                        Detail_Update(CONTEXT,Bagian, model._UserId);
                                    }
                                }

                                if (model.DetailBagians_.deletedRowKeys != null)
                                {
                                    foreach (var id in model.DetailBagians_.deletedRowKeys)
                                    {
                                        MasterSettingEngine_BagianModel Bagian = new MasterSettingEngine_BagianModel();
                                        Bagian.Id = id;
                                        Detail_Delete(CONTEXT,Bagian);
                                    }
                                }
                            }

                            if (model.DetailItems_ != null)
                            {
                                if (model.DetailItems_.insertedRowValues != null)
                                {
                                    foreach (var Item in model.DetailItems_.insertedRowValues)
                                    {
                                        Detail_Add(CONTEXT, Item, model._UserId);
                                    }
                                }


                                if (model.DetailItems_.modifiedRowValues != null)
                                {
                                    foreach (var Item in model.DetailItems_.modifiedRowValues)
                                    {
                                        Detail_Update(CONTEXT, Item, model._UserId);
                                    }
                                }

                                if (model.DetailItems_.deletedRowKeys != null)
                                {
                                    foreach (var id in model.DetailItems_.deletedRowKeys)
                                    {
                                        MasterSettingEngine_ItemModel Item = new MasterSettingEngine_ItemModel();
                                        Item.Id = id;
                                        Detail_Delete(CONTEXT, Item);
                                    }
                                }
                            }
                            if (model.DetailSubItems_ != null)
                            {
                                if (model.DetailSubItems_.insertedRowValues != null)
                                {
                                    foreach (var SubItem in model.DetailSubItems_.insertedRowValues)
                                    {
                                        Detail_Add(CONTEXT, SubItem, model._UserId);
                                    }
                                }


                                if (model.DetailSubItems_.modifiedRowValues != null)
                                {
                                    foreach (var SubItem in model.DetailSubItems_.modifiedRowValues)
                                    {
                                        Detail_Update(CONTEXT, SubItem, model._UserId);
                                    }
                                }

                                if (model.DetailSubItems_.deletedRowKeys != null)
                                {
                                    foreach (var id in model.DetailSubItems_.deletedRowKeys)
                                    {
                                        MasterSettingEngine_SubItemModel SubItem = new MasterSettingEngine_SubItemModel();
                                        SubItem.Id = id;
                                        Detail_Delete(CONTEXT, SubItem);
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


        public MasterSettingEngineModel GetNewModel()
        {
            MasterSettingEngineModel model = new MasterSettingEngineModel();

            return model;
        }

        public MasterSettingEngineModel GetById(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, id);
            }
        }

        public MasterSettingEngineModel GetById(HANA_APP CONTEXT, long id = 0)
        {
            MasterSettingEngineModel model = null;

            Tm_MasterSetting tm_MasterSetting = CONTEXT.Database.SqlQuery<Tm_MasterSetting>("SELECT TOP 1 * FROM \"Tm_MasterSetting\" ").SingleOrDefault();

            if (tm_MasterSetting != null)
            {
                model = new MasterSettingEngineModel();
                CopyProperty.CopyProperties(tm_MasterSetting, model, false);

                model.ListBagians_ = this.MasterSettingEngine_Bagians(CONTEXT);
                model.ListItems_ = this.MasterSettingEngine_Items(CONTEXT);
                model.ListSubItems_ = this.MasterSettingEngine_SubItems(CONTEXT);
            }
            else
            {
                model = GetNewModel();
            }

            return model;
        }

        //-------------------------------------
        //Detail  MasterSettingEngine_BagianModel
        //-------------------------------------
        public List<MasterSettingEngine_BagianModel> MasterSettingEngine_Bagians(HANA_APP CONTEXT)
        {
            return CONTEXT.Database.SqlQuery<MasterSettingEngine_BagianModel>("SELECT T0.* FROM \"Tm_MasterSettingEngine_Bagian\" T0 ").ToList();
        }

        //---------------------------------------
        //MasterSettingEngine_BagianModel
        //--------------------------------------- 
        public long Detail_Add(HANA_APP CONTEXT, MasterSettingEngine_BagianModel model, int UserId)
        {
            long Id = 0;

            if (model != null)
            {

                Tm_MasterSettingEngine_Bagian tm_MasterSettingEngine_Bagian = new Tm_MasterSettingEngine_Bagian();

                CopyProperty.CopyProperties(model, tm_MasterSettingEngine_Bagian, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tm_MasterSettingEngine_Bagian.CreatedDate = dtModified;
                tm_MasterSettingEngine_Bagian.CreatedUser = UserId;
                tm_MasterSettingEngine_Bagian.ModifiedDate = dtModified;
                tm_MasterSettingEngine_Bagian.ModifiedUser = UserId;

                CONTEXT.Tm_MasterSettingEngine_Bagian.Add(tm_MasterSettingEngine_Bagian);
                CONTEXT.SaveChanges();
                Id = tm_MasterSettingEngine_Bagian.Id;

            }

            return Id;

        }

        public void Detail_Update(HANA_APP CONTEXT, MasterSettingEngine_BagianModel model, int UserId)
        {
            if (model != null)
            {

                Tm_MasterSettingEngine_Bagian tm_MasterSettingEngine_Bagian = CONTEXT.Tm_MasterSettingEngine_Bagian.Find(model.Id);

                if (tm_MasterSettingEngine_Bagian != null)
                {
                    var exceptColumns = new string[] { "Id", "SortCode", "Code", "Name" };
                    CopyProperty.CopyProperties(model, tm_MasterSettingEngine_Bagian, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    tm_MasterSettingEngine_Bagian.ModifiedDate = dtModified;
                    tm_MasterSettingEngine_Bagian.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }
            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, MasterSettingEngine_BagianModel model)
        {
            if (model.Id != null)
            {
                if (model.Id != 0)
                {
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_MasterSettingEngine_Bagian\"  WHERE \"Id\"=:p0", model.Id);

                    CONTEXT.SaveChanges();
                }
            }

        }
        //-------------------------------------
        //Detail  MasterSettingEngine_ItemModel
        //-------------------------------------
        public List<MasterSettingEngine_ItemModel> MasterSettingEngine_Items(HANA_APP CONTEXT)
        {
            return CONTEXT.Database.SqlQuery<MasterSettingEngine_ItemModel>("SELECT T0.* FROM \"Tm_MasterSettingEngine_Item\" T0 ").ToList();
        }

        //---------------------------------------
        //MasterSettingEngine_ItemModel
        //--------------------------------------- 
        public long Detail_Add(HANA_APP CONTEXT, MasterSettingEngine_ItemModel model, int UserId)
        {
            long Id = 0;

            if (model != null)
            {

                Tm_MasterSettingEngine_Item tm_MasterSettingEngine_Item = new Tm_MasterSettingEngine_Item();

                CopyProperty.CopyProperties(model, tm_MasterSettingEngine_Item, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tm_MasterSettingEngine_Item.CreatedDate = dtModified;
                tm_MasterSettingEngine_Item.CreatedUser = UserId;
                tm_MasterSettingEngine_Item.ModifiedDate = dtModified;
                tm_MasterSettingEngine_Item.ModifiedUser = UserId;

                CONTEXT.Tm_MasterSettingEngine_Item.Add(tm_MasterSettingEngine_Item);
                CONTEXT.SaveChanges();
                Id = tm_MasterSettingEngine_Item.Id;

            }

            return Id;

        }

        public void Detail_Update(HANA_APP CONTEXT, MasterSettingEngine_ItemModel model, int UserId)
        {
            if (model != null)
            {

                Tm_MasterSettingEngine_Item tm_MasterSettingEngine_Item = CONTEXT.Tm_MasterSettingEngine_Item.Find(model.Id);

                if (tm_MasterSettingEngine_Item != null)
                {
                    var exceptColumns = new string[] { "Id", "SortCode", "Code", "Name" };
                    CopyProperty.CopyProperties(model, tm_MasterSettingEngine_Item, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    tm_MasterSettingEngine_Item.ModifiedDate = dtModified;
                    tm_MasterSettingEngine_Item.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }
            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, MasterSettingEngine_ItemModel model)
        {
            if (model.Id != null)
            {
                if (model.Id != 0)
                {
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_MasterSettingEngine_Item\"  WHERE \"Id\"=:p0", model.Id);

                    CONTEXT.SaveChanges();
                }
            }

        }

        //-------------------------------------
        //Detail  MasterSettingEngine_ItemModel
        //-------------------------------------
        public List<MasterSettingEngine_SubItemModel> MasterSettingEngine_SubItems(HANA_APP CONTEXT)
        {
            return CONTEXT.Database.SqlQuery<MasterSettingEngine_SubItemModel>("SELECT T0.* FROM \"Tm_MasterSettingEngine_SubItem\" T0 ").ToList();
        }

        //---------------------------------------
        //MasterSettingEngine_ItemModel
        //--------------------------------------- 
        public long Detail_Add(HANA_APP CONTEXT, MasterSettingEngine_SubItemModel model, int UserId)
        {
            long Id = 0;

            if (model != null)
            {

                Tm_MasterSettingEngine_SubItem tm_MasterSettingEngine_SubItem = new Tm_MasterSettingEngine_SubItem();

                CopyProperty.CopyProperties(model, tm_MasterSettingEngine_SubItem, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tm_MasterSettingEngine_SubItem.CreatedDate = dtModified;
                tm_MasterSettingEngine_SubItem.CreatedUser = UserId;
                tm_MasterSettingEngine_SubItem.ModifiedDate = dtModified;
                tm_MasterSettingEngine_SubItem.ModifiedUser = UserId;

                CONTEXT.Tm_MasterSettingEngine_SubItem.Add(tm_MasterSettingEngine_SubItem);
                CONTEXT.SaveChanges();
                Id = tm_MasterSettingEngine_SubItem.Id;

            }

            return Id;

        }

        public void Detail_Update(HANA_APP CONTEXT, MasterSettingEngine_SubItemModel model, int UserId)
        {
            if (model != null)
            {

                Tm_MasterSettingEngine_SubItem tm_MasterSettingEngine_SubItem = CONTEXT.Tm_MasterSettingEngine_SubItem.Find(model.Id);

                if (tm_MasterSettingEngine_SubItem != null)
                {
                    var exceptColumns = new string[] { "Id", "SortCode", "Code", "Name" };
                    CopyProperty.CopyProperties(model, tm_MasterSettingEngine_SubItem, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    tm_MasterSettingEngine_SubItem.ModifiedDate = dtModified;
                    tm_MasterSettingEngine_SubItem.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }
            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, MasterSettingEngine_SubItemModel model)
        {
            if (model.Id != null)
            {
                if (model.Id != 0)
                {
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_MasterSettingEngine_SubItem\"  WHERE \"Id\"=:p0", model.Id);

                    CONTEXT.SaveChanges();
                }
            }

        }


    }


    #endregion

}