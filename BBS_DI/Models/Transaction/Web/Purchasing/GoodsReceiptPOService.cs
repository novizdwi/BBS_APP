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

namespace Models.Transaction.Web.Purchasing
{
    #region Models

    public class GoodsReceiptPOModel
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

        public long? BaseId { get; set; }

        public long? BaseDetId { get; set; }

        public string TransNo { get; set; }

        public DateTime? TransDate { get; set; }

        [Required(ErrorMessage = "required")]
        public string VendorCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string VendorName { get; set; }

        public string Address { get; set; }

        public long? DocEntry { get; set; }

        public string DocNum { get; set; }

        public long? BaseEntry { get; set; }

        public string BaseDocNum { get; set; }

        public string RefNo { get; set; }

        public string ScanDeviceId { get; set; }

        public string Status { get; set; }

        public string IsAfterPosted { get; set; }

        public string Comments { get; set; }

        public string CancelReason { get; set; }

        public List<GoodsReceiptPO_DetailModel> ListDetails_ = new List<GoodsReceiptPO_DetailModel>();

        public GoodsReceiptPO_Detail Details_ { get; set; }
    }
    public class GoodsReceiptPO_Detail
    {
        public List<long> deletedRowKeys { get; set; }
        public List<GoodsReceiptPO_DetailModel> insertedRowValues { get; set; }
        public List<GoodsReceiptPO_DetailModel> modifiedRowValues { get; set; }
    }

    public class GoodsReceiptPO_DetailModel
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

        public string Bagian { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string FreeText { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? QuantityOpen { get; set; }

        public decimal? QuantityScan { get; set; }

        public int? UomEntry { get; set; }

        public string Uom { get; set; }

        public long? DocEntry { get; set; }

        public long? LineNum { get; set; }

        public long? BaseEntry { get; set; }

        public int? BaseLine { get; set; }

        public string LineStatus { get; set; }
    }

    public class GoodsReceiptPOItemTagView___
    {
        public long Id { get; set; }

        public long DetId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public List<GoodsReceiptPOItemTagModel> GoodsReceiptPOItemTagModel___ { get; set; }
    }

    public class GoodsReceiptPOItemTagModel
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

    public class GRPOAddResultModel
    {
        public string DocEntry { get; set; }
        public Dictionary<long, int> LineMapping { get; set; } // LineId -> LineNum
    }

    #endregion

    #region Services

    public class GoodsReceiptPOService
    {

        public GoodsReceiptPOModel GetNewModel(int userId)
        {
            GoodsReceiptPOModel model = new GoodsReceiptPOModel();
            model.Status = "Draft";
            model.TransDate = DateTime.Now;
            return model;
        }

        public GoodsReceiptPOModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public GoodsReceiptPOModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            GoodsReceiptPOModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, T1.""FirstName"" AS ""UserName"" 
                            FROM ""Tx_GoodsReceiptPO"" T0
                            LEFT JOIN ""Tm_User"" T1 ON T0.""ModifiedUser"" = T1.""Id""
                            WHERE T0.""Id"" = :p0 
                            ORDER BY T0.""Id"" ASC
                ";

                model = CONTEXT.Database.SqlQuery<GoodsReceiptPOModel>(ssql, id).Single();

                model.ListDetails_ = this.GoodsReceiptPO_Details(CONTEXT, id);
            }

            return model;
        }
        public List<GoodsReceiptPO_DetailModel> GoodsReceiptPO_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GoodsReceiptPO_Details(CONTEXT, id);
            }

        }

        public List<GoodsReceiptPO_DetailModel> GoodsReceiptPO_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT * 
                FROM ""Tx_GoodsReceiptPO_Item"" 
                WHERE ""Id"" =:p0
                ORDER BY ""DetId"" ASC
            ";
            var goodsReceiptPO = CONTEXT.Database.SqlQuery<GoodsReceiptPO_DetailModel>(ssql, id).ToList();
            return goodsReceiptPO;
        }

        public GoodsReceiptPOModel NavFirst(int userId)
        {
            GoodsReceiptPOModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodsReceiptPO");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodsReceiptPO\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public GoodsReceiptPOModel NavPrevious(int userId, long id = 0)
        {
            GoodsReceiptPOModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodsReceiptPO");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodsReceiptPO\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public GoodsReceiptPOModel NavNext(int userId, long id = 0)
        {
            GoodsReceiptPOModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodsReceiptPO");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodsReceiptPO\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public GoodsReceiptPOModel NavLast(int userId)
        {
            GoodsReceiptPOModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodsReceiptPO");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodsReceiptPO\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

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
                    CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPO_AddItemDetail\"(:p0,:p1,'Refresh')", userId, id);
                }
            }
            return ret;
        }

        public long Add(GoodsReceiptPOModel model)
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
                            Tx_GoodsReceiptPO Tx_GoodsReceiptPO = new Tx_GoodsReceiptPO();
                            CopyProperty.CopyProperties(model, Tx_GoodsReceiptPO, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_GoodsReceiptPO.TransType = "GoodsReceiptPO";
                            Tx_GoodsReceiptPO.CreatedDate = dtModified;
                            Tx_GoodsReceiptPO.CreatedUser = model._UserId;
                            Tx_GoodsReceiptPO.ModifiedDate = dtModified;
                            Tx_GoodsReceiptPO.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'GoodsReceiptPO','" + dateX + "','') ").SingleOrDefault();
                            Tx_GoodsReceiptPO.TransNo = transNo;

                            CONTEXT.Tx_GoodsReceiptPO.Add(Tx_GoodsReceiptPO);
                            CONTEXT.SaveChanges();
                            Id = Tx_GoodsReceiptPO.Id;

                            String keyValue;
                            keyValue = Tx_GoodsReceiptPO.Id.ToString();
                            
                            SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPO", CONTEXT, "after", "GoodsReceiptPO", "add", "Id", keyValue);

                            CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPO_AddItemDetail\"(:p0,:p1,'Add')", model._UserId, Id);

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

        public void Update(GoodsReceiptPOModel model)
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
                                
                                SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPO", CONTEXT, "before", "GoodsReceiptPO", "update", "Id", keyValue);


                                Tx_GoodsReceiptPO Tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_GoodsReceiptPO.ModifiedDate = dtModified;
                                Tx_GoodsReceiptPO.ModifiedUser = model._UserId;

                                if (Tx_GoodsReceiptPO != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_GoodsReceiptPO, false, exceptColumns);
                                    Tx_GoodsReceiptPO.ModifiedDate = dtModified;
                                    Tx_GoodsReceiptPO.ModifiedUser = model._UserId;

                                    //if (model.StartDate != null)
                                    //{
                                    //    Tx_GoodsReceiptPO.Status2 = "On Progress";
                                    //}
                                    //else
                                    //{
                                    //    Tx_GoodsReceiptPO.Status2 = "Open";
                                    //}

                                    //if (model.EndDate != null)
                                    //{
                                    //    Tx_GoodsReceiptPO.Status2 = "Close";
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

                                    //    if (model.Details_.modifiedRowValues != null)
                                    //    {
                                    //        foreach (var detail in model.Details_.modifiedRowValues)
                                    //        {
                                    //            Detail_Update(CONTEXT, detail, model._UserId);
                                    //        }
                                    //    }

                                    //    if (model.Details_.deletedRowKeys != null)
                                    //    {
                                    //        foreach (var detId in model.Details_.deletedRowKeys)
                                    //        {
                                    //            GoodsReceiptPO_DetailModel detailModel = new GoodsReceiptPO_DetailModel();
                                    //            detailModel.DetId = detId;
                                    //            Detail_Delete(CONTEXT, detailModel);
                                    //        }
                                    //    }
                                    //}
                                    SpNotif.SpSysControllerTransNotif(model._UserId, "GoodsReceiptPO", CONTEXT, "after", "GoodsReceiptPO", "update", "Id", keyValue);
                                    
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

        //public long Detail_Add(HANA_APP CONTEXT, GoodsReceiptPO_DetailModel model, long Id, int UserId)
        //{
        //    long DetId = 0;

        //    if (model != null)
        //    {

        //        Tx_GoodsReceiptPO_Item Tx_GoodsReceiptPO_Item = new Tx_GoodsReceiptPO_Item();

        //        CopyProperty.CopyProperties(model, Tx_GoodsReceiptPO_Item, false);

        //        DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
        //        Tx_GoodsReceiptPO_Item.Id = Id;
        //        Tx_GoodsReceiptPO_Item.CreatedDate = dtModified;
        //        Tx_GoodsReceiptPO_Item.CreatedUser = UserId;
        //        Tx_GoodsReceiptPO_Item.ModifiedDate = dtModified;
        //        Tx_GoodsReceiptPO_Item.ModifiedUser = UserId;
        //        if (model.StartDate != null && model.EndDate == null)
        //        {
        //            Tx_GoodsReceiptPO_Item.Status = "On Progress";
        //        }
        //        else if (model.StartDate != null && model.EndDate != null)
        //        {
        //            Tx_GoodsReceiptPO_Item.Status = "Close";
        //        }
        //        else
        //        {
        //            Tx_GoodsReceiptPO_Item.Status = "Open";
        //        }

        //        CONTEXT.Tx_GoodsReceiptPO_Item.Add(Tx_GoodsReceiptPO_Item);
        //        CONTEXT.SaveChanges();
        //        DetId = Tx_GoodsReceiptPO_Item.DetId;

        //    }

        //    return DetId;

        //}

        //public void Detail_Update(HANA_APP CONTEXT, GoodsReceiptPO_DetailModel model, int UserId)
        //{
        //    if (model != null)
        //    {

        //        Tx_GoodsReceiptPO_Item Tx_GoodsReceiptPO_Item = CONTEXT.Tx_GoodsReceiptPO_Item.Find(model.DetId);

        //        if (Tx_GoodsReceiptPO_Item != null)
        //        {
        //            var exceptColumns = new string[] { "DetId", "Id" };
        //            CopyProperty.CopyProperties(model, Tx_GoodsReceiptPO_Item, false, exceptColumns);


        //            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

        //            Tx_GoodsReceiptPO_Item.ModifiedDate = dtModified;
        //            Tx_GoodsReceiptPO_Item.ModifiedUser = UserId;
        //            if (model.StartDate != null && model.EndDate == null)
        //            {
        //                Tx_GoodsReceiptPO_Item.Status = "On Progress";
        //            }
        //            else if (model.StartDate != null && model.EndDate != null)
        //            {
        //                Tx_GoodsReceiptPO_Item.Status = "Close";
        //            }
        //            else
        //            {
        //                Tx_GoodsReceiptPO_Item.Status = "Open";
        //            }
        //            CONTEXT.SaveChanges();

        //        }


        //    }

        //}

        //public void Detail_Delete(HANA_APP CONTEXT, GoodsReceiptPO_DetailModel model)
        //{
        //    if (model.DetId != null)
        //    {
        //        if (model.DetId != 0)
        //        {

        //            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_GoodsReceiptPO_Item\"  WHERE \"DetId\"=:p0", model.DetId);

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

            GoodsReceiptPOModel syncGRPO = GetById(userId, id);
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


                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPO", CONTEXT, "before", "Tx_GoodsReceiptPO", "post", "Id", keyValue);

                        Tx_GoodsReceiptPO tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(id);
                        if (tx_GoodsReceiptPO != null)
                        {
                            GRPOAddResultModel GRPOResult = AddGoodsReceiptPO(oCompany, userId, id, syncGRPO);
                            string ssql = @"SELECT ""DocNum"" 
                                FROM """+ DbProvider.dbSap_Name +@""".""OPDN"" T0
                                WHERE T0.""DocEntry"" = "+ GRPOResult.DocEntry + @" 
                             ";

                            string docNum = CONTEXT.Database.SqlQuery<string>(ssql, id).FirstOrDefault();

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            tx_GoodsReceiptPO.PostingDate = dtModified;
                            tx_GoodsReceiptPO.DocEntry = Convert.ToInt64(GRPOResult.DocEntry) ;
                            tx_GoodsReceiptPO.DocNum = docNum;

                            tx_GoodsReceiptPO.Status = "Posted";
                            tx_GoodsReceiptPO.IsAfterPosted = "Y";
                            tx_GoodsReceiptPO.ModifiedDate = dtModified;
                            tx_GoodsReceiptPO.ModifiedUser = userId;

                            CONTEXT.SaveChanges();

                            var caseStatements = string.Join(" ",
                            GRPOResult.LineMapping.Select(kv => $"WHEN T0.\"DetId\" = {kv.Key} THEN {kv.Value}"));

                            var whereIn = string.Join(", ",GRPOResult.LineMapping.Keys);

                            string sqlLine = $@"
                                UPDATE ""Tx_GoodsReceiptPO_Item"" T0
                                SET ""LineNum"" = CASE {caseStatements} END
                                WHERE T0.""DetId"" IN ({whereIn})";
                            CONTEXT.Database.ExecuteSqlCommand(sqlLine);
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPO", CONTEXT, "after", "Tx_GoodsReceiptPO", "post", "Id", keyValue);
                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPO_UpdatePOStatus\"(:p0,:p1,'post')", userId, id);

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
                    finally
                    {
                        SapCompany.CleanUpGCCollect();
                        SAPCachedCompany.Release(oCompany);
                    }
                }
            }

        }


        private GRPOAddResultModel AddGoodsReceiptPO(Company oCompany, int userId, long id, GoodsReceiptPOModel model)
        {
            GRPOAddResultModel result = new GRPOAddResultModel();

            int nErr;
            string errMsg;
            string newEntry_ = string.Empty;
            //SAPbobsCOM.Recordset rsDetailSO = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            SAPbobsCOM.Documents oDocument = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);

            oDocument.DocDate = (DateTime)model.TransDate;
            oDocument.DocDueDate = (DateTime)model.TransDate;
            oDocument.TaxDate = (DateTime)model.TransDate;

            oDocument.CardCode = model.VendorCode;
            oDocument.CardName = model.VendorCode;

            if (model.RefNo != null)
            {
                oDocument.NumAtCard = model.RefNo;
            }

            if (model.Comments != null)
            {
                oDocument.Comments = model.Comments;
            }

            if (model.Address != null)
            {
                oDocument.Address = model.Address;
            }

            var insertedLineIds = new Dictionary<long, int>();
            int i = 0;
            if (model.ListDetails_.Count > 0)
            {
                foreach (var item in model.ListDetails_)
                {
                    oDocument.Lines.BaseType = 22;
                    oDocument.Lines.BaseEntry = Convert.ToInt32(item.BaseEntry);
                    oDocument.Lines.BaseLine = Convert.ToInt32(item.BaseLine);

                    oDocument.Lines.ItemCode = item.ItemCode;
                    oDocument.Lines.WarehouseCode = item.WhsCode;
                    oDocument.Lines.Quantity = (double)item.Quantity;

                    if (item.UomEntry != null)
                    {
                        oDocument.Lines.UoMEntry = Convert.ToInt32(item.UomEntry);
                    }

                    if (item.FreeText != null)
                    {
                        oDocument.Lines.FreeText = item.FreeText;
                    }

                    oDocument.Lines.Add();
                    insertedLineIds.Add(Convert.ToInt64(item.DetId), i);
                    i += 1;
                }
            }

            if (oDocument.Add() != 0)
            {
                nErr = oCompany.GetLastErrorCode();
                errMsg = oCompany.GetLastErrorDescription();

                SapCompany.CleanUp(oDocument);

                throw new Exception("[VALIDATION] - Add Goods Receipt PO : " + nErr.ToString() + "|" + errMsg);
            }
            result.DocEntry = oCompany.GetNewObjectKey();
            result.LineMapping = insertedLineIds;

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

                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPO", CONTEXT, "before", "Tx_GoodsReceiptPO", "cancel", "Id", keyValue);

                        Tx_GoodsReceiptPO tx_GoodsReceiptPO = CONTEXT.Tx_GoodsReceiptPO.Find(Id);
                        if (tx_GoodsReceiptPO != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_GoodsReceiptPO.Status = "Cancel";
                            tx_GoodsReceiptPO.CancelReason = cancelReason;
                            tx_GoodsReceiptPO.ModifiedDate = dtModified;
                            tx_GoodsReceiptPO.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPO", CONTEXT, "after", "Tx_GoodsReceiptPO", "cancel", "Id", keyValue);

                        CONTEXT.Database.ExecuteSqlCommand("CALL \"SpGoodsReceiptPO_UpdatePOStatus\"(:p0,:p1,'cancel')", userId, Id);

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


        public GoodsReceiptPOItemTagView___ GetItemTags(long id, long detId)
        {
            string sql = null;
            GoodsReceiptPOItemTagView___ model = new GoodsReceiptPOItemTagView___();

            using (var CONTEXT = new HANA_APP())
            {
                sql = @"SELECT T0.""Id"", T0.""DetId"", T0.""ItemCode"", T0.""ItemName""
                                FROM ""Tx_GoodsReceiptPO_Item"" T0   
                                WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model = CONTEXT.Database.SqlQuery<GoodsReceiptPOItemTagView___>(sql, id, detId).FirstOrDefault();

                sql = @"SELECT ROW_NUMBER() OVER (ORDER BY ""DetDetId"") AS ""RowNo"", T0.* 
                            FROM ""Tx_GoodsReceiptPO_Item_Tag"" T0   
                            WHERE T0.""Id""=:p0 AND ""DetId"" = :p1 ";

                model.GoodsReceiptPOItemTagModel___ = CONTEXT.Database.SqlQuery<GoodsReceiptPOItemTagModel>(sql, id, detId).ToList();
            }

            return model;
        }

    }


    #endregion

}