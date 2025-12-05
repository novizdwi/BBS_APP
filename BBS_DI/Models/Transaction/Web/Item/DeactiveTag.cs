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

namespace Models.Transaction.Web.Item
{
    #region Models
    public class DeactiveTagModel
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

        public DateTime? PostingDate { get; set; }

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

        public string CheckNeedApproval_ { get; set; }

        public string ApprovalStatus { get; set; }

        public string IsOpeningBalance { get; set; }

        public List<DeactiveTag_ItemModel> ListDetails_ = new List<DeactiveTag_ItemModel>();

        public DeactiveTag_Detail Details_ { get; set; }

        public List<DeactiveTag_AttachmentModel> ListAttachments_ = new List<DeactiveTag_AttachmentModel>();
    }

    public class DeactiveTag_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<DeactiveTag_ItemModel> insertedRowValues { get; set; }
        public List<DeactiveTag_ItemModel> modifiedRowValues { get; set; }
    }

    public class DeactiveTag_ItemModel
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

        public decimal? UnitPriceTc { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUser { get; set; }

    }

    public class DeactiveTagItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public List<DeactiveTag_Item_TagModel> DeactiveTag_Item_TagModel___ { get; set; }

    }

    public class DeactiveTag_Item_TagModel
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


    public class DeactiveTag_AttachmentModel
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
    public class DeactiveTagService
    {
        public DeactiveTagModel GetNewModel(int userId)
        {
            DeactiveTagModel model = new DeactiveTagModel();
            model.Status = "Draft";
            return model;
        }

        public DeactiveTagModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public DeactiveTagModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            DeactiveTagModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, T1.""FirstName"" AS ""UserName"" 
                            FROM ""Tx_DeactiveTag"" T0
                            LEFT JOIN ""Tm_User"" T1 ON T0.""ModifiedUser"" = T1.""Id""
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";


                model = CONTEXT.Database.SqlQuery<DeactiveTagModel>(ssql, id).Single();

                if(model.DocEntry != null)
                {
                    string getDocNum = @"SELECT T1.""DocNum""
                        FROM ""Tx_DeactiveTag"" T0
                        INNER JOIN """ + DbProvider.dbSap_Name + @""".""OIGN"" T1 ON T0.""DocEntry"" = T1.""DocEntry""
                        WHERE T0.""Id"" = :p0 
                        ORDER BY T0.""Id"" ASC
                    ";
                    model.DocNum_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id).FirstOrDefault();
                }

                //if (model.Id > 0)
                //{
                //    ssql = "CALL \"SpSysCheckNeedApproval\" (" + userId.ToString() + "," + model.Id.ToString() + ",'GoodsReceiptPO') ";
                //    model.CheckNeedApproval_ = CONTEXT.Database.SqlQuery<string>(ssql).FirstOrDefault();
                //}

                model.ListDetails_ = this.DeactiveTag_Details(CONTEXT, id);
                model.ListAttachments_ = this.GetDeactiveTag_Attachments(id);

            }

            return model;
        }

        public List<DeactiveTag_ItemModel> DeactiveTag_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return DeactiveTag_Details(CONTEXT, id);
            }

        }

        public List<DeactiveTag_AttachmentModel> GetDeactiveTag_Attachments(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDeactiveTag_Attachments(CONTEXT, id);
            }

        }

        public List<DeactiveTag_AttachmentModel> GetDeactiveTag_Attachments(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*
                FROM ""Tx_DeactiveTag_Attachment"" T0
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var ret =  CONTEXT.Database.SqlQuery<DeactiveTag_AttachmentModel>(ssql, id).ToList();
            return ret;
        }

        public List<DeactiveTag_ItemModel> DeactiveTag_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, 
                    CASE WHEN T1.""Status"" = 'Draft' THEN 
                        T0.""QuantityScan"" - (SELECT 
                                COALESCE( COUNT(Tx.""TagId""), 0) 
                                FROM ""Tx_DeactiveTag_Item_Tag"" Tx
                                INNER JOIN ""Tm_Item_Warehouse_Tag"" Ty ON Tx.""TagId"" = Ty.""TagId"" AND Ty.""Status"" = 'A'
                                WHERE Tx.""DetId"" = T0.""DetId""
                            ) 
                    ELSE NULL 
                    END AS ""EstQuantityPosted_""
                FROM ""Tx_DeactiveTag_Item"" T0
                INNER JOIN ""Tx_DeactiveTag"" T1 ON T0.""Id"" = T1.""Id""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var DeactiveTag = CONTEXT.Database.SqlQuery<DeactiveTag_ItemModel>(ssql, id).ToList();
            return DeactiveTag;
        }


        public DeactiveTagModel NavFirst(int userId)
        {
            DeactiveTagModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DeactiveTag");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_DeactiveTag\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public DeactiveTagModel NavPrevious(int userId, long id = 0)
        {
            DeactiveTagModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DeactiveTag");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_DeactiveTag\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public DeactiveTagModel NavNext(int userId, long id = 0)
        {
            DeactiveTagModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DeactiveTag");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_DeactiveTag\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public DeactiveTagModel NavLast(int userId)
        {
            DeactiveTagModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DeactiveTag");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_DeactiveTag\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public void Update(DeactiveTagModel model)
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

                                SpNotif.SpSysControllerTransNotif(model._UserId, "DeactiveTag", CONTEXT, "before", "DeactiveTag", "update", "Id", keyValue);


                                Tx_DeactiveTag Tx_DeactiveTag = CONTEXT.Tx_DeactiveTag.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_DeactiveTag.ModifiedDate = dtModified;
                                Tx_DeactiveTag.ModifiedUser = model._UserId;

                                if (Tx_DeactiveTag != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_DeactiveTag, false, exceptColumns);
                                    Tx_DeactiveTag.ModifiedDate = dtModified;
                                    Tx_DeactiveTag.ModifiedUser = model._UserId;
                                    
                                    CONTEXT.SaveChanges();
                                    
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "DeactiveTag", CONTEXT, "after", "DeactiveTag", "update", "Id", keyValue);

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
                        oCompany = SAPCachedCompany.GetCompany();
                        oCompany.StartTransaction();

                        String keyValue;
                        keyValue = id.ToString();

                        DeactiveTagModel syncDeactiveTag = GetById(userId, id);
                        Tx_DeactiveTag tx_DeactiveTag = CONTEXT.Tx_DeactiveTag.Find(id);

                        SpNotif.SpSysControllerTransNotif(userId, "DeactiveTag", CONTEXT, "before", "Tx_DeactiveTag", "post", "Id", keyValue);

                        if (tx_DeactiveTag != null)
                        {
                            string docEntry = AddGoodsReceipt(oCompany, userId, id, syncDeactiveTag);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_DeactiveTag.PostingDate = dtModified;
                            tx_DeactiveTag.Status = "Posted";
                            
                            tx_DeactiveTag.ModifiedDate = dtModified;
                            tx_DeactiveTag.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        //CONTEXT.Database.ExecuteSqlCommand("CALL \"SpDeactiveTag_DeactiveItemTag\"(:p0,:p1)", userId, id);
                        SpNotif.SpSysControllerTransNotif(userId, "DeactiveTag", CONTEXT, "after", "Tx_DeactiveTag", "post", "Id", keyValue);

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

        private string AddGoodsReceipt(Company oCompany, int userId, long id, DeactiveTagModel model)
        {
            string result = "";

            string CoaAdjustment = GeneralGetList.GetSAPCoaAdjustment(model.AdjustmentTypeCode);
            int nErr;
            string errMsg;
            SAPbobsCOM.Documents oDocument = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);
            oDocument.DocDate = DateTime.Now;

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

                        oDocument.Lines.Price = (double)(item.UnitPriceTc ?? 0m);
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

                        Tx_DeactiveTag tx_DeactiveTag = CONTEXT.Tx_DeactiveTag.Find(Id);

                        SpNotif.SpSysControllerTransNotif(userId, "DeactiveTag", CONTEXT, "before", "Tx_DeactiveTag", "cancel", "Id", keyValue);
                        if (tx_DeactiveTag != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_DeactiveTag.Status = "Cancel";
                            tx_DeactiveTag.CancelReason = cancelReason;
                            tx_DeactiveTag.ModifiedDate = dtModified;
                            tx_DeactiveTag.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "DeactiveTag", CONTEXT, "after", "Tx_DeactiveTag", "cancel", "Id", keyValue);


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


        public DeactiveTagItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;

            DeactiveTagItemTagView___ model = new DeactiveTagItemTagView___();
            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName""
                                FROM ""Tx_DeactiveTag_Item"" T0   
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<DeactiveTagItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY T0.""DetDetId"") AS ""RowNo"", T0.*,
                        CASE WHEN T1.""Status"" = 'Draft' THEN 
                            CASE WHEN COALESCE(T3.""TagId"", '') = '' THEN 'New Entry'
                                 WHEN T3.""Status"" = 'I' THEN 'Reactivation'
                                 WHEN T1.""WhsCode"" != T3.""WhsCode"" THEN 'Change Warehouse'
                            ELSE 'Invalid' END 
                        ELSE T0.""Status"" END AS ""Information""
                        FROM ""Tx_DeactiveTag_Item_Tag"" T0  
                        INNER JOIN ""Tx_DeactiveTag"" T1 ON T0.""Id"" = T1.""Id""  
                        LEFT JOIN ""Tm_Item_Warehouse_Tag"" T3 ON T0.""TagId"" = T3.""TagId""
                        WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 
                ";

                model.DeactiveTag_Item_TagModel___ = CONTEXT.Database.SqlQuery<DeactiveTag_Item_TagModel>(sql, id, detId).ToList();
            }

            return model;
        }
        #region Attachment
        public DeactiveTag_AttachmentModel GetDeactiveTag_Attachments_GetById(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDeactiveTag_Attachments_GetById(CONTEXT, id);
            }
        }

        public DeactiveTag_AttachmentModel GetDeactiveTag_Attachments_GetById(HANA_APP CONTEXT, long id = 0)
        {
            DeactiveTag_AttachmentModel model = null;
            if (id != 0)
            { 
                string ssql = "SELECT TOP 1 T0.*  "
                                + " FROM \"Tx_DeactiveTag_Attachment\" T0 "
                                + " WHERE T0.\"DetId\" = :p0 ";

            model = CONTEXT.Database.SqlQuery<DeactiveTag_AttachmentModel>(ssql, id).Single();
            }

            return model;
        }

          


        #endregion
    }

    #endregion Services
}
