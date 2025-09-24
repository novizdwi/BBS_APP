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


namespace Models.Transaction.TukarFakturReceipt
{
    #region Models

    public class TukarFakturReceiptModel
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

        public string NoTglTerima { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime? TglTerima { get; set; }
        public string NoTtd { get; set; }
        public string NamaPenerima { get; set; }
        public string Resi { get; set; }
        public string Keterangan { get; set; }
        public string MetodeKirim { get; set; }
        public string Pengiriman { get; set; }

        public string Status { get; set; }

    }


    #endregion

    #region Services

    public class TukarFakturReceiptService
    {

        public long Add(TukarFakturReceiptModel model)
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

                            Tx_TukarFakturReceipt tx_TukarFakturReceipt = new Tx_TukarFakturReceipt();
                            CopyProperty.CopyProperties(model, tx_TukarFakturReceipt, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TukarFakturReceipt.CreatedDate = dtModified;
                            tx_TukarFakturReceipt.CreatedUser = model._UserId;
                            tx_TukarFakturReceipt.ModifiedDate = dtModified;
                            tx_TukarFakturReceipt.ModifiedUser = model._UserId;


                            string dateX = model.TglTerima.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'TukarFakturReceipt','" + dateX + "','') ").SingleOrDefault();
                            tx_TukarFakturReceipt.NoTglTerima = transNo;

                            CONTEXT.Tx_TukarFakturReceipt.Add(tx_TukarFakturReceipt);
                            CONTEXT.SaveChanges();
                            Id = tx_TukarFakturReceipt.Id;

                            String keyValue;
                            keyValue = tx_TukarFakturReceipt.Id.ToString();

                            SpNotif.SpSysControllerTransNotif(model._UserId, "TukarFakturReceipt", CONTEXT, "after", "Tx_TukarFakturReceipt", "add", "Id", keyValue);

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

        public void Update(TukarFakturReceiptModel model)
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

                                SpNotif.SpSysControllerTransNotif(model._UserId, "TukarFakturReceipt", CONTEXT, "before", "Tx_TukarFakturReceipt", "update", "Id", keyValue);

                                Tx_TukarFakturReceipt tx_TukarFakturReceipt = CONTEXT.Tx_TukarFakturReceipt.Find(model.Id);
                                if (tx_TukarFakturReceipt != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransType", "Status", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_TukarFakturReceipt, false, exceptColumns);

                                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                    tx_TukarFakturReceipt.ModifiedDate = dtModified;
                                    tx_TukarFakturReceipt.ModifiedUser = model._UserId;

                                    CONTEXT.SaveChanges();

                                    SpNotif.SpSysControllerTransNotif(model._UserId, "TukarFakturReceipt", CONTEXT, "after", "Tx_TukarFakturReceipt", "update", "Id", keyValue);
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

        public void Invoice(int userId, long Id)
        {
            String keyValue;
            keyValue = Id.ToString();

            SAPbobsCOM.Company oCompany = SAPCachedCompany.GetCompany();
            try
            {
                oCompany.StartTransaction();
                UpdateInvoice(oCompany, userId, Id);

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

        public bool UpdateInvoice(SAPbobsCOM.Company oCompany, int userId, long Id)
        {
            int nErr;
            string errMsg;

            //string ssql = "SELECT TOP 1 T0.\"DocEntry\" FROM \"ODLN\" T0 WHERE T0.\"U_IDU_WebTransId\"='{0}' AND T0.\"U_IDU_WebTransType\"='{1}'";
            //ssql = string.Format(ssql, Id, "LoadingOrder");
            //string tempId = _Utils.SapCompany.RetRstField(oCompany, ssql);
            //if (!string.IsNullOrEmpty(tempId))
            //{
            //    throw new Exception("[VALIDATION] - Sudah dibuat deliverynya");
            //}

            //SAPbobsCOM.Recordset rsDetailSO = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            //rsDetailSO = _Utils.SapCompany.GetRs(oCompany, "SELECT * FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_LoadingOrder_SalesOrder\" T0 WHERE T0.\"Id\" ='" + Id.ToString() + "'");
            //int LineSo = 0;
            //while (!rsDetailSO.EoF)
            //{
            //    long soDetId = long.Parse(rsDetailSO.Fields.Item("DetId").Value.ToString());

            //    string CustCode = rsDetailSO.Fields.Item("CustomerCode").Value.ToString();

            //    //ADD DELIVERY
            //    SAPbobsCOM.Recordset rsDelivery = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            //    rsDelivery = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpLoadingOrder_SapAddDeliverys\" ('" + Id.ToString() + "', '" + soDetId + "')");

            //    if (!rsDelivery.EoF)
            //    {
            //        SAPbobsCOM.Documents oDelivery = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
            //        if (rsDelivery.Fields.Item("Hdr_Series").Value.ToString() != "")
            //        {
            //            oDelivery.Series = int.Parse(rsDelivery.Fields.Item("Hdr_Series").Value.ToString());
            //        }
            //        oDelivery.CardCode = rsDelivery.Fields.Item("Hdr_CardCode").Value.ToString();

            //        string ControlAccount = rsDelivery.Fields.Item("Hdr_ControlAccount").Value.ToString();
            //        if (!string.IsNullOrEmpty(ControlAccount))
            //        {
            //            ControlAccount = ControlAccount.Replace("-", "");
            //            oDelivery.ControlAccount = SapCompany.RetCoaCode(oCompany, ControlAccount);
            //        }

            //        oDelivery.SalesPersonCode = int.Parse(rsDelivery.Fields.Item("Hdr_SlpCode").Value.ToString());

            //        oDelivery.DocDate = (DateTime)rsDelivery.Fields.Item("Hdr_DocDate").Value;
            //        oDelivery.DocDueDate = (DateTime)rsDelivery.Fields.Item("Hdr_DocDueDate").Value;

            //        oDelivery.DocCurrency = rsDelivery.Fields.Item("Hdr_DocCurrency").Value.ToString();
            //        oDelivery.DocRate = double.Parse(rsDelivery.Fields.Item("Hdr_DocRate").Value.ToString());

            //        if (rsDelivery.Fields.Item("Hdr_PaymentGroupCode").Value.ToString() != "")
            //        {
            //            oDelivery.PaymentGroupCode = int.Parse(rsDelivery.Fields.Item("Hdr_PaymentGroupCode").Value.ToString());
            //        }

            //        oDelivery.UserFields.Fields.Item("U_IDU_WebTransType").Value = rsDelivery.Fields.Item("Hdr_U_IDU_WebTransType").Value.ToString();
            //        oDelivery.UserFields.Fields.Item("U_IDU_WebTransNo").Value = rsDelivery.Fields.Item("Hdr_U_IDU_WebTransNo").Value.ToString();
            //        oDelivery.UserFields.Fields.Item("U_IDU_WebTransId").Value = rsDelivery.Fields.Item("Hdr_U_IDU_WebTransId").Value.ToString();
            //        oDelivery.UserFields.Fields.Item("U_IDU_WebUserId").Value = userId.ToString();
            //        oDelivery.UserFields.Fields.Item("U_Area").Value = rsDelivery.Fields.Item("Hdr_U_Area").Value.ToString();

            //        oDelivery.UserFields.Fields.Item("U_NamaTransportir").Value = rsDelivery.Fields.Item("Hdr_U_NamaTransportir").Value.ToString();
            //        oDelivery.UserFields.Fields.Item("U_NamaSupir").Value = rsDelivery.Fields.Item("Hdr_U_NamaSupir").Value.ToString();
            //        oDelivery.UserFields.Fields.Item("U_NoPolisi").Value = rsDelivery.Fields.Item("Hdr_U_NoPolisi").Value.ToString();
            //        oDelivery.UserFields.Fields.Item("U_PBBKB").Value = rsDelivery.Fields.Item("Hdr_PBBKB").Value.ToString();
            //        oDelivery.UserFields.Fields.Item("U_PPH22").Value = rsDelivery.Fields.Item("Hdr_PPH22").Value.ToString();
            //        oDelivery.UserFields.Fields.Item("U_PO_Cust").Value = rsDelivery.Fields.Item("U_PO_Cust").Value.ToString();
            //        oDelivery.UserFields.Fields.Item("U_BiayaKirim").Value = rsDelivery.Fields.Item("U_BiayaKirim").Value.ToString();
            //        oDelivery.UserFields.Fields.Item("U_CashKeras").Value = rsDelivery.Fields.Item("U_CashKeras").Value.ToString();
            //        oDelivery.UserFields.Fields.Item("U_IDU_RemarkSaos").Value = rsDelivery.Fields.Item("Remarks").Value.ToString();

            //        if (rsDelivery.Fields.Item("Hdr_DocType").Value.ToString() == "I")
            //        {
            //            oDelivery.DocType = BoDocumentTypes.dDocument_Items;
            //        }
            //        else
            //        {
            //            oDelivery.DocType = BoDocumentTypes.dDocument_Service;
            //        }

            //        if (rsDelivery.Fields.Item("Hdr_WTCode").Value.ToString() != "")
            //        {
            //            oDelivery.WithholdingTaxData.WTCode = rsDelivery.Fields.Item("Hdr_WTCode").Value.ToString();
            //            oDelivery.WithholdingTaxData.TaxableAmount = double.Parse(rsDelivery.Fields.Item("Hdr_TaxableAmount").Value.ToString());
            //            oDelivery.WithholdingTaxData.WTAmount = double.Parse(rsDelivery.Fields.Item("Hdr_WTAmount").Value.ToString());
            //        }

            //        int lineItem = 0;
            //        while (!rsDelivery.EoF)
            //        {

            //            int baseType = int.Parse(rsDelivery.Fields.Item("Det_BaseType").Value.ToString());

            //            if (baseType >= 0)
            //            {
            //                oDelivery.Lines.BaseType = int.Parse(rsDelivery.Fields.Item("Det_BaseType").Value.ToString());
            //                oDelivery.Lines.BaseEntry = int.Parse(rsDelivery.Fields.Item("Det_BaseEntry").Value.ToString());
            //                oDelivery.Lines.BaseLine = int.Parse(rsDelivery.Fields.Item("Det_BaseLine").Value.ToString());
            //            }

            //            if (rsDelivery.Fields.Item("Hdr_DocType").Value.ToString() == "I")
            //            {
            //                if (baseType != 0) //Additional Item
            //                {
            //                    oDelivery.Lines.ItemCode = rsDelivery.Fields.Item("Det_ItemCode").Value.ToString();
            //                    if (rsDelivery.Fields.Item("Det_ItemDescription").Value.ToString() != "")
            //                    {
            //                        oDelivery.Lines.ItemDescription = rsDelivery.Fields.Item("Det_ItemDescription").Value.ToString();
            //                    }
            //                    oDelivery.Lines.UnitPrice = double.Parse(rsDelivery.Fields.Item("Det_UnitPrice").Value.ToString());

            //                    if (rsDelivery.Fields.Item("Det_AccountCode").Value.ToString() != "")
            //                    {
            //                        string CoaCode = rsDelivery.Fields.Item("Det_AccountCode").Value.ToString();
            //                        CoaCode = CoaCode.Replace("-", "");
            //                        oDelivery.Lines.AccountCode = SapCompany.RetCoaCode(oCompany, CoaCode);
            //                    }

            //                    if (rsDelivery.Fields.Item("Det_ProjectCode").Value.ToString() != "")
            //                    {
            //                        oDelivery.Lines.ProjectCode = rsDelivery.Fields.Item("Det_ProjectCode").Value.ToString();
            //                    }

            //                    if (rsDelivery.Fields.Item("Det_CostingCode").Value.ToString() != "")
            //                    {
            //                        oDelivery.Lines.CostingCode = rsDelivery.Fields.Item("Det_CostingCode").Value.ToString();
            //                    }

            //                    if (rsDelivery.Fields.Item("Det_CostingCode2").Value.ToString() != "")
            //                    {
            //                        oDelivery.Lines.CostingCode2 = rsDelivery.Fields.Item("Det_CostingCode2").Value.ToString();
            //                    }

            //                    if (rsDelivery.Fields.Item("Det_CostingCode3").Value.ToString() != "")
            //                    {
            //                        oDelivery.Lines.CostingCode3 = rsDelivery.Fields.Item("Det_CostingCode3").Value.ToString();
            //                    }

            //                    if (rsDelivery.Fields.Item("Det_CostingCode4").Value.ToString() != "")
            //                    {
            //                        oDelivery.Lines.CostingCode4 = rsDelivery.Fields.Item("Det_CostingCode4").Value.ToString();
            //                    }

            //                    if (rsDelivery.Fields.Item("Det_CostingCode5").Value.ToString() != "")
            //                    {
            //                        oDelivery.Lines.CostingCode5 = rsDelivery.Fields.Item("Det_CostingCode5").Value.ToString();
            //                    }

            //                    oDelivery.Lines.TaxCode = rsDelivery.Fields.Item("Det_TaxCode").Value.ToString();
            //                    oDelivery.Lines.VatGroup = rsDelivery.Fields.Item("Det_VatGroup").Value.ToString();
            //                    oDelivery.Lines.LineTotal = double.Parse(rsDelivery.Fields.Item("Det_LineTotal").Value.ToString());

            //                }//Additional Item

            //                oDelivery.Lines.Quantity = double.Parse(rsDelivery.Fields.Item("Det_Quantity").Value.ToString());
            //                if (rsDelivery.Fields.Item("Det_WarehouseCode").Value.ToString() != "")
            //                {
            //                    oDelivery.Lines.WarehouseCode = rsDelivery.Fields.Item("Det_WarehouseCode").Value.ToString();
            //                }

            //                oDelivery.Lines.BinAllocations.BinAbsEntry = Int32.Parse(rsDelivery.Fields.Item("Det_BinAbsEntry").Value.ToString());
            //                oDelivery.Lines.BinAllocations.Quantity = double.Parse(rsDelivery.Fields.Item("Det_Quantity").Value.ToString());
            //                //oGoodReceiptPO.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;
            //                oDelivery.Lines.BinAllocations.AllowNegativeQuantity = BoYesNoEnum.tYES;
            //                oDelivery.Lines.BinAllocations.Add();

            //            }
            //            else
            //            {
            //                //oDelivery.Lines.ItemDescription = rsDelivery.Fields.Item("Det_ItemDescription").Value.ToString();
            //            }


            //            oDelivery.Lines.Add();

            //            lineItem += 1;
            //            rsDelivery.MoveNext();

            //        }

            //        if (oDelivery.Add() != 0)
            //        {

            //            nErr = oCompany.GetLastErrorCode();
            //            errMsg = oCompany.GetLastErrorDescription();
            //            throw new Exception("[VALIDATION] - Add Delivery | " + nErr.ToString() + "|" + errMsg);

            //        }

            //        string docEntry;
            //        docEntry = oCompany.GetNewObjectKey();

            //        string sqlUpdateSO;
            //        sqlUpdateSO = "UPDATE T0 SET   "
            //                + " T0.\"SapDeliveryId\"=" + docEntry + " "
            //                + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_LoadingOrder_SalesOrder\" T0 "
            //                + " WHERE T0.\"Id\"=" + Id.ToString() + " AND T0.\"DetId\" = " + soDetId;

            //        SapCompany.ExecuteQuery(oCompany, sqlUpdateSO);

            //    }

            //    //END ADD DELIVERY

            //    LineSo += 1;
            //    rsDetailSO.MoveNext();
            //}

            return true;
        }


        public void Post(int userId, long Id)
        {

            using (var CONTEXT = new HANA_APP())
            {

                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        String keyValue;
                        keyValue = Id.ToString();

                        SpNotif.SpSysControllerTransNotif(userId, "TukarFakturReceipt", CONTEXT, "before", "Tx_TukarFakturReceipt", "post", "Id", keyValue);

                        Tx_TukarFakturReceipt tx_TukarFakturReceipt = CONTEXT.Tx_TukarFakturReceipt.Find(Id);
                        if (tx_TukarFakturReceipt != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TukarFakturReceipt.Status = "Finished";
                            tx_TukarFakturReceipt.ModifiedDate = dtModified;
                            tx_TukarFakturReceipt.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }
                        //CONTEXT.Database.ExecuteSqlCommand("UPDATE T0 SET T0.\"Status\" = 'Finished' FROM \"Tx_TukarFakturSend\" T0 LEFT JOIN \"Tx_TukarFakturReceipt\" T1 ON T0.\"NoDokumen\" = T1.\"NoTtd\" WHERE T1.\"Id\"=:p0", keyValue);

                        SpNotif.SpSysControllerTransNotif(userId, "TukarFakturReceipt", CONTEXT, "after", "Tx_TukarFakturReceipt", "post", "Id", keyValue);

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

                        SpNotif.SpSysControllerTransNotif(userId, "TukarFakturReceipt", CONTEXT, "before", "Tx_TukarFakturReceipt", "cancel", "Id", keyValue);

                        Tx_TukarFakturReceipt tx_TukarFakturReceipt = CONTEXT.Tx_TukarFakturReceipt.Find(Id);
                        if (tx_TukarFakturReceipt != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TukarFakturReceipt.Status = "Cancel";
                            tx_TukarFakturReceipt.ModifiedDate = dtModified;
                            tx_TukarFakturReceipt.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "TukarFakturReceipt", CONTEXT, "after", "Tx_TukarFakturReceipt", "cancel", "Id", keyValue);


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

         

        public TukarFakturReceiptModel GetNewModel(int userId)
        {
            TukarFakturReceiptModel model = new TukarFakturReceiptModel();
            model.Status = "Draft";
            return model;
        }

        public TukarFakturReceiptModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public TukarFakturReceiptModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            TukarFakturReceiptModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.* 
                            FROM ""Tx_TukarFakturReceipt"" T0   
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<TukarFakturReceiptModel>(ssql, id).Single();

            }

            return model;
        }

        public TukarFakturReceiptModel NavFirst(int userId)
        {
            TukarFakturReceiptModel model = null;


            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TukarFakturReceipt");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TukarFakturReceipt\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public TukarFakturReceiptModel NavPrevious(int userId, long id = 0)
        {
            TukarFakturReceiptModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TukarFakturReceipt");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TukarFakturReceipt\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public TukarFakturReceiptModel NavNext(int userId, long id = 0)
        {
            TukarFakturReceiptModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TukarFakturReceipt");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TukarFakturReceipt\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public TukarFakturReceiptModel NavLast(int userId)
        {
            TukarFakturReceiptModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TukarFakturReceipt");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TukarFakturReceipt\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
    }


    #endregion

}