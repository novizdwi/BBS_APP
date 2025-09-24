using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Transactions;
using Models._Utils;
using Models._Ef;
using BBS_DI.Models._EF;

using Models._Sap;
using SAPbobsCOM;


namespace Models.Master.DocGeneral
{

    #region Models




    public class DocGeneralModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }


        public int Id { get; set; }

        [Required(ErrorMessage = "required")]
        public string DocumentName { get; set; }

        [Required(ErrorMessage = "required")]
        public int? Type { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public DateTime? WarningDate { get; set; }

        public string IsActive { get; set; }

        public string Remarks { get; set; }

        public List<DocGeneral_ReferenceModel> ListReferences_ = new List<DocGeneral_ReferenceModel>();
        public List<DocGeneral_AttachmentModel> ListAttachments_ = new List<DocGeneral_AttachmentModel>();
    }

    public class DocGeneral_ReferenceModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public int Id { get; set; }

        public int DetId { get; set; }

        [Required(ErrorMessage = "required")]
        public int? ContentId { get; set; }

        [Required(ErrorMessage = "required")]
        public int? ContentDetId { get; set; }

        public string ContentName_ { get; set; }

        public string ContentValue { get; set; }

        public string IsMandatory_ { get; set; }

    }

    public class DocGeneral_AttachmentModel
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

    public class DocGeneralService
    {

        public long Add(DocGeneralModel model)
        {
            int Id = 0;

            if (model != null)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {

                            Tm_DocGeneral Tm_DocGeneral = new Tm_DocGeneral();
                            CopyProperty.CopyProperties(model, Tm_DocGeneral, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tm_DocGeneral.TransType = "DocGeneral";
                            Tm_DocGeneral.CreatedDate = dtModified;
                            Tm_DocGeneral.CreatedUser = model._UserId;
                            Tm_DocGeneral.ModifiedDate = dtModified;
                            Tm_DocGeneral.ModifiedUser = model._UserId;


                            CONTEXT.Tm_DocGeneral.Add(Tm_DocGeneral);
                            CONTEXT.SaveChanges();
                            Id = Tm_DocGeneral.Id;

                            String keyValue;
                            keyValue = Tm_DocGeneral.Id.ToString();


                            if (model.ListReferences_ != null)
                            {
                                foreach (var product in model.ListReferences_)
                                {
                                    Detail_Add(CONTEXT, product, Id, model._UserId);
                                }
                            }

                            //if (model.DetailReferences_ != null)
                            //{
                            //    //di Add Dulu data dari master, baru di update datanya di bawah
                            //    CONTEXT.Database.ExecuteSqlCommand("CALL \"SpDocGeneral_AddReference\"(:p0,:p1, :p2)",model._UserId,  Id, model.Type);

                            //    //kalo baru pasti disini
                            //    if (model.DetailReferences_.modifiedRowValues != null)
                            //    {
                            //        foreach (var reference in model.DetailReferences_.modifiedRowValues)
                            //        {
                            //            Detail_AddBaseOnContentDetId(CONTEXT, reference, Id, model._UserId);
                            //            //Detail_Add(CONTEXT, reference, Id, model._UserId);
                            //        }
                            //    }
                            //}

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_DocGeneral", "add", "Id", keyValue);


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

   
        public void Update(DocGeneralModel model)
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

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", "Tm_DocGeneral", "update", "Id", keyValue);


                                Tm_DocGeneral Tm_DocGeneral = CONTEXT.Tm_DocGeneral.Find(model.Id);
                                if (Tm_DocGeneral != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransType"};
                                    CopyProperty.CopyProperties(model, Tm_DocGeneral, false, exceptColumns);
                                    CONTEXT.SaveChanges();

                                    if (model.ListReferences_ != null)
                                    {
                                        foreach (var product in model.ListReferences_)
                                        {
                                            Detail_Update(CONTEXT ,product, model._UserId);
                                        }
                                    }

                                    //if (model.DetailReferences_ != null)
                                    //{
                                    //    if (model.DetailReferences_.insertedRowValues != null)
                                    //    {
                                    //        foreach (var reference in model.DetailReferences_.insertedRowValues)
                                    //        {
                                    //            Detail_Add(CONTEXT, reference, model.Id, model._UserId);
                                    //        }
                                    //    }

                                    //    if (model.DetailReferences_.modifiedRowValues != null)
                                    //    {
                                    //        foreach (var reference in model.DetailReferences_.modifiedRowValues)
                                    //        {
                                    //            Detail_Update(CONTEXT, reference, model._UserId);
                                    //        }
                                    //    }

                                    //    if (model.DetailReferences_.deletedRowKeys != null)
                                    //    {
                                    //        foreach (var detId in model.DetailReferences_.deletedRowKeys)
                                    //        {
                                    //            DocGeneral_ReferenceModel referenceModel = new DocGeneral_ReferenceModel();
                                    //            referenceModel.DetId = detId;
                                    //            Detail_Delete(CONTEXT, referenceModel);
                                    //        }
                                    //    }
                                    //}


                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_DocGeneral", "update", "Id", keyValue);

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


        public DocGeneralModel GetNewModel(int userId)
        {
            DocGeneralModel model = new DocGeneralModel();
            return model;
        }

        public DocGeneralModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public DocGeneralModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            DocGeneralModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.*
                            FROM ""Tm_DocGeneral"" T0
                            WHERE T0.""Id""=:p0 ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                model = CONTEXT.Database.SqlQuery<DocGeneralModel>(ssql, id).Single();

                //add dulu dari master yang belum ada
                CONTEXT.Database.ExecuteSqlCommand("CALL \"SpDocGeneral_AddNewReference\"(:p0, :p1)", userId, id);

                model.ListReferences_ = this.DocGeneral_References(CONTEXT, id);
                model.ListAttachments_ = this.DocGeneral_Attachments(CONTEXT, id);
            }

            return model;
        }

        public DocGeneralModel NavFirst(int userId)
        {
            DocGeneralModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DocGeneral");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_DocGeneral\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public DocGeneralModel NavPrevious(int userId, long id = 0)
        {
            DocGeneralModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DocGeneral");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_DocGeneral\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public DocGeneralModel NavNext(int userId, long id = 0)
        {
            DocGeneralModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DocGeneral");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_DocGeneral\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public DocGeneralModel NavLast(int userId)
        {
            DocGeneralModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DocGeneral");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_DocGeneral\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }


        public List<DocGeneral_AttachmentModel> DocGeneral_Attachments(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return DocGeneral_Attachments(CONTEXT, id);
            }

        }

        public List<DocGeneral_AttachmentModel> DocGeneral_Attachments(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<DocGeneral_AttachmentModel>("SELECT T0.\"Id\", T0.\"DetId\", T0.\"FileName\" FROM \"Tm_DocGeneral_Attachment\" T0 WHERE T0.\"Id\"=:p0 ORDER BY T0.\"DetId\" ", id).ToList();


        }

        public long Detail_Add(HANA_APP CONTEXT, DocGeneral_AttachmentModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tm_DocGeneral_Attachment tx_DocGeneral_Attachment = new Tm_DocGeneral_Attachment();

                CopyProperty.CopyProperties(model, tx_DocGeneral_Attachment, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_DocGeneral_Attachment.Id = Id;
                tx_DocGeneral_Attachment.CreatedDate = dtModified;
                tx_DocGeneral_Attachment.CreatedUser = UserId;
                tx_DocGeneral_Attachment.ModifiedDate = dtModified;
                tx_DocGeneral_Attachment.ModifiedUser = UserId;

                CONTEXT.Tm_DocGeneral_Attachment.Add(tx_DocGeneral_Attachment);
                CONTEXT.SaveChanges();
                DetId = tx_DocGeneral_Attachment.DetId;

            }

            return DetId;

        }

        public long Detail_Add(List<DocGeneral_AttachmentModel> ListModel)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Detail_Add(CONTEXT, ListModel);
            }

        }

        public long Detail_Add(HANA_APP CONTEXT, List<DocGeneral_AttachmentModel> ListModel)
        {
            long Id = 0;
            long DetId = 0;

            if (ListModel != null)
            {

                for (int i = 0; i < ListModel.Count; i++)
                {
                    Tm_DocGeneral_Attachment tx_DocGeneral_Attachment = new Tm_DocGeneral_Attachment();
                    var model = ListModel[i];

                    CopyProperty.CopyProperties(model, tx_DocGeneral_Attachment, false);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    tx_DocGeneral_Attachment.Id = model.Id;
                    tx_DocGeneral_Attachment.CreatedDate = dtModified;
                    tx_DocGeneral_Attachment.CreatedUser = model._UserId;
                    tx_DocGeneral_Attachment.ModifiedDate = dtModified;
                    tx_DocGeneral_Attachment.ModifiedUser = model._UserId;

                    CONTEXT.Tm_DocGeneral_Attachment.Add(tx_DocGeneral_Attachment);
                    CONTEXT.SaveChanges();
                    DetId = tx_DocGeneral_Attachment.DetId;
                }



            }

            return Id;

        }

        public void Detail_Delete(DocGeneral_AttachmentModel model)
        {
            if (model.DetId != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    Detail_Delete(CONTEXT, model);
                }
            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, DocGeneral_AttachmentModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_DocGeneral_Attachment\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }

        public bool ChooseItem(int UserId, long Id, string[] data)
        {


            return true;

        }

        public List<DocGeneral_ReferenceModel> DocGeneral_References(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return DocGeneral_References(CONTEXT, id);
            }

        }

        public List<DocGeneral_ReferenceModel> DocGeneral_References(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<DocGeneral_ReferenceModel>("SELECT T0.*, IFNULL(T1.\"Reference\", '') As \"ContentName_\", IFNULL(T1.\"IsMandatory\", 'N') AS \"IsMandatory_\" FROM \"Tm_DocGeneral_Reference\" T0 INNER JOIN \"Tm_DocContent_Ref\" T1 ON T0.\"ContentId\" = T1.\"Id\" AND T0.\"ContentDetId\" = T1.\"DetId\" WHERE T0.\"Id\"=:p0 ORDER BY T0.\"DetId\" ", id).ToList();
        }

        public List<DocGeneral_ReferenceModel> GetModelByDocType(int docType = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return DocGeneral_GetRefByDocType(CONTEXT, docType);
            }
        }

        public List<DocGeneral_ReferenceModel> DocGeneral_GetRefByDocType(HANA_APP CONTEXT, int docType = 0)
        {
            return CONTEXT.Database.SqlQuery<DocGeneral_ReferenceModel>("SELECT T1.\"Id\", T1.\"DetId\", T1.\"Id\" AS \"ContentId\", T1.\"DetId\" AS \"ContentDetId\", T1.\"Reference\" AS \"ContentName_\", T1.\"IsMandatory\" AS \"IsMandatory_\"  FROM \"Tm_DocContent\" T0 INNER JOIN \"Tm_DocContent_Ref\" T1 ON T1.\"Id\" = T0.\"Id\" WHERE T0.\"Id\"=:p0 ORDER BY T1.\"DetId\" ", docType).ToList();
        }

        public long Detail_Add(HANA_APP CONTEXT, DocGeneral_ReferenceModel model, int Id, int UserId)
        {
            int DetId = 0;

            if (model != null)
            {

                Tm_DocGeneral_Reference Tm_DocGeneral_Reference = new Tm_DocGeneral_Reference();

                CopyProperty.CopyProperties(model, Tm_DocGeneral_Reference, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tm_DocGeneral_Reference.Id = Id;
                Tm_DocGeneral_Reference.CreatedDate = dtModified;
                Tm_DocGeneral_Reference.CreatedUser = UserId;
                Tm_DocGeneral_Reference.ModifiedDate = dtModified;
                Tm_DocGeneral_Reference.ModifiedUser = UserId;

                CONTEXT.Tm_DocGeneral_Reference.Add(Tm_DocGeneral_Reference);
                CONTEXT.SaveChanges();
                DetId = Tm_DocGeneral_Reference.DetId;

            }

            return DetId;

        }

        public void Detail_Update(HANA_APP CONTEXT, DocGeneral_ReferenceModel model, int UserId)
        {
            if (model != null)
            {

                Tm_DocGeneral_Reference Tm_DocGeneral_Reference = CONTEXT.Tm_DocGeneral_Reference.Find(model.DetId);

                if (Tm_DocGeneral_Reference != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tm_DocGeneral_Reference, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tm_DocGeneral_Reference.ModifiedDate = dtModified;
                    Tm_DocGeneral_Reference.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, DocGeneral_ReferenceModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_DocGeneral_Reference\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }


     

    }


    #endregion

}