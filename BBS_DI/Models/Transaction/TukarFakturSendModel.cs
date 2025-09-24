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


namespace Models.Transaction.TukarFakturSend
{
    #region Models
    public static class TukarFakturSendGetList
    {
        public static DataTable GetInvoiceList(long Id)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetInvoiceList(CONTEXT, Id);
            }
        }
        public static DataTable GetInvoiceList(HANA_APP CONTEXT, long Id)
        {
            var ssql = string.Format(@"SELECT T0.""DocNum"" AS ""Code"", T0.""DocNum"" AS ""Name"" FROM ""{0}"".""OINV"" T0 WHERE ""DocStatus"" = 'O' AND ""U_IDU_NoTFS"" IS NULL AND T0.""CardCode"" = (SELECT ""CardCode"" FROM ""{0}"".""OCRD"" WHERE ""CardName"" = (SELECT TOP 1 ""NamaCustomer"" FROM ""{1}"".""Tx_TukarFakturSend"" WHERE ""Id"" = '{2}' LIMIT 1) LIMIT 1)", DbProvider.dbSap_Name, DbProvider.dbApp_Name, Id);

            return GetDataTable(CONTEXT, ssql, Id);
        }
        public static DataTable GetDataTable(HANA_APP CONTEXT, string sql, params object[] parameters)
        {
            var s = EfIduHanaRsExtensionsApp.IduGetDataTable(CONTEXT, sql, parameters);

            return s;
        }

        public static DataTable GetDetailNoArInvoice(long docNum)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDetailNoArInvoice(CONTEXT, docNum);
            }
        }
        public static DataTable GetDetailNoArInvoice(HANA_APP CONTEXT, long docNum)
        {
            var ssql = "SELECT T0.\"DocDate\" AS \"TanggalInvoice\", T0.\"DocTotal\" AS \"TotalInvoice\", T0.\"NumAtCard\" AS \"NoInvoiceRevisi\" FROM \"" + DbProvider.dbSap_Name + "\".\"OINV\" T0 WHERE \"DocStatus\" = 'O' AND T0.\"DocNum\" = " + docNum;
            //var ssql = "SELECT T0.\"DocNum\" AS \"Code\", T0.\"DocNum\" AS \"Name\" FROM \"" + DbProvider.dbSap_Name + "\".\"OINV\" T0 WHERE \"DocStatus\" = 'O'";

            return GetDataTable(CONTEXT, ssql, "");
        }

    }

    public class TukarFakturSendModel
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

        public string NoDokumen { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime? TglDokumen { get; set; }
        public string MetodeKirim { get; set; }
        public string Pengiriman { get; set; }
        public string NamaCustomer { get; set; }
        public string AlamatKirim { get; set; }
        public string NoInvoice { get; set; }

        public string Status { get; set; }

        public List<TukarFakturSend_DetailModel> ListDetails_ = new List<TukarFakturSend_DetailModel>();
        public TukarFakturSend_Details DetailDetails_ { get; set; }

    }
    public class TukarFakturSend_DetailModel
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

        public string NoArInvoice { get; set; }

        public DateTime? TanggalInvoice { get; set; }

        public decimal? TotalInvoice { get; set; }

        public string NoInvoiceRevisi { get; set; }

    }
    public class TukarFakturSend_Details
    {
        public List<int> deletedRowKeys { get; set; }
        public List<TukarFakturSend_DetailModel> insertedRowValues { get; set; }
        public List<TukarFakturSend_DetailModel> modifiedRowValues { get; set; }
    }


    #endregion

    #region Services

    public class TukarFakturSendService
    {

        public long Add(TukarFakturSendModel model)
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

                            Tx_TukarFakturSend tx_TukarFakturSend = new Tx_TukarFakturSend();
                            CopyProperty.CopyProperties(model, tx_TukarFakturSend, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TukarFakturSend.CreatedDate = dtModified;
                            tx_TukarFakturSend.CreatedUser = model._UserId;
                            tx_TukarFakturSend.ModifiedDate = dtModified;
                            tx_TukarFakturSend.ModifiedUser = model._UserId;


                            string dateX = model.TglDokumen.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'TukarFakturSend','" + dateX + "','') ").SingleOrDefault();
                            tx_TukarFakturSend.NoDokumen = transNo;

                            CONTEXT.Tx_TukarFakturSend.Add(tx_TukarFakturSend);
                            CONTEXT.SaveChanges();
                            Id = tx_TukarFakturSend.Id;

                            String keyValue;
                            keyValue = tx_TukarFakturSend.Id.ToString();

                            SpNotif.SpSysControllerTransNotif(model._UserId, "TukarFakturSend", CONTEXT, "after", "Tx_TukarFakturSend", "add", "Id", keyValue);

                            if(model.DetailDetails_ != null)
                            {
                                if (model.DetailDetails_.insertedRowValues != null)
                                {
                                    foreach (var coa in model.DetailDetails_.insertedRowValues)
                                    {
                                        Detail_Add(CONTEXT, coa, Id, model._UserId);
                                    }
                                }


                                if (model.DetailDetails_.modifiedRowValues != null)
                                {
                                    foreach (var coa in model.DetailDetails_.modifiedRowValues)
                                    {
                                        Detail_Update(CONTEXT, coa, model._UserId);
                                    }
                                }

                                if (model.DetailDetails_.deletedRowKeys != null)
                                {
                                    foreach (var id in model.DetailDetails_.deletedRowKeys)
                                    {
                                        TukarFakturSend_DetailModel detail = new TukarFakturSend_DetailModel();
                                        detail.Id = id;
                                        Detail_Delete(CONTEXT, detail);
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


            return Id;

        }

        public void Update(TukarFakturSendModel model)
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

                                SpNotif.SpSysControllerTransNotif(model._UserId, "TukarFakturSend", CONTEXT, "before", "Tx_TukarFakturSend", "update", "Id", keyValue);

                                Tx_TukarFakturSend tx_TukarFakturSend = CONTEXT.Tx_TukarFakturSend.Find(model.Id);
                                if (tx_TukarFakturSend != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransType", "Status", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_TukarFakturSend, false, exceptColumns);

                                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                    tx_TukarFakturSend.ModifiedDate = dtModified;
                                    tx_TukarFakturSend.ModifiedUser = model._UserId;

                                    CONTEXT.SaveChanges();

                                    SpNotif.SpSysControllerTransNotif(model._UserId, "TukarFakturSend", CONTEXT, "after", "Tx_TukarFakturSend", "update", "Id", keyValue);
                                }
                                if (model.DetailDetails_ != null)
                                {
                                    if (model.DetailDetails_.insertedRowValues != null)
                                    {
                                        foreach (var coa in model.DetailDetails_.insertedRowValues)
                                        {
                                            Detail_Add(CONTEXT, coa, model.Id, model._UserId);
                                        }
                                    }


                                    if (model.DetailDetails_.modifiedRowValues != null)
                                    {
                                        foreach (var coa in model.DetailDetails_.modifiedRowValues)
                                        {
                                            Detail_Update(CONTEXT, coa, model._UserId);
                                        }
                                    }

                                    if (model.DetailDetails_.deletedRowKeys != null)
                                    {
                                        foreach (var id in model.DetailDetails_.deletedRowKeys)
                                        {
                                            TukarFakturSend_DetailModel detail = new TukarFakturSend_DetailModel();
                                            detail.Id = id;
                                            Detail_Delete(CONTEXT, detail);
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

                        SpNotif.SpSysControllerTransNotif(userId, "TukarFakturSend", CONTEXT, "before", "Tx_TukarFakturSend", "post", "Id", keyValue);

                        Tx_TukarFakturSend tx_TukarFakturSend = CONTEXT.Tx_TukarFakturSend.Find(Id);
                        if (tx_TukarFakturSend != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TukarFakturSend.Status = "Posted";
                            tx_TukarFakturSend.ModifiedDate = dtModified;
                            tx_TukarFakturSend.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "TukarFakturSend", CONTEXT, "after", "Tx_TukarFakturSend", "post", "Id", keyValue);

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

                        SpNotif.SpSysControllerTransNotif(userId, "TukarFakturSend", CONTEXT, "before", "Tx_TukarFakturSend", "cancel", "Id", keyValue);

                        Tx_TukarFakturSend tx_TukarFakturSend = CONTEXT.Tx_TukarFakturSend.Find(Id);
                        if (tx_TukarFakturSend != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_TukarFakturSend.Status = "Cancel";
                            tx_TukarFakturSend.ModifiedDate = dtModified;
                            tx_TukarFakturSend.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "TukarFakturSend", CONTEXT, "after", "Tx_TukarFakturSend", "cancel", "Id", keyValue);


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

        public TukarFakturSendModel GetNewModel(int userId)
        {
            TukarFakturSendModel model = new TukarFakturSendModel();
            model.Status = "Draft";
            return model;
        }
        public List<TukarFakturSend_DetailModel> TukarFakturSend_Details(HANA_APP CONTEXT, long id = 0)
        {
            string ssql = "SELECT * FROM \"Tx_TukarFakturSend_Detail\" T0 WHERE \"Id\" = :p0 ORDER BY T0.\"DetId\" ";

            return CONTEXT.Database.SqlQuery<TukarFakturSend_DetailModel>(ssql, id).ToList();
        }

        public TukarFakturSendModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public TukarFakturSendModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            TukarFakturSendModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.* 
                            FROM ""Tx_TukarFakturSend"" T0   
                            WHERE T0.""Id""=:p0 ";

                TukarFakturSendModel tx_TukarFakturSend = CONTEXT.Database.SqlQuery<TukarFakturSendModel>(ssql, id).Single();
                if (tx_TukarFakturSend != null)
                {
                    model = new TukarFakturSendModel();
                    CopyProperty.CopyProperties(tx_TukarFakturSend, model, false);
                    model.ListDetails_ = this.TukarFakturSend_Details(CONTEXT, id);
                }
                else
                {
                    model = GetNewModel();
                }


            }

            return model;
        }

        public TukarFakturSendModel GetNewModel()
        {
            TukarFakturSendModel model = new TukarFakturSendModel();

            return model;
        }

        public TukarFakturSendModel NavFirst(int userId)
        {
            TukarFakturSendModel model = null;


            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TukarFakturSend");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TukarFakturSend\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public TukarFakturSendModel NavPrevious(int userId, long id = 0)
        {
            TukarFakturSendModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TukarFakturSend");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TukarFakturSend\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public TukarFakturSendModel NavNext(int userId, long id = 0)
        {
            TukarFakturSendModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TukarFakturSend");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TukarFakturSend\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public TukarFakturSendModel NavLast(int userId)
        {
            TukarFakturSendModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "TukarFakturSend");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_TukarFakturSend\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        public long Detail_Add(HANA_APP CONTEXT, TukarFakturSend_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_TukarFakturSend_Detail tx_TukarFakturSend_Detail = new Tx_TukarFakturSend_Detail();

                CopyProperty.CopyProperties(model, tx_TukarFakturSend_Detail, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_TukarFakturSend_Detail.Id = Id;
                tx_TukarFakturSend_Detail.CreatedDate = dtModified;
                tx_TukarFakturSend_Detail.CreatedUser = UserId;
                tx_TukarFakturSend_Detail.ModifiedDate = dtModified;
                tx_TukarFakturSend_Detail.ModifiedUser = UserId;

                CONTEXT.Tx_TukarFakturSend_Detail.Add(tx_TukarFakturSend_Detail);
                CONTEXT.SaveChanges();

                DetId = tx_TukarFakturSend_Detail.DetId;
                //DetId = Convert.ToInt32(tx_TukarFakturSend_Detail.Id);

            }

            return DetId;

        }

        public void Detail_Update(HANA_APP CONTEXT, TukarFakturSend_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_TukarFakturSend_Detail tx_TukarFakturSend_Detail = CONTEXT.Tx_TukarFakturSend_Detail.Find(model.Id);

                if (tx_TukarFakturSend_Detail != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    //var exceptColumns = new string[] { "Id", "SortCode", "Code", "Name" };
                    CopyProperty.CopyProperties(model, tx_TukarFakturSend_Detail, false, exceptColumns);

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    tx_TukarFakturSend_Detail.ModifiedDate = dtModified;
                    tx_TukarFakturSend_Detail.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }
            }

        }

        public void Detail_Delete(HANA_APP CONTEXT, TukarFakturSend_DetailModel model)
        {
            if (model.Id != null)
            {
                if (model.Id != 0)
                {
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_TukarFakturSend_Detail\"  WHERE \"Id\"=:p0", model.Id);

                    CONTEXT.SaveChanges();
                }
            }

        }
    }




    #endregion

}