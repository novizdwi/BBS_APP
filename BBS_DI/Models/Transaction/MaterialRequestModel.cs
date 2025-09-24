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



namespace Models.Transaction.MaterialRequest
{
    #region Models

    public class MaterialRequestModel
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
        public int? ShipId { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipSection { get; set; }

        public string Remark { get; set; }

        [Required(ErrorMessage = "required")]
        public string WarehouseCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string WarehouseName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ReqType { get; set; }

        public int? SapDocEntry { get; set; }

        public int? SapDocNum { get; set; }

        public string RefNum { get; set; }

        public string RefType { get; set; }

        public string Kategori { get; set; }


        public List<MaterialRequest_DetailModel> ListDetails_ = new List<MaterialRequest_DetailModel>();

        public MaterialRequest_Detail Details_ { get; set;}

        public List<MaterialRequest_AttachmentModel> ListAttachments_ = new List<MaterialRequest_AttachmentModel>();

        public MaterialRequest_Attachments Attachments_ { get; set; }
    }
    public class MaterialRequest_Detail    {
        public List<long> deletedRowKeys { get; set; }
        public List<MaterialRequest_DetailModel> insertedRowValues { get; set; }
        public List<MaterialRequest_DetailModel> modifiedRowValues { get; set; }
    }
    public class MaterialRequest_DetailModel
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

        [Required(ErrorMessage = "required")]
        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        [Required(ErrorMessage = "required")]
        public decimal? Qty  { get; set; }

        public string UoMCode { get; set; }

        public int? UomEntry { get; set; }

        public decimal? ROB { get; set; }

        public string Remark { get; set; }

        public int? SapDocEntry { get; set; }

        public int? SapDocNum { get; set; }

        
    }
    public class MaterialRequest_Attachments
    {
        public List<long> deletedRowKeys { get; set; }
        public List<MaterialRequest_AttachmentModel> insertedRowValues { get; set; }
        public List<MaterialRequest_AttachmentModel> modifiedRowValues { get; set; }
    }
    public class MaterialRequest_AttachmentModel
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

    public class MaterialRequestService
    {

        public MaterialRequestModel GetNewModel(int userId)
        {
            MaterialRequestModel model = new MaterialRequestModel();
            model.Status = "Draft";
            return model;
        }
        public MaterialRequestModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }
        public MaterialRequestModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            MaterialRequestModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.* 
                            FROM ""Tx_MaterialRequest"" T0   
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<MaterialRequestModel>(ssql, id).Single();

                model.ListDetails_ = this.MaterialRequest_Details(CONTEXT, id);
                model.ListAttachments_ = this.MaterialRequest_Attachments(CONTEXT, id);
            }

            return model;
        }
        public List<MaterialRequest_DetailModel> MaterialRequest_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return MaterialRequest_Details(CONTEXT, id);
            }

        }
        public List<MaterialRequest_DetailModel> MaterialRequest_Details(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<MaterialRequest_DetailModel>("SELECT * FROM \"Tx_MaterialRequest_Detail\" WHERE \"Id\" =:p0", id).ToList();
        }
        public MaterialRequestModel NavFirst(int userId)
        {
            MaterialRequestModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "MaterialRequest");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_MaterialRequest\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public MaterialRequestModel NavPrevious(int userId, long id = 0)
        {
            MaterialRequestModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "MaterialRequest");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_MaterialRequest\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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
        public MaterialRequestModel NavNext(int userId, long id = 0)
        {
            MaterialRequestModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "MaterialRequest");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_MaterialRequest\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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
        public MaterialRequestModel NavLast(int userId)
        {
            MaterialRequestModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "MaterialRequest");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_MaterialRequest\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        public long Add(MaterialRequestModel model)
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

                            Tx_MaterialRequest Tx_MaterialRequest = new Tx_MaterialRequest();
                            CopyProperty.CopyProperties(model, Tx_MaterialRequest, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_MaterialRequest.TransType = "MaterialRequest";
                            Tx_MaterialRequest.CreatedDate = dtModified;
                            Tx_MaterialRequest.CreatedUser = model._UserId;
                            Tx_MaterialRequest.ModifiedDate = dtModified;
                            Tx_MaterialRequest.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string ReqType = model.ReqType.ToString();
                            string ShipSection = model.ShipSection.ToString();
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'MaterialRequest"+ ReqType + ShipSection +"','" + dateX + "','') ").SingleOrDefault();
                            Tx_MaterialRequest.TransNo = transNo;

                            CONTEXT.Tx_MaterialRequest.Add(Tx_MaterialRequest);
                            CONTEXT.SaveChanges();
                            Id = Tx_MaterialRequest.Id;

                            String keyValue;
                            keyValue = Tx_MaterialRequest.Id.ToString();

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
                                        MaterialRequest_DetailModel detailModel = new MaterialRequest_DetailModel();
                                        detailModel.DetId = detId;
                                        Detail_Delete(CONTEXT, detailModel);
                                    }
                                }
                            }



                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_MaterialRequest", "add", "Id", keyValue);


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
        public void Update(MaterialRequestModel model)
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


                                Tx_MaterialRequest Tx_MaterialRequest = CONTEXT.Tx_MaterialRequest.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_MaterialRequest.ModifiedDate = dtModified;
                                Tx_MaterialRequest.ModifiedUser = model._UserId;
                                if (Tx_MaterialRequest != null)
                                {
                                    var exceptColumns = new string[] { "Id","TransNo","CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_MaterialRequest, false, exceptColumns);
                                    Tx_MaterialRequest.ModifiedDate = dtModified;
                                    Tx_MaterialRequest.ModifiedUser = model._UserId;
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
                                                MaterialRequest_DetailModel detailModel = new MaterialRequest_DetailModel();
                                                detailModel.DetId = detId;
                                               Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }

                                    
                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", " Tx_MaterialRequest", "update", "Id", keyValue);

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
        public long Detail_Add(HANA_APP CONTEXT, MaterialRequest_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_MaterialRequest_Detail Tx_MaterialRequest_Detail = new Tx_MaterialRequest_Detail();

                CopyProperty.CopyProperties(model, Tx_MaterialRequest_Detail, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_MaterialRequest_Detail.Id = Id;
                Tx_MaterialRequest_Detail.CreatedDate = dtModified;
                Tx_MaterialRequest_Detail.CreatedUser = UserId;
                Tx_MaterialRequest_Detail.ModifiedDate = dtModified;
                Tx_MaterialRequest_Detail.ModifiedUser = UserId;
                Tx_MaterialRequest_Detail.RowNum = 0;


                CONTEXT.Tx_MaterialRequest_Detail.Add(Tx_MaterialRequest_Detail);
                CONTEXT.SaveChanges();
                DetId = Tx_MaterialRequest_Detail.DetId;

            }

            return DetId;

        }
        public void Detail_Update(HANA_APP CONTEXT, MaterialRequest_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_MaterialRequest_Detail Tx_MaterialRequest_Detail = CONTEXT.Tx_MaterialRequest_Detail.Find(model.DetId);

                if (Tx_MaterialRequest_Detail != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_MaterialRequest_Detail, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_MaterialRequest_Detail.ModifiedDate = dtModified;
                    Tx_MaterialRequest_Detail.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Detail_Delete(HANA_APP CONTEXT, MaterialRequest_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_MaterialRequest_Detail\"  WHERE \"DetId\"=:p0", model.DetId);

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
                SpNotif.SpSysControllerTransNotif(userId, "MaterialRequest", oCompany, "before", "MaterialRequest", "post", "Id", keyValue);

                AddPR(oCompany, userId, Id);

                var sql1 = "UPDATE T0 SET   "
                        + " T0.\"Status\"='Posted',"
                        + " T0.\"IsAfterPosted\"='Y',"
                        + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                        + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP, "
                        + " T0.\"SapDocNum\"= T1.\"DocNum\" "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_MaterialRequest\" T0 "
                        + " LEFT JOIN \"OPRQ\" T1 ON T0.\"SapDocEntry\" = T1.\"DocEntry\" "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);

                SpNotif.SpSysControllerTransNotif(userId, "MaterialRequest", oCompany, "after", "MaterialRequest", "post", "Id", keyValue);

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
            ssql = string.Format(ssql, Id, "MaterialRequest");
            string tempId = _Utils.SapCompany.RetRstField(oCompany, ssql);
            if (!string.IsNullOrEmpty(tempId))
            {
                return false;
            }

            //ADD GRPO
            SAPbobsCOM.Recordset rsPR = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsPR = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpMaterialRequest_SapAddPR\" ('" + Id.ToString() + "')");

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
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_MaterialRequest\" T0 "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sqlUpdateSO);
            }

            //END ADD DELIVERY

            //throw new Exception("[VALIDATION] - Lagi test jangan di save dulu");

            return true;
        }
        public List<MaterialRequest_AttachmentModel> MaterialRequest_Attachments(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return MaterialRequest_Attachments(CONTEXT, id);
            }

        }
        public List<MaterialRequest_AttachmentModel> MaterialRequest_Attachments(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<MaterialRequest_AttachmentModel>("SELECT T0.\"Id\", T0.\"DetId\", T0.\"FileName\" FROM \"Tx_MaterialRequest_Attachment\" T0 WHERE T0.\"Id\"=:p0 ORDER BY T0.\"DetId\" ", id).ToList();


        }
        public long Attachment_Add(HANA_APP CONTEXT, MaterialRequest_AttachmentModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_MaterialRequest_Attachment tx_MaterialRequest_Attachment = new Tx_MaterialRequest_Attachment();

                CopyProperty.CopyProperties(model, tx_MaterialRequest_Attachment, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_MaterialRequest_Attachment.Id = Id;
                tx_MaterialRequest_Attachment.CreatedDate = dtModified;
                tx_MaterialRequest_Attachment.CreatedUser = UserId;
                tx_MaterialRequest_Attachment.ModifiedDate = dtModified;
                tx_MaterialRequest_Attachment.ModifiedUser = UserId;

                CONTEXT.Tx_MaterialRequest_Attachment.Add(tx_MaterialRequest_Attachment);
                CONTEXT.SaveChanges();
                DetId = tx_MaterialRequest_Attachment.DetId;

            }

            return DetId;

        }
        public long Attachment_Add(List<MaterialRequest_AttachmentModel> ListModel)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Attachment_Add(CONTEXT, ListModel);
            }

        }
        public long Attachment_Add(HANA_APP CONTEXT, List<MaterialRequest_AttachmentModel> ListModel)
        {
            long Id = 0;
            long DetId = 0;

            if (ListModel != null)
            {

                for (int i = 0; i < ListModel.Count; i++)
                {
                    Tx_MaterialRequest_Attachment tx_MaterialRequest_Attachment = new Tx_MaterialRequest_Attachment();
                    var model = ListModel[i];

                    CopyProperty.CopyProperties(model, tx_MaterialRequest_Attachment, false);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    tx_MaterialRequest_Attachment.Id = model.Id;
                    tx_MaterialRequest_Attachment.CreatedDate = dtModified;
                    tx_MaterialRequest_Attachment.CreatedUser = model._UserId;
                    tx_MaterialRequest_Attachment.ModifiedDate = dtModified;
                    tx_MaterialRequest_Attachment.ModifiedUser = model._UserId;

                    CONTEXT.Tx_MaterialRequest_Attachment.Add(tx_MaterialRequest_Attachment);
                    CONTEXT.SaveChanges();
                    DetId = tx_MaterialRequest_Attachment.DetId;
                }



            }

            return Id;

        }
        public void Attachment_Delete(MaterialRequest_AttachmentModel model)
        {
            if (model.DetId != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    Attachment_Delete(CONTEXT, model);
                }
            }

        }
        public void Attachment_Delete(HANA_APP CONTEXT, MaterialRequest_AttachmentModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_MaterialRequest_Attachment\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
    }

    #endregion

}