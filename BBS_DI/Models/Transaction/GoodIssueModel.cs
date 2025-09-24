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



namespace Models.Transaction.GoodIssue
{
    #region Models

    public class GoodIssueModel
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
        
        public string Jenis { get; set; }

        public string Bagian { get; set; }

        public string Ref { get; set; }
        
        public int? Series { get; set; }
        

        [Required(ErrorMessage = "required")]
        public string WarehouseCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string WarehouseName { get; set; }

        public int? SapDocEntry { get; set; }

        public int? SapDocNum { get; set; }

        public string Remark { get; set; }

        public string Kategori { get; set; }

        public List<GoodIssue_DetailModel> ListDetails_ = new List<GoodIssue_DetailModel>();

        public GoodIssue_Detail Details_ { get; set;}
    }
    public class GoodIssue_Detail    {
        public List<long> deletedRowKeys { get; set; }
        public List<GoodIssue_DetailModel> insertedRowValues { get; set; }
        public List<GoodIssue_DetailModel> modifiedRowValues { get; set; }
    }
    public class GoodIssue_DetailModel
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

        public string ItemCodeKey { get; set; }

        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public decimal? Qty  { get; set; }

        public string UoMCode { get; set; }

        public int? UomEntry { get; set; }

        public string Remark { get; set; }

        public int? SapDocEntry { get; set; }

        public int? SapDocNum { get; set; }

        public string ManBtchNum { get; set; }

        public List<GoodIssueBatchDetailModel> ListBatchDetails_ = new List<GoodIssueBatchDetailModel>();

        public GoodIssueBatchDetails BatchDetails_ { get; set; }
    }
    public class GoodIssueBatchDetailModel
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

    public class GoodIssueBatchDetails
    {
        public List<int> deletedRowKeys { get; set; }
        public List<GoodIssueBatchDetailModel> insertedRowValues { get; set; }
        public List<GoodIssueBatchDetailModel> modifiedRowValues { get; set; }
    }


    #endregion

    #region Services

    public class GoodIssueService
    {

        public GoodIssueModel GetNewModel(int userId)
        {
            GoodIssueModel model = new GoodIssueModel();
            var CONTEXT = new HANA_APP();
            DateTime dateX = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
            string dateV = dateX.ToString("yyMM");
            string ssqlSeries = @"SELECT T0.""Series"" FROM ""{DbSap}"".NNM1 T0 WHERE T0.""ObjectCode"" = '60' AND RIGHT(T0.""SeriesName"",4) ='" + dateV + "' ";
            ssqlSeries = ssqlSeries.Replace("{DbSap}", DbProvider.dbSap_Name);
            model.Series = CONTEXT.Database.SqlQuery<int>(ssqlSeries).FirstOrDefault();
            model.Status = "Draft";
            return model;
        }
        public GoodIssueModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public GoodIssueModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            GoodIssueModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.* 
                            FROM ""Tx_GoodIssue"" T0   
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<GoodIssueModel>(ssql, id).Single();

                model.ListDetails_ = this.GoodIssue_Details(CONTEXT, id);
            }

            return model;
        }
        public List<GoodIssue_DetailModel> GoodIssue_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GoodIssue_Details(CONTEXT, id);
            }

        }

        public List<GoodIssue_DetailModel> GoodIssue_Details(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<GoodIssue_DetailModel>("SELECT T0.*,CAST(T0.\"DetId\" AS NVARCHAR(10)) AS \"ItemCodeKey\", T1.\"Status\" FROM \"Tx_GoodIssue_Detail\" T0 LEFT JOIN \"Tx_GoodIssue\" T1 ON T0.\"Id\" = T1.\"Id\" WHERE T0.\"Id\" =:p0", id).ToList();
        }
        public GoodIssueModel NavFirst(int userId)
        {
            GoodIssueModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodIssue");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodIssue\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public GoodIssueModel NavPrevious(int userId, long id = 0)
        {
            GoodIssueModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodIssue");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodIssue\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public GoodIssueModel NavNext(int userId, long id = 0)
        {
            GoodIssueModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodIssue");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodIssue\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public GoodIssueModel NavLast(int userId)
        {
            GoodIssueModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GoodIssue");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GoodIssue\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        public long Add(GoodIssueModel model)
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

                            Tx_GoodIssue Tx_GoodIssue = new Tx_GoodIssue();
                            CopyProperty.CopyProperties(model, Tx_GoodIssue, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_GoodIssue.TransType = "GoodIssue";
                            Tx_GoodIssue.CreatedDate = dtModified;
                            Tx_GoodIssue.CreatedUser = model._UserId;
                            Tx_GoodIssue.ModifiedDate = dtModified;
                            Tx_GoodIssue.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'GoodIssue','" + dateX + "','') ").SingleOrDefault();
                            Tx_GoodIssue.TransNo = transNo;

                            CONTEXT.Tx_GoodIssue.Add(Tx_GoodIssue);
                            CONTEXT.SaveChanges();
                            Id = Tx_GoodIssue.Id;

                            String keyValue;
                            keyValue = Tx_GoodIssue.Id.ToString();

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
                                        GoodIssue_DetailModel detailModel = new GoodIssue_DetailModel();
                                        detailModel.DetId = detId;
                                        Detail_Delete(CONTEXT, detailModel);
                                    }
                                }
                            }



                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_GoodIssue", "add", "Id", keyValue);


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
        public void Update(GoodIssueModel model)
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


                                Tx_GoodIssue Tx_GoodIssue = CONTEXT.Tx_GoodIssue.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_GoodIssue.ModifiedDate = dtModified;
                                Tx_GoodIssue.ModifiedUser = model._UserId;
                                if (Tx_GoodIssue != null)
                                {
                                    var exceptColumns = new string[] { "Id","TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_GoodIssue, false, exceptColumns);
                                    Tx_GoodIssue.ModifiedDate = dtModified;
                                    Tx_GoodIssue.ModifiedUser = model._UserId;
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
                                                GoodIssue_DetailModel detailModel = new GoodIssue_DetailModel();
                                                detailModel.DetId = detId;
                                               Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }

                                    
                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", " Tx_GoodIssue", "update", "Id", keyValue);

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
        public long Detail_Add(HANA_APP CONTEXT, GoodIssue_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_GoodIssue_Detail Tx_GoodIssue_Detail = new Tx_GoodIssue_Detail();

                CopyProperty.CopyProperties(model, Tx_GoodIssue_Detail, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_GoodIssue_Detail.Id = Id;
                Tx_GoodIssue_Detail.CreatedDate = dtModified;
                Tx_GoodIssue_Detail.CreatedUser = UserId;
                Tx_GoodIssue_Detail.ModifiedDate = dtModified;
                Tx_GoodIssue_Detail.ModifiedUser = UserId;
                Tx_GoodIssue_Detail.RowNum = 0;


                CONTEXT.Tx_GoodIssue_Detail.Add(Tx_GoodIssue_Detail);
                CONTEXT.SaveChanges();
                DetId = Tx_GoodIssue_Detail.DetId;

            }

            return DetId;

        }
        public void Detail_Update(HANA_APP CONTEXT, GoodIssue_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_GoodIssue_Detail Tx_GoodIssue_Detail = CONTEXT.Tx_GoodIssue_Detail.Find(model.DetId);

                if (Tx_GoodIssue_Detail != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_GoodIssue_Detail, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_GoodIssue_Detail.ModifiedDate = dtModified;
                    Tx_GoodIssue_Detail.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Detail_Delete(HANA_APP CONTEXT, GoodIssue_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_GoodIssue_Detail\"  WHERE \"DetId\"=:p0", model.DetId);

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
                SpNotif.SpSysControllerTransNotif(userId, "GoodIssue", oCompany, "before", "GoodIssue", "post", "Id", keyValue);

                AddGI(oCompany, userId, Id);

                var sql1 = "UPDATE T0 SET   "
                        + " T0.\"Status\"='Posted',"
                        + " T0.\"IsAfterPosted\"='Y',"
                        + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                        + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP, "
                        + " T0.\"SapDocNum\"= T1.\"DocNum\" "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_GoodIssue\" T0 "
                        + " LEFT JOIN \"OIGE\" T1 ON T0.\"SapDocEntry\" = T1.\"DocEntry\" "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);

                SpNotif.SpSysControllerTransNotif(userId, "GoodIssue", oCompany, "after", "GoodIssue", "post", "Id", keyValue);

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
        
        public bool AddGI(SAPbobsCOM.Company oCompany, int userId, long Id)
        {
            int nErr;
            string errMsg;

            string ssql = "SELECT TOP 1 T0.\"DocEntry\" FROM \"OIGE\" T0 WHERE T0.\"U_IDU_WebTransId\"='{0}' AND T0.\"U_IDU_WebTransType\"='{1}'";
            ssql = string.Format(ssql, Id, "GoodIssue");
            string tempId = _Utils.SapCompany.RetRstField(oCompany, ssql);
            if (!string.IsNullOrEmpty(tempId))
            {
                return false;
            }

            //ADD GRPO
            SAPbobsCOM.Recordset rsGI = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsGI = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpGoodIssue_SapAddGI\" ('" + Id.ToString() + "')");

            if (!rsGI.EoF)
            {
                SAPbobsCOM.Documents oInventoryGenExit = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
                oInventoryGenExit.Series = rsGI.Fields.Item("Hdr_Series").Value;
                oInventoryGenExit.DocDate = (DateTime)rsGI.Fields.Item("Hdr_DocDate").Value;
                oInventoryGenExit.TaxDate = (DateTime)rsGI.Fields.Item("Hdr_DocDueDate").Value;
                oInventoryGenExit.Reference2 = rsGI.Fields.Item("Hdr_Ref").Value;
                oInventoryGenExit.Project = rsGI.Fields.Item("Hdr_ProjectCode").Value;
                
                oInventoryGenExit.UserFields.Fields.Item("U_IDU_WebTransType").Value = rsGI.Fields.Item("Hdr_WebTransType").Value.ToString();
                oInventoryGenExit.UserFields.Fields.Item("U_IDU_WebTransNo").Value = rsGI.Fields.Item("Hdr_WebTransNo").Value.ToString();
                oInventoryGenExit.UserFields.Fields.Item("U_IDU_WebTransId").Value = rsGI.Fields.Item("Hdr_WebTransId").Value.ToString();
                oInventoryGenExit.UserFields.Fields.Item("U_IDU_WebUserId").Value = userId.ToString();
                
                int lineItem = 0;
                while (!rsGI.EoF)
                {
                    
                    oInventoryGenExit.Lines.ItemCode = rsGI.Fields.Item("Det_ItemCode").Value.ToString();
                    if (rsGI.Fields.Item("Det_ItemDescription").Value.ToString() != "")
                    {
                        oInventoryGenExit.Lines.ItemDescription = rsGI.Fields.Item("Det_ItemDescription").Value.ToString();
                    }
                    oInventoryGenExit.Lines.ProjectCode = rsGI.Fields.Item("Hdr_ProjectCode").Value.ToString();
                    oInventoryGenExit.Lines.Quantity = double.Parse(rsGI.Fields.Item("Det_Quantity").Value.ToString());
                    oInventoryGenExit.Lines.UoMEntry = rsGI.Fields.Item("Det_UomEntry").Value;
                    if (rsGI.Fields.Item("Hdr_Jenis").Value == "Docking")
                    {
                        oInventoryGenExit.Lines.AccountCode = "122.1";
                    }
                    if (rsGI.Fields.Item("Hdr_WhsCode").Value.ToString() != "")
                    {
                        oInventoryGenExit.Lines.WarehouseCode = rsGI.Fields.Item("Hdr_WhsCode").Value.ToString();
                    }
                    oInventoryGenExit.Lines.FreeText = rsGI.Fields.Item("Det_Remark").Value.ToString();
                    oInventoryGenExit.Lines.UserFields.Fields.Item("U_IDU_WebDetId").Value = rsGI.Fields.Item("Det_DetId").Value.ToString();
                    if (rsGI.Fields.Item("Det_ManBtchNum").Value.ToString() == "Y")
                    {

                        string DetId = rsGI.Fields.Item("Det_DetId").Value.ToString();
                        SAPbobsCOM.Recordset rsGIBatch = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        rsGIBatch = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpGoodIssue_SapAddGI_Batch\" ('" + DetId.ToString() + "')");
                        int lineR = lineItem;
                        while (!rsGIBatch.EoF)
                        {
                            oInventoryGenExit.Lines.BatchNumbers.BatchNumber = rsGIBatch.Fields.Item("Det_BatchNum").Value.ToString();
                            oInventoryGenExit.Lines.BatchNumbers.Quantity = double.Parse(rsGIBatch.Fields.Item("Det_BatchQty").Value.ToString());
                            //oInventoryGenExit.Lines.SetCurrentLine(lineR);
                            oInventoryGenExit.Lines.BatchNumbers.Add();
                            lineItem = lineR;
                            rsGIBatch.MoveNext();
                        }
                    }


                    oInventoryGenExit.Lines.Add();

                    lineItem += 1;
                    rsGI.MoveNext();

                }

                if (oInventoryGenExit.Add() != 0)
                {

                    nErr = oCompany.GetLastErrorCode();
                    errMsg = oCompany.GetLastErrorDescription();
                    throw new Exception("[VALIDATION] - Add Good Issue | " + nErr.ToString() + "|" + errMsg);

                }

                string docEntry;
                docEntry = oCompany.GetNewObjectKey();

                string sqlUpdateSO;
                sqlUpdateSO = "UPDATE T0 SET   "
                        + " T0.\"SapDocEntry\"=" + docEntry + " "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_GoodIssue\" T0 "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sqlUpdateSO);
            }

            //END ADD DELIVERY

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

                SpNotif.SpSysControllerTransNotif(userId, "GoodIssue", oCompany, "before", "GoodIssue", "cancel", "Id", keyValue);

                AddGR(oCompany, userId, Id);

                string sql1 = "UPDATE T0 SET   "
                       + " T0.\"Status\"='Cancel',"
                       + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                       + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP "
                       + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_GoodIssue\" T0 "
                       + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);

                SpNotif.SpSysControllerTransNotif(userId, "GoodIssue", oCompany, "after", "GoodIssue", "cancel", "Id", keyValue);

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
        public bool AddGR(SAPbobsCOM.Company oCompany, int userId, long Id)
        {
            int nErr;
            string errMsg;

            string ssql = "SELECT TOP 1 T0.\"DocEntry\" FROM \"OIGE\" T0 WHERE T0.\"U_IDU_WebTransId\"='{0}' AND T0.\"U_IDU_WebTransType\"='{1}'";
            ssql = string.Format(ssql, Id, "GoodIssue");
            string tempId = _Utils.SapCompany.RetRstField(oCompany, ssql);
            if (!string.IsNullOrEmpty(tempId))
            {

                //ADD GRPO
                SAPbobsCOM.Recordset rsGR = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                rsGR = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpGoodIssue_SapAddGr\" ('" + Id.ToString() + "')");

                if (!rsGR.EoF)
                {
                    SAPbobsCOM.Documents oInventoryGenEntry = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);
                    oInventoryGenEntry.Series = rsGR.Fields.Item("Hdr_Series").Value;
                    oInventoryGenEntry.DocDate = (DateTime)rsGR.Fields.Item("Hdr_DocDate").Value;
                    oInventoryGenEntry.TaxDate = (DateTime)rsGR.Fields.Item("Hdr_DocDueDate").Value;
                    oInventoryGenEntry.Reference2 = rsGR.Fields.Item("Hdr_Ref").Value;
                    oInventoryGenEntry.Project = rsGR.Fields.Item("Hdr_ProjectCode").Value;

                    oInventoryGenEntry.UserFields.Fields.Item("U_IDU_WebTransType").Value = rsGR.Fields.Item("Hdr_WebTransType").Value.ToString();
                    oInventoryGenEntry.UserFields.Fields.Item("U_IDU_WebTransNo").Value = rsGR.Fields.Item("Hdr_WebTransNo").Value.ToString();
                    oInventoryGenEntry.UserFields.Fields.Item("U_IDU_WebTransId").Value = rsGR.Fields.Item("Hdr_WebTransId").Value.ToString();
                    oInventoryGenEntry.UserFields.Fields.Item("U_IDU_WebUserId").Value = userId.ToString();
                    oInventoryGenEntry.DocType = BoDocumentTypes.dDocument_Items;
                    int lineItem = 0;
                    while (!rsGR.EoF)
                    {

                        oInventoryGenEntry.Lines.ItemCode = rsGR.Fields.Item("Det_ItemCode").Value.ToString();
                        if (rsGR.Fields.Item("Det_ItemDescription").Value.ToString() != "")
                        {
                            oInventoryGenEntry.Lines.ItemDescription = rsGR.Fields.Item("Det_ItemDescription").Value.ToString();
                        }
                        oInventoryGenEntry.Lines.ProjectCode = rsGR.Fields.Item("Hdr_ProjectCode").Value.ToString();
                        oInventoryGenEntry.Lines.Quantity = double.Parse(rsGR.Fields.Item("Det_Quantity").Value.ToString());
                        oInventoryGenEntry.Lines.UoMEntry = rsGR.Fields.Item("Det_UomEntry").Value;
                        oInventoryGenEntry.Lines.AccountCode = rsGR.Fields.Item("Det_AcctCode").Value;
                        oInventoryGenEntry.Lines.Price = rsGR.Fields.Item("Det_Price").Value;
                        if (rsGR.Fields.Item("Hdr_WhsCode").Value.ToString() != "")
                        {
                            oInventoryGenEntry.Lines.WarehouseCode = rsGR.Fields.Item("Hdr_WhsCode").Value.ToString();
                        }
                        oInventoryGenEntry.Lines.FreeText = rsGR.Fields.Item("Det_Remark").Value.ToString();
                        oInventoryGenEntry.Lines.UserFields.Fields.Item("U_IDU_WebDetId").Value = rsGR.Fields.Item("Det_DetId").Value.ToString();
                        if (rsGR.Fields.Item("Det_ManBtchNum").Value.ToString() == "Y")
                        {

                            string DetId = rsGR.Fields.Item("Det_DetId").Value.ToString();
                            SAPbobsCOM.Recordset rsGIBatch = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            rsGIBatch = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpGoodIssue_SapAddGI_Batch\" ('" + DetId.ToString() + "')");
                            int lineR = lineItem;
                            while (!rsGIBatch.EoF)
                            {
                                oInventoryGenEntry.Lines.BatchNumbers.BatchNumber = rsGIBatch.Fields.Item("Det_BatchNum").Value.ToString();
                                oInventoryGenEntry.Lines.BatchNumbers.Quantity = double.Parse(rsGIBatch.Fields.Item("Det_BatchQty").Value.ToString());
                                oInventoryGenEntry.Lines.SetCurrentLine(lineR);
                                oInventoryGenEntry.Lines.BatchNumbers.Add();
                                lineItem = lineR;
                                rsGIBatch.MoveNext();
                            }
                        }


                        oInventoryGenEntry.Lines.Add();

                        lineItem += 1;
                        rsGR.MoveNext();

                    }

                    if (oInventoryGenEntry.Add() != 0)
                    {

                        nErr = oCompany.GetLastErrorCode();
                        errMsg = oCompany.GetLastErrorDescription();
                        throw new Exception("[VALIDATION] - Cancel Good Issue | " + nErr.ToString() + "|" + errMsg);

                    }

                    string docEntry;
                    docEntry = oCompany.GetNewObjectKey();

                    string sqlUpdateSO;
                    sqlUpdateSO = "UPDATE T0 SET   "
                            + " T0.\"SapDocEntry\"=" + docEntry + " "
                            + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_GoodIssue\" T0 "
                            + " WHERE T0.\"Id\"=" + Id.ToString();

                    SapCompany.ExecuteQuery(oCompany, sqlUpdateSO);
                }

                //END ADD DELIVERY

                //throw new Exception("[VALIDATION] - Lagi test jangan di save dulu");

                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class GoodIssueBatchDetailService
    {
        //public GoodIssueBatchDetailModel GetNewModel(int userId)
        //{
        //    GoodIssueBatchDetailModel model = new GoodIssueBatchDetailModel();

        //    return model;
        //}
        //public GoodIssue_DetailModel GetByIdNewBatchDetail(long id = 0)
        //{
        //    GoodIssue_DetailModel model = new GoodIssue_DetailModel();
        //    model.Id = id;
        //    model.DetId = id;
        //    return model;
        //}
        public GoodIssue_DetailModel GetById(int userId, long detId = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, detId);
            }
        }
        public GoodIssue_DetailModel GetById(HANA_APP CONTEXT, int userId, long detId = 0)
        {
            GoodIssue_DetailModel model = null;
            if (detId != 0)
            {

                string ssql = "SELECT T0.*, T1.\"Status\" FROM \"Tx_GoodIssue_Detail\" T0 LEFT JOIN \"Tx_GoodIssue\" T1 ON T0.\"Id\" = T1.\"Id\" WHERE T0.\"DetId\"=:p0";
                model = CONTEXT.Database.SqlQuery<GoodIssue_DetailModel>(ssql, detId).Single();

                if (model != null)
                {
                    model.ListBatchDetails_ = this.GoodIssueBatchDetailModel(detId);
                    
                }
            }
            return model;
        }
        public List<GoodIssueBatchDetailModel> GoodIssueBatchDetailModel(long detId = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {

                return GoodIssueBatchDetails(CONTEXT, detId);
            }
        }
        public List<GoodIssueBatchDetailModel> GoodIssueBatchDetails(HANA_APP CONTEXT, long detId = 0)
        {
            return CONTEXT.Database.SqlQuery<GoodIssueBatchDetailModel>("SELECT T0.*, T1.\"Status\" FROM \"Tx_GoodIssue_Detail_Batch\" T0 LEFT JOIN \"Tx_GoodIssue\" T1 ON T0.\"Id\" = T1.\"Id\" WHERE T0.\"DetId\"=:p0  ", detId).ToList();
        }
        public long Add(GoodIssue_DetailModel model)
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

                            Tx_GoodIssue_Detail Tx_GoodIssue_Detail = CONTEXT.Tx_GoodIssue_Detail.Find(model.DetId);


                            if (Tx_GoodIssue_Detail != null)
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
                                            GoodIssueBatchDetailModel detailBatchModel = new GoodIssueBatchDetailModel();
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
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_GoodIssue_Detail_Batch\" WHERE \"DetId\"=:p0 ", detid);
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

        public long BatchDetail_Add(HANA_APP CONTEXT, GoodIssueBatchDetailModel model, long Id, long DetId, int UserId)
        {
            long DetDetId = 0;

            if (model != null)
            {

                Tx_GoodIssue_Detail_Batch Tx_GoodIssue_Detail_Batch = new Tx_GoodIssue_Detail_Batch();

                CopyProperty.CopyProperties(model, Tx_GoodIssue_Detail_Batch, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_GoodIssue_Detail_Batch.Id = Id;
                Tx_GoodIssue_Detail_Batch.DetId = DetId;
                CONTEXT.Tx_GoodIssue_Detail_Batch.Add(Tx_GoodIssue_Detail_Batch);
                CONTEXT.SaveChanges();
                DetDetId = Tx_GoodIssue_Detail_Batch.DetDetId;

            }

            return DetId;

        }
        public void BatchDetail_Update(HANA_APP CONTEXT, GoodIssueBatchDetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_GoodIssue_Detail_Batch Tx_GoodIssue_Detail_Batch = CONTEXT.Tx_GoodIssue_Detail_Batch.Find(model.DetDetId);

                if (Tx_GoodIssue_Detail_Batch != null)
                {
                    var exceptColumns = new string[] { "DetDetId", "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_GoodIssue_Detail_Batch, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();


                    CONTEXT.SaveChanges();

                }


            }

        }
        public void BatchDetail_Delete(HANA_APP CONTEXT, GoodIssueBatchDetailModel model)
        {
            if (model.DetDetId != null)
            {
                if (model.DetDetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_GoodIssue_Detail_Batch\"  WHERE \"DetDetId\"=:p0", model.DetDetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
    }

    #endregion

}