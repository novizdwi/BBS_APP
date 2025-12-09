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

namespace Models.Transaction.Inventory
{
    #region Models

    public class TransferSummaryInModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int? CreatedUser { get; set; }

        public int? ModifiedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string UserName { get; set; }

        public long Id { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        public DateTime? PostingDate { get; set; }

        public long? DocEntry { get; set; }

        public string DocNum { get; set; }

        public long? BaseId { get; set; }

        public string BaseTransNo { get; set; }

        public string BaseType { get; set; }

        public string VendorCode { get; set; }

        public string VendorName { get; set; }

        [Required(ErrorMessage = "required")]
        public string FromWhsCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string FromWhsName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ToWhsCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string ToWhsName { get; set; }

        public string TransitWhsCode { get; set; }

        public string TransitWhsName { get; set; }

        public string Address { get; set; }

        public string RefNo { get; set; }

        public string ScanDeviceId { get; set; }

        public string Status { get; set; }

        public string IsAfterPosted { get; set; }

        public string Comments { get; set; }

        public string CancelReason { get; set; }

        public List<TransferSummaryIn_DetailModel> ListDetails_ = new List<TransferSummaryIn_DetailModel>();

        public List<TransferSummaryIn_RefModel> ListRef_ = new List<TransferSummaryIn_RefModel>();

        public TransferSummaryIn_Detail Details_ { get; set; }
    }
    public class TransferSummaryIn_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<TransferSummaryIn_DetailModel> insertedRowValues { get; set; }
        public List<TransferSummaryIn_DetailModel> modifiedRowValues { get; set; }
    }

    public class TransferSummaryIn_RefModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public long? BaseId { get; set; }

        public string BaseNo { get; set; }

        public DateTime? BaseCreatedDate { get; set; }

        public string ScanDeviceId { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }

        public string BaseCreatedUser_ { get; set; }
    }

    public class TransferSummaryIn_DetailModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long? Id { get; set; }

        public long? DetId { get; set; }
        
        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string FreeText { get; set; }

        public string FromWhsCode { get; set; }

        public string FromWhsName { get; set; }

        public string ToWhsCode { get; set; }

        public string ToWhsName { get; set; }

        public string TransitWhsCode { get; set; }

        public string TransitWhsName { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? QuantityOpen_ { get; set; }

        public decimal? QuantityScan { get; set; }

        public decimal? QuantityValid { get; set; }

        public decimal? QuantityPosted { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public long? DocEntry { get; set; }

        public long? LineNum { get; set; }

        public long? BaseId { get; set; }

        public int? BaseDetId { get; set; }

        public string LineStatus { get; set; }
    }

    public class TransferSummaryInItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public List<TransferSummaryInItemTagModel> TransferSummaryInItemTagModel___ { get; set; }
    }

    public class TransferSummaryInItemTagModel
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
    }

    public class TransferSummaryInAddResultModel
    {
        public string DocEntry { get; set; }
        public Dictionary<long, int> LineMapping { get; set; } // LineId -> LineNum
    }

    #endregion

    #region Services

    public class TransferSummaryInService
    {

        public TransferSummaryInModel GetNewModel(int userId)
        {
            TransferSummaryInModel model = new TransferSummaryInModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            return model;
        }

        public TransferSummaryInModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public TransferSummaryInModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            TransferSummaryInModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, T1.""FirstName"" AS ""UserName"" 
                            FROM ""Tx_TransferSummaryIn"" T0
                            LEFT JOIN ""Tm_User"" T1 ON T0.""ModifiedUser"" = T1.""Id""
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<TransferSummaryInModel>(ssql, id).Single();

                model.ListRef_ = this.TransferSummaryIn_Refs(CONTEXT, id);
                model.ListDetails_ = this.TransferSummaryIn_Details(CONTEXT, id);
            }

            return model;
        }

        public List<TransferSummaryIn_RefModel> TransferSummaryIn_Refs(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return TransferSummaryIn_Refs(CONTEXT, id);
            }

        }

        public List<TransferSummaryIn_RefModel> TransferSummaryIn_Refs(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.*, T1.""TransDate"", T2.""FirstName"" AS ""BaseCreatedUser_""
                FROM ""Tx_TransferSummaryIn_Ref"" T0
                INNER JOIN ""Tx_TransferIn"" T1 ON T0.""BaseId"" = T1.""Id""
                LEFT JOIN ""Tm_User"" T2 ON T0.""BaseCreatedUser"" = T2.""Id""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var result = CONTEXT.Database.SqlQuery<TransferSummaryIn_RefModel>(ssql, id).ToList();
            return result;
        }

        public List<TransferSummaryIn_DetailModel> TransferSummaryIn_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return TransferSummaryIn_Details(CONTEXT, id);
            }

        }

        public List<TransferSummaryIn_DetailModel> TransferSummaryIn_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT T0.* , T1.""QuantityToReceipt"" AS ""QuantityOpen_"" 
                FROM ""Tx_TransferSummaryIn_Item"" T0
                LEFT JOIN ""Tx_TransferSummaryOut_Item"" T1 ON T0.""BaseDetId"" = T1.""DetId""
                WHERE T0.""Id"" =:p0
                ORDER BY T0.""DetId"" ASC
            ";
            var goodsReceiptPO = CONTEXT.Database.SqlQuery<TransferSummaryIn_DetailModel>(ssql, id).ToList();
            return goodsReceiptPO;
        }

        public TransferSummaryInModel NavFirst(int userId)
        {
            TransferSummaryInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferSummaryIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferSummaryIn\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public TransferSummaryInModel NavPrevious(int userId, long id = 0)
        {
            TransferSummaryInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferSummaryIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferSummaryIn\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public TransferSummaryInModel NavNext(int userId, long id = 0)
        {
            TransferSummaryInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferSummaryIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferSummaryIn\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public TransferSummaryInModel NavLast(int userId)
        {
            TransferSummaryInModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TransferSummaryIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TransferSummaryIn\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public bool RefreshItem(int userId, long id)
        {
            bool ret = true;
            if (id != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferSummaryIn_AddItemDetail\"(:p0,:p1,'Refresh')", userId, id);
                }
            }
            return ret;
        }

        public long Add(TransferSummaryInModel model)
        {
            long Id = 0;

            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            Tx_TransferSummaryIn Tx_TransferSummaryIn = new Tx_TransferSummaryIn();
                            CopyProperty.CopyProperties(model, Tx_TransferSummaryIn, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_TransferSummaryIn.TransType = "TransferSummaryIn";
                            Tx_TransferSummaryIn.CreatedDate = dtModified;
                            Tx_TransferSummaryIn.CreatedUser = model._UserId;
                            Tx_TransferSummaryIn.ModifiedDate = dtModified;
                            Tx_TransferSummaryIn.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'TransferSummaryIn','" + dateX + "','') ").SingleOrDefault();
                            Tx_TransferSummaryIn.TransNo = transNo;

                            CONTEXT.Tx_TransferSummaryIn.Add(Tx_TransferSummaryIn);
                            CONTEXT.SaveChanges();
                            Id = Tx_TransferSummaryIn.Id;

                            String keyValue;
                            keyValue = Tx_TransferSummaryIn.Id.ToString();
                            
                            SpNotif.SpSysControllerTransNotif(model._UserId, "TransferSummaryIn", CONTEXT, "after", "TransferSummaryIn", "add", "Id", keyValue);

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferSummaryIn_AddItemDetail\"(:p0,:p1,'Add')", model._UserId, Id);

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

            return Id;

        }

        public void Update(TransferSummaryInModel model)
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "TransferSummaryIn", CONTEXT, "before", "TransferSummaryIn", "update", "Id", keyValue);


                                Tx_TransferSummaryIn Tx_TransferSummaryIn = CONTEXT.Tx_TransferSummaryIn.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_TransferSummaryIn.ModifiedDate = dtModified;
                                Tx_TransferSummaryIn.ModifiedUser = model._UserId;

                                if (Tx_TransferSummaryIn != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_TransferSummaryIn, false, exceptColumns);
                                    Tx_TransferSummaryIn.ModifiedDate = dtModified;
                                    Tx_TransferSummaryIn.ModifiedUser = model._UserId;

                                    //if (model.StartDate != null)
                                    //{
                                    //    Tx_TransferSummaryIn.Status2 = "On Progress";
                                    //}
                                    //else
                                    //{
                                    //    Tx_TransferSummaryIn.Status2 = "Open";
                                    //}

                                    //if (model.EndDate != null)
                                    //{
                                    //    Tx_TransferSummaryIn.Status2 = "Close";
                                    //}
                                    CONTEXT.SaveChanges();

                                    //if (model.Details_ != null)
                                    //{
                                    //    if (model.Details_.insertedRowValues != null)
                                    //    {
                                    //        foreach (var detail in model.Details_.insertedRowValues)
                                    //        {
                                    //            Detail_Add(CONTEXT, detail, model.Id, model._UserId);
                                    //        }
                                    //    }

                                    if (model.Details_.modifiedRowValues != null)
                                    {
                                        foreach (var detail in model.Details_.modifiedRowValues)
                                        {
                                            Detail_Update(CONTEXT, detail, model._UserId);
                                        }
                                    }

                                    //    if (model.Details_.deletedRowKeys != null)
                                    //    {
                                    //        foreach (var detId in model.Details_.deletedRowKeys)
                                    //        {
                                    //            TransferSummaryIn_DetailModel detailModel = new TransferSummaryIn_DetailModel();
                                    //            detailModel.DetId = detId;
                                    //            Detail_Delete(CONTEXT, detailModel);
                                    //        }
                                    //    }
                                    //}
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "TransferSummaryIn", CONTEXT, "after", "TransferSummaryIn", "update", "Id", keyValue);
                                    
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

        //public long Detail_Add(HANA_APP CONTEXT, TransferSummaryIn_DetailModel model, long Id, int UserId)
        //{
        //    long DetId = 0;

        //    if (model != null)
        //    {

        //        Tx_TransferSummaryIn_Item Tx_TransferSummaryIn_Item = new Tx_TransferSummaryIn_Item();

        //        CopyProperty.CopyProperties(model, Tx_TransferSummaryIn_Item, false);

        //        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
        //        Tx_TransferSummaryIn_Item.Id = Id;
        //        Tx_TransferSummaryIn_Item.CreatedDate = dtModified;
        //        Tx_TransferSummaryIn_Item.CreatedUser = UserId;
        //        Tx_TransferSummaryIn_Item.ModifiedDate = dtModified;
        //        Tx_TransferSummaryIn_Item.ModifiedUser = UserId;
        //        if (model.StartDate != null && model.EndDate == null)
        //        {
        //            Tx_TransferSummaryIn_Item.Status = "On Progress";
        //        }
        //        else if (model.StartDate != null && model.EndDate != null)
        //        {
        //            Tx_TransferSummaryIn_Item.Status = "Close";
        //        }
        //        else
        //        {
        //            Tx_TransferSummaryIn_Item.Status = "Open";
        //        }

        //        CONTEXT.Tx_TransferSummaryIn_Item.Add(Tx_TransferSummaryIn_Item);
        //        CONTEXT.SaveChanges();
        //        DetId = Tx_TransferSummaryIn_Item.DetId;

        //    }

        //    return DetId;

        //}

        public void Detail_Update(HANA_APP CONTEXT, TransferSummaryIn_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_TransferSummaryIn_Item Tx_TransferSummaryIn_Item = CONTEXT.Tx_TransferSummaryIn_Item.Find(model.DetId);

                if (Tx_TransferSummaryIn_Item != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id", "QuantityOpen" };
                    CopyProperty.CopyProperties(model, Tx_TransferSummaryIn_Item, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_TransferSummaryIn_Item.ModifiedDate = dtModified;
                    Tx_TransferSummaryIn_Item.ModifiedUser = UserId;
                    CONTEXT.SaveChanges();

                }


            }

        }

        //public void Detail_Delete(HANA_APP CONTEXT, TransferSummaryIn_DetailModel model)
        //{
        //    if (model.DetId != null)
        //    {
        //        if (model.DetId != 0)
        //        {

        //            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_TransferSummaryIn_Item\"  WHERE \"DetId\"=:p0", model.DetId);

        //            CONTEXT.SaveChanges();


        //        }
        //    }

        //}
        public void Post(int userId, long id)
        {
            try
            {
                PostSAP(userId, id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PostSAP(int userId, long id)
        {
            SAPbobsCOM.Company oCompany = null;

            TransferSummaryInModel syncTransferSummaryIn = GetById(userId, id);
            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferSummaryIn_UpdateItem\"(:p0,:p1, 'before')", userId, id);
                        CONTEXT.SaveChanges();

                        //oCompany.StartTransaction();
                        oCompany = SAPCachedCompany.GetCompany();

                        String keyValue;
                        keyValue = id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "TransferSummaryIn", CONTEXT, "before", "Tx_TransferSummaryIn", "post", "Id", keyValue);

                        Tx_TransferSummaryIn tx_TransferSummaryIn = CONTEXT.Tx_TransferSummaryIn.Find(id);
                        if (syncTransferSummaryIn.ListDetails_.All(q => !q.QuantityPosted.HasValue || q.QuantityPosted == 0))
                        {
                            throw new Exception("No record posted");
                        }

                        if (tx_TransferSummaryIn != null)
                        {
                            string docEntry_ = AddTransferSummaryIn(oCompany, userId, id, syncTransferSummaryIn);

                            if (!string.IsNullOrEmpty(docEntry_))
                            {
                                string ssql = @"SELECT ""DocNum"" 
                                            FROM """ + DbProvider.dbSap_Name + @""".""OWTR"" T0
                                            WHERE T0.""DocEntry"" = " + docEntry_ + @" 
                                            ";

                                string docNum = CONTEXT.Database.SqlQuery<string>(ssql, id).FirstOrDefault();

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                
                                tx_TransferSummaryIn.DocEntry = Convert.ToInt64(docEntry_);
                                tx_TransferSummaryIn.DocNum = docNum;
                                tx_TransferSummaryIn.PostingDate = dtModified;

                                tx_TransferSummaryIn.Status = "Posted";
                                tx_TransferSummaryIn.IsAfterPosted = "Y";
                                tx_TransferSummaryIn.ModifiedDate = dtModified;
                                tx_TransferSummaryIn.ModifiedUser = userId;

                                CONTEXT.SaveChanges();

                                CONTEXT.Database.ExecuteSqlCommand("CALL \"SpItem_TransferItemTag\"(:p0,:p1, 'TransferSummaryIn', 'A')", userId, id);
                                CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferSummaryIn_UpdateItem\"(:p0,:p1, 'after')", userId, id);
                                SpNotif.SpSysControllerTransNotif(userId, "TransferSummaryIn", CONTEXT, "after", "Tx_TransferSummaryIn", "post", "Id", keyValue);

                                CONTEXT_TRANS.Commit();
                            }
                        }

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
                    finally
                    {
                        SapCompany.CleanUpGCCollect();
                        SAPCachedCompany.Release(oCompany);
                    }
                }
            }

        }

        private string AddTransferSummaryIn(Company oCompany, int userId, long id, TransferSummaryInModel model)
        {
            string result;

            int nErr;
            string errMsg;
            string newEntry_ = string.Empty;
            //SAPbobsCOM.Recordset rsDetailSO = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            SAPbobsCOM.StockTransfer oInventoryTransfer = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

            oInventoryTransfer.UserFields.Fields.Item("U_IDU_WebId").Value = model.Id.ToString();
            oInventoryTransfer.UserFields.Fields.Item("U_IDU_WebTransNo").Value = model.TransNo;

            oInventoryTransfer.DocDate = DateTime.Now;
            oInventoryTransfer.DueDate = DateTime.Now;
            oInventoryTransfer.TaxDate = DateTime.Now;

            oInventoryTransfer.FromWarehouse = model.TransitWhsCode;
            oInventoryTransfer.ToWarehouse = model.ToWhsCode;

            //oDocument.CardCode = model.VendorCode;
            //oDocument.CardName = model.VendorCode;

            if (model.Comments != null)
            {
                oInventoryTransfer.Comments = model.Comments;
            }

            if (model.Address != null)
            {
                oInventoryTransfer.Address = model.Address;
            } 

            if (model.ListDetails_.Count > 0)
            {
                foreach (var item in model.ListDetails_.Where(x => x.QuantityPosted.HasValue && x.QuantityPosted > 0))
                {
                    oInventoryTransfer.Lines.ItemCode = item.ItemCode;
                    oInventoryTransfer.Lines.FromWarehouseCode = model.TransitWhsCode;
                    oInventoryTransfer.Lines.WarehouseCode = model.ToWhsCode;
                    oInventoryTransfer.Lines.Quantity = (double)item.QuantityPosted;

                    if (item.UomEntry != null)
                    {
                        oInventoryTransfer.Lines.UoMEntry = Convert.ToInt32(item.UomEntry);
                    }

                    oInventoryTransfer.Lines.Add();
                }
            }

            if (oInventoryTransfer.Add() != 0)
            {
                nErr = oCompany.GetLastErrorCode();
                errMsg = oCompany.GetLastErrorDescription();

                SapCompany.CleanUp(oInventoryTransfer);

                throw new Exception("[VALIDATION] - Add Transfer Summary Out : " + nErr.ToString() + "|" + errMsg);
            }
            result = oCompany.GetNewObjectKey(); 

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

                        SpNotif.SpSysControllerTransNotif(userId, "TransferSummaryIn", CONTEXT, "before", "Tx_TransferSummaryIn", "cancel", "Id", keyValue);

                        Tx_TransferSummaryIn tx_TransferSummaryIn = CONTEXT.Tx_TransferSummaryIn.Find(Id);
                        if (tx_TransferSummaryIn != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TransferSummaryIn.Status = "Cancel";
                            tx_TransferSummaryIn.CancelReason = cancelReason;
                            tx_TransferSummaryIn.ModifiedDate = dtModified;
                            tx_TransferSummaryIn.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "TransferSummaryIn", CONTEXT, "after", "Tx_TransferSummaryIn", "cancel", "Id", keyValue);

                        //CONTEXT.Database.ExecuteSqlCommand("CALL \"SpTransferSummaryIn_UpdateStatus\"(:p0,:p1,'cancel')", userId, Id);

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


        public TransferSummaryInItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;
            TransferSummaryInItemTagView___ model = new TransferSummaryInItemTagView___();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName""
                                FROM ""Tx_TransferSummaryIn_Item"" T0   
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<TransferSummaryInItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_TransferSummaryIn_Item_Tag"" T0   
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model.TransferSummaryInItemTagModel___ = CONTEXT.Database.SqlQuery<TransferSummaryInItemTagModel>(sql, id, detId).ToList();
            }

            return model;
        }

    }


    #endregion

}