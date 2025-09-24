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

using Models._Sap;
using SAPbobsCOM;


namespace Models.Transaction.AdjustmentIn
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

        public int? CreatedUser { get; set; }

        public int? ModifiedUser { get; set; }

        public long Id { get; set; }

        public string TransNo { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime? TransDate { get; set; } 

        public string Status { get; set; }

        [Required(ErrorMessage = "required")]
        public string WarehouseCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string WarehouseName { get; set; }

        //-------------------------------------
        [Required(ErrorMessage = "required")]
        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        [Required(ErrorMessage = "required")]
        public decimal? QtyStock { get; set; }

        public string Remarks { get; set; }

        //------------------------------------

        public List<AdjustmentIn_LocationModel> ListLocations_ = new List<AdjustmentIn_LocationModel>();

        public AdjustmentIn_Locations DetailLocations_ { get; set; }

        public List<AdjustmentIn_AttachmentModel> ListAttachments_ = new List<AdjustmentIn_AttachmentModel>();
    }

    public class AdjustmentIn_LocationModel
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

        [Required(ErrorMessage = "required")]
        public Int32? BinAbsEntry { get; set; }

        public string BinCode { get; set; }

        [Required(ErrorMessage = "required")]
        public decimal? TotalReceipt { get; set; }
      
    }

    public class AdjustmentIn_Locations
    {
        public List<int> deletedRowKeys { get; set; }
        public List<AdjustmentIn_LocationModel> insertedRowValues { get; set; }
        public List<AdjustmentIn_LocationModel> modifiedRowValues { get; set; }
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

    #endregion

    #region Services

    public class AdjustmentInService
    {

        public long Add(AdjustmentInModel model)
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

                            Tx_AdjustmentIn tx_AdjustmentIn = new Tx_AdjustmentIn();
                            CopyProperty.CopyProperties(model, tx_AdjustmentIn, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_AdjustmentIn.TransType = "AdjustmentIn";
                            tx_AdjustmentIn.CreatedDate = dtModified;
                            tx_AdjustmentIn.CreatedUser = model._UserId;
                            tx_AdjustmentIn.ModifiedDate = dtModified;
                            tx_AdjustmentIn.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'AdjustmentIn','" + dateX + "','') ").SingleOrDefault();
                            tx_AdjustmentIn.TransNo = transNo;

                            CONTEXT.Tx_AdjustmentIn.Add(tx_AdjustmentIn);
                            CONTEXT.SaveChanges();
                            Id = tx_AdjustmentIn.Id;

                            String keyValue;
                            keyValue = tx_AdjustmentIn.Id.ToString();

                            if (model.DetailLocations_ != null)
                            {
                                foreach (var loc in model.DetailLocations_.insertedRowValues)
                                {
                                    Detail_Add(loc, Id, model._UserId);
                                }
                            }

                            //throw new Exception("[VALIDATION] - testing");

                            SpNotif.SpSysControllerTransNotif(model._UserId, "AdjustmentIn", CONTEXT, "after", "AdjustmentIn", "add", "Id", keyValue);

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

        public void Update(AdjustmentInModel model)
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
                            if (tx_AdjustmentIn != null)
                            {
                                var exceptColumns = new string[] { "Id", "TransType", "Status", "CreatedUser" };
                                CopyProperty.CopyProperties(model, tx_AdjustmentIn, false, exceptColumns);

                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                tx_AdjustmentIn.ModifiedDate = dtModified;
                                tx_AdjustmentIn.ModifiedUser = model._UserId;

                                CONTEXT.SaveChanges();

                                if (model.DetailLocations_ != null)
                                {
                                    if (model.DetailLocations_.insertedRowValues != null)
                                    {
                                        foreach (var loc in model.DetailLocations_.insertedRowValues)
                                        {
                                            Detail_Add(loc, model.Id, model._UserId);
                                        }
                                    }

                                    if (model.DetailLocations_.modifiedRowValues != null)
                                    {
                                        foreach (var loc in model.DetailLocations_.modifiedRowValues)
                                        {
                                            Detail_Update(loc, model._UserId);
                                        }
                                    }

                                    if (model.DetailLocations_.deletedRowKeys != null)
                                    {
                                        foreach (var detId in model.DetailLocations_.deletedRowKeys)
                                        {
                                            AdjustmentIn_LocationModel locModel = new AdjustmentIn_LocationModel();
                                            locModel.DetId = detId;
                                            Detail_Delete(locModel);
                                        }
                                    }
                                }

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

        public void Post(int userId, long Id)
        {

            String keyValue;
            keyValue = Id.ToString();

            SAPbobsCOM.Company oCompany = SAPCachedCompany.GetCompany();
            try
            {
                oCompany.StartTransaction();
                SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", oCompany, "before", "AdjustmentIn", "post", "Id", keyValue);

                AddGoodReceipt(oCompany, userId, Id);

                var sql1 = "UPDATE T0 SET   "
                        + " T0.\"Status\"='Posted',"
                        + " T0.\"IsAfterPosted\"='Y',"
                        + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                        + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_AdjustmentIn\" T0 "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);

                SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", oCompany, "after", "AdjustmentIn", "post", "Id", keyValue);

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
      * add Good Receipt
      * ---------------------------------------
      */
        public bool AddGoodReceipt(SAPbobsCOM.Company oCompany, int userId, long Id)
        {
            int nErr;
            string errMsg;

            string ssql = "SELECT TOP 1 T0.\"DocEntry\" FROM \"OIGN\" T0 WHERE T0.\"U_IDU_WebTransId\"='{0}' AND T0.\"U_IDU_WebTransType\"='{1}'";
            ssql = string.Format(ssql, Id, "AdjustmentIn");
            string tempId = _Utils.SapCompany.RetRstField(oCompany, ssql);
            if (!string.IsNullOrEmpty(tempId))
            {
                throw new Exception("[VALIDATION] - AddGoodReceipt : Sudah dibuat GR-nya");
            }
               

            //ADD Good Receipt
            SAPbobsCOM.Recordset rsGR = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsGR = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpAdjustmentIn_SapAddGoodReceipt\" ('" + Id.ToString() + "')");

            if (!rsGR.EoF)
            {
                SAPbobsCOM.Documents oGoodReceipt = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);
                if (rsGR.Fields.Item("Hdr_Series").Value.ToString() != "")
                {
                    oGoodReceipt.Series = int.Parse(rsGR.Fields.Item("Hdr_Series").Value.ToString());
                }

                string ControlAccount = rsGR.Fields.Item("Hdr_ControlAccount").Value.ToString();
                if (!string.IsNullOrEmpty(ControlAccount))
                {
                    ControlAccount = ControlAccount.Replace("-", "");
                    oGoodReceipt.ControlAccount = SapCompany.RetCoaCode(oCompany, ControlAccount);
                }

                //oGoodReceipt.SalesPersonCode = int.Parse(rsGR.Fields.Item("Hdr_SlpCode").Value.ToString());

                oGoodReceipt.DocDate = (DateTime)rsGR.Fields.Item("Hdr_DocDate").Value;
                oGoodReceipt.DocDueDate = (DateTime)rsGR.Fields.Item("Hdr_DocDueDate").Value;

                oGoodReceipt.DocCurrency = rsGR.Fields.Item("Hdr_DocCurrency").Value.ToString();
                oGoodReceipt.DocRate = double.Parse(rsGR.Fields.Item("Hdr_DocRate").Value.ToString());

                if (rsGR.Fields.Item("Hdr_PaymentGroupCode").Value.ToString() != "")
                {
                    oGoodReceipt.PaymentGroupCode = int.Parse(rsGR.Fields.Item("Hdr_PaymentGroupCode").Value.ToString());
                }

                oGoodReceipt.UserFields.Fields.Item("U_IDU_WebTransType").Value = rsGR.Fields.Item("Hdr_U_IDU_WebTransType").Value.ToString();
                oGoodReceipt.UserFields.Fields.Item("U_IDU_WebTransNo").Value = rsGR.Fields.Item("Hdr_U_IDU_WebTransNo").Value.ToString();
                oGoodReceipt.UserFields.Fields.Item("U_IDU_WebTransId").Value = rsGR.Fields.Item("Hdr_U_IDU_WebTransId").Value.ToString();
                oGoodReceipt.UserFields.Fields.Item("U_IDU_WebUserId").Value = userId.ToString();

                if (rsGR.Fields.Item("Hdr_DocType").Value.ToString() == "I")
                {
                    oGoodReceipt.DocType = BoDocumentTypes.dDocument_Items;
                }
                else
                {
                    oGoodReceipt.DocType = BoDocumentTypes.dDocument_Service;
                }

                oGoodReceipt.Comments = rsGR.Fields.Item("Hdr_Remarks").Value.ToString();

                if (rsGR.Fields.Item("Hdr_WTCode").Value.ToString() != "")
                {
                    oGoodReceipt.WithholdingTaxData.WTCode = rsGR.Fields.Item("Hdr_WTCode").Value.ToString();
                    oGoodReceipt.WithholdingTaxData.TaxableAmount = double.Parse(rsGR.Fields.Item("Hdr_TaxableAmount").Value.ToString());
                    oGoodReceipt.WithholdingTaxData.WTAmount = double.Parse(rsGR.Fields.Item("Hdr_WTAmount").Value.ToString());
                }

                int lineItem = 0;
                while (!rsGR.EoF)
                {
                    if (rsGR.Fields.Item("Hdr_DocType").Value.ToString() == "I")
                    {
                        oGoodReceipt.Lines.ItemCode = rsGR.Fields.Item("Det_ItemCode").Value.ToString();
                        if (rsGR.Fields.Item("Det_ItemDescription").Value.ToString() != "")
                        {
                            oGoodReceipt.Lines.ItemDescription = rsGR.Fields.Item("Det_ItemDescription").Value.ToString();
                        }
                        oGoodReceipt.Lines.UnitPrice = double.Parse(rsGR.Fields.Item("Det_UnitPrice").Value.ToString());

                        oGoodReceipt.Lines.Quantity = double.Parse(rsGR.Fields.Item("Det_Quantity").Value.ToString());
                        if (rsGR.Fields.Item("Det_WarehouseCode").Value.ToString() != "")
                        {
                            oGoodReceipt.Lines.WarehouseCode = rsGR.Fields.Item("Det_WarehouseCode").Value.ToString();
                        }

                        oGoodReceipt.Lines.BinAllocations.BinAbsEntry = Int32.Parse(rsGR.Fields.Item("Det_BinAbsEntry").Value.ToString());
                        oGoodReceipt.Lines.BinAllocations.Quantity = double.Parse(rsGR.Fields.Item("Det_Quantity").Value.ToString());
                        //oGoodReceiptPO.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;
                        oGoodReceipt.Lines.BinAllocations.AllowNegativeQuantity = BoYesNoEnum.tYES;
                        oGoodReceipt.Lines.BinAllocations.Add();

                        if (rsGR.Fields.Item("Det_AccountCode").Value.ToString() != "")
                        {
                            string CoaCode = rsGR.Fields.Item("Det_AccountCode").Value.ToString();
                            CoaCode = CoaCode.Replace("-", "");
                            oGoodReceipt.Lines.AccountCode = SapCompany.RetCoaCode(oCompany, CoaCode);
                        }
                    }
                    else
                    {
                        //oGoodReceipt.Lines.ItemDescription = rsDelivery.Fields.Item("Det_ItemDescription").Value.ToString();
                    }

                    oGoodReceipt.Lines.Add();

                    lineItem += 1;
                    rsGR.MoveNext();
                }

                if (oGoodReceipt.Add() != 0)
                {

                    nErr = oCompany.GetLastErrorCode();
                    errMsg = oCompany.GetLastErrorDescription();
                    throw new Exception("[VALIDATION] - Add Good Issue | " + nErr.ToString() + "|" + errMsg);

                }

                string docEntry;
                docEntry = oCompany.GetNewObjectKey();

                string sqlUpdateSO;
                sqlUpdateSO = "UPDATE T0 SET   "
                        + " T0.\"SapGoodReceiptId\"=" + docEntry + " "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_AdjustmentIn\" T0 "
                        + " WHERE T0.\"Id\"=" + Id.ToString() ;

                SapCompany.ExecuteQuery(oCompany, sqlUpdateSO);

            }

            //END ADD Good Receipt


            string sqlx = "UPDATE T0 SET   "
                           + " T0.\"SapGoodReceiptNo\"= T1.\"DocNum\" "
                           + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_AdjustmentIn\" T0 "
                           + " INNER JOIN \"OIGN\" T1 ON T0.\"SapGoodReceiptId\" = T1.\"DocEntry\" "
                           + " WHERE T0.\"Id\"=" + Id.ToString();

            SapCompany.ExecuteQuery(oCompany, sqlx);

            //throw new Exception("[VALIDATION] - Lagi test jangan di save dulu");

            return true;
        }

        /*
       * ---------------------------------------
       * add Good Issue
       * ---------------------------------------
       */
        public bool AddGoodIssue(SAPbobsCOM.Company oCompany, int userId, long Id)
        {
            int nErr;
            string errMsg;

            string ssql = "SELECT TOP 1 T0.\"DocEntry\" FROM \"OIGE\" T0 WHERE T0.\"U_IDU_WebTransId\"='{0}' AND T0.\"U_IDU_WebTransType\"='{1}'";
            ssql = string.Format(ssql, Id, "AdjustmentIn");
            string tempId = _Utils.SapCompany.RetRstField(oCompany, ssql);
            if (!string.IsNullOrEmpty(tempId))
            {
                throw new Exception("[VALIDATION] - AddGoodIssue : Sudah dibuat GI-nya");
            }


            //ADD Good Issue
            SAPbobsCOM.Recordset rsGI = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsGI = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpAdjustmentIn_SapAddGoodIssue\" ('" + Id.ToString() + "')");

            if (!rsGI.EoF)
            {
                SAPbobsCOM.Documents oGoodIssue = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
                if (rsGI.Fields.Item("Hdr_Series").Value.ToString() != "")
                {
                    oGoodIssue.Series = int.Parse(rsGI.Fields.Item("Hdr_Series").Value.ToString());
                }

                string ControlAccount = rsGI.Fields.Item("Hdr_ControlAccount").Value.ToString();
                if (!string.IsNullOrEmpty(ControlAccount))
                {
                    ControlAccount = ControlAccount.Replace("-", "");
                    oGoodIssue.ControlAccount = SapCompany.RetCoaCode(oCompany, ControlAccount);
                }

                //oGoodIssue.SalesPersonCode = int.Parse(rsGI.Fields.Item("Hdr_SlpCode").Value.ToString());

                oGoodIssue.DocDate = (DateTime)rsGI.Fields.Item("Hdr_DocDate").Value;
                oGoodIssue.DocDueDate = (DateTime)rsGI.Fields.Item("Hdr_DocDueDate").Value;

                oGoodIssue.DocCurrency = rsGI.Fields.Item("Hdr_DocCurrency").Value.ToString();
                oGoodIssue.DocRate = double.Parse(rsGI.Fields.Item("Hdr_DocRate").Value.ToString());

                if (rsGI.Fields.Item("Hdr_PaymentGroupCode").Value.ToString() != "")
                {
                    oGoodIssue.PaymentGroupCode = int.Parse(rsGI.Fields.Item("Hdr_PaymentGroupCode").Value.ToString());
                }

                  
                oGoodIssue.UserFields.Fields.Item("U_IDU_WebTransType").Value = rsGI.Fields.Item("Hdr_U_IDU_WebTransType").Value.ToString();
                oGoodIssue.UserFields.Fields.Item("U_IDU_WebTransNo").Value = rsGI.Fields.Item("Hdr_U_IDU_WebTransNo").Value.ToString();
                oGoodIssue.UserFields.Fields.Item("U_IDU_WebTransId").Value = rsGI.Fields.Item("Hdr_U_IDU_WebTransId").Value.ToString();
                oGoodIssue.UserFields.Fields.Item("U_IDU_WebUserId").Value = userId.ToString();

                oGoodIssue.Comments = rsGI.Fields.Item("Hdr_Remarks").Value.ToString();

                if (rsGI.Fields.Item("Hdr_DocType").Value.ToString() == "I")
                {
                    oGoodIssue.DocType = BoDocumentTypes.dDocument_Items;
                }
                else
                {
                    oGoodIssue.DocType = BoDocumentTypes.dDocument_Service;
                }

                if (rsGI.Fields.Item("Hdr_WTCode").Value.ToString() != "")
                {
                    oGoodIssue.WithholdingTaxData.WTCode = rsGI.Fields.Item("Hdr_WTCode").Value.ToString();
                    oGoodIssue.WithholdingTaxData.TaxableAmount = double.Parse(rsGI.Fields.Item("Hdr_TaxableAmount").Value.ToString());
                    oGoodIssue.WithholdingTaxData.WTAmount = double.Parse(rsGI.Fields.Item("Hdr_WTAmount").Value.ToString());
                }

                int lineItem = 0;
                while (!rsGI.EoF)
                {
                    if (rsGI.Fields.Item("Hdr_DocType").Value.ToString() == "I")
                    {
                        oGoodIssue.Lines.ItemCode = rsGI.Fields.Item("Det_ItemCode").Value.ToString();
                        if (rsGI.Fields.Item("Det_ItemDescription").Value.ToString() != "")
                        {
                            oGoodIssue.Lines.ItemDescription = rsGI.Fields.Item("Det_ItemDescription").Value.ToString();
                        }
                        oGoodIssue.Lines.UnitPrice = double.Parse(rsGI.Fields.Item("Det_UnitPrice").Value.ToString());

                        oGoodIssue.Lines.Quantity = double.Parse(rsGI.Fields.Item("Det_Quantity").Value.ToString());
                        if (rsGI.Fields.Item("Det_WarehouseCode").Value.ToString() != "")
                        {
                            oGoodIssue.Lines.WarehouseCode = rsGI.Fields.Item("Det_WarehouseCode").Value.ToString();
                        }

                        oGoodIssue.Lines.BinAllocations.BinAbsEntry = Int32.Parse(rsGI.Fields.Item("Det_BinAbsEntry").Value.ToString());
                        oGoodIssue.Lines.BinAllocations.Quantity = double.Parse(rsGI.Fields.Item("Det_Quantity").Value.ToString());
                        //oGoodReceiptPO.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;
                        oGoodIssue.Lines.BinAllocations.AllowNegativeQuantity = BoYesNoEnum.tNO;
                        oGoodIssue.Lines.BinAllocations.Add();

                        if (rsGI.Fields.Item("Det_AccountCode").Value.ToString() != "")
                        {
                            string CoaCode = rsGI.Fields.Item("Det_AccountCode").Value.ToString();
                            CoaCode = CoaCode.Replace("-", "");
                            oGoodIssue.Lines.AccountCode = SapCompany.RetCoaCode(oCompany, CoaCode);
                        }

                    }
                    else
                    {
                        //oGoodIssue.Lines.ItemDescription = rsDelivery.Fields.Item("Det_ItemDescription").Value.ToString();
                    }

                    oGoodIssue.Lines.Add();

                    lineItem += 1;
                    rsGI.MoveNext();

                }

                if (oGoodIssue.Add() != 0)
                {

                    nErr = oCompany.GetLastErrorCode();
                    errMsg = oCompany.GetLastErrorDescription();
                    throw new Exception("[VALIDATION] - Add Good Issue | " + nErr.ToString() + "|" + errMsg);

                }

                string docEntry;
                docEntry = oCompany.GetNewObjectKey();

                string sqlUpdate;
                sqlUpdate = "UPDATE T0 SET   "
                        + " T0.\"SapGoodIssueId\"=" + docEntry + " "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_AdjustmentIn\" T0 "
                        + " WHERE T0.\"Id\"=" + Id.ToString() ;

                SapCompany.ExecuteQuery(oCompany, sqlUpdate);

            }

            string sqlx = "UPDATE T0 SET   "
                           + " T0.\"SapGoodIssueNo\"= T1.\"DocNum\" "
                           + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_AdjustmentIn\" T0 "
                           + " INNER JOIN \"OIGE\" T1 ON T0.\"SapGoodIssueId\" = T1.\"DocEntry\" "
                           + " WHERE T0.\"Id\"=" + Id.ToString();

            SapCompany.ExecuteQuery(oCompany, sqlx);

            //throw new Exception("[VALIDATION] - Lagi test jangan di save dulu");

            return true;
        }



        public void Cancel(int userId, long Id)
        {
            String keyValue;
            keyValue = Id.ToString();


            SAPbobsCOM.Company oCompany = SAPCachedCompany.GetCompany();

            try
            {
                oCompany.StartTransaction();

                SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", oCompany, "before", "AdjustmentIn", "cancel", "Id", keyValue);

                AddGoodIssue(oCompany, userId, Id);

                string sql1 = "UPDATE T0 SET   "
                       + " T0.\"Status\"='Cancel',"
                       + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                       + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP "
                       + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_AdjustmentIn\" T0 "
                       + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);

                SpNotif.SpSysControllerTransNotif(userId, "AdjustmentIn", oCompany, "after", "AdjustmentIn", "cancel", "Id", keyValue);

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
        * cancel GRPO
        * ---------------------------------------
        */
        public bool CancelGRPO(SAPbobsCOM.Company oCompany, int userId, long Id)
        {
            int nErr;
            string errMsg;

            SAPbobsCOM.Recordset rsGrpo = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsGrpo = _Utils.SapCompany.GetRs(oCompany, "SELECT * FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_AdjustmentIn\" T0 WHERE T0.\"Id\" ='" + Id.ToString() + "' AND IFNULL(T0.\"SapGrpoId\",0)>0 ");

            if (!rsGrpo.EoF)
            {
                SAPbobsCOM.Documents oGrpo = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);

                oGrpo.GetByKey(int.Parse(rsGrpo.Fields.Item("SapGrpoId").Value.ToString()));

                SAPbobsCOM.Documents oCancelDoc = oGrpo.CreateCancellationDocument();
              
                oCancelDoc.DocDate = (DateTime)rsGrpo.Fields.Item("TransDate").Value;

                if (oCancelDoc.Add() != 0)
                {
                    nErr = oCompany.GetLastErrorCode();
                    errMsg = oCompany.GetLastErrorDescription();
                    throw new Exception("[VALIDATION] - Cancel GRPO - " + nErr.ToString() + "|" + errMsg);
                }

            }

            return true;
        }


        public AdjustmentInModel GetNewModel(int userId)
        {
            AdjustmentInModel model = new AdjustmentInModel();
            model.Status = "Draft";
            return model;
        }

        public AdjustmentInModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public AdjustmentInModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            AdjustmentInModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.* 
                            FROM ""Tx_AdjustmentIn"" T0   
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<AdjustmentInModel>(ssql, id).Single();

                model.ListLocations_ = this.AdjustmentIn_Locations(CONTEXT, id);
                model.ListAttachments_ = this.AdjustmentIn_Attachments(CONTEXT, id);
            }

            return model;
        }

        public List<AdjustmentIn_LocationModel> AdjustmentIn_Locations(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return AdjustmentIn_Locations(CONTEXT, id);
            }
        }

        public List<AdjustmentIn_LocationModel> AdjustmentIn_Locations(HANA_APP CONTEXT, long id = 0)
        {
            return CONTEXT.Database.SqlQuery<AdjustmentIn_LocationModel>("SELECT T0.* FROM \"Tx_AdjustmentIn_Location\" T0 WHERE T0.\"Id\"=:p0  ", id).ToList();
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

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentIn\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(CONTEXT, userId, Id.Value);
                }
                //else
                //{
                //    model = this.NavFirst(userId);
                //}
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

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentIn\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
                if (Id.HasValue)
                {
                    model = this.GetById(CONTEXT, userId, Id.Value);
                }
                //else
                //{
                //    model = model = this.NavLast(userId);
                //}
            }

            if (model == null)
            {
                model = model = this.NavLast(userId);
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

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_AdjustmentIn\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }


        //-------------------------------------
        //Detail  AdjustmentIn_LocationModel
        //-------------------------------------
        public AdjustmentIn_LocationModel Location_GetById(int userId, long detId = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Location_GetById(CONTEXT, userId, detId);
            }
        }
        public AdjustmentIn_LocationModel Location_GetById(HANA_APP CONTEXT, int userId, long detId = 0)
        {
            AdjustmentIn_LocationModel model = null;
            if (detId != 0)
            {
                string ssql = @"SELECT T0.""Id"", T0.""DetId"", T0.""BinAbsEntry"", T0.""BinCode"", 
                                TO_DECIMAL(T0.""DippingCmStart"" , 19,2) AS ""DippingCmStart"",
                                TO_DECIMAL(T0.""DippingCmEnd"" , 19,2) AS ""DippingCmEnd"",
                                TO_DECIMAL(T0.""DippingLtrStart"" , 19,2) AS ""DippingLtrStart"",
                                TO_DECIMAL(T0.""DippingLtrEnd"" , 19,2) AS ""DippingLtrEnd"",
                                TO_DECIMAL(T0.""TotalReceipt"" , 19,2) AS ""TotalReceipt""
                            FROM ""Tx_AdjustmentIn_Location"" T0
                            WHERE T0.""DetId""=:p0 ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                model = CONTEXT.Database.SqlQuery<AdjustmentIn_LocationModel>(ssql, detId).Single();
            }
            return model;
        }

        public AdjustmentIn_LocationModel Location_GetNewModel(int userId, long id)
        {
            AdjustmentIn_LocationModel model = new AdjustmentIn_LocationModel();
            model.Id = id;
            return model;
        }

        public List<AdjustmentIn_LocationModel> GetAdjustmentIn_Locations(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAdjustmentIn_Locations(CONTEXT, id);
            }
        }

        public List<AdjustmentIn_LocationModel> GetAdjustmentIn_Locations(HANA_APP CONTEXT, long id = 0)
        {
            return CONTEXT.Database.SqlQuery<AdjustmentIn_LocationModel>("SELECT T0.* FROM \"Tx_AdjustmentIn_Location\" T0 WHERE T0.\"Id\"=:p0  ", id).ToList();
        }

        public long Detail_Add(AdjustmentIn_LocationModel model, long Id, int UserId)
        {
            long detId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        detId = Detail_Add(CONTEXT, model, Id, UserId);
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
                return detId;
            }
        }

        public long Detail_Add(HANA_APP CONTEXT, AdjustmentIn_LocationModel model, long Id, int UserId)
        {
            long detId = 0;

            if (model != null)
            {
                Tx_AdjustmentIn_Location tx_AdjustmentIn_Location = new Tx_AdjustmentIn_Location();

                CopyProperty.CopyProperties(model, tx_AdjustmentIn_Location, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_AdjustmentIn_Location.Id = Id;
                tx_AdjustmentIn_Location.CreatedDate = dtModified;
                tx_AdjustmentIn_Location.CreatedUser = UserId;
                tx_AdjustmentIn_Location.ModifiedDate = dtModified;
                tx_AdjustmentIn_Location.ModifiedUser = UserId;

                String keyValue;
                CONTEXT.Tx_AdjustmentIn_Location.Add(tx_AdjustmentIn_Location);
                CONTEXT.SaveChanges();
                keyValue = tx_AdjustmentIn_Location.DetId.ToString();
                detId = tx_AdjustmentIn_Location.DetId;

                SpNotif.SpSysControllerTransNotif(UserId, "AdjustmentIn", CONTEXT, "after", "AdjustmentIn_Location", "add", "DetId", keyValue);
            }

            return detId;

        }

        public void Detail_Update(AdjustmentIn_LocationModel model, int UserId)
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

                            SpNotif.SpSysControllerTransNotif(UserId, "AdjustmentIn", CONTEXT, "before", "AdjustmentIn_Location", "update", "DetId", keyValue);

                            Tx_AdjustmentIn_Location tx_AdjustmentIn_Location = CONTEXT.Tx_AdjustmentIn_Location.Find(model.DetId);
                            if (tx_AdjustmentIn_Location != null)
                            {
                                var exceptColumns = new string[] { "Id", "DetId", };
                                CopyProperty.CopyProperties(model, tx_AdjustmentIn_Location, false, exceptColumns);
                                CONTEXT.SaveChanges();

                                SpNotif.SpSysControllerTransNotif(UserId, "AdjustmentIn", CONTEXT, "after", "AdjustmentIn_Location", "update", "DetId", keyValue);
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

        //public void Detail_Delete(int detId)
        //{
        //    using (var CONTEXT = new HANA_APP())
        //    {
        //        Detail_Delete(CONTEXT, detId);
        //    }
        //}

        public void Detail_Delete(AdjustmentIn_LocationModel model)
        {
            using (var CONTEXT = new HANA_APP())
            {
                Detail_Delete(CONTEXT, model.DetId);
            }
        }

        public void Detail_Delete(HANA_APP CONTEXT, long detId)
        {
            //if (detId != null)
            //{
                if (detId != 0)
                {
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_AdjustmentIn_Location\"  WHERE \"DetId\"=:p0", detId);
                    CONTEXT.SaveChanges();
                }
            //}
        }


        public List<AdjustmentIn_AttachmentModel> AdjustmentIn_Attachments(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return AdjustmentIn_Attachments(CONTEXT, id);
            }

        }

        public List<AdjustmentIn_AttachmentModel> AdjustmentIn_Attachments(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<AdjustmentIn_AttachmentModel>("SELECT T0.\"Id\", T0.\"DetId\", T0.\"FileName\" FROM \"Tx_AdjustmentIn_Attachment\" T0 WHERE T0.\"Id\"=:p0 ORDER BY T0.\"DetId\" ", id).ToList();
        }

        public long Detail_Add(HANA_APP CONTEXT, AdjustmentIn_AttachmentModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_AdjustmentIn_Attachment tx_AdjustmentIn_Attachment = new Tx_AdjustmentIn_Attachment();

                CopyProperty.CopyProperties(model, tx_AdjustmentIn_Attachment, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_AdjustmentIn_Attachment.Id = Id;
                tx_AdjustmentIn_Attachment.CreatedDate = dtModified;
                tx_AdjustmentIn_Attachment.CreatedUser = UserId;
                tx_AdjustmentIn_Attachment.ModifiedDate = dtModified;
                tx_AdjustmentIn_Attachment.ModifiedUser = UserId;

                CONTEXT.Tx_AdjustmentIn_Attachment.Add(tx_AdjustmentIn_Attachment);
                CONTEXT.SaveChanges();
                DetId = tx_AdjustmentIn_Attachment.DetId;

            }

            return DetId;

        }

        public long Detail_Add(List<AdjustmentIn_AttachmentModel> ListModel)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Detail_Add(CONTEXT, ListModel);
            }

        }

        public long Detail_Add(HANA_APP CONTEXT, List<AdjustmentIn_AttachmentModel> ListModel)
        {
            long Id = 0;
            long DetId = 0;

            if (ListModel != null)
            {

                for (int i = 0; i < ListModel.Count; i++)
                {
                    Tx_AdjustmentIn_Attachment tx_AdjustmentIn_Attachment = new Tx_AdjustmentIn_Attachment();
                    var model = ListModel[i];

                    CopyProperty.CopyProperties(model, tx_AdjustmentIn_Attachment, false);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    tx_AdjustmentIn_Attachment.Id = model.Id;
                    tx_AdjustmentIn_Attachment.CreatedDate = dtModified;
                    tx_AdjustmentIn_Attachment.CreatedUser = model._UserId;
                    tx_AdjustmentIn_Attachment.ModifiedDate = dtModified;
                    tx_AdjustmentIn_Attachment.ModifiedUser = model._UserId;

                    CONTEXT.Tx_AdjustmentIn_Attachment.Add(tx_AdjustmentIn_Attachment);
                    CONTEXT.SaveChanges();
                    DetId = tx_AdjustmentIn_Attachment.DetId;
                }



            }

            return Id;

        }

        public void Detail_Delete(AdjustmentIn_AttachmentModel model)
        {
            if (model.DetId != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    Detail_Delete(CONTEXT, model);
                }
            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, AdjustmentIn_AttachmentModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_AdjustmentIn_Attachment\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }


        
    }


    #endregion

}