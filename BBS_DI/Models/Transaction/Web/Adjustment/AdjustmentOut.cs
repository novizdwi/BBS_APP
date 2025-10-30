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

using Models._Sap;
using SAPbobsCOM;
using Models;

namespace Models.Transaction.Web.Adjustment
{
    #region Models
    public class AdjustmentOutModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long Id { get; set; }

        public string TransType { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public string Status { get; set; }

        public string IsAfterPosted { get; set; }

        public int? DocEntry { get; set; }

        public string DocNum_ { get; set; }

        public string Comments { get; set; }

        public string ScanDeviceId { get; set; }

        public string AdjustmentTypeCode { get; set; }

        public string AdjustmentTypeName { get; set; }

        public string WhsCode { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string CancelReason { get; set; }


        public int? ModifiedUser { get; set; }
        public List<AdjustmentOut_ItemModel> ListDetails_ = new List<AdjustmentOut_ItemModel>();

        public AdjustmentOut_Detail Details_ { get; set; }

        public List<AdjustmentOut_AttachmentModel> ListAttachments_ = new List<AdjustmentOut_AttachmentModel>();
    }

    public class AdjustmentOut_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<AdjustmentOut_ItemModel> insertedRowValues { get; set; }
        public List<AdjustmentOut_ItemModel> modifiedRowValues { get; set; }
    }

    public class AdjustmentOut_ItemModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string FreeText { get; set; }

        public string WhsCode { get; set; }

        public decimal? QuantityScan { get; set; }

        public decimal? QuantityPosted { get; set; }

        public decimal? EstQuantityPosted_ { get; set; }

        public string LineStatus { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUser { get; set; }

    }

    public class AdjustmentOutItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public List<AdjustmentOut_Item_TagModel> AdjustmentOut_Item_TagModel___ { get; set; }

    }

    public class AdjustmentOut_Item_TagModel
    {
        public int RowNo { get; set; }

        public long Id { get; set; }

        public long DetId { get; set; }

        public long DetDetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string TagId { get; set; }

        public decimal? Quantity { get; set; }

        public string EventType { get; set; }

        public string Status { get; set; }

        public string Information { get; set; }

        public string PostResultNote { get; set; }
    }


    public class AdjustmentOut_AttachmentModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int FileIndex_ { get; set; }

        public long Id { get; set; }

        public long DetId { get; set; }

        public string FileName { get; set; }

        public string Guid { get; set; }


    }
    #endregion Models

    #region Services
    public class AdjustmentOutService
    {
        public AdjustmentOutModel GetNewModel(int userId)
        {
            AdjustmentOutModel model = new AdjustmentOutModel();
            model.Status = "Draft";
            return model;
        }

        public AdjustmentOutModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public AdjustmentOutModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            AdjustmentOutModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, T1.""FirstName"" AS ""UserName"" 
                            FROM ""Tx_AdjustmentOut"" T0
                            LEFT JOIN ""Tm_User"" T1 ON T0.""ModifiedUser"" = T1.""Id""
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";


                model = CONTEXT.Database.SqlQuery<AdjustmentOutModel>(ssql, id).Single();

                if(model.DocEntry != null)
                {
                    string getDocNum = @"SELECT T1.""DocNum""
                        FROM ""Tx_AdjustmentOut"" T0
                        INNER JOIN """ + DbProvider.dbSap_Name + @""".""OIGE"" T1 ON T0.""DocEntry"" = T1.""DocEntry""
                        WHERE T0.""Id"" = :p0 
                        ORDER BY T0.""Id"" ASC
                    ";
                    model.DocNum_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id).FirstOrDefault();
                }

                model.ListDetails_ = this.AdjustmentOut_Details(CONTEXT, id);
                model.ListAttachments_ = this.GetAdjustmentOut_Attachments(id);

            }

            return model;
        }

        public List<AdjustmentOut_ItemModel> AdjustmentOut_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return AdjustmentOut_Details(CONTEXT, id);
            }

        }

        public List<AdjustmentOut_AttachmentModel> GetAdjustmentOut_Attachments(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAdjustmentOut_Attachments(CONTEXT, id);
            }

        }

        public List<AdjustmentOut_AttachmentModel> GetAdjustmentOut_Attachments(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*
                FROM ""Tx_AdjustmentOut_Attachment"" T0
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var ret =  CONTEXT.Database.SqlQuery<AdjustmentOut_AttachmentModel>(ssql, id).ToList();
            return ret;
        }

        public List<AdjustmentOut_ItemModel> AdjustmentOut_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, 
                     COALESCE ( (SELECT COUNT(Tx.""TagId"")
                        FROM ""Tx_AdjustmentOut_Item_Tag"" Tx
                        INNER JOIN ""Tm_Item_Warehouse_Tag"" Ty ON Tx.""TagId"" = Ty.""TagId"" AND T2.""WhsCode"" = Ty.""WhsCode""  AND Ty.""Status"" = 'A'
                        WHERE Tx.""DetId"" = T0.""DetId""
                    ) , 0) AS ""EstQuantityPosted_""
                FROM ""Tx_AdjustmentOut_Item"" T0
                INNER JOIN ""Tx_AdjustmentOut"" T2 ON T0.""Id"" = T2.""Id""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var AdjustmentOut = CONTEXT.Database.SqlQuery<AdjustmentOut_ItemModel>(ssql, id).ToList();
            return AdjustmentOut;
        }

        public AdjustmentOutModel NavFirst(int userId)
        {
            AdjustmentOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentOut\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public AdjustmentOutModel NavPrevious(int userId, long id = 0)
        {
            AdjustmentOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentOut\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(CONTEXT, userId, Id.Value);
                }
            }

            if (model == null)
            {
                model = this.NavFirst(userId);
            }


            return model;
        }

        public AdjustmentOutModel NavNext(int userId, long id = 0)
        {
            AdjustmentOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentOut\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(CONTEXT, userId, Id.Value);
                }
            }

            if (model == null)
            {
                model = this.NavFirst(userId);
            }

            return model;
        }

        public AdjustmentOutModel NavLast(int userId)
        {
            AdjustmentOutModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentOut");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentOut\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public void Update(AdjustmentOutModel model)
        {
            if (model != null)
            {
                if (model != null)
                {
                    using (var CONTEXT = new HANA_APP())
                    {
                        using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                        {
                            try
                            {
                                String keyValue;
                                keyValue = model.Id.ToString();

                                SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "before", "AdjustmentOut", "update", "Id", keyValue);


                                Tx_AdjustmentOut Tx_AdjustmentOut = CONTEXT.Tx_AdjustmentOut.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_AdjustmentOut.ModifiedDate = dtModified;
                                Tx_AdjustmentOut.ModifiedUser = model._UserId;

                                if (Tx_AdjustmentOut != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_AdjustmentOut, false, exceptColumns);
                                    Tx_AdjustmentOut.ModifiedDate = dtModified;
                                    Tx_AdjustmentOut.ModifiedUser = model._UserId;
                                    
                                    CONTEXT.SaveChanges();
                                    
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "after", "AdjustmentOut", "update", "Id", keyValue);

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
        }


        public void Post(int userId, long id)
        {
            SAPbobsCOM.Company oCompany = null;

            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentOut__UpdateItem\"(:p0,:p1)", userId, id);
                        CONTEXT.SaveChanges();

                        oCompany = SAPCachedCompany.GetCompany();
                        oCompany.StartTransaction();

                        String keyValue;
                        keyValue = id.ToString();

                        AdjustmentOutModel syncAdjustmentOut = GetById(userId, id);
                        Tx_AdjustmentOut tx_AdjustmentOut = CONTEXT.Tx_AdjustmentOut.Find(id);

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "before", "Tx_AdjustmentOut", "post", "Id", keyValue);

                        if (tx_AdjustmentOut != null)
                        {
                            string docEntry = AddGoodsIssue(oCompany, userId, id, syncAdjustmentOut);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_AdjustmentOut.DocEntry = Convert.ToInt32(docEntry);
                            tx_AdjustmentOut.Status = "Posted";

                            tx_AdjustmentOut.IsAfterPosted = "Y";
                            tx_AdjustmentOut.ModifiedDate = dtModified;
                            tx_AdjustmentOut.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentOut__UpdateItemTag\"(:p0,:p1)", userId, id);
                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", "post", "Id", keyValue);

                        if (oCompany.InTransaction)
                        {
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                        }

                        CONTEXT_TRANS.Commit();
                    }

                    catch (Exception ex)
                    {
                        if (oCompany.InTransaction)
                        {
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }

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
                    finally
                    {
                        SAPCachedCompany.Release(oCompany);
                    }
                }
                
            }

        }

        private string AddGoodsIssue(Company oCompany, int userId, long id, AdjustmentOutModel model)
        {
            string result = "";

            string CoaAdjustment = GeneralGetList.GetSAPCoaAdjustment(model.AdjustmentTypeCode);
            int nErr;
            string errMsg;
            SAPbobsCOM.Documents oDocument = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
            oDocument.DocDate = model.TransDate ?? DateTime.Now;

            if (!string.IsNullOrWhiteSpace(model.Comments))
            {
                oDocument.Comments = model.Comments;
            }

            oDocument.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
            oDocument.UserFields.Fields.Item("U_IDU_WebTransNo").Value = model.TransNo;
            oDocument.UserFields.Fields.Item("U_IDU_AdjustmentType").Value = model.AdjustmentTypeName;

            if(model.ListDetails_.Count > 0)
            {
                foreach(var item in model.ListDetails_)
                {
                    if(item.QuantityPosted > 0)
                    {
                        oDocument.Lines.ItemCode = item.ItemCode;
                        oDocument.Lines.ItemDescription = item.ItemName;
                        
                        oDocument.Lines.Quantity = double.Parse(item.QuantityPosted.ToString());
                        oDocument.Lines.AccountCode = CoaAdjustment;
                        oDocument.Lines.WarehouseCode = item.WhsCode;

                        //if (item.UomEntry != null)
                        //{
                        //    oDocument.Lines.UoMEntry = Convert.ToInt32(item.UomEntry);
                        //}

                        oDocument.Lines.UserFields.Fields.Item("U_IDU_WebId").Value = Convert.ToInt32(model.Id);
                        oDocument.Lines.UserFields.Fields.Item("U_IDU_DetId").Value = Convert.ToInt32(item.DetId);

                        oDocument.Lines.Add();
                    }
                }
            }

            int docAdd = oDocument.Add();
            if (docAdd != 0)
            {
                nErr = oCompany.GetLastErrorCode();
                errMsg = oCompany.GetLastErrorDescription();

                SapCompany.CleanUp(oDocument);

                throw new Exception("[VALIDATION] - Add Goods Receipt : " + nErr.ToString() + "|" + errMsg);
            }
            result = oCompany.GetNewObjectKey();
            SapCompany.CleanUp(oDocument);

            return result;
        }

        public void Cancel(int userId, long Id, string cancelReason)
        {
            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = Id.ToString();

                        Tx_AdjustmentOut tx_AdjustmentOut = CONTEXT.Tx_AdjustmentOut.Find(Id);

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "before", "Tx_AdjustmentOut", "cancel", "Id", keyValue);
                        if (tx_AdjustmentOut != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_AdjustmentOut.Status = "Cancel";
                            tx_AdjustmentOut.CancelReason = cancelReason;
                            tx_AdjustmentOut.ModifiedDate = dtModified;
                            tx_AdjustmentOut.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", "cancel", "Id", keyValue);


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


        public AdjustmentOutItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;

            AdjustmentOutItemTagView___ model = new AdjustmentOutItemTagView___();
            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName"", T1.""WhsCode""
                                FROM ""Tx_AdjustmentOut_Item"" T0   
                                INNER JOIN ""Tx_AdjustmentOut"" T1 ON T0.""Id"" = T1.""Id""
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<AdjustmentOutItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY T0.""DetDetId"") AS ""RowNo"", T0.*,
                        CASE WHEN T1.""Status"" = 'Draft' THEN 
                             CASE WHEN COALESCE(T3.""TagId"", '') = '' THEN 'Tag Id not Exists in master item'
                                  WHEN T1.""WhsCode"" != T3.""WhsCode"" THEN 'Tag Id in different Warehouse'
                                  WHEN T3.""Status"" != 'A' THEN 'Tag Id is not acitve'
                             ELSE 'Valid' END 
                        ELSE T0.""Status"" END AS ""Information""
                        FROM ""Tx_AdjustmentOut_Item_Tag"" T0  
                        INNER JOIN ""Tx_AdjustmentOut"" T1 ON T0.""Id"" = T1.""Id""  
                        LEFT JOIN ""Tm_Item_Warehouse_Tag"" T3 ON T0.""TagId"" = T3.""TagId"" 
                        WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 
                ";

                model.AdjustmentOut_Item_TagModel___ = CONTEXT.Database.SqlQuery<AdjustmentOut_Item_TagModel>(sql, id, detId).ToList();
            }

            return model;
        }
        #region Attachment
        public AdjustmentOut_AttachmentModel GetAdjustmentOut_Attachments_GetById(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAdjustmentOut_Attachments_GetById(CONTEXT, id);
            }
        }

        public AdjustmentOut_AttachmentModel GetAdjustmentOut_Attachments_GetById(HANA_APP CONTEXT, long id = 0)
        {
            AdjustmentOut_AttachmentModel model = null;
            if (id != 0)
            { 
                string ssql = "SELECT TOP 1 T0.*  "
                                + " FROM \"Tx_AdjustmentOut_Attachment\" T0 "
                                + " WHERE T0.\"DetId\" = :p0 ";

            model = CONTEXT.Database.SqlQuery<AdjustmentOut_AttachmentModel>(ssql, id).Single();
            }

            return model;
        }


        public long Detail_Add(List<AdjustmentOut_AttachmentModel> ListModel)
        {
            long Id = 0;

            if (ListModel != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        for (int i = 0; i < ListModel.Count; i++)
                        {
                            Tx_AdjustmentOut_Attachment tx_AdjustmentOut_Attachment = new Tx_AdjustmentOut_Attachment();
                            var model = ListModel[i];

                            CopyProperty.CopyProperties(model, tx_AdjustmentOut_Attachment, false);
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_AdjustmentOut_Attachment.CreatedDate = dtModified;
                            tx_AdjustmentOut_Attachment.CreatedUser = model._UserId;
                            tx_AdjustmentOut_Attachment.ModifiedDate = dtModified;
                            tx_AdjustmentOut_Attachment.ModifiedUser = model._UserId;

                            CONTEXT.Tx_AdjustmentOut_Attachment.Add(tx_AdjustmentOut_Attachment);
                            CONTEXT.SaveChanges();
                            string keyValue = tx_AdjustmentOut_Attachment.Id.ToString();
                            SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", "add", "Id", keyValue);
                        }

                        CONTEXT_TRANS.Commit();
                    }
                }

            }

            return Id;

        }

        public void Attachment_Delete(AdjustmentOut_AttachmentModel model)
        {

            if (model != null)
            {
                if (model.DetId != 0)
                {
                    using (var CONTEXT = new HANA_APP())
                    {
                        using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                        {
                            try
                            {
                                string keyValue = model.Id.ToString();
                                Tx_AdjustmentOut_Attachment tx_AdjustmentOutAttachment = CONTEXT.Tx_AdjustmentOut_Attachment.Find(model.DetId);

                                SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "before", "Tx_AdjustmentOut", "delete", "Id", keyValue);

                                if (tx_AdjustmentOutAttachment != null)
                                {
                                    CONTEXT.Tx_AdjustmentOut_Attachment.Remove(tx_AdjustmentOutAttachment);
                                    CONTEXT.SaveChanges();
                                }

                                SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentOut", CONTEXT, "after", "Tx_AdjustmentOut", "delete", "Id", keyValue);
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
        }


        #endregion
    }

    #endregion Services
}
