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



namespace Models.Transaction.KerusakanKapal
{
    #region Models

    public class KerusakanKapalModel
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

        public string Stop { get; set; }

        public string ReqPerbaikan { get; set; }

        public decimal? EstPerbaikan { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Lokasi { get; set; }

        public string Remark { get; set; }

        public string Status { get; set; }

        public string Status2 { get; set; }

        public string Os { get; set; }

        public List<KerusakanKapal_DetailModel> ListDetails_ = new List<KerusakanKapal_DetailModel>();

        public KerusakanKapal_Detail Details_ { get; set;}
    }
    public class KerusakanKapal_Detail    {
        public List<long> deletedRowKeys { get; set; }
        public List<KerusakanKapal_DetailModel> insertedRowValues { get; set; }
        public List<KerusakanKapal_DetailModel> modifiedRowValues { get; set; }
    }
    public class KerusakanKapal_DetailModel
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

        public string Item { get; set; }

        public string SubItem { get; set; }

        public string Penyebab { get; set; }

        public string Tindakan { get; set; }

        public string Part { get; set; }

        public decimal? Qty { get; set; }

        public string UoM { get; set; }

        public string EstPerbaikan { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Status { get; set; }

        public string Status2 { get; set; }

        public string PIC { get; set; }

        public string Remark { get; set; }
    }


    #endregion

    #region Services

    public class KerusakanKapalService
    {

        public KerusakanKapalModel GetNewModel(int userId)
        {
            KerusakanKapalModel model = new KerusakanKapalModel();
            model.Status = "Open";
            return model;
        }
        public KerusakanKapalModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public KerusakanKapalModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            KerusakanKapalModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, T1.""FirstName"" AS ""UserName"" 
                            FROM ""Tx_KerusakanKapal"" T0
                            LEFT JOIN ""Tm_User"" T1 ON T0.""ModifiedUser"" = T1.""Id""
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<KerusakanKapalModel>(ssql, id).Single();

                model.ListDetails_ = this.KerusakanKapal_Details(CONTEXT, id);
            }

            return model;
        }
        public List<KerusakanKapal_DetailModel> KerusakanKapal_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return KerusakanKapal_Details(CONTEXT, id);
            }

        }

        public List<KerusakanKapal_DetailModel> KerusakanKapal_Details(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<KerusakanKapal_DetailModel>("SELECT * FROM \"Tx_KerusakanKapal_Detail\" WHERE \"Id\" =:p0", id).ToList();
        }
        public KerusakanKapalModel NavFirst(int userId)
        {
            KerusakanKapalModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_KerusakanKapal\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public KerusakanKapalModel NavPrevious(int userId, long id = 0)
        {
            KerusakanKapalModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_KerusakanKapal\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public KerusakanKapalModel NavNext(int userId, long id = 0)
        {
            KerusakanKapalModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_KerusakanKapal\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public KerusakanKapalModel NavLast(int userId)
        {
            KerusakanKapalModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_KerusakanKapal\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        public long Add(KerusakanKapalModel model)
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

                            Tx_KerusakanKapal Tx_KerusakanKapal = new Tx_KerusakanKapal();
                            CopyProperty.CopyProperties(model, Tx_KerusakanKapal, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_KerusakanKapal.TransType = "KerusakanKapal";
                            Tx_KerusakanKapal.CreatedDate = dtModified;
                            Tx_KerusakanKapal.CreatedUser = model._UserId;
                            Tx_KerusakanKapal.ModifiedDate = dtModified;
                            Tx_KerusakanKapal.ModifiedUser = model._UserId;
                            if (model.StartDate != null)
                            {
                                Tx_KerusakanKapal.Status2 = "On Progress";
                            }
                            else
                            {
                                Tx_KerusakanKapal.Status2 = "Open";
                            }

                            if (model.EndDate != null)
                            {
                                Tx_KerusakanKapal.Status2 = "Close";
                            }

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'KerusakanKapal','" + dateX + "','') ").SingleOrDefault();
                            Tx_KerusakanKapal.TransNo = transNo;

                            CONTEXT.Tx_KerusakanKapal.Add(Tx_KerusakanKapal);
                            CONTEXT.SaveChanges();
                            Id = Tx_KerusakanKapal.Id;

                            String keyValue;
                            keyValue = Tx_KerusakanKapal.Id.ToString();
                            
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
                                        KerusakanKapal_DetailModel detailModel = new KerusakanKapal_DetailModel();
                                        detailModel.DetId = detId;
                                        Detail_Delete(CONTEXT, detailModel);
                                    }
                                }
                            }



                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_KerusakanKapal", "add", "Id", keyValue);


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
        public void Update(KerusakanKapalModel model)
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


                                Tx_KerusakanKapal Tx_KerusakanKapal = CONTEXT.Tx_KerusakanKapal.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_KerusakanKapal.ModifiedDate = dtModified;
                                Tx_KerusakanKapal.ModifiedUser = model._UserId;
                                
                                if (Tx_KerusakanKapal != null)
                                {
                                    var exceptColumns = new string[] { "Id","TransNo","CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_KerusakanKapal, false, exceptColumns);
                                    Tx_KerusakanKapal.ModifiedDate = dtModified;
                                    Tx_KerusakanKapal.ModifiedUser = model._UserId;
                                    if (model.StartDate != null)
                                    {
                                        Tx_KerusakanKapal.Status2 = "On Progress";
                                    }
                                    else
                                    {
                                        Tx_KerusakanKapal.Status2 = "Open";
                                    }

                                    if (model.EndDate != null)
                                    {
                                        Tx_KerusakanKapal.Status2 = "Close";
                                    }
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
                                                KerusakanKapal_DetailModel detailModel = new KerusakanKapal_DetailModel();
                                                detailModel.DetId = detId;
                                               Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }

                                    
                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", " Tx_KerusakanKapal", "update", "Id", keyValue);

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
        public long Detail_Add(HANA_APP CONTEXT, KerusakanKapal_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_KerusakanKapal_Detail Tx_KerusakanKapal_Detail = new Tx_KerusakanKapal_Detail();

                CopyProperty.CopyProperties(model, Tx_KerusakanKapal_Detail, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_KerusakanKapal_Detail.Id = Id;
                Tx_KerusakanKapal_Detail.CreatedDate = dtModified;
                Tx_KerusakanKapal_Detail.CreatedUser = UserId;
                Tx_KerusakanKapal_Detail.ModifiedDate = dtModified;
                Tx_KerusakanKapal_Detail.ModifiedUser = UserId;
                if (model.StartDate != null && model.EndDate == null)
                {
                    Tx_KerusakanKapal_Detail.Status = "On Progress";
                }
                else if (model.StartDate != null && model.EndDate != null)
                {
                    Tx_KerusakanKapal_Detail.Status = "Close";
                }
                else
                {
                    Tx_KerusakanKapal_Detail.Status = "Open";
                }

                CONTEXT.Tx_KerusakanKapal_Detail.Add(Tx_KerusakanKapal_Detail);
                CONTEXT.SaveChanges();
                DetId = Tx_KerusakanKapal_Detail.DetId;

            }

            return DetId;

        }
        public void Detail_Update(HANA_APP CONTEXT, KerusakanKapal_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_KerusakanKapal_Detail Tx_KerusakanKapal_Detail = CONTEXT.Tx_KerusakanKapal_Detail.Find(model.DetId);

                if (Tx_KerusakanKapal_Detail != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_KerusakanKapal_Detail, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_KerusakanKapal_Detail.ModifiedDate = dtModified;
                    Tx_KerusakanKapal_Detail.ModifiedUser = UserId;
                    if (model.StartDate != null && model.EndDate == null)
                    {
                        Tx_KerusakanKapal_Detail.Status = "On Progress";
                    }
                    else if(model.StartDate != null && model.EndDate != null)
                    {
                        Tx_KerusakanKapal_Detail.Status = "Close";
                    }
                    else
                    {
                        Tx_KerusakanKapal_Detail.Status = "Open";
                    }
                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Detail_Delete(HANA_APP CONTEXT, KerusakanKapal_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_KerusakanKapal_Detail\"  WHERE \"DetId\"=:p0", model.DetId);

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
                SpNotif.SpSysControllerTransNotif(userId, "KerusakanKapal", oCompany, "before", "KerusakanKapal", "post", "Id", keyValue);

                AddPR(oCompany, userId, Id);

                var sql1 = "UPDATE T0 SET   "
                        + " T0.\"Status\"='Posted',"
                        + " T0.\"IsAfterPosted\"='Y',"
                        + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                        + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP, "
                        + " T0.\"SapDocNum\"= T1.\"DocNum\" "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_KerusakanKapal\" T0 "
                        + " LEFT JOIN \"OPRQ\" T1 ON T0.\"SapDocEntry\" = T1.\"DocEntry\" "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);

                SpNotif.SpSysControllerTransNotif(userId, "KerusakanKapal", oCompany, "after", "KerusakanKapal", "post", "Id", keyValue);

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
            ssql = string.Format(ssql, Id, "KerusakanKapal");
            string tempId = _Utils.SapCompany.RetRstField(oCompany, ssql);
            if (!string.IsNullOrEmpty(tempId))
            {
                return false;
            }

            //ADD GRPO
            SAPbobsCOM.Recordset rsPR = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsPR = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpKerusakanKapal_SapAddPR\" ('" + Id.ToString() + "')");

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
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_KerusakanKapal\" T0 "
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