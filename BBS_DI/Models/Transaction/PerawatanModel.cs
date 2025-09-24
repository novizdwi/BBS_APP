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



namespace Models.Transaction.Perawatan
{
    #region Models

    public class PerawatanModel
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
        
        public string TransType { get; set; }

        public DateTime? TransDate { get; set; }

        [Required(ErrorMessage = "required")]
        public int? ShipId { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipName { get; set; }


        [Required(ErrorMessage = "required")]
        public string ShipCode { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }


        public List<Perawatan_DetailModel> ListDetails_ = new List<Perawatan_DetailModel>();

        public Perawatan_Detail Details_ { get; set;}

        public List<Perawatan_AttachmentModel> ListAttachments_ = new List<Perawatan_AttachmentModel>();

        public Perawatan_Attachments Attachments_ { get; set; }
    }
    public class Perawatan_Detail    {
        public List<long> deletedRowKeys { get; set; }
        public List<Perawatan_DetailModel> insertedRowValues { get; set; }
        public List<Perawatan_DetailModel> modifiedRowValues { get; set; }
    }
    public class Perawatan_DetailModel
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
        
        public string Bagian { get; set; }

        public string Item { get; set; }

        public string SubItem { get; set; }

        public string Uraian { get; set; }

        public string Material { get; set; }

        public string PIC { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Remark { get; set; }
    }
    public class Perawatan_Attachments
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Perawatan_AttachmentModel> insertedRowValues { get; set; }
        public List<Perawatan_AttachmentModel> modifiedRowValues { get; set; }
    }
    public class Perawatan_AttachmentModel
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

    public class PerawatanService
    {

        public PerawatanModel GetNewModel(int userId)
        {
            PerawatanModel model = new PerawatanModel();
            model.Status = "Draft";
            return model;
        }
        public PerawatanModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public PerawatanModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            PerawatanModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, T1.""FirstName"" AS ""UserName"" 
                            FROM ""Tx_Perawatan"" T0
                            LEFT JOIN ""Tm_User"" T1 ON T0.""ModifiedUser"" = T1.""Id""
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<PerawatanModel>(ssql, id).Single();

                model.ListDetails_ = this.Perawatan_Details(CONTEXT, id);
            }

            return model;
        }
        public List<Perawatan_DetailModel> Perawatan_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Perawatan_Details(CONTEXT, id);
            }

        }

        public List<Perawatan_DetailModel> Perawatan_Details(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Perawatan_DetailModel>("SELECT * FROM \"Tx_Perawatan_Detail\" WHERE \"Id\" =:p0", id).ToList();
        }
        public PerawatanModel NavFirst(int userId)
        {
            PerawatanModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Perawatan\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public PerawatanModel NavPrevious(int userId, long id = 0)
        {
            PerawatanModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Perawatan\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public PerawatanModel NavNext(int userId, long id = 0)
        {
            PerawatanModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Perawatan\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public PerawatanModel NavLast(int userId)
        {
            PerawatanModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Perawatan\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        public long Add(PerawatanModel model)
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

                            Tx_Perawatan Tx_Perawatan = new Tx_Perawatan();
                            CopyProperty.CopyProperties(model, Tx_Perawatan, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_Perawatan.TransType = "Perawatan";
                            Tx_Perawatan.CreatedDate = dtModified;
                            Tx_Perawatan.CreatedUser = model._UserId;
                            Tx_Perawatan.ModifiedDate = dtModified;
                            Tx_Perawatan.ModifiedUser = model._UserId;

                            //string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = "MAIN/DECK/" + model.ShipCode;
                            Tx_Perawatan.TransNo = transNo;

                            string cek = CONTEXT.Database.SqlQuery<string>(@"SELECT ""TransNo"" FROM  ""Tx_Perawatan"" WHERE ""TransNo""='" + transNo + "'").FirstOrDefault();
                            if (cek == null)
                            {
                                CONTEXT.Tx_Perawatan.Add(Tx_Perawatan);
                                CONTEXT.SaveChanges();
                                Id = Tx_Perawatan.Id;

                                String keyValue;
                                keyValue = Tx_Perawatan.Id.ToString();

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
                                            Perawatan_DetailModel detailModel = new Perawatan_DetailModel();
                                            detailModel.DetId = detId;
                                            Detail_Delete(CONTEXT, detailModel);
                                        }
                                    }
                                }



                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_Perawatan", "add", "Id", keyValue);


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
        public void Update(PerawatanModel model)
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


                                Tx_Perawatan Tx_Perawatan = CONTEXT.Tx_Perawatan.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_Perawatan.ModifiedDate = dtModified;
                                Tx_Perawatan.ModifiedUser = model._UserId;
                                if (Tx_Perawatan != null)
                                {
                                    var exceptColumns = new string[] { "Id","TransNo","CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_Perawatan, false, exceptColumns);
                                    Tx_Perawatan.ModifiedDate = dtModified;
                                    Tx_Perawatan.ModifiedUser = model._UserId;
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
                                                Perawatan_DetailModel detailModel = new Perawatan_DetailModel();
                                                detailModel.DetId = detId;
                                               Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }

                                    
                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", " Tx_Perawatan", "update", "Id", keyValue);

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
        public long Detail_Add(HANA_APP CONTEXT, Perawatan_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_Perawatan_Detail Tx_Perawatan_Detail = new Tx_Perawatan_Detail();

                CopyProperty.CopyProperties(model, Tx_Perawatan_Detail, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_Perawatan_Detail.Id = Id;
                Tx_Perawatan_Detail.CreatedDate = dtModified;
                Tx_Perawatan_Detail.CreatedUser = UserId;
                Tx_Perawatan_Detail.ModifiedDate = dtModified;
                Tx_Perawatan_Detail.ModifiedUser = UserId;
               

                CONTEXT.Tx_Perawatan_Detail.Add(Tx_Perawatan_Detail);
                CONTEXT.SaveChanges();
                DetId = Tx_Perawatan_Detail.DetId;

            }

            return DetId;

        }
        public void Detail_Update(HANA_APP CONTEXT, Perawatan_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_Perawatan_Detail Tx_Perawatan_Detail = CONTEXT.Tx_Perawatan_Detail.Find(model.DetId);

                if (Tx_Perawatan_Detail != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_Perawatan_Detail, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_Perawatan_Detail.ModifiedDate = dtModified;
                    Tx_Perawatan_Detail.ModifiedUser = UserId;
                   
                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Detail_Delete(HANA_APP CONTEXT, Perawatan_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_Perawatan_Detail\"  WHERE \"DetId\"=:p0", model.DetId);

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
                SpNotif.SpSysControllerTransNotif(userId, "Perawatan", oCompany, "before", "Perawatan", "post", "Id", keyValue);

                AddPR(oCompany, userId, Id);

                var sql1 = "UPDATE T0 SET   "
                        + " T0.\"Status\"='Posted',"
                        + " T0.\"IsAfterPosted\"='Y',"
                        + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                        + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP, "
                        + " T0.\"SapDocNum\"= T1.\"DocNum\" "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_Perawatan\" T0 "
                        + " LEFT JOIN \"OPRQ\" T1 ON T0.\"SapDocEntry\" = T1.\"DocEntry\" "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);

                SpNotif.SpSysControllerTransNotif(userId, "Perawatan", oCompany, "after", "Perawatan", "post", "Id", keyValue);

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
            ssql = string.Format(ssql, Id, "Perawatan");
            string tempId = _Utils.SapCompany.RetRstField(oCompany, ssql);
            if (!string.IsNullOrEmpty(tempId))
            {
                return false;
            }

            //ADD GRPO
            SAPbobsCOM.Recordset rsPR = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsPR = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpPerawatan_SapAddPR\" ('" + Id.ToString() + "')");

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
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_Perawatan\" T0 "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sqlUpdateSO);
            }

            //END ADD DELIVERY

            //throw new Exception("[VALIDATION] - Lagi test jangan di save dulu");

            return true;
        }
        public List<Perawatan_AttachmentModel> Perawatan_Attachments(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Perawatan_Attachments(CONTEXT, id);
            }

        }
        public List<Perawatan_AttachmentModel> Perawatan_Attachments(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Perawatan_AttachmentModel>("SELECT T0.\"Id\", T0.\"DetId\", T0.\"FileName\" FROM \"Tx_Perawatan_Attachment\" T0 WHERE T0.\"Id\"=:p0 ORDER BY T0.\"DetId\" ", id).ToList();


        }
        public long Attachment_Add(HANA_APP CONTEXT, Perawatan_AttachmentModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_Perawatan_Attachment tx_Perawatan_Attachment = new Tx_Perawatan_Attachment();

                CopyProperty.CopyProperties(model, tx_Perawatan_Attachment, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_Perawatan_Attachment.Id = Id;
                tx_Perawatan_Attachment.CreatedDate = dtModified;
                tx_Perawatan_Attachment.CreatedUser = UserId;
                tx_Perawatan_Attachment.ModifiedDate = dtModified;
                tx_Perawatan_Attachment.ModifiedUser = UserId;

                CONTEXT.Tx_Perawatan_Attachment.Add(tx_Perawatan_Attachment);
                CONTEXT.SaveChanges();
                DetId = tx_Perawatan_Attachment.DetId;

            }

            return DetId;

        }
        public long Attachment_Add(List<Perawatan_AttachmentModel> ListModel)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Attachment_Add(CONTEXT, ListModel);
            }

        }
        public long Attachment_Add(HANA_APP CONTEXT, List<Perawatan_AttachmentModel> ListModel)
        {
            long Id = 0;
            long DetId = 0;

            if (ListModel != null)
            {

                for (int i = 0; i < ListModel.Count; i++)
                {
                    Tx_Perawatan_Attachment tx_Perawatan_Attachment = new Tx_Perawatan_Attachment();
                    var model = ListModel[i];

                    CopyProperty.CopyProperties(model, tx_Perawatan_Attachment, false);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    tx_Perawatan_Attachment.Id = model.Id;
                    tx_Perawatan_Attachment.CreatedDate = dtModified;
                    tx_Perawatan_Attachment.CreatedUser = model._UserId;
                    tx_Perawatan_Attachment.ModifiedDate = dtModified;
                    tx_Perawatan_Attachment.ModifiedUser = model._UserId;

                    CONTEXT.Tx_Perawatan_Attachment.Add(tx_Perawatan_Attachment);
                    CONTEXT.SaveChanges();
                    DetId = tx_Perawatan_Attachment.DetId;
                }



            }

            return Id;

        }
        public void Attachment_Delete(Perawatan_AttachmentModel model)
        {
            if (model.DetId != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    Attachment_Delete(CONTEXT, model);
                }
            }

        }
        public void Attachment_Delete(HANA_APP CONTEXT, Perawatan_AttachmentModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_Perawatan_Attachment\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }


    }


    #endregion

}