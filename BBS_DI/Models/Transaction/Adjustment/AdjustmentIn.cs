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

namespace Models.Transaction.Adjustment
{
    #region Models
    public class AdjustmentInModel
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

        public string ScanDeviceId { get; set; }

        public string AdjustmentTypeCode { get; set; }

        public string AdjustmentTypeName { get; set; }

        public string WhsCode { get; set; }
        
        public string PillarsCode { get; set; }

        public string PillarsName { get; set; }

        public string ClassCode { get; set; }

        public string ClassName { get; set; }

        public string SubClass1Code { get; set; }

        public string SubClass1Name { get; set; }

        public string SubClass2Code { get; set; }

        public string SubClass2Name { get; set; }

        public string ProjectCode { get; set; }

        public string ProjectName { get; set; }

        public string CancelReason { get; set; }

        public string CheckNeedApproval_ { get; set; }

        public string ApprovalStatus { get; set; }

        public string ApprovalMessages { get; set; }

        public string IsApproval { get; set; }

        public string IsOpeningBalance { get; set; }

        [Required(ErrorMessage = "required")]
        public string Comments { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUser { get; set; }

        public string CreatedDate_ { get; set; }

        public string ModifiedDate_ { get; set; }

        public List<AdjustmentIn_ItemModel> ListDetails_ = new List<AdjustmentIn_ItemModel>();

        public AdjustmentIn_Detail Details_ { get; set; }

        public List<AdjustmentIn_AttachmentModel> ListAttachments_ = new List<AdjustmentIn_AttachmentModel>();

        public List<GetCodeNameModel> PillarsList { get; set; }

        public List<GetCodeNameModel> ClassList { get; set; }

        public List<GetCodeNameModel> SubClass1List { get; set; }

        public List<GetCodeNameModel> SubClass2List { get; set; }

        public List<GetCodeNameModel> ProjectList { get; set; }

        public List<AdjustmentIn_ApprovalModel> ListApprovalStep_ = new List<AdjustmentIn_ApprovalModel>();

        public AdjustmentIn_Approval ApprovalStep_ { get; set; }
    }

    public class AdjustmentIn_ApprovalModel
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

        public int? StageId { get; set; }

        public int? UserId { get; set; }

        public string Username { get; set; }

        public int? Step { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }

        public DateTime? ActionDate { get; set; }
    }


    public class AdjustmentIn_Approval
    {
        public List<long> deletedRowKeys { get; set; }
        public List<AdjustmentIn_ApprovalModel> insertedRowValues { get; set; }
        public List<AdjustmentIn_ApprovalModel> modifiedRowValues { get; set; }
    }

    public class AdjustmentIn_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<AdjustmentIn_ItemModel> insertedRowValues { get; set; }
        public List<AdjustmentIn_ItemModel> modifiedRowValues { get; set; }
    }

    public class AdjustmentIn_ItemModel
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
        
        public string PillarsCode { get; set; }

        public string PillarsName { get; set; }

        public string ClassCode { get; set; }

        public string ClassName { get; set; }

        public string SubClass1Code { get; set; }

        public string SubClass1Name { get; set; }

        public string SubClass2Code { get; set; }

        public string SubClass2Name { get; set; }

        public string ProjectCode { get; set; }

        public string ProjectName { get; set; }


        public decimal? QuantityScan { get; set; }

        public decimal? QuantityPosted { get; set; }

        public decimal? EstQuantityPosted_ { get; set; }

        public decimal? OnHand_ { get; set; }

        public string LineStatus { get; set; }

        public decimal? UnitPriceTc { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUser { get; set; }

    }

    public class AdjustmentInItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public List<AdjustmentIn_Item_TagModel> AdjustmentIn_Item_TagModel___ { get; set; }

    }

    public class AdjustmentIn_Item_TagModel
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


    public class AdjustmentIn_AttachmentModel
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
    public class AdjustmentInService
    {
        public AdjustmentInModel GetNewModel(int userId)
        {
            AdjustmentInModel model = new AdjustmentInModel();
            model.Status = "Draft";
            return model;
        }

        public AdjustmentInModel GetById(int userId, long id = 0, string method = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id, method);
            }
        }

        public AdjustmentInModel GetById(HANA_APP CONTEXT, int userId, long id = 0, string method ="")
        {
            AdjustmentInModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, 
                            TO_VARCHAR(T0.""CreatedDate"", 'DD/MM/YYYY') AS ""CreatedDate_"",
                            TO_VARCHAR(T0.""ModifiedDate"", 'DD/MM/YYYY') AS ""ModifiedDate_""
                            FROM ""Tx_AdjustmentIn"" T0
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";


                model = CONTEXT.Database.SqlQuery<AdjustmentInModel>(ssql, id).Single();

                if(model.DocEntry != null)
                {
                    string getDocNum = @"SELECT T1.""DocNum""
                        FROM ""Tx_AdjustmentIn"" T0
                        INNER JOIN """ + DbProvider.dbSap_Name + @""".""OIGN"" T1 ON T0.""DocEntry"" = T1.""DocEntry""
                        WHERE T0.""Id"" = :p0 
                        ORDER BY T0.""Id"" ASC
                    ";
                    model.DocNum_ = CONTEXT.Database.SqlQuery<string>(getDocNum, id).FirstOrDefault();
                }

                model.ListDetails_ = this.AdjustmentIn_Details(CONTEXT, id);
                model.ListAttachments_ = this.GetAdjustmentIn_Attachments(id);
                model.ListApprovalStep_ = this.GetAdjustmentIn_ApprovalSteps(id);

                if (method != "post")
                {

                    model.PillarsList = GeneralGetList.GetCostCenterList("1");
                    model.ClassList = GeneralGetList.GetCostCenterList("2");
                    model.SubClass1List = GeneralGetList.GetCostCenterList("3");
                    model.SubClass2List = GeneralGetList.GetCostCenterList("4");
                    model.ProjectList = GeneralGetList.GetProjectList();
                }

            }

            return model;
        }

        public List<AdjustmentIn_ItemModel> AdjustmentIn_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return AdjustmentIn_Details(CONTEXT, id);
            }

        }

        public List<AdjustmentIn_AttachmentModel> GetAdjustmentIn_Attachments(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAdjustmentIn_Attachments(CONTEXT, id);
            }

        }

        public List<AdjustmentIn_AttachmentModel> GetAdjustmentIn_Attachments(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*
                FROM ""Tx_AdjustmentIn_Attachment"" T0
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var ret =  CONTEXT.Database.SqlQuery<AdjustmentIn_AttachmentModel>(ssql, id).ToList();
            return ret;
        }

        public List<AdjustmentIn_ItemModel> AdjustmentIn_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, 
                    CASE WHEN T1.""Status"" = 'Draft' THEN 
                        T0.""QuantityScan"" - ((SELECT 
                                COALESCE( COUNT(Tx.""TagId""), 0) 
                                FROM ""Tx_AdjustmentIn_Item_Tag"" Tx
                                INNER JOIN ""Tm_Item_Warehouse_Tag"" Ty ON Tx.""TagId"" = Ty.""TagId"" AND Ty.""Status"" = 'A'
                                WHERE Tx.""DetId"" = T0.""DetId""
                            ) + COALESCE((
                        SELECT COUNT(*)
                        FROM (
                            SELECT Tx.""TagId""
                            FROM ""Tx_AdjustmentIn_Item_Tag"" Tx
                            WHERE Tx.""DetId"" = T0.""DetId""
                            GROUP BY Tx.""TagId""
                            HAVING COUNT(*) > 1
                        ) DupTags
                    ), 0))
                    ELSE NULL 
                    END AS ""EstQuantityPosted_"",
                    T2.""OnHand"" AS ""OnHand_""
                FROM ""Tx_AdjustmentIn_Item"" T0
                INNER JOIN ""Tx_AdjustmentIn"" T1 ON T0.""Id"" = T1.""Id""
                LEFT JOIN ""Tm_Item_Warehouse"" T2 ON T0.""ItemCode"" = T2.""ItemCode"" AND T0.""WhsCode"" = T2.""WhsCode""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var AdjustmentIn = CONTEXT.Database.SqlQuery<AdjustmentIn_ItemModel>(ssql, id).ToList();
            return AdjustmentIn;
        }

        public List<AdjustmentIn_ApprovalModel> GetAdjustmentIn_ApprovalSteps(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAdjustmentIn_ApprovalSteps(CONTEXT, id);
            }

        }

        public List<AdjustmentIn_ApprovalModel> GetAdjustmentIn_ApprovalSteps(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""UserName""  AS Username
                FROM ""Tx_AdjustmentIn_Approval"" T0
                LEFT JOIN ""Tm_User"" T1 ON T1.""Id"" = T0.""UserId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""Step"" ASC
            ";
            var listData = CONTEXT.Database.SqlQuery<AdjustmentIn_ApprovalModel>(ssql, id).ToList();
            return listData;
        }

        public AdjustmentInModel NavFirst(int userId)
        {
            AdjustmentInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentIn\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public AdjustmentInModel NavPrevious(int userId, long id = 0)
        {
            AdjustmentInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentIn\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public AdjustmentInModel NavNext(int userId, long id = 0)
        {
            AdjustmentInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentIn\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public AdjustmentInModel NavLast(int userId)
        {
            AdjustmentInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "AdjustmentIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentIn\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public void Update(AdjustmentInModel model, string method = "")
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

                                SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentIn", CONTEXT, "before", "AdjustmentIn", "update", "Id", keyValue);


                                Tx_AdjustmentIn tx_AdjustmentIn = CONTEXT.Tx_AdjustmentIn.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                           
                                if (tx_AdjustmentIn != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_AdjustmentIn, false, exceptColumns);
                                    var isApprovalActive = _Utils.GeneralGetList.GetApprovalActive("AdjustmentIn");

                                    tx_AdjustmentIn.IsApproval = !string.IsNullOrEmpty(isApprovalActive) ? isApprovalActive : "N";
                                    tx_AdjustmentIn.ModifiedDate = dtModified;
                                    tx_AdjustmentIn.ModifiedUser = model._UserId;
                                    
                                    //CONTEXT.SaveChanges();
                                    if (model.Details_ != null)
                                    {
                                        if (model.Details_.modifiedRowValues != null)
                                        {
                                            foreach (var detail in model.Details_.modifiedRowValues)
                                            {
                                                Detail_Update(CONTEXT, detail, model._UserId);
                                            }
                                        }
                                    }

                                    if(method == "Post")
                                    {
                                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentIn_UpdateItem\"(:p0,:p1)", model._UserId, model.Id);
                                    }

                                    CONTEXT.SaveChanges();
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentIn", CONTEXT, "after", "AdjustmentIn", "update", "Id", keyValue);

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

        public void Detail_Update(HANA_APP CONTEXT, AdjustmentIn_ItemModel model, int UserId)
        {
            if (model != null)
            {

                Tx_AdjustmentIn_Item tx_AdjustmentIn_Item = CONTEXT.Tx_AdjustmentIn_Item.Find(model.DetId);

                if (tx_AdjustmentIn_Item != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, tx_AdjustmentIn_Item, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    tx_AdjustmentIn_Item.ModifiedDate = dtModified;
                    tx_AdjustmentIn_Item.ModifiedUser = UserId;

                    //CONTEXT.SaveChanges();
                }

            }

        }

        public void Post(int userId, AdjustmentInModel adjustmentInModel)
        {
            try
            {
                Update(adjustmentInModel, "Post");
                PostSAP(userId, adjustmentInModel.Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PostSAP(int userId, long id)
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

                        AdjustmentInModel syncAdjustmentIn = GetById(userId, id, "post");
                        Tx_AdjustmentIn tx_AdjustmentIn = CONTEXT.Tx_AdjustmentIn.Find(id);

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "before", "Tx_AdjustmentIn", "post", "Id", keyValue);

                        if (tx_AdjustmentIn != null)
                        {
                            string docEntry = string.Empty;
                            if (syncAdjustmentIn.IsOpeningBalance != "Y")
                            {
                                docEntry = AddGoodsReceipt(oCompany, userId, id, syncAdjustmentIn);
                            }

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_AdjustmentIn.PostingDate = dtModified;
                            if(!string.IsNullOrEmpty(docEntry))
                            {
                                tx_AdjustmentIn.DocEntry = Convert.ToInt32(docEntry);
                            }

                            tx_AdjustmentIn.Status = "Posted";

                            tx_AdjustmentIn.IsAfterPosted = "Y";
                            tx_AdjustmentIn.ModifiedDate = dtModified;
                            tx_AdjustmentIn.ModifiedUser = userId;

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentIn_InsertItemTag\"(:p0,:p1)", userId, id);
                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "after", "Tx_AdjustmentIn", "post", "Id", keyValue);

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

        private string AddGoodsReceipt(Company oCompany, int userId, long id, AdjustmentInModel model)
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

            if (!string.IsNullOrWhiteSpace(model.Comments))
            {
                oDocument.JournalMemo = model.Comments;
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

                        oDocument.Lines.CostingCode = item.PillarsCode;
                        oDocument.Lines.CostingCode2 = item.ClassCode;
                        oDocument.Lines.CostingCode3 = item.SubClass1Code;
                        oDocument.Lines.CostingCode4 = item.SubClass2Code;
                        oDocument.Lines.ProjectCode = item.ProjectCode;

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

                        Tx_AdjustmentIn tx_AdjustmentIn = CONTEXT.Tx_AdjustmentIn.Find(Id);

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "before", "Tx_AdjustmentIn", "cancel", "Id", keyValue);
                        if (tx_AdjustmentIn != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_AdjustmentIn.Status = "Cancel";
                            tx_AdjustmentIn.CancelReason = cancelReason;
                            tx_AdjustmentIn.ModifiedDate = dtModified;
                            tx_AdjustmentIn.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "after", "Tx_AdjustmentIn", "cancel", "Id", keyValue);


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

        public void Approve(int userId, long Id, string approvalMessages)
        {
            using (var CONTEXT = new HANA_APP())
            {
               AdjustmentInModel adjustmentInModel = GetById(userId, Id);

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = Id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "before", "Tx_AdjustmentIn", "approve", "Id", keyValue);

                        Tx_AdjustmentIn tx_AdjustmentIn = CONTEXT.Tx_AdjustmentIn.Find(Id);
                        if (tx_AdjustmentIn != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            var isApprovalActive = _Utils.GeneralGetList.GetApprovalActive("AdjustmentIn");
                            //tx_AdjustmentIn.Status = "";
                            tx_AdjustmentIn.ApprovalStatus = isApprovalActive == "Y" && string.IsNullOrEmpty(tx_AdjustmentIn.ApprovalStatus) ? "Waiting" : tx_AdjustmentIn.ApprovalStatus;
                            tx_AdjustmentIn.ApprovalMessages = approvalMessages;
                            tx_AdjustmentIn.ModifiedDate = dtModified;
                            tx_AdjustmentIn.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpAdjustmentIn_UpdateItem\"(:p0,:p1)", userId, Id);

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "after", "Tx_AdjustmentIn", "approve", "Id", keyValue);

                        if (tx_AdjustmentIn.ApprovalStatus == "Approved")
                        {
                            PostSAP(userId, adjustmentInModel.Id);
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

        public void Reject(int userId, long Id, string approvalMessages)
        {
            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = Id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "before", "Tx_AdjustmentIn", "reject", "Id", keyValue);

                        Tx_AdjustmentIn tx_AdjustmentIn = CONTEXT.Tx_AdjustmentIn.Find(Id);
                        if (tx_AdjustmentIn != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            //tx_AdjustmentIn.Status = "";
                            tx_AdjustmentIn.ApprovalMessages = approvalMessages;
                            tx_AdjustmentIn.ModifiedDate = dtModified;
                            tx_AdjustmentIn.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", CONTEXT, "after", "Tx_AdjustmentIn", "reject", "Id", keyValue);


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


        public AdjustmentInItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;

            AdjustmentInItemTagView___ model = new AdjustmentInItemTagView___();
            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName""
                                FROM ""Tx_AdjustmentIn_Item"" T0   
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<AdjustmentInItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @" 
                    SELECT 
                        ROW_NUMBER() OVER (ORDER BY T0.""DetDetId"") AS ""RowNo"", 
                        T0.*,
                        CASE 
                        WHEN T1.""Status"" = 'Draft' THEN 
                            CASE 
                            WHEN duplicate_tag > 1 THEN 'Duplicate'  -- Mark duplicates as Invalid
                            WHEN COALESCE(T3.""TagId"", '') = '' THEN 'New Entry'
                            ELSE 'Invalid' 
                            END 
                        ELSE T0.""Status"" 
                        END AS ""Information""
                    FROM (
                        SELECT *,
                        ROW_NUMBER() OVER (PARTITION BY ""TagId"" ORDER BY ""DetDetId"") AS duplicate_tag
                        FROM ""Tx_AdjustmentIn_Item_Tag""
                        WHERE ""Id""=:p0 AND ""DetId"" = :p1 
                    ) T0  
                    INNER JOIN ""Tx_AdjustmentIn"" T1 ON T0.""Id"" = T1.""Id""  
                    LEFT JOIN ""Tm_Item_Warehouse_Tag"" T3 ON T0.""TagId"" = T3.""TagId""
                    ORDER BY T0.""DetDetId""
                ";

                model.AdjustmentIn_Item_TagModel___ = CONTEXT.Database.SqlQuery<AdjustmentIn_Item_TagModel>(sql, id, detId).ToList();
            }

            return model;
        }
        #region Attachment
        public AdjustmentIn_AttachmentModel GetAdjustmentIn_Attachments_GetById(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAdjustmentIn_Attachments_GetById(CONTEXT, id);
            }
        }

        public AdjustmentIn_AttachmentModel GetAdjustmentIn_Attachments_GetById(HANA_APP CONTEXT, long id = 0)
        {
            AdjustmentIn_AttachmentModel model = null;
            if (id != 0)
            { 
                string ssql = "SELECT TOP 1 T0.*  "
                                + " FROM \"Tx_AdjustmentIn_Attachment\" T0 "
                                + " WHERE T0.\"DetId\" = :p0 ";

            model = CONTEXT.Database.SqlQuery<AdjustmentIn_AttachmentModel>(ssql, id).Single();
            }

            return model;
        }


        public long Detail_Add(List<AdjustmentIn_AttachmentModel> ListModel)
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
                            Tx_AdjustmentIn_Attachment tx_AdjustmentIn_Attachment = new Tx_AdjustmentIn_Attachment();
                            var model = ListModel[i];

                            CopyProperty.CopyProperties(model, tx_AdjustmentIn_Attachment, false);
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_AdjustmentIn_Attachment.CreatedDate = dtModified;
                            tx_AdjustmentIn_Attachment.CreatedUser = model._UserId;
                            tx_AdjustmentIn_Attachment.ModifiedDate = dtModified;
                            tx_AdjustmentIn_Attachment.ModifiedUser = model._UserId;

                            CONTEXT.Tx_AdjustmentIn_Attachment.Add(tx_AdjustmentIn_Attachment);
                            CONTEXT.SaveChanges();
                            string keyValue = tx_AdjustmentIn_Attachment.Id.ToString();
                            SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentIn", CONTEXT, "after", "Tx_AdjustmentIn", "add", "Id", keyValue);
                        }

                        CONTEXT_TRANS.Commit();
                    }
                }

            }

            return Id;

        }

        public void Attachment_Delete(AdjustmentIn_AttachmentModel model)
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
                                Tx_AdjustmentIn_Attachment tx_AdjustmentInAttachment = CONTEXT.Tx_AdjustmentIn_Attachment.Find(model.DetId);

                                SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentIn", CONTEXT, "before", "Tx_AdjustmentIn", "delete", "Id", keyValue);

                                if (tx_AdjustmentInAttachment != null)
                                {
                                    CONTEXT.Tx_AdjustmentIn_Attachment.Remove(tx_AdjustmentInAttachment);
                                    CONTEXT.SaveChanges();
                                }

                                SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentIn", CONTEXT, "after", "Tx_AdjustmentIn", "delete", "Id", keyValue);
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

        public List<AdjustmentIn_ItemModel> GetAdjustmentIn_Items(long id)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return AdjustmentIn_Details(id);
            }
        }

        #endregion
    }

    #endregion Services
}
