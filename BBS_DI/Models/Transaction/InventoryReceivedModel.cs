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


namespace Models.Transaction.InventoryReceived
{
    #region Models

    public class InventoryReceivedModel
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

        public long Id { get; set; }

        public string TransNo { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime? TransDate { get; set; }

        public long BaseTransId { get; set; }

        public string BaseTransNo { get; set; }
        
        public string Status { get; set; }

        [Required(ErrorMessage = "required")]
        public string FromWarehouseCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string FromWarehouseName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ToWarehouseCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string ToWarehouseName { get; set; }

        public string Remark { get; set; }

        public string Remark2 { get; set; }
        
        public int? SapDocEntry { get; set; }

        public string SapDocNum { get; set; }

        public int? BaseSapDocEntry { get; set; }

        public string BaseSapDocNum { get; set; }

        public List<InventoryReceived_DetailModel> ListDetails_ = new List<InventoryReceived_DetailModel>();

        public InventoryReceived_Details Details_ { get; set; }
    }


    public class InventoryReceived_DetailModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long Id { get; set; }

        public long DetId { get; set; }

        public long? BaseDetId { get; set; }

        public string ItemCodeKey { get; set; }

        [Required(ErrorMessage = "required")]
        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public decimal? Qty { get; set; }

        public decimal? QtyOpen { get; set; }

        public decimal? QtyReceived { get; set; }

        public string UoMCode { get; set; }

        public int? UomEntry { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }

        public int? SapDocEntry { get; set; }

        public int? SapLineNum { get; set; }

        public string ManBtchNum { get; set; }

        public List<InventoryReceivedBatchDetailModel> ListBatchDetails_ = new List<InventoryReceivedBatchDetailModel>();

        public InventoryReceivedBatchDetails BatchDetails_ { get; set; }
    }

    public class InventoryReceived_Details
    {
        public List<int> deletedRowKeys { get; set; }
        public List<InventoryReceived_DetailModel> insertedRowValues { get; set; }
        public List<InventoryReceived_DetailModel> modifiedRowValues { get; set; }
    }
    public class InventoryReceivedBatchDetailModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long Id { get; set; }

        public long? DetId { get; set; }
        public long? DetDetId { get; set; }
        public string ItemCode { get; set; }
        public int? BaseLine { get; set; }
        public string BatchNum { get; set; }
        public decimal? Quantity { get; set; }
        public DateTime? ExpDate { get; set; }
        public string Status { get; set; }
    }

    public class InventoryReceivedBatchDetails
    {
        public List<int> deletedRowKeys { get; set; }
        public List<InventoryReceivedBatchDetailModel> insertedRowValues { get; set; }
        public List<InventoryReceivedBatchDetailModel> modifiedRowValues { get; set; }
    }
    #endregion

    #region Services

    public class InventoryReceivedService
    {
        public InventoryReceivedModel GetNewModel(int userId)
        {
            InventoryReceivedModel model = new InventoryReceivedModel();
            
            return model;
        }
        public InventoryReceivedModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }
        public InventoryReceivedModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            InventoryReceivedModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.* 
                            FROM ""Tx_InventoryReceived"" T0   
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<InventoryReceivedModel>(ssql, id).Single();

                model.ListDetails_ = this.InventoryReceived_Details(CONTEXT, id);
            }

            return model;
        }
        public List<InventoryReceived_DetailModel> InventoryReceived_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return InventoryReceived_Details(CONTEXT, id);
            }

        }
        public List<InventoryReceived_DetailModel> InventoryReceived_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT A1.*,
                                   B1.""QtyOpen"",
                                   CAST(A1.""DetId"" AS NVARCHAR(10)) AS ""ItemCodeKey""
                            FROM ""Tx_InventoryReceived_Detail"" A1 
                            LEFT JOIN ""Tx_InventorySend_Detail"" B1 ON A1.""BaseDetId"" = B1.""DetId""
                            WHERE A1.""Id"" = :p0";
            return CONTEXT.Database.SqlQuery<InventoryReceived_DetailModel>(ssql, id).ToList();
        }
        public InventoryReceivedModel NavFirst(int userId)
        {
            InventoryReceivedModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "InventoryReceived");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_InventoryReceived\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public InventoryReceivedModel NavPrevious(int userId, long id = 0)
        {
            InventoryReceivedModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "InventoryReceived");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_InventoryReceived\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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
        public InventoryReceivedModel NavNext(int userId, long id = 0)
        {
            InventoryReceivedModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "InventoryReceived");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_InventoryReceived\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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
        public InventoryReceivedModel NavLast(int userId)
        {
            InventoryReceivedModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "InventoryReceived");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_InventoryReceived\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        public long Add(InventoryReceivedModel model)
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

                            Tx_InventoryReceived Tx_InventoryReceived = new Tx_InventoryReceived();
                            CopyProperty.CopyProperties(model, Tx_InventoryReceived, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_InventoryReceived.TransType = "InventoryReceived";
                            Tx_InventoryReceived.CreatedDate = dtModified;
                            Tx_InventoryReceived.CreatedUser = model._UserId;
                            Tx_InventoryReceived.ModifiedDate = dtModified;
                            Tx_InventoryReceived.ModifiedUser = model._UserId;
                            Tx_InventoryReceived.Status = "Draft";
                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'InventoryReceived','" + dateX + "','') ").SingleOrDefault();
                            Tx_InventoryReceived.TransNo = transNo;

                            CONTEXT.Tx_InventoryReceived.Add(Tx_InventoryReceived);
                            CONTEXT.SaveChanges();
                            Id = Tx_InventoryReceived.Id;

                            String keyValue;
                            keyValue = Tx_InventoryReceived.Id.ToString();

                            if (model.Details_ != null)
                            {
                                if (model.Details_.modifiedRowValues != null)
                                {
                                    foreach (var detail in model.Details_.modifiedRowValues)
                                    {
                                        Detail_Add(CONTEXT, detail, Id, model._UserId);
                                    }
                                }

                                
                            }
                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_InventoryReceived", "add", "Id", keyValue);

                            try
                            {

                               
                                Tx_InventorySend Tx_InventorySend = CONTEXT.Tx_InventorySend.Find(model.BaseTransId);
                                
                                if (Tx_InventoryReceived != null)
                                {
                                    Tx_InventorySend.ModifiedDate = dtModified;
                                    Tx_InventorySend.Received = "Yes";
                                    CONTEXT.SaveChanges();
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
        public void Update(InventoryReceivedModel model)
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

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", " Tx_InventoryReceived", "update", "Id", keyValue);


                                Tx_InventoryReceived Tx_InventoryReceived = CONTEXT.Tx_InventoryReceived.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_InventoryReceived.ModifiedDate = dtModified;
                                Tx_InventoryReceived.ModifiedUser = model._UserId;
                                if (Tx_InventoryReceived != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_InventoryReceived, false, exceptColumns);
                                    Tx_InventoryReceived.ModifiedDate = dtModified;
                                    Tx_InventoryReceived.ModifiedUser = model._UserId;
                                    CONTEXT.SaveChanges();

                                    if (model.Details_ != null)
                                    {
                                        if (model.Details_.insertedRowValues != null)
                                        {
                                            foreach (var detail in model.Details_.insertedRowValues)
                                            {
                                                Detail_Add(CONTEXT, detail, model.Id, model._UserId);
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
                                            foreach (var detId in model.Details_.deletedRowKeys)
                                            {
                                                InventoryReceived_DetailModel detailModel = new InventoryReceived_DetailModel();
                                                detailModel.DetId = detId;
                                                Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }


                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", " Tx_InventoryReceived", "update", "Id", keyValue);

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
        public long Detail_Add(HANA_APP CONTEXT, InventoryReceived_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;
            

            if (model != null)
            {

                Tx_InventoryReceived_Detail Tx_InventoryReceived_Detail = new Tx_InventoryReceived_Detail();

                CopyProperty.CopyProperties(model, Tx_InventoryReceived_Detail, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_InventoryReceived_Detail.Id = Id;
                Tx_InventoryReceived_Detail.CreatedDate = dtModified;
                Tx_InventoryReceived_Detail.CreatedUser = UserId;
                Tx_InventoryReceived_Detail.ModifiedDate = dtModified;
                Tx_InventoryReceived_Detail.ModifiedUser = UserId;


                CONTEXT.Tx_InventoryReceived_Detail.Add(Tx_InventoryReceived_Detail);
                CONTEXT.SaveChanges();
                DetId = Tx_InventoryReceived_Detail.DetId;
                long? BaseDetId = 0;
                var qtyReceived = Tx_InventoryReceived_Detail.QtyReceived;
                BaseDetId = Tx_InventoryReceived_Detail.BaseDetId;
                
                if (BaseDetId != null)
                {
                    //var ssql = @"UPDATE A1 SET 
                    //        A1.""QtyOpen"" = IFNULL(A1.""QtyOpen"",0) - IFNULL(B1.""QtyReceived"",0)
                    //        FROM ""Tx_InventorySend_Detail"" A1
                    //        LEFT JOIN ""Tx_InventoryReceived_Detail"" B1 ON A1.""DetId"" = B1.""BaseDetId""
                    //        WHERE B1.""DetId"" = :p0";
                    Tx_InventorySend_Detail Tx_InventorySend_Detail = CONTEXT.Tx_InventorySend_Detail.Find(BaseDetId);
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_InventorySend_Detail, false, exceptColumns);
                    var qtyOpen = Tx_InventorySend_Detail.QtyOpen - qtyReceived;
                    Tx_InventorySend_Detail.QtyOpen = qtyOpen;
                    CONTEXT.SaveChanges();
                }

            }

            return DetId;

        }
        public void Detail_Update(HANA_APP CONTEXT, InventoryReceived_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_InventoryReceived_Detail Tx_InventoryReceived_Detail = CONTEXT.Tx_InventoryReceived_Detail.Find(model.DetId);

                if (Tx_InventoryReceived_Detail != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_InventoryReceived_Detail, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_InventoryReceived_Detail.ModifiedDate = dtModified;
                    Tx_InventoryReceived_Detail.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Detail_Delete(HANA_APP CONTEXT, InventoryReceived_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_InventoryReceived_Detail\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
        public void Post(int userId, long Id)
        {

            String keyValue;
            keyValue = Id.ToString();

            SAPbobsCOM.Company oCompany = SAPCachedCompany.GetCompany();
            try
            {
                oCompany.StartTransaction();
                SpNotif.SpSysControllerTransNotif(userId, "InventoryReceived", oCompany, "before", "InventoryReceived", "post", "Id", keyValue);

                AddInventoryTransfer(oCompany, userId, Id);

                var sql1 = "UPDATE T0 SET   "
                        + " T0.\"Status\"='Posted',"
                        + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                        + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP, "
                        + " T0.\"SapDocNum\"= T1.\"DocNum\" "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_InventoryReceived\" T0 "
                        + " LEFT JOIN \"OWTR\" T1 ON T0.\"SapDocEntry\" = T1.\"DocEntry\" "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);

                SpNotif.SpSysControllerTransNotif(userId, "InventoryReceived", oCompany, "after", "InventoryReceived", "post", "Id", keyValue);

                oCompany.EndTransaction(BoWfTransOpt.wf_Commit);

            }

            catch (Exception ex)
            {

                if (oCompany.InTransaction)
                {
                    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                }

                throw ex;
            }
            finally
            {
                SAPCachedCompany.Release(oCompany);
            }

        }

        /*
        * ---------------------------------------
        * add Inventory Transfer
        * ---------------------------------------
        */
        public bool AddInventoryTransfer(SAPbobsCOM.Company oCompany, int userId, long Id)
        {
            int nErr;
            string errMsg;

            string ssql = "SELECT TOP 1 T0.\"DocEntry\" FROM \"OWTR\" T0 WHERE T0.\"U_IDU_WebTransId\"='{0}' AND T0.\"U_IDU_WebTransType\"='{1}'";
            ssql = string.Format(ssql, Id, "InventoryReceived");
            string tempId = _Utils.SapCompany.RetRstField(oCompany, ssql);
            if (!string.IsNullOrEmpty(tempId))
            {
                return false;
            }

            //ADD Inventory Transfer
            SAPbobsCOM.Recordset rsInvTransfer = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsInvTransfer = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpInventoryReceived_SapAddInventoryTransfer\" ('" + Id.ToString() + "')");

            if (!rsInvTransfer.EoF)
            {
                SAPbobsCOM.StockTransfer oInventoryTransfer = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);
                if (rsInvTransfer.Fields.Item("Hdr_Series").Value.ToString() != "")
                {
                    oInventoryTransfer.Series = int.Parse(rsInvTransfer.Fields.Item("Hdr_Series").Value.ToString());
                }

                oInventoryTransfer.DocDate = (DateTime)rsInvTransfer.Fields.Item("Hdr_DocDate").Value;
                oInventoryTransfer.FromWarehouse = rsInvTransfer.Fields.Item("Hdr_WhsCodeFrom").Value.ToString();
                oInventoryTransfer.ToWarehouse = rsInvTransfer.Fields.Item("Hdr_WhsCodeTo").Value.ToString();

                oInventoryTransfer.UserFields.Fields.Item("U_IDU_WebTransType").Value = rsInvTransfer.Fields.Item("Hdr_WebTransType").Value.ToString();
                oInventoryTransfer.UserFields.Fields.Item("U_IDU_WebTransNo").Value = rsInvTransfer.Fields.Item("Hdr_WebTransNo").Value.ToString();
                oInventoryTransfer.UserFields.Fields.Item("U_IDU_WebTransId").Value = rsInvTransfer.Fields.Item("Hdr_WebTransId").Value.ToString();
                oInventoryTransfer.UserFields.Fields.Item("U_IDU_WebUserId").Value = userId.ToString();
                oInventoryTransfer.UserFields.Fields.Item("U_IDU_WebTransDate").Value = (DateTime)rsInvTransfer.Fields.Item("Hdr_DocDueDate").Value;
                oInventoryTransfer.UserFields.Fields.Item("U_IDU_WebRemark").Value = rsInvTransfer.Fields.Item("Hdr_Remark").Value.ToString();

                int lineItem = 0;
                while (!rsInvTransfer.EoF)
                {
                    oInventoryTransfer.Lines.ItemCode = rsInvTransfer.Fields.Item("Det_ItemCode").Value.ToString();
                    if (rsInvTransfer.Fields.Item("Det_ItemDescription").Value.ToString() != "")
                    {
                        oInventoryTransfer.Lines.ItemDescription = rsInvTransfer.Fields.Item("Det_ItemDescription").Value.ToString();
                    }
                    oInventoryTransfer.Lines.Quantity = double.Parse(rsInvTransfer.Fields.Item("Det_Quantity").Value.ToString());


                    if (rsInvTransfer.Fields.Item("Hdr_WhsCodeFrom").Value.ToString() != "")
                    {
                        oInventoryTransfer.Lines.FromWarehouseCode = rsInvTransfer.Fields.Item("Hdr_WhsCodeFrom").Value.ToString();
                    }
                    if (rsInvTransfer.Fields.Item("Hdr_WhsCodeTo").Value.ToString() != "")
                    {
                        oInventoryTransfer.Lines.WarehouseCode = rsInvTransfer.Fields.Item("Hdr_WhsCodeTo").Value.ToString();
                    }
                    oInventoryTransfer.Lines.UserFields.Fields.Item("U_IDU_WebDetId").Value = rsInvTransfer.Fields.Item("Det_DetId").Value.ToString();
                    if (rsInvTransfer.Fields.Item("Det_ManBtchNum").Value.ToString() == "Y")
                    {

                        string DetId = rsInvTransfer.Fields.Item("Det_DetId").Value.ToString();
                        SAPbobsCOM.Recordset rsInvTransferBatch = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        rsInvTransferBatch = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpInventoryReceived_SapAddInventoryTransfer_Batch\" ('" + DetId.ToString() + "')");
                        int lineR = lineItem;
                        while (!rsInvTransferBatch.EoF)
                        {
                            oInventoryTransfer.Lines.BatchNumbers.BatchNumber = rsInvTransferBatch.Fields.Item("Det_BatchNum").Value.ToString();
                            oInventoryTransfer.Lines.BatchNumbers.Quantity = double.Parse(rsInvTransferBatch.Fields.Item("Det_BatchQty").Value.ToString());
                            //oInventoryTransfer.Lines.SetCurrentLine(lineR);
                            oInventoryTransfer.Lines.BatchNumbers.Add();
                            lineItem = lineR;
                            rsInvTransferBatch.MoveNext();
                        }
                    }
                    oInventoryTransfer.Lines.Add();

                    lineItem += 1;
                    rsInvTransfer.MoveNext();

                }

                if (oInventoryTransfer.Add() != 0)
                {

                    nErr = oCompany.GetLastErrorCode();
                    errMsg = oCompany.GetLastErrorDescription();
                    throw new Exception("[VALIDATION] - Add Inventory Transfer (Send) | " + nErr.ToString() + "|" + errMsg);

                }

                string docEntry;
                docEntry = oCompany.GetNewObjectKey();

                string sqlUpdateSO;
                sqlUpdateSO = "UPDATE T0 SET   "
                        + " T0.\"SapDocEntry\"=" + docEntry + " "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_InventoryReceived\" T0 "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sqlUpdateSO);
            }

            //END ADD DELIVERY

            //throw new Exception("[VALIDATION] - Lagi test jangan di save dulu");

            return true;
        }
        public void Cancel(int userId, long Id, string cancelReason)
        {
            String keyValue;
            keyValue = Id.ToString();


            SAPbobsCOM.Company oCompany = SAPCachedCompany.GetCompany();

            try
            {
                oCompany.StartTransaction();

                SpNotif.SpSysControllerTransNotif(userId, "InventoryReceived", oCompany, "before", "InventoryReceived", "cancel", "Id", keyValue);

                CancelInvTransfer(oCompany, userId, Id);

                string sql1 = "UPDATE T0 SET   "
                       + " T0.\"Status\"='Cancel',"
                       + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                       + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP "
                       + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_InventoryReceived\" T0 "
                       + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);

                SpNotif.SpSysControllerTransNotif(userId, "InventoryReceived", oCompany, "after", "InventoryReceived", "cancel", "Id", keyValue);

                oCompany.EndTransaction(BoWfTransOpt.wf_Commit);

            }

            catch (Exception ex)
            {

                if (oCompany.InTransaction)
                {
                    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                }

                throw ex;
            }
            finally
            {
                SAPCachedCompany.Release(oCompany);
            }

        }

        /*
       * ---------------------------------------
       * cancel Inv Transfer
       * ---------------------------------------
       */
        public bool CancelInvTransfer(SAPbobsCOM.Company oCompany, int userId, long Id)
        {
            int nErr;
            string errMsg;

            SAPbobsCOM.Recordset rsInvTransfer = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsInvTransfer = _Utils.SapCompany.GetRs(oCompany, "SELECT * FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_InventoryReceived\" T0 WHERE T0.\"Id\" ='" + Id.ToString() + "' AND IFNULL(T0.\"SapDocEntry\",0)>0 ");

            if (!rsInvTransfer.EoF)
            {
                SAPbobsCOM.StockTransfer oInvTransfer = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);
                oInvTransfer.GetByKey(int.Parse(rsInvTransfer.Fields.Item("SapDocEntry").Value.ToString()));

                if (oInvTransfer.Cancel() != 0)
                {
                    nErr = oCompany.GetLastErrorCode();
                    errMsg = oCompany.GetLastErrorDescription();
                    throw new Exception("[VALIDATION] - Cancel Inv Transfer - " + nErr.ToString() + "|" + errMsg);
                }

            }

            return true;
        }
        //Get InventorySend
        public InventoryReceivedModel GetInventorySendById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetInventorySendById(CONTEXT, userId, id);
            }
        }
        public InventoryReceivedModel GetInventorySendById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            InventoryReceivedModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT ""Id"" AS ""BaseTransId"",
                                        ""TransNo"" AS ""BaseTransNo"",
	                                    ""ToWarehouseCode"" AS ""FromWarehouseCode"",
	                                    ""ToWarehouseName"" AS ""FromWarehouseName"",
	                                    ""Remark"",
	                                    ""Remark2"",
	                                    ""SapDocEntry"" AS ""BaseSapDocEntry"",
	                                    ""SapDocNum"" AS  ""BaseSapDocNum"",
                                        'Draft' AS ""Status""
                            FROM ""Tx_InventorySend"" T0   
                            WHERE T0.""Id""=:p0 ";
                model = CONTEXT.Database.SqlQuery<InventoryReceivedModel>(ssql, id).Single();
                model.ListDetails_ = this.InventorySend_Details(CONTEXT, id);
            }

            return model;
        }
        public List<InventoryReceived_DetailModel> InventorySend_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return InventorySend_Details(CONTEXT, id);
            }

        }
        public List<InventoryReceived_DetailModel> InventorySend_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = @"SELECT A1.*,
		                            A1.""DetId"" AS ""BaseDetId"",
                                    IFNULL(A1.""Qty"", 0) - IFNULL(B1.""QtyReceived"", 0) AS ""QtyOpen""
                            FROM ""Tx_InventorySend_Detail"" A1 
                            LEFT JOIN ""Tx_InventoryReceived_Detail"" B1 ON A1.""DetId"" = B1.""BaseDetId""
                            WHERE A1.""Id"" = :p0";
            //return CONTEXT.Database.SqlQuery<InventoryReceived_DetailModel>("SELECT *, \"DetId\" AS \"BaseDetId\"  FROM \"Tx_InventorySend_Detail\" WHERE \"Id\" =:p0", id).ToList();
            return CONTEXT.Database.SqlQuery<InventoryReceived_DetailModel>(ssql, id).ToList();
        }
    }
    public class InventoryReceivedBatchDetailService
    {
        
        public InventoryReceived_DetailModel GetById(int userId, long detId = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, detId);
            }
        }
        public InventoryReceived_DetailModel GetById(HANA_APP CONTEXT, int userId, long detId = 0)
        {
            InventoryReceived_DetailModel model = null;
            if (detId != 0)
            {

                string ssql = "SELECT T0.*, T1.\"Status\" FROM \"Tx_InventoryReceived_Detail\" T0 LEFT JOIN \"Tx_InventoryReceived\" T1 ON T0.\"Id\" = T1.\"Id\" WHERE T0.\"DetId\"=:p0";
                model = CONTEXT.Database.SqlQuery<InventoryReceived_DetailModel>(ssql, detId).Single();

                if (model != null)
                {
                    model.ListBatchDetails_ = this.InventoryReceivedBatchDetailModel(detId);
                }
            }
            return model;
        }
        public List<InventoryReceivedBatchDetailModel> InventoryReceivedBatchDetailModel(long detId = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {

                return InventoryReceivedBatchDetails(CONTEXT, detId);
            }
        }
        public List<InventoryReceivedBatchDetailModel> InventoryReceivedBatchDetails(HANA_APP CONTEXT, long detId = 0)
        {
            return CONTEXT.Database.SqlQuery<InventoryReceivedBatchDetailModel>("SELECT T0.*, T1.\"Status\" FROM \"Tx_InventoryReceived_Detail_Batch\" T0 LEFT JOIN \"Tx_InventoryReceived\" T1 ON T0.\"Id\" = T1.\"Id\" WHERE T0.\"DetId\"=:p0  ", detId).ToList();
        }
        public long Add(InventoryReceived_DetailModel model)
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
                            keyValue = model.DetId.ToString();

                            Tx_InventoryReceived_Detail Tx_InventoryReceived_Detail = CONTEXT.Tx_InventoryReceived_Detail.Find(model.DetId);


                            if (Tx_InventoryReceived_Detail != null)
                            {

                                if (model.BatchDetails_ != null)
                                {
                                    if (model.BatchDetails_.insertedRowValues != null)
                                    {
                                        foreach (var detail in model.BatchDetails_.insertedRowValues)
                                        {
                                            BatchDetail_Add(CONTEXT, detail, model.Id, model.DetId, model._UserId);
                                        }
                                    }

                                    if (model.BatchDetails_.modifiedRowValues != null)
                                    {
                                        foreach (var detail in model.BatchDetails_.modifiedRowValues)
                                        {
                                            BatchDetail_Update(CONTEXT, detail, model._UserId);
                                        }
                                    }

                                    if (model.BatchDetails_.deletedRowKeys != null)
                                    {
                                        foreach (var detDetId in model.BatchDetails_.deletedRowKeys)
                                        {
                                            InventoryReceivedBatchDetailModel detailBatchModel = new InventoryReceivedBatchDetailModel();
                                            detailBatchModel.DetDetId = detDetId;
                                            BatchDetail_Delete(CONTEXT, detailBatchModel);
                                        }
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
            return model.DetId;

        }
        public void Delete(int detid = 0)
        {
            if (detid != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            string keyValue = detid.ToString();
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_InventoryReceived_Detail_Batch\" WHERE \"DetId\"=:p0 ", detid);
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

        public long BatchDetail_Add(HANA_APP CONTEXT, InventoryReceivedBatchDetailModel model, long Id, long DetId, int UserId)
        {
            long DetDetId = 0;

            if (model != null)
            {

                Tx_InventoryReceived_Detail_Batch Tx_InventoryReceived_Detail_Batch = new Tx_InventoryReceived_Detail_Batch();

                CopyProperty.CopyProperties(model, Tx_InventoryReceived_Detail_Batch, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_InventoryReceived_Detail_Batch.Id = Id;
                Tx_InventoryReceived_Detail_Batch.DetId = DetId;
                CONTEXT.Tx_InventoryReceived_Detail_Batch.Add(Tx_InventoryReceived_Detail_Batch);
                CONTEXT.SaveChanges();
                DetDetId = Tx_InventoryReceived_Detail_Batch.DetDetId;

            }

            return DetId;

        }
        public void BatchDetail_Update(HANA_APP CONTEXT, InventoryReceivedBatchDetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_InventoryReceived_Detail_Batch Tx_InventoryReceived_Detail_Batch = CONTEXT.Tx_InventoryReceived_Detail_Batch.Find(model.DetDetId);

                if (Tx_InventoryReceived_Detail_Batch != null)
                {
                    var exceptColumns = new string[] { "DetDetId", "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_InventoryReceived_Detail_Batch, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();


                    CONTEXT.SaveChanges();

                }


            }

        }
        public void BatchDetail_Delete(HANA_APP CONTEXT, InventoryReceivedBatchDetailModel model)
        {
            if (model.DetDetId != null)
            {
                if (model.DetDetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_InventoryReceived_Detail_Batch\"  WHERE \"DetDetId\"=:p0", model.DetDetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
    }

    #endregion

}