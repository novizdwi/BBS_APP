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



namespace Models.Transaction.FreshWaterControl
{
    #region Models

    public class FreshWaterControlModel
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

        [Required(ErrorMessage = "required")]
        public int? ShipId { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipName { get; set; }


        [Required(ErrorMessage = "required")]
        public string ShipCode { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }


        public List<FreshWaterControl_DetailModel> ListDetails_ = new List<FreshWaterControl_DetailModel>();

        public FreshWaterControl_Detail Details_ { get; set;}
    }
    public class FreshWaterControl_Detail    {
        public List<long> deletedRowKeys { get; set; }
        public List<FreshWaterControl_DetailModel> insertedRowValues { get; set; }
        public List<FreshWaterControl_DetailModel> modifiedRowValues { get; set; }
    }
    public class FreshWaterControl_DetailModel
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

        public DateTime? Date { get; set; }

        //[Required(ErrorMessage = "required")]
        public int? RowNum { get; set; }

        [Required(ErrorMessage = "required")]
        public int? ShipId { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipCode { get; set; }

        public string Status { get; set; }

        public decimal? Voyage { get; set; }

        public decimal? ROB { get; set; }

        public string Supplier { get; set; }

        public decimal? QtyTerima { get; set; }

        public decimal? QtyTotal { get; set; }

        public decimal? Pemakaian { get; set; }

        public decimal? Balance { get; set; }

        public string Customer { get; set; }

        public string Route { get; set; }

        public string Remark { get; set; }
    }


    #endregion

    #region Services

    public class FreshWaterControlService
    {

        public FreshWaterControlModel GetNewModel(int userId)
        {
            FreshWaterControlModel model = new FreshWaterControlModel();
            model.Status = "Draft";
            return model;
        }
        public FreshWaterControlModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public FreshWaterControlModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            FreshWaterControlModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, T1.""FirstName"" AS ""UserName"" 
                            FROM ""Tx_FreshWaterControl"" T0
                            LEFT JOIN ""Tm_User"" T1 ON T0.""ModifiedUser"" = T1.""Id""
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<FreshWaterControlModel>(ssql, id).Single();

                model.ListDetails_ = this.FreshWaterControl_Details(CONTEXT, id);
            }

            return model;
        }
        public List<FreshWaterControl_DetailModel> FreshWaterControl_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return FreshWaterControl_Details(CONTEXT, id);
            }

        }

        public List<FreshWaterControl_DetailModel> FreshWaterControl_Details(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<FreshWaterControl_DetailModel>("SELECT * FROM \"Tx_FreshWaterControl_Detail\" WHERE \"Id\" =:p0", id).ToList();
        }
        public FreshWaterControlModel NavFirst(int userId)
        {
            FreshWaterControlModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_FreshWaterControl\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public FreshWaterControlModel NavPrevious(int userId, long id = 0)
        {
            FreshWaterControlModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_FreshWaterControl\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public FreshWaterControlModel NavNext(int userId, long id = 0)
        {
            FreshWaterControlModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_FreshWaterControl\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public FreshWaterControlModel NavLast(int userId)
        {
            FreshWaterControlModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_FreshWaterControl\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        public long Add(FreshWaterControlModel model)
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

                            Tx_FreshWaterControl Tx_FreshWaterControl = new Tx_FreshWaterControl();
                            CopyProperty.CopyProperties(model, Tx_FreshWaterControl, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_FreshWaterControl.TransType = "FreshWaterControl";
                            Tx_FreshWaterControl.CreatedDate = dtModified;
                            Tx_FreshWaterControl.CreatedUser = model._UserId;
                            Tx_FreshWaterControl.ModifiedDate = dtModified;
                            Tx_FreshWaterControl.ModifiedUser = model._UserId;

                            //string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = "FWC/" + model.ShipCode;
                            Tx_FreshWaterControl.TransNo = transNo;

                            string cek = CONTEXT.Database.SqlQuery<string>(@"SELECT ""TransNo"" FROM  ""Tx_FreshWaterControl"" WHERE ""TransNo""='" + transNo + "'").FirstOrDefault();
                            if (cek == null)
                            {
                                CONTEXT.Tx_FreshWaterControl.Add(Tx_FreshWaterControl);
                                CONTEXT.SaveChanges();
                                Id = Tx_FreshWaterControl.Id;

                                String keyValue;
                                keyValue = Tx_FreshWaterControl.Id.ToString();

                                if (model.Details_ != null)
                                {
                                    if (model.Details_.insertedRowValues != null)
                                    {
                                        foreach (var detail in model.Details_.insertedRowValues)
                                        {
                                            Detail_Add(CONTEXT, detail, Id, model._UserId);
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
                                            FreshWaterControl_DetailModel detailModel = new FreshWaterControl_DetailModel();
                                            detailModel.DetId = detId;
                                            Detail_Delete(CONTEXT, detailModel);
                                        }
                                    }
                                }



                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_FreshWaterControl", "add", "Id", keyValue);


                                CONTEXT_TRANS.Commit();
                            }
                            else
                            {
                                throw new Exception("Nomor transaksi sudah ada di database.");
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
        public void Update(FreshWaterControlModel model)
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

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", " Tm_Ship", "update", "Id", keyValue);


                                Tx_FreshWaterControl Tx_FreshWaterControl = CONTEXT.Tx_FreshWaterControl.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_FreshWaterControl.ModifiedDate = dtModified;
                                Tx_FreshWaterControl.ModifiedUser = model._UserId;
                                if (Tx_FreshWaterControl != null)
                                {
                                    var exceptColumns = new string[] { "Id","TransNo","CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_FreshWaterControl, false, exceptColumns);
                                    Tx_FreshWaterControl.ModifiedDate = dtModified;
                                    Tx_FreshWaterControl.ModifiedUser = model._UserId;
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
                                                FreshWaterControl_DetailModel detailModel = new FreshWaterControl_DetailModel();
                                                detailModel.DetId = detId;
                                               Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }

                                    
                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", " Tx_FreshWaterControl", "update", "Id", keyValue);

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
        public long Detail_Add(HANA_APP CONTEXT, FreshWaterControl_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_FreshWaterControl_Detail Tx_FreshWaterControl_Detail = new Tx_FreshWaterControl_Detail();

                CopyProperty.CopyProperties(model, Tx_FreshWaterControl_Detail, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_FreshWaterControl_Detail.Id = Id;
                Tx_FreshWaterControl_Detail.CreatedDate = dtModified;
                Tx_FreshWaterControl_Detail.CreatedUser = UserId;
                Tx_FreshWaterControl_Detail.ModifiedDate = dtModified;
                Tx_FreshWaterControl_Detail.ModifiedUser = UserId;


                CONTEXT.Tx_FreshWaterControl_Detail.Add(Tx_FreshWaterControl_Detail);
                CONTEXT.SaveChanges();
                DetId = Tx_FreshWaterControl_Detail.DetId;

            }

            return DetId;

        }
        public void Detail_Update(HANA_APP CONTEXT, FreshWaterControl_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_FreshWaterControl_Detail Tx_FreshWaterControl_Detail = CONTEXT.Tx_FreshWaterControl_Detail.Find(model.DetId);

                if (Tx_FreshWaterControl_Detail != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_FreshWaterControl_Detail, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_FreshWaterControl_Detail.ModifiedDate = dtModified;
                    Tx_FreshWaterControl_Detail.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Detail_Delete(HANA_APP CONTEXT, FreshWaterControl_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_FreshWaterControl_Detail\"  WHERE \"DetId\"=:p0", model.DetId);

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
                SpNotif.SpSysControllerTransNotif(userId, "FreshWaterControl", oCompany, "before", "FreshWaterControl", "post", "Id", keyValue);

                AddPR(oCompany, userId, Id);

                var sql1 = "UPDATE T0 SET   "
                        + " T0.\"Status\"='Posted',"
                        + " T0.\"IsAfterPosted\"='Y',"
                        + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                        + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP, "
                        + " T0.\"SapDocNum\"= T1.\"DocNum\" "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_FreshWaterControl\" T0 "
                        + " LEFT JOIN \"OPRQ\" T1 ON T0.\"SapDocEntry\" = T1.\"DocEntry\" "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);

                SpNotif.SpSysControllerTransNotif(userId, "FreshWaterControl", oCompany, "after", "FreshWaterControl", "post", "Id", keyValue);

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
        public bool AddPR(SAPbobsCOM.Company oCompany, int userId, long Id)
        {
            int nErr;
            string errMsg;

            string ssql = "SELECT TOP 1 T0.\"DocEntry\" FROM \"OPRQ\" T0 WHERE T0.\"U_IDU_WebTransId\"='{0}' AND T0.\"U_IDU_WebTransType\"='{1}'";
            ssql = string.Format(ssql, Id, "FreshWaterControl");
            string tempId = _Utils.SapCompany.RetRstField(oCompany, ssql);
            if (!string.IsNullOrEmpty(tempId))
            {
                return false;
            }

            //ADD GRPO
            SAPbobsCOM.Recordset rsPR = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsPR = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpFreshWaterControl_SapAddPR\" ('" + Id.ToString() + "')");

            if (!rsPR.EoF)
            {
                SAPbobsCOM.Documents oPurchaseRequest = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseRequest);
                oPurchaseRequest.Series = rsPR.Fields.Item("Hdr_Series").Value;

                
                

                oPurchaseRequest.DocDate = (DateTime)rsPR.Fields.Item("Hdr_DocDate").Value;
                oPurchaseRequest.DocDueDate = (DateTime)rsPR.Fields.Item("Hdr_DocDueDate").Value;
                oPurchaseRequest.RequriedDate = (DateTime)rsPR.Fields.Item("Hdr_DocDueDate").Value;
                oPurchaseRequest.DocCurrency = rsPR.Fields.Item("Hdr_DocCurrency").Value.ToString();
                oPurchaseRequest.DocRate = double.Parse(rsPR.Fields.Item("Hdr_DocRate").Value.ToString());

                if (rsPR.Fields.Item("Hdr_PaymentGroupCode").Value.ToString() != "")
                {
                    oPurchaseRequest.PaymentGroupCode = int.Parse(rsPR.Fields.Item("Hdr_PaymentGroupCode").Value.ToString());
                }

                if (rsPR.Fields.Item("Hdr_DocType").Value.ToString() == "I")
                {
                    oPurchaseRequest.DocType = BoDocumentTypes.dDocument_Items;
                }
                else
                {
                    oPurchaseRequest.DocType = BoDocumentTypes.dDocument_Service;
                }

                oPurchaseRequest.UserFields.Fields.Item("U_IDU_WebTransType").Value = rsPR.Fields.Item("Hdr_U_IDU_WebTransType").Value.ToString();
                oPurchaseRequest.UserFields.Fields.Item("U_IDU_WebTransNo").Value = rsPR.Fields.Item("Hdr_U_IDU_WebTransNo").Value.ToString();
                oPurchaseRequest.UserFields.Fields.Item("U_IDU_WebTransId").Value = rsPR.Fields.Item("Hdr_U_IDU_WebTransId").Value.ToString();
                oPurchaseRequest.UserFields.Fields.Item("U_IDU_WebUserId").Value = userId.ToString();
                
                int lineItem = 0;
                while (!rsPR.EoF)
                {
                    if (rsPR.Fields.Item("Hdr_DocType").Value.ToString() == "I")
                    {
                        oPurchaseRequest.Lines.ItemCode = rsPR.Fields.Item("Det_ItemCode").Value.ToString();
                        if (rsPR.Fields.Item("Det_ItemDscription").Value.ToString() != "")
                        {
                            oPurchaseRequest.Lines.ItemDescription = rsPR.Fields.Item("Det_ItemDscription").Value.ToString();
                        }
                        oPurchaseRequest.Lines.ProjectCode = rsPR.Fields.Item("Det_Project").Value.ToString();
                        oPurchaseRequest.Lines.Quantity = double.Parse(rsPR.Fields.Item("Det_Qty").Value.ToString());
                        oPurchaseRequest.Lines.UoMEntry = rsPR.Fields.Item("Det_UomEntry").Value;
                        //oPurchaseRequest.Lines.UoMCode = rsPR.Fields.Item("Det_UomCode");
                        if (rsPR.Fields.Item("Det_WhsCode").Value.ToString() != "")
                        {
                            oPurchaseRequest.Lines.WarehouseCode = rsPR.Fields.Item("Det_WhsCode").Value.ToString();
                        }
                        oPurchaseRequest.Lines.FreeText = rsPR.Fields.Item("Det_FreeText").Value.ToString();

                        

                    }
                    else
                    {
                        
                    }


                    oPurchaseRequest.Lines.Add();

                    lineItem += 1;
                    rsPR.MoveNext();

                }

                if (oPurchaseRequest.Add() != 0)
                {

                    nErr = oCompany.GetLastErrorCode();
                    errMsg = oCompany.GetLastErrorDescription();
                    throw new Exception("[VALIDATION] - Add Purchase Request | " + nErr.ToString() + "|" + errMsg);

                }

                string docEntry;
                docEntry = oCompany.GetNewObjectKey();

                string sqlUpdateSO;
                sqlUpdateSO = "UPDATE T0 SET   "
                        + " T0.\"SapDocEntry\"=" + docEntry + " "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_FreshWaterControl\" T0 "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sqlUpdateSO);
            }

            //END ADD DELIVERY

            //throw new Exception("[VALIDATION] - Lagi test jangan di save dulu");

            return true;
        }



    }


    #endregion

}