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



namespace Models.Transaction.ApprovalMR
{
    #region Models

    public class ApprovalMRModel
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
        
        public DateTime? ApprovedDate { get; set; }

        public long BaseTransId { get; set; }

        public string BaseTransNo { get; set; }

        public int? RequesterId { get; set; }
        [Required(ErrorMessage = "required")]
        public string RequesterUserName { get; set; }

        [Required(ErrorMessage = "required")]
        public int? ShipId { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipSection { get; set; }

        [Required(ErrorMessage = "required")]
        public string WarehouseCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string WarehouseName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ReqType { get; set; }

        public DateTime? ReqDate { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }

        public int? SapDocEntry { get; set; }

        public int? SapDocNum { get; set; }

        public string RefNum { get; set; }

        public string RefType { get; set; }

        public string Kategori { get; set; }

        public List<ApprovalMR_DetailModel> ListDetails_ = new List<ApprovalMR_DetailModel>();

        public ApprovalMR_Detail Details_ { get; set;}
    }
    public class ApprovalMR_Detail    {
        public List<long> deletedRowKeys { get; set; }
        public List<ApprovalMR_DetailModel> insertedRowValues { get; set; }
        public List<ApprovalMR_DetailModel> modifiedRowValues { get; set; }
    }
    public class ApprovalMR_DetailModel
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

        public long? BaseDetId { get; set; }


        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public decimal? Qty  { get; set; }

        public string UoMCode { get; set; }

        public int? UomEntry { get; set; }

        public decimal? ROB { get; set; }

        public string Remark { get; set; }

        public string Remark2 { get; set; }

        [Required(ErrorMessage = "required")]
        public decimal? QtyVA { get; set; }

        [Required(ErrorMessage = "required")]
        public decimal? QtyApproved { get; set; }

        public string IsApproved { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal? QtySJ { get; set; }

        public decimal? WhsQty { get; set; }

        public int? DocEntryPR { get; set; }

        public string DocNumPR { get; set; }

        public decimal? QtyPR { get; set; }

        public int? DocEntryPO { get; set; }

        public string DocNumPO { get; set; }

        public decimal? QtyPO { get; set; }


    }


    #endregion

    #region Services

    public class ApprovalMRService
    {

        public ApprovalMRModel GetNewModel(int userId)
        {
            ApprovalMRModel model = new ApprovalMRModel();
            model.Status = "Draft";
            return model;
        }
        public ApprovalMRModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }
        public ApprovalMRModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            ApprovalMRModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.* 
                            FROM ""Tx_ApprovalMR"" T0   
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<ApprovalMRModel>(ssql, id).Single();

                model.ListDetails_ = this.ApprovalMR_Details(CONTEXT, id);
            }

            return model;
        }
        public List<ApprovalMR_DetailModel> ApprovalMR_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return ApprovalMR_Details(CONTEXT, id);
            }

        }
        public List<ApprovalMR_DetailModel> ApprovalMR_Details(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<ApprovalMR_DetailModel>("SELECT * FROM \"Tx_ApprovalMR_Detail\" WHERE \"Id\" =:p0", id).ToList();
        }
        public ApprovalMRModel NavFirst(int userId)
        {
            ApprovalMRModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ApprovalMR");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ApprovalMR\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public ApprovalMRModel NavPrevious(int userId, long id = 0)
        {
            ApprovalMRModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ApprovalMR");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ApprovalMR\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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
        public ApprovalMRModel NavNext(int userId, long id = 0)
        {
            ApprovalMRModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ApprovalMR");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ApprovalMR\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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
        public ApprovalMRModel NavLast(int userId)
        {
            ApprovalMRModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ApprovalMR");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ApprovalMR\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        public long Add(ApprovalMRModel model)
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
                            
                            Tx_ApprovalMR Tx_ApprovalMR = new Tx_ApprovalMR();
                            CopyProperty.CopyProperties(model, Tx_ApprovalMR, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            string alias = CONTEXT.Database.SqlQuery<string>("SELECT \"Alias\" FROM \"Tm_Ship\" WHERE \"Id\"= :p0",model.ShipId.Value).Single();
                            Tx_ApprovalMR.TransType = "ApprovalMR";
                            Tx_ApprovalMR.CreatedDate = dtModified;
                            Tx_ApprovalMR.CreatedUser = model._UserId;
                            Tx_ApprovalMR.ModifiedDate = dtModified;
                            Tx_ApprovalMR.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string dateV = model.TransDate.Value.ToString("YY/MM");
                            string ReqType = model.ReqType.ToString();
                            string ShipSection = model.ShipSection.ToString();
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'ApprovalMR','" + dateX + "','') ").SingleOrDefault();
                            Tx_ApprovalMR.TransNo = "VA" + "/" + alias + "/" + transNo;

                            CONTEXT.Tx_ApprovalMR.Add(Tx_ApprovalMR);
                            CONTEXT.SaveChanges();
                            Id = Tx_ApprovalMR.Id;

                            String keyValue;
                            keyValue = Tx_ApprovalMR.Id.ToString();
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



                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_ApprovalMR", "add", "Id", keyValue);
                            
                            try
                            {

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", " Tx_MaterialRequest", "update", "Id", keyValue);


                                Tx_MaterialRequest Tx_MaterialRequest = CONTEXT.Tx_MaterialRequest.Find(model.BaseTransId);
                                Tx_MaterialRequest.ModifiedDate = dtModified;
                                Tx_MaterialRequest.ModifiedUser = model._UserId;
                                if (Tx_ApprovalMR != null)
                                {
                                   
                                    Tx_MaterialRequest.ModifiedDate = dtModified;
                                    Tx_MaterialRequest.Status = "Process Approval";
                                    CONTEXT.SaveChanges();
                                    
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
        public void Update(ApprovalMRModel model)
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

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", " Tx_ApprovalMR", "update", "Id", keyValue);


                                Tx_ApprovalMR Tx_ApprovalMR = CONTEXT.Tx_ApprovalMR.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_ApprovalMR.ModifiedDate = dtModified;
                                Tx_ApprovalMR.ModifiedUser = model._UserId;
                                if (Tx_ApprovalMR != null)
                                {
                                    var exceptColumns = new string[] { "Id","TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_ApprovalMR, false, exceptColumns);
                                    Tx_ApprovalMR.ModifiedDate = dtModified;
                                    Tx_ApprovalMR.ModifiedUser = model._UserId;
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
                                                ApprovalMR_DetailModel detailModel = new ApprovalMR_DetailModel();
                                                detailModel.DetId = detId;
                                               Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }

                                    
                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", " Tx_ApprovalMR", "update", "Id", keyValue);

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
        public long Detail_Add(HANA_APP CONTEXT, ApprovalMR_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_ApprovalMR_Detail Tx_ApprovalMR_Detail = new Tx_ApprovalMR_Detail();

                CopyProperty.CopyProperties(model, Tx_ApprovalMR_Detail, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_ApprovalMR_Detail.Id = Id;
                Tx_ApprovalMR_Detail.CreatedDate = dtModified;
                Tx_ApprovalMR_Detail.CreatedUser = UserId;
                Tx_ApprovalMR_Detail.ModifiedDate = dtModified;
                Tx_ApprovalMR_Detail.ModifiedUser = UserId;
                Tx_ApprovalMR_Detail.RowNum = 0;
                if(model.QtyApproved <= 0)
                {
                    Tx_ApprovalMR_Detail.IsApproved = "N";
                }
                else
                {
                    Tx_ApprovalMR_Detail.IsApproved = "Y";
                }

                CONTEXT.Tx_ApprovalMR_Detail.Add(Tx_ApprovalMR_Detail);
                CONTEXT.SaveChanges();
                DetId = Tx_ApprovalMR_Detail.DetId;

            }

            return DetId;

        }
        public void Detail_Update(HANA_APP CONTEXT, ApprovalMR_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_ApprovalMR_Detail Tx_ApprovalMR_Detail = CONTEXT.Tx_ApprovalMR_Detail.Find(model.DetId);

                if (Tx_ApprovalMR_Detail != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_ApprovalMR_Detail, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_ApprovalMR_Detail.ModifiedDate = dtModified;
                    Tx_ApprovalMR_Detail.ModifiedUser = UserId;
                    if (model.QtyApproved <= 0)
                    {
                        Tx_ApprovalMR_Detail.IsApproved = "N";
                    }
                    else
                    {
                        Tx_ApprovalMR_Detail.IsApproved = "Y";
                    }
                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Detail_Delete(HANA_APP CONTEXT, ApprovalMR_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_ApprovalMR_Detail\"  WHERE \"DetId\"=:p0", model.DetId);

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
                SpNotif.SpSysControllerTransNotif(userId, "ApprovalMR", oCompany, "before", "ApprovalMR", "post", "Id", keyValue);

                AddPR(oCompany, userId, Id);

                var sql1 = "UPDATE T0 SET   "
                        + " T0.\"Status\"='Posted',"
                        + " T0.\"IsAfterPosted\"='Y',"
                        + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                        + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP, "
                        + " T0.\"SapDocNum\"= T1.\"DocNum\" "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_ApprovalMR\" T0 "
                        + " LEFT JOIN \"OPRQ\" T1 ON T0.\"SapDocEntry\" = T1.\"DocEntry\" "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);
                

                SpNotif.SpSysControllerTransNotif(userId, "ApprovalMR", oCompany, "after", "ApprovalMR", "post", "Id", keyValue);

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
        public void Cancel(int userId, long Id, long BaseId)
        {
            String keyValue;
            String keyValue2;
            keyValue = Id.ToString();
            keyValue2 = BaseId.ToString();

            SAPbobsCOM.Company oCompany = SAPCachedCompany.GetCompany();

            try
            {
                oCompany.StartTransaction();

                SpNotif.SpSysControllerTransNotif(userId, "ApprovalMR", oCompany, "before", "ApprovalMR", "cancel", "Id", keyValue);

                CancelPR(oCompany, userId, Id);

                string sql1 = "UPDATE T0 SET   "
                       + " T0.\"Status\"='Cancel',"
                       + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                       + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP "
                       + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_ApprovalMR\" T0 "
                       + " WHERE T0.\"Id\"=" + Id.ToString();
                string sql2 = "UPDATE T0 SET   "
                       + " T0.\"Status\"='Cancel'"
                       + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_MaterialRequest\" T0 "
                       + " WHERE T0.\"Id\"=" + BaseId.ToString();
                SapCompany.ExecuteQuery(oCompany, sql1);
                SapCompany.ExecuteQuery(oCompany, sql2);
                SpNotif.SpSysControllerTransNotif(userId, "ApprovalMR", oCompany, "after", "ApprovalMR", "cancel", "Id", keyValue);

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
            ssql = string.Format(ssql, Id, "ApprovalMR");
            string tempId = _Utils.SapCompany.RetRstField(oCompany, ssql);
            if (!string.IsNullOrEmpty(tempId))
            {
                return false;
            }

            //ADD PR
            SAPbobsCOM.Recordset rsPR = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsPR = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpApprovalMR_SapAddPR\" ('" + Id.ToString() + "')");

            if (!rsPR.EoF)
            {
                SAPbobsCOM.Documents oPurchaseRequest = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseRequest);
                oPurchaseRequest.Series = rsPR.Fields.Item("Hdr_Series").Value;
                oPurchaseRequest.DocDate = (DateTime)rsPR.Fields.Item("Hdr_DocDate").Value;
                oPurchaseRequest.DocDueDate = (DateTime)rsPR.Fields.Item("Hdr_DocDueDate").Value;
                oPurchaseRequest.RequriedDate = (DateTime)rsPR.Fields.Item("Hdr_DocDueDate").Value;
                oPurchaseRequest.DocCurrency = rsPR.Fields.Item("Hdr_DocCurrency").Value.ToString();
                oPurchaseRequest.DocRate = double.Parse(rsPR.Fields.Item("Hdr_DocRate").Value.ToString());
                oPurchaseRequest.Comments = rsPR.Fields.Item("Hdr_Remark").Value.ToString();
                oPurchaseRequest.Project = rsPR.Fields.Item("Det_Project").Value.ToString();
                
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
                oPurchaseRequest.UserFields.Fields.Item("U_IDU_RequesterName").Value = rsPR.Fields.Item("Hdr_RequesterUserName").Value.ToString();
                oPurchaseRequest.UserFields.Fields.Item("U_IDU_WebRequestDate").Value = (DateTime)rsPR.Fields.Item("Hdr_U_IDU_WebReqDate").Value;
                oPurchaseRequest.UserFields.Fields.Item("U_IDU_WebTransDate").Value = (DateTime)rsPR.Fields.Item("Hdr_DocDueDate").Value;
                oPurchaseRequest.UserFields.Fields.Item("U_IDU_WebRemark").Value = rsPR.Fields.Item("Hdr_Remark").Value.ToString();
                oPurchaseRequest.UserFields.Fields.Item("U_IDU_Bagian").Value = rsPR.Fields.Item("Hdr_Bagian").Value.ToString();
                oPurchaseRequest.UserFields.Fields.Item("U_IDU_RequestType").Value = rsPR.Fields.Item("Hdr_ReqType").Value.ToString();
                oPurchaseRequest.UserFields.Fields.Item("U_IDU_AreaSupply").Value = rsPR.Fields.Item("Hdr_WarehouseName").Value.ToString();
                oPurchaseRequest.UserFields.Fields.Item("U_IDU_WebTransNo2").Value = rsPR.Fields.Item("Hdr_U_IDU_WebTransNo2").Value.ToString();
                oPurchaseRequest.UserFields.Fields.Item("U_IDU_KATEGORIINV").Value = rsPR.Fields.Item("Hdr_U_IDU_KATEGORIINV").Value.ToString();
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
                        if (rsPR.Fields.Item("Det_WhsCode").Value.ToString() != "")
                        {
                            oPurchaseRequest.Lines.WarehouseCode = rsPR.Fields.Item("Det_WhsCode").Value.ToString();
                        }
                        oPurchaseRequest.Lines.FreeText = rsPR.Fields.Item("Det_FreeText").Value.ToString();
                        oPurchaseRequest.Lines.UserFields.Fields.Item("U_IDU_WebDetId").Value = rsPR.Fields.Item("Det_DetId").Value.ToString();
                        oPurchaseRequest.Lines.UserFields.Fields.Item("U_IDU_RemarksVA").Value = rsPR.Fields.Item("Det_FreeText2").Value.ToString();
                        oPurchaseRequest.Lines.UserFields.Fields.Item("U_IDU_ProjectName").Value = rsPR.Fields.Item("Det_ProjectName").Value.ToString();
                        oPurchaseRequest.Lines.UserFields.Fields.Item("U_IDU_QtySJ").Value = rsPR.Fields.Item("Det_QtySJ").Value.ToString();
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
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_ApprovalMR\" T0 "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                 
                SapCompany.ExecuteQuery(oCompany, sqlUpdateSO);

                string sqlUpdateDet;
                sqlUpdateDet = "UPDATE T0 SET   "
                       + " T0.\"DocEntryPR\"=" + docEntry + " "
                       + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_ApprovalMR_Detail\" T0 "
                       + " WHERE T0.\"Id\"=" + Id.ToString();
                SapCompany.ExecuteQuery(oCompany, sqlUpdateDet);
            }

            //END ADD DELIVERY

            //throw new Exception("[VALIDATION] - Lagi test jangan di save dulu");

            return true;
        }
        public bool CancelPR(SAPbobsCOM.Company oCompany, int userId, long Id)
        {
            int nErr;
            string errMsg;

            SAPbobsCOM.Recordset rsPR = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsPR = _Utils.SapCompany.GetRs(oCompany, "SELECT * FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_ApprovalMR\" T0 WHERE T0.\"Id\" ='" + Id.ToString() + "' AND IFNULL(T0.\"SapDocEntry\",0)>0 ");

            if (!rsPR.EoF)
            {
                SAPbobsCOM.Documents oPurchaseRequest = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseRequest);
                oPurchaseRequest.GetByKey(int.Parse(rsPR.Fields.Item("SapDocEntry").Value.ToString()));

                //SAPbobsCOM.Documents oCancelDoc = oPurchaseRequest.CreateCancellationDocument();

                //oCancelDoc.DocDate = (DateTime)rsPR.Fields.Item("TransDate").Value;

                if (oPurchaseRequest.Cancel() != 0)
                {
                    nErr = oCompany.GetLastErrorCode();
                    errMsg = oCompany.GetLastErrorDescription();
                    throw new Exception("[VALIDATION] - Cancel Purchase Request - " + nErr.ToString() + "|" + errMsg);
                }

            }

            return true;
        }
        public ApprovalMRModel GetByIdMR(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetByIdMR(CONTEXT, userId, id);
            }
        }
        public ApprovalMRModel GetByIdMR(HANA_APP CONTEXT, int userId, long id = 0)
        {
            ApprovalMRModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT 
                            T0.""Id"" AS ""BaseTransId"",
                            T0.""TransNo"" AS ""BaseTransNo"",
                            T0.""ShipId"",
                            T0.""ShipName"",
                            T0.""ShipCode"",
                            T0.""ShipSection"", 
                            T0.""WarehouseCode"",
                            T0.""WarehouseName"",
                            T0.""Remark"",
                            T2.""Id"" AS ""RequesterId"",
                            T2.""FirstName"" AS ""RequesterUserName"",
                            T1.""Name"" AS ""ReqType"",
                            T0.""TransDate"" AS ""ReqDate""
                            FROM  ""Tx_MaterialRequest"" T0
                            LEFT JOIN ""Ts_List"" T1 ON T0.""ReqType"" = T1.""Code"" AND T1.""Type"" = 'MaterialRequestReqType'
                            LEFT JOIN ""Tm_User"" T2 ON T0.""CreatedUser"" = T2.""Id""
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<ApprovalMRModel>(ssql, id).Single();
                model.Status = "Draft";
                string ShipCode = model.ShipCode;
                model.ListDetails_ = this.MR_Details(CONTEXT, id, ShipCode);
                //model.Details_.insertedRowValues = model.ListDetails_.ToList();
            }

            return model;
        }
        public List<ApprovalMR_DetailModel> MR_Details(long id = 0, string ShipCode = null)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return MR_Details(CONTEXT, id, ShipCode);
            }

        }
        public List<ApprovalMR_DetailModel> MR_Details(HANA_APP CONTEXT, long id = 0,  string ShipCode = null)
        {
            string ssql = @"SELECT A.*,A.""Qty"" AS ""QtyVA"",C.""DocNumPR"" , C.""QtyPR"", D.""DocNumPO"", D.""QtyPO"", B.""OnHand"" AS ""WhsQty"", E.""DocDate"" AS ""DeliveryDate"", A.""DetId"" AS ""BaseDetId"", E.""QtySJ""
                            FROM ""{DbApp}"".""Tx_MaterialRequest"" A1
                            LEFT JOIN ""{DbApp}"".""Tx_MaterialRequest_Detail"" A ON A1.""Id"" = A.""Id""
                            LEFT JOIN ""{DbSap}"".OITW B ON A.""ItemCode"" = B.""ItemCode""
                            LEFT JOIN
                            (
                                SELECT
                                STRING_AGG(A.""DocNum"",','ORDER BY A1.""DocEntry"")AS ""DocNumPR"",
                                IFNULL(SUM(A1.""OpenQty""),0) AS ""QtyPR"",
                                A1.""ItemCode""
                                FROM ""{DbSap}"".PRQ1 A1
                                LEFT JOIN ""{DbSap}"".OPRQ A ON A.""DocEntry"" = A1.""DocEntry""
                                WHERE
                                A1.""OpenQty"" > 0
                                AND A.""DocStatus"" = 'O'
                                AND A1.""Project"" = :p1
                                GROUP BY A1.""ItemCode""
                            ) C ON A.""ItemCode"" = C.""ItemCode""
                            LEFT JOIN
                            (
                                SELECT
                                STRING_AGG(A.""DocNum"",','ORDER BY A1.""DocEntry"")AS ""DocNumPO"",
                                IFNULL(SUM(A1.""OpenQty""),0) AS ""QtyPO"",
                                A1.""ItemCode""
                                FROM ""{DbSap}"".POR1 A1
                                LEFT JOIN ""{DbSap}"".OPOR A ON A.""DocEntry"" = A1.""DocEntry""
                                WHERE
                                A1.""OpenQty"" > 0
                                AND A.""DocStatus"" = 'O'
                                AND A1.""Project"" = :p2
                                GROUP BY A1.""ItemCode""
                            ) D ON A.""ItemCode"" = D.""ItemCode""
                            LEFT JOIN
                            (
                                SELECT MAX(A.""DocDate"") AS ""DocDate"", MAX(A.""DocNum"") AS ""DocNum"", B.""ItemCode"" AS ""ItemCode"", MAX(B.""Quantity"")  AS ""QtySJ""
                                FROM ""{DbSap}"".OIGE A
                                LEFT JOIN ""{DbSap}"".IGE1 B ON A.""DocEntry"" = B.""DocEntry""
                                WHERE A.""Project"" = :p3
                                GROUP BY B.""ItemCode""
                            ) E ON A.""ItemCode"" = E.""ItemCode""
                            WHERE A.""Id"" = :p0
                            AND B.""WhsCode"" = A1.""WarehouseCode""";
            ssql = ssql.Replace("{DbSap}", DbProvider.dbSap_Name);
            ssql = ssql.Replace("{DbApp}", DbProvider.dbApp_Name);
            return CONTEXT.Database.SqlQuery<ApprovalMR_DetailModel>(ssql, id, ShipCode, ShipCode, ShipCode).ToList();
        }

    }


    #endregion

}