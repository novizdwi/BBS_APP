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



namespace Models.Transaction.CleaningTank
{
    #region Models

    public class CleaningTankModel
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
        
        public int? ShipId { get; set; }
        
        public string ShipName { get; set; }

        public DateTime? Periode { get; set; }
        
        public string ShipCode { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }


        public List<CleaningTank_DetailModel> ListDetails_ = new List<CleaningTank_DetailModel>();

        public CleaningTank_Detail Details_ { get; set;}

        public List<CleaningTank_AttachmentModel> ListAttachments_ = new List<CleaningTank_AttachmentModel>();

        public CleaningTank_Attachments Attachments_ { get; set; }
    }
    public class CleaningTank_Detail    {
        public List<long> deletedRowKeys { get; set; }
        public List<CleaningTank_DetailModel> insertedRowValues { get; set; }
        public List<CleaningTank_DetailModel> modifiedRowValues { get; set; }
    }
    public class CleaningTank_DetailModel
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
        
        public int? RowNum { get; set; }

        [Required(ErrorMessage = "required")]
        public int? ShipId { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipName { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipCode { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Remark { get; set; }
    }
    public class CleaningTank_Attachments
    {
        public List<long> deletedRowKeys { get; set; }
        public List<CleaningTank_AttachmentModel> insertedRowValues { get; set; }
        public List<CleaningTank_AttachmentModel> modifiedRowValues { get; set; }
    }
    public class CleaningTank_AttachmentModel
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

    public class CleaningTankService
    {

        public CleaningTankModel GetNewModel(int userId)
        {
            CleaningTankModel model = new CleaningTankModel();
            model.Status = "Draft";
            return model;
        }
        public CleaningTankModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public CleaningTankModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            CleaningTankModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, T1.""FirstName"" AS ""UserName"" 
                            FROM ""Tx_CleaningTank"" T0
                            LEFT JOIN ""Tm_User"" T1 ON T0.""ModifiedUser"" = T1.""Id""
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<CleaningTankModel>(ssql, id).Single();

                model.ListDetails_ = this.CleaningTank_Details(CONTEXT, id);
                model.ListAttachments_ = this.CleaningTank_Attachments(CONTEXT, id);
            }

            return model;
        }
        public List<CleaningTank_DetailModel> CleaningTank_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return CleaningTank_Details(CONTEXT, id);
            }

        }

        public List<CleaningTank_DetailModel> CleaningTank_Details(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<CleaningTank_DetailModel>("SELECT * FROM \"Tx_CleaningTank_Detail\" WHERE \"Id\" =:p0", id).ToList();
        }
        public CleaningTankModel NavFirst(int userId)
        {
            CleaningTankModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_CleaningTank\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public CleaningTankModel NavPrevious(int userId, long id = 0)
        {
            CleaningTankModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_CleaningTank\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public CleaningTankModel NavNext(int userId, long id = 0)
        {
            CleaningTankModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_CleaningTank\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public CleaningTankModel NavLast(int userId)
        {
            CleaningTankModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_CleaningTank\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        public long Add(CleaningTankModel model)
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

                            Tx_CleaningTank Tx_CleaningTank = new Tx_CleaningTank();
                            CopyProperty.CopyProperties(model, Tx_CleaningTank, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_CleaningTank.TransType = "CleaningTank";
                            Tx_CleaningTank.CreatedDate = dtModified;
                            Tx_CleaningTank.CreatedUser = model._UserId;
                            Tx_CleaningTank.ModifiedDate = dtModified;
                            Tx_CleaningTank.ModifiedUser = model._UserId;

                            string dateX = model.Periode.Value.ToString("yyyy");
                        
                            string transNo = "CT/" + dateX;
                            Tx_CleaningTank.TransNo = transNo;

                            string cek = CONTEXT.Database.SqlQuery<string>(@"SELECT ""TransNo"" FROM  ""Tx_CleaningTank"" WHERE ""TransNo""='" + transNo + "'").FirstOrDefault();
                            if (cek == null)
                            {
                                CONTEXT.Tx_CleaningTank.Add(Tx_CleaningTank);
                                CONTEXT.SaveChanges();
                                Id = Tx_CleaningTank.Id;

                                String keyValue;
                                keyValue = Tx_CleaningTank.Id.ToString();

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
                                            CleaningTank_DetailModel detailModel = new CleaningTank_DetailModel();
                                            detailModel.DetId = detId;
                                            Detail_Delete(CONTEXT, detailModel);
                                        }
                                    }
                                }



                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_CleaningTank", "add", "Id", keyValue);


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
        public void Update(CleaningTankModel model)
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


                                Tx_CleaningTank Tx_CleaningTank = CONTEXT.Tx_CleaningTank.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_CleaningTank.ModifiedDate = dtModified;
                                Tx_CleaningTank.ModifiedUser = model._UserId;
                                if (Tx_CleaningTank != null)
                                {
                                    var exceptColumns = new string[] { "Id","TransNo","CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_CleaningTank, false, exceptColumns);
                                    Tx_CleaningTank.ModifiedDate = dtModified;
                                    Tx_CleaningTank.ModifiedUser = model._UserId;
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
                                                CleaningTank_DetailModel detailModel = new CleaningTank_DetailModel();
                                                detailModel.DetId = detId;
                                               Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }

                                    
                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", " Tx_CleaningTank", "update", "Id", keyValue);

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
        public long Detail_Add(HANA_APP CONTEXT, CleaningTank_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_CleaningTank_Detail Tx_CleaningTank_Detail = new Tx_CleaningTank_Detail();

                CopyProperty.CopyProperties(model, Tx_CleaningTank_Detail, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_CleaningTank_Detail.Id = Id;
                Tx_CleaningTank_Detail.CreatedDate = dtModified;
                Tx_CleaningTank_Detail.CreatedUser = UserId;
                Tx_CleaningTank_Detail.ModifiedDate = dtModified;
                Tx_CleaningTank_Detail.ModifiedUser = UserId;
               

                CONTEXT.Tx_CleaningTank_Detail.Add(Tx_CleaningTank_Detail);
                CONTEXT.SaveChanges();
                DetId = Tx_CleaningTank_Detail.DetId;

            }

            return DetId;

        }
        public void Detail_Update(HANA_APP CONTEXT, CleaningTank_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_CleaningTank_Detail Tx_CleaningTank_Detail = CONTEXT.Tx_CleaningTank_Detail.Find(model.DetId);

                if (Tx_CleaningTank_Detail != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_CleaningTank_Detail, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_CleaningTank_Detail.ModifiedDate = dtModified;
                    Tx_CleaningTank_Detail.ModifiedUser = UserId;
                    
                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Detail_Delete(HANA_APP CONTEXT, CleaningTank_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_CleaningTank_Detail\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
        
        public List<CleaningTank_AttachmentModel> CleaningTank_Attachments(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return CleaningTank_Attachments(CONTEXT, id);
            }

        }
        public List<CleaningTank_AttachmentModel> CleaningTank_Attachments(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<CleaningTank_AttachmentModel>("SELECT T0.\"Id\", T0.\"DetId\", T0.\"FileName\" FROM \"Tx_CleaningTank_Attachment\" T0 WHERE T0.\"Id\"=:p0 ORDER BY T0.\"DetId\" ", id).ToList();


        }
        public long Attachment_Add(HANA_APP CONTEXT, CleaningTank_AttachmentModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_CleaningTank_Attachment tx_CleaningTank_Attachment = new Tx_CleaningTank_Attachment();

                CopyProperty.CopyProperties(model, tx_CleaningTank_Attachment, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_CleaningTank_Attachment.Id = Id;
                tx_CleaningTank_Attachment.CreatedDate = dtModified;
                tx_CleaningTank_Attachment.CreatedUser = UserId;
                tx_CleaningTank_Attachment.ModifiedDate = dtModified;
                tx_CleaningTank_Attachment.ModifiedUser = UserId;

                CONTEXT.Tx_CleaningTank_Attachment.Add(tx_CleaningTank_Attachment);
                CONTEXT.SaveChanges();
                DetId = tx_CleaningTank_Attachment.DetId;

            }

            return DetId;

        }
        public long Attachment_Add(List<CleaningTank_AttachmentModel> ListModel)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Attachment_Add(CONTEXT, ListModel);
            }

        }
        public long Attachment_Add(HANA_APP CONTEXT, List<CleaningTank_AttachmentModel> ListModel)
        {
            long Id = 0;
            long DetId = 0;

            if (ListModel != null)
            {

                for (int i = 0; i < ListModel.Count; i++)
                {
                    Tx_CleaningTank_Attachment tx_CleaningTank_Attachment = new Tx_CleaningTank_Attachment();
                    var model = ListModel[i];

                    CopyProperty.CopyProperties(model, tx_CleaningTank_Attachment, false);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    tx_CleaningTank_Attachment.Id = model.Id;
                    tx_CleaningTank_Attachment.CreatedDate = dtModified;
                    tx_CleaningTank_Attachment.CreatedUser = model._UserId;
                    tx_CleaningTank_Attachment.ModifiedDate = dtModified;
                    tx_CleaningTank_Attachment.ModifiedUser = model._UserId;

                    CONTEXT.Tx_CleaningTank_Attachment.Add(tx_CleaningTank_Attachment);
                    CONTEXT.SaveChanges();
                    DetId = tx_CleaningTank_Attachment.DetId;
                }



            }

            return Id;

        }
        public void Attachment_Delete(CleaningTank_AttachmentModel model)
        {
            if (model.DetId != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    Attachment_Delete(CONTEXT, model);
                }
            }

        }
        public void Attachment_Delete(HANA_APP CONTEXT, CleaningTank_AttachmentModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_CleaningTank_Attachment\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }


    }


    #endregion

}