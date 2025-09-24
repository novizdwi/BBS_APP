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


namespace Models.Transaction.GeneralDocRenewal
{
    #region Models

    public class GeneralDocRenewalModel
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
        public int? DocumentId { get; set; }

        [Required(ErrorMessage = "required")]
        public string DocumentName { get; set; }

        public DateTime? OldExpiredDate { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime? ExpiredDate { get; set; }

        public DateTime? OldWarningDate { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime? WarningDate { get; set; }

        public int? Type { get; set; }

        public String Remarks { get; set; }

        public List<GeneralDocRenewal_ReferenceModel> ListReferences_ = new List<GeneralDocRenewal_ReferenceModel>();

    }

    public class GeneralDocRenewal_ReferenceModel
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

        public Int32? ContentId { get; set; }

        public Int32? ContentDetId { get; set; }

        public string ContentValue { get; set; }

        public string ContentOldValue { get; set; }

        public string ContentName_ { get; set; }
        public string IsMandatory_ { get; set; }
        public string IsFixed_ { get; set; }

    }


    #endregion

    #region Services

    public class GeneralDocRenewalService
    {

        public long Add(GeneralDocRenewalModel model)
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

                            Tx_GeneralDocRenewal tx_GeneralDocRenewal = new Tx_GeneralDocRenewal();
                            CopyProperty.CopyProperties(model, tx_GeneralDocRenewal, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_GeneralDocRenewal.TransType = "GeneralDocRenewal";
                            tx_GeneralDocRenewal.CreatedDate = dtModified;
                            tx_GeneralDocRenewal.CreatedUser = model._UserId;
                            tx_GeneralDocRenewal.ModifiedDate = dtModified;
                            tx_GeneralDocRenewal.ModifiedUser = model._UserId;


                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'GeneralDocRenewal','" + dateX + "','') ").SingleOrDefault();
                            tx_GeneralDocRenewal.TransNo = transNo;

                            CONTEXT.Tx_GeneralDocRenewal.Add(tx_GeneralDocRenewal);
                            CONTEXT.SaveChanges();
                            Id = tx_GeneralDocRenewal.Id;

                            String keyValue;
                            keyValue = tx_GeneralDocRenewal.Id.ToString();

                            if (model.ListReferences_ != null)
                            {
                                foreach (var product in model.ListReferences_)
                                {
                                    Detail_Add(CONTEXT, product, Id, model._UserId);
                                }
                            }


                            SpNotif.SpSysControllerTransNotif(model._UserId, "GeneralDocRenewal", CONTEXT, "after", "Tx_GeneralDocRenewal", "add", "Id", keyValue);

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

        public void Update(GeneralDocRenewalModel model)
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

                                SpNotif.SpSysControllerTransNotif(model._UserId, "GeneralDocRenewal", CONTEXT, "before", "Tx_GeneralDocRenewal", "update", "Id", keyValue);

                                Tx_GeneralDocRenewal tx_GeneralDocRenewal = CONTEXT.Tx_GeneralDocRenewal.Find(model.Id);
                                if (tx_GeneralDocRenewal != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransType", "CreatedUser", "Status" };
                                    CopyProperty.CopyProperties(model, tx_GeneralDocRenewal, false, exceptColumns);

                                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                    tx_GeneralDocRenewal.ModifiedDate = dtModified;
                                    tx_GeneralDocRenewal.ModifiedUser = model._UserId;

                                    CONTEXT.SaveChanges();

                                   if (model.ListReferences_ != null)
                                    {
                                        foreach (var product in model.ListReferences_)
                                        {
                                            Detail_Update(CONTEXT, product, model._UserId);
                                        }
                                    }


                                   SpNotif.SpSysControllerTransNotif(model._UserId, "GeneralDocRenewal", CONTEXT, "after", "Tx_GeneralDocRenewal", "update", "Id", keyValue);

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

                        SpNotif.SpSysControllerTransNotif(userId, "GeneralDocRenewal", CONTEXT, "before", "Tx_GeneralDocRenewal", "post", "Id", keyValue);

                        Tx_GeneralDocRenewal tx_GeneralDocRenewal = CONTEXT.Tx_GeneralDocRenewal.Find(Id);
                        if (tx_GeneralDocRenewal != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_GeneralDocRenewal.Status = "Posted";
                            tx_GeneralDocRenewal.ModifiedDate = dtModified;
                            tx_GeneralDocRenewal.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }
                        SpNotif.SpSysControllerTransNotif(userId, "GeneralDocRenewal", CONTEXT, "after", "Tx_GeneralDocRenewal", "post", "Id", keyValue);

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

                        SpNotif.SpSysControllerTransNotif(userId, "GeneralDocRenewal", CONTEXT, "before", "Tx_GeneralDocRenewal", "cancel", "Id", keyValue);

                        Tx_GeneralDocRenewal tx_GeneralDocRenewal = CONTEXT.Tx_GeneralDocRenewal.Find(Id);
                        if (tx_GeneralDocRenewal != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_GeneralDocRenewal.Status = "Cancel";
                            tx_GeneralDocRenewal.ModifiedDate = dtModified;
                            tx_GeneralDocRenewal.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }
                        SpNotif.SpSysControllerTransNotif(userId, "GeneralDocRenewal", CONTEXT, "after", "Tx_GeneralDocRenewal", "cancel", "Id", keyValue);


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

         

        public GeneralDocRenewalModel GetNewModel(int userId)
        {
            GeneralDocRenewalModel model = new GeneralDocRenewalModel();
            model.Status = "Draft";
            return model;
        }

        public GeneralDocRenewalModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public GeneralDocRenewalModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            GeneralDocRenewalModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.* 
                            FROM ""Tx_GeneralDocRenewal"" T0   
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<GeneralDocRenewalModel>(ssql, id).Single();

                model.ListReferences_ = this.GeneralDocRenewal_References(CONTEXT, id);
            }

            return model;
        }

        public GeneralDocRenewalModel NavFirst(int userId)
        {
            GeneralDocRenewalModel model = null;


            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GeneralDocRenewal");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GeneralDocRenewal\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public GeneralDocRenewalModel NavPrevious(int userId, long id = 0)
        {
            GeneralDocRenewalModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GeneralDocRenewal");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GeneralDocRenewal\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public GeneralDocRenewalModel NavNext(int userId, long id = 0)
        {
            GeneralDocRenewalModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GeneralDocRenewal");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GeneralDocRenewal\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public GeneralDocRenewalModel NavLast(int userId)
        {
            GeneralDocRenewalModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "GeneralDocRenewal");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_GeneralDocRenewal\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public List<GeneralDocRenewal_ReferenceModel> GeneralDocRenewal_References(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GeneralDocRenewal_References(CONTEXT, id);
            }

        }
        public List<GeneralDocRenewal_ReferenceModel> GeneralDocRenewal_References(HANA_APP CONTEXT, long id = 0)
        {
            return CONTEXT.Database.SqlQuery<GeneralDocRenewal_ReferenceModel>("SELECT T0.*, T1.\"Reference\" As \"ContentName_\", T1.\"IsMandatory\" AS \"IsMandatory_\", T1.\"IsFixed\" AS \"IsFixed_\" FROM \"Tx_GeneralDocRenewal_Reference\" T0 LEFT JOIN \"Tm_DocContent_Ref\" T1 ON T1.\"Id\" = T0.\"ContentId\" AND T1.\"DetId\" = T0.\"ContentDetId\" WHERE T0.\"Id\"=:p0 ORDER BY T0.\"DetId\" ", id).ToList();
        }

        public List<GeneralDocRenewal_ReferenceModel> GetModelByDocumentId(long documentId = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetModelByDocumentId(CONTEXT, documentId);
            }
        }

        public List<GeneralDocRenewal_ReferenceModel> GetModelByDocumentId(HANA_APP CONTEXT, long documentId = 0)
        {
            //SELECT T0."Id", T0."DetId", T0."ContentValue" AS "ContentOldValue" , T0."ContentId" AS "ContentId", T0."ContentDetId" AS "ContentDetId", T1."Reference" AS "ContentName_", T1."IsMandatory" AS "IsMandatory_", T1."ISFIXED" AS "IsFixed_"  
            //FROM "Tm_DocGeneral_Reference" T0 
            //INNER JOIN "Tm_DocContent_Ref" T1 ON T0."ContentId" = T1."Id" AND T0."ContentDetId" = T1."DetId"
            return CONTEXT.Database.SqlQuery<GeneralDocRenewal_ReferenceModel>("SELECT T0.\"Id\", T0.\"DetId\", T0.\"ContentId\" AS \"ContentId\", T0.\"ContentDetId\" AS \"ContentDetId\", T1.\"Reference\" AS \"ContentName_\", IFNULL(T0.\"ContentValue\",'')  AS \"ContentOldValue\", T1.\"IsMandatory\" AS \"IsMandatory_\", T1.\"IsFixed\" AS \"IsFixed_\"  FROM \"Tm_DocGeneral_Reference\" T0 INNER JOIN \"Tm_DocContent_Ref\" T1 ON T0.\"ContentId\" = T1.\"Id\" AND T0.\"ContentDetId\" = T1.\"DetId\"  WHERE T0.\"Id\"=:p0 ORDER BY T0.\"DetId\" ", documentId).ToList();
        }

        public long Detail_Add(HANA_APP CONTEXT, GeneralDocRenewal_ReferenceModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_GeneralDocRenewal_Reference tx_GeneralDocRenewal_Reference = new Tx_GeneralDocRenewal_Reference();

                CopyProperty.CopyProperties(model, tx_GeneralDocRenewal_Reference, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_GeneralDocRenewal_Reference.Id = Id;
                tx_GeneralDocRenewal_Reference.CreatedDate = dtModified;
                tx_GeneralDocRenewal_Reference.CreatedUser = UserId;
                tx_GeneralDocRenewal_Reference.ModifiedDate = dtModified;
                tx_GeneralDocRenewal_Reference.ModifiedUser = UserId;

                CONTEXT.Tx_GeneralDocRenewal_Reference.Add(tx_GeneralDocRenewal_Reference);
                CONTEXT.SaveChanges();
                DetId = tx_GeneralDocRenewal_Reference.DetId;

            }

            return DetId;

        }

        public void Detail_Update(HANA_APP CONTEXT, GeneralDocRenewal_ReferenceModel model, int UserId)
        {
            if (model != null)
            {

                Tx_GeneralDocRenewal_Reference tx_GeneralDocRenewal_Reference = CONTEXT.Tx_GeneralDocRenewal_Reference.Find(model.DetId);

                if (tx_GeneralDocRenewal_Reference != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, tx_GeneralDocRenewal_Reference, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    tx_GeneralDocRenewal_Reference.ModifiedDate = dtModified;
                    tx_GeneralDocRenewal_Reference.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, GeneralDocRenewal_ReferenceModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_GeneralDocRenewal_Reference\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
        
        public GeneralDocRenewalModel GetExpiredDate(int userId, int docId = 0, DateTime? expiredDate = null)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetWarningDate(CONTEXT, docId, expiredDate);
            }
        }

        public GeneralDocRenewalModel GetWarningDate(HANA_APP CONTEXT, int docId, DateTime? expiredDate)
        {
            GeneralDocRenewalModel model = null;
            if (docId != 0)
            {
                string ssql = @"CALL ""SpShippingDocRenewal_GetExpiredDate"" (:p0, :p1) ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                model = CONTEXT.Database.SqlQuery<GeneralDocRenewalModel>(ssql, docId, expiredDate).Single();
            }

            return model;
        }
    }


    #endregion

}