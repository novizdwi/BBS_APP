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


namespace Models.Master.DocContent
{

    #region Models




    public class DocContentModel
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

        public string Type { get; set; }

         [Required(ErrorMessage = "required")]
        public string ExpiredType { get; set; }

        public int? WarningNotif { get; set; }

        public string IsActive { get; set; }

        public string Remarks { get; set; }

        public List<DocContent_RefModel> ListRefs_ = new List<DocContent_RefModel>();

        public DocContent_Refs DetailRefs_ { get; set; }
      
    }

    public class DocContent_Refs
    {
        public List<int> deletedRowKeys { get; set; }
        public List<DocContent_RefModel> insertedRowValues { get; set; }
        public List<DocContent_RefModel> modifiedRowValues { get; set; }
    }

    public class DocContent_RefModel
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

        public string Reference { get; set; }

        public string IsMandatory { get; set; }

        public string IsFixed { get; set; }
    }

    //Get JSON
    public class DocContentWarningModel
    {

        public DateTime? WarningDate { get; set; }

    }

    #endregion

    #region Services

    public class DocContentService
    {

        public long Add(DocContentModel model)
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

                            Tm_DocContent Tm_DocContent = new Tm_DocContent();
                            CopyProperty.CopyProperties(model, Tm_DocContent, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tm_DocContent.TransType = "DocContent";
                            Tm_DocContent.CreatedDate = dtModified;
                            Tm_DocContent.CreatedUser = model._UserId;
                            Tm_DocContent.ModifiedDate = dtModified;
                            Tm_DocContent.ModifiedUser = model._UserId;


                            CONTEXT.Tm_DocContent.Add(Tm_DocContent);
                            CONTEXT.SaveChanges();
                            Id = Tm_DocContent.Id;

                            String keyValue;
                            keyValue = Tm_DocContent.Id.ToString();

                            if (model.DetailRefs_ != null)
                            {
                                if (model.DetailRefs_.insertedRowValues != null)
                                {
                                    foreach (var reference in model.DetailRefs_.insertedRowValues)
                                    {
                                        Detail_Add(CONTEXT, reference, Id, model._UserId);
                                    }
                                }

                                if (model.DetailRefs_.modifiedRowValues != null)
                                {
                                    foreach (var reference in model.DetailRefs_.modifiedRowValues)
                                    {
                                        Detail_Update(CONTEXT, reference, model._UserId);
                                    }
                                }

                                if (model.DetailRefs_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.DetailRefs_.deletedRowKeys)
                                    {
                                        DocContent_RefModel referenceModel = new DocContent_RefModel();
                                        referenceModel.DetId = detId;
                                        Detail_Delete(CONTEXT, referenceModel);
                                    }
                                }
                            }

                            

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_DocContent", "add", "Id", keyValue);


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

   
        public void Update(DocContentModel model)
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

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", "Tm_DocContent", "update", "Id", keyValue);


                                Tm_DocContent Tm_DocContent = CONTEXT.Tm_DocContent.Find(model.Id);
                                if (Tm_DocContent != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransType"};
                                    CopyProperty.CopyProperties(model, Tm_DocContent, false, exceptColumns);
                                    CONTEXT.SaveChanges();


                                    if (model.DetailRefs_ != null)
                                    {
                                        if (model.DetailRefs_.insertedRowValues != null)
                                        {
                                            foreach (var reference in model.DetailRefs_.insertedRowValues)
                                            {
                                                Detail_Add(CONTEXT, reference, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.DetailRefs_.modifiedRowValues != null)
                                        {
                                            foreach (var reference in model.DetailRefs_.modifiedRowValues)
                                            {
                                                Detail_Update(CONTEXT, reference, model._UserId);
                                            }
                                        }

                                        if (model.DetailRefs_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.DetailRefs_.deletedRowKeys)
                                            {
                                                DocContent_RefModel referenceModel = new DocContent_RefModel();
                                                referenceModel.DetId = detId;
                                                Detail_Delete(CONTEXT, referenceModel);
                                            }
                                        }
                                    }


                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_DocContent", "update", "Id", keyValue);

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


        public DocContentModel GetNewModel(int userId)
        {
            DocContentModel model = new DocContentModel();
            return model;
        }

        public DocContentModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public DocContentModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            DocContentModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.*
                            FROM ""Tm_DocContent"" T0   
                            WHERE T0.""Id""=:p0 ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                model = CONTEXT.Database.SqlQuery<DocContentModel>(ssql, id).Single();
                model.ListRefs_ = this.DocContent_Refs(CONTEXT, id);
      
            }

            return model;
        }

        public DocContentModel NavFirst(int userId)
        {
            DocContentModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DocContent");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_DocContent\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public DocContentModel NavPrevious(int userId, long id = 0)
        {
            DocContentModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DocContent");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_DocContent\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public DocContentModel NavNext(int userId, long id = 0)
        {
            DocContentModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DocContent");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_DocContent\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public DocContentModel NavLast(int userId)
        {
            DocContentModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "DocContent");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_DocContent\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }

        public bool ChooseItem(int UserId, long Id, string[] data)
        {


            return true;

        }

        public List<DocContent_RefModel> DocContent_Refs(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return DocContent_Refs(CONTEXT, id);
            }

        }
        public List<DocContent_RefModel> DocContent_Refs(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<DocContent_RefModel>("SELECT T0.* FROM \"Tm_DocContent_Ref\" T0 WHERE T0.\"Id\"=:p0 ORDER BY T0.\"DetId\" ", id).ToList();
        }

        public long Detail_Add(HANA_APP CONTEXT, DocContent_RefModel model, int Id, int UserId)
        {
            int DetId = 0;

            if (model != null)
            {

                Tm_DocContent_Ref Tm_DocContent_Ref = new Tm_DocContent_Ref();

                CopyProperty.CopyProperties(model, Tm_DocContent_Ref, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tm_DocContent_Ref.Id = Id;
                Tm_DocContent_Ref.CreatedDate = dtModified;
                Tm_DocContent_Ref.CreatedUser = UserId;
                Tm_DocContent_Ref.ModifiedDate = dtModified;
                Tm_DocContent_Ref.ModifiedUser = UserId;

                CONTEXT.Tm_DocContent_Ref.Add(Tm_DocContent_Ref);
                CONTEXT.SaveChanges();
                DetId = Tm_DocContent_Ref.DetId;

            }

            return DetId;

        }

        public void Detail_Update(HANA_APP CONTEXT, DocContent_RefModel model, int UserId)
        {
            if (model != null)
            {

                Tm_DocContent_Ref Tm_DocContent_Ref = CONTEXT.Tm_DocContent_Ref.Find(model.DetId);

                if (Tm_DocContent_Ref != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tm_DocContent_Ref, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tm_DocContent_Ref.ModifiedDate = dtModified;
                    Tm_DocContent_Ref.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, DocContent_RefModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_DocContent_Ref\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }



        public DocContentWarningModel GetWarningDate(int userId, int docId = 0, DateTime? expiredDate = null)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetWarningDate(CONTEXT, docId, expiredDate);
            }
        }

        public DocContentWarningModel GetWarningDate(HANA_APP CONTEXT, int docId, DateTime? expiredDate)
        {
            DocContentWarningModel model = null;
            if (docId != 0)
            {
                string ssql = @"CALL ""SpDocContent_GetWarningDate"" (:p0, :p1) ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                model = CONTEXT.Database.SqlQuery<DocContentWarningModel>(ssql, docId, expiredDate).Single();
            }

            return model;
        }


        public string GetExpiredMandatory(int userId, int docId = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetExpiredMandatory(CONTEXT, docId);
            }
        }

        public string GetExpiredMandatory(HANA_APP CONTEXT, int docId)
        {
            string mandatory = "N";
            if (docId != 0)
            {
                string ssql = @"SELECT CASE WHEN ""ExpiredType"" = 'Expired' THEN 'Y' ELSE 'N' END AS ""Mandatory"" FROM ""Tm_DocContent"" WHERE ""Id"" = :p0 ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                mandatory = CONTEXT.Database.SqlQuery<string>(ssql, docId).SingleOrDefault();
            }

            return mandatory;
        }

    }


    #endregion

}