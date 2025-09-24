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


namespace Models.Transaction.ReceiptOli
{
    #region Models

    public class ReceiptOliModel
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

        public DateTime? ReceiptOliTime { get; set; }

        public DateTime? FisikSJKembaliDate { get; set; }

        public DateTime? FisikSJKembaliTime { get; set; }

        public string Status { get; set; }

        public long? SuratJalanId { get; set; }

        public string ShippingType { get; set; }

        [Required(ErrorMessage = "required")]
        public string SuratJalanNo { get; set; }

        public int? UnitId { get; set; }

        public string UnitName { get; set; }

        public decimal? UnitCapacity { get; set; }

        public string UnitPoliceNo { get; set; }

        //[Required(ErrorMessage = "required")]
        public int? DriverId { get; set; }

        public string DriverName { get; set; }

        public decimal? QtyDelivery { get; set; }

        public DateTime? LoadingDate { get; set; }

        public DateTime? LoadingTime { get; set; }

        public DateTime? FinishDate { get; set; }

        public DateTime? FinishTime { get; set; } 

        public string T2DepanBelakang { get; set; }

        public decimal? Temperature { get; set; }

        public decimal? QtyReceiptOli { get; set; }

        public decimal? FlowMeterStart { get; set; }

        public decimal? FlowMeterEnd { get; set; }

        public String Remarks { get; set; }

        public DateTime? PASReceiptOliDate { get; set; }

        public string PASPic { get; set; }


    }


    #endregion

    #region Services

    public class ReceiptOliService
    {

        public long Add(ReceiptOliModel model)
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

                            Tx_ReceiptOli tx_ReceiptOli = new Tx_ReceiptOli();
                            CopyProperty.CopyProperties(model, tx_ReceiptOli, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_ReceiptOli.TransType = "ReceiptOli";
                            tx_ReceiptOli.CreatedDate = dtModified;
                            tx_ReceiptOli.CreatedUser = model._UserId;
                            tx_ReceiptOli.ModifiedDate = dtModified;
                            tx_ReceiptOli.ModifiedUser = model._UserId;


                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'ReceiptOli','" + dateX + "','') ").SingleOrDefault();
                            tx_ReceiptOli.TransNo = transNo;

                            CONTEXT.Tx_ReceiptOli.Add(tx_ReceiptOli);
                            CONTEXT.SaveChanges();
                            Id = tx_ReceiptOli.Id;

                            String keyValue;
                            keyValue = tx_ReceiptOli.Id.ToString();

                            SpNotif.SpSysControllerTransNotif(model._UserId, "ReceiptOli", CONTEXT, "after", "Tx_ReceiptOli", "add", "Id", keyValue);

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

        public void Update(ReceiptOliModel model)
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

                                SpNotif.SpSysControllerTransNotif(model._UserId, "ReceiptOli", CONTEXT, "before", "Tx_ReceiptOli", "update", "Id", keyValue);

                                Tx_ReceiptOli tx_ReceiptOli = CONTEXT.Tx_ReceiptOli.Find(model.Id);
                                if (tx_ReceiptOli != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransType", "Status", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_ReceiptOli, false, exceptColumns);

                                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                    tx_ReceiptOli.ModifiedDate = dtModified;
                                    tx_ReceiptOli.ModifiedUser = model._UserId;

                                    CONTEXT.SaveChanges();

                                    SpNotif.SpSysControllerTransNotif(model._UserId, "ReceiptOli", CONTEXT, "after", "Tx_ReceiptOli", "update", "Id", keyValue);
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

                        SpNotif.SpSysControllerTransNotif(userId, "ReceiptOli", CONTEXT, "before", "Tx_ReceiptOli", "post", "Id", keyValue);

                        Tx_ReceiptOli tx_ReceiptOli = CONTEXT.Tx_ReceiptOli.Find(Id);
                        if (tx_ReceiptOli != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_ReceiptOli.Status = "Posted";
                            tx_ReceiptOli.ModifiedDate = dtModified;
                            tx_ReceiptOli.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "ReceiptOli", CONTEXT, "after", "Tx_ReceiptOli", "post", "Id", keyValue);

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

                        SpNotif.SpSysControllerTransNotif(userId, "ReceiptOli", CONTEXT, "before", "Tx_ReceiptOli", "cancel", "Id", keyValue);

                        Tx_ReceiptOli tx_ReceiptOli = CONTEXT.Tx_ReceiptOli.Find(Id);
                        if (tx_ReceiptOli != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_ReceiptOli.Status = "Cancel";
                            tx_ReceiptOli.ModifiedDate = dtModified;
                            tx_ReceiptOli.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "ReceiptOli", CONTEXT, "after", "Tx_ReceiptOli", "cancel", "Id", keyValue);


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

         

        public ReceiptOliModel GetNewModel(int userId)
        {
            ReceiptOliModel model = new ReceiptOliModel();
            model.Status = "Draft";
            return model;
        }

        public ReceiptOliModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public ReceiptOliModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            ReceiptOliModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.* 
                            FROM ""Tx_ReceiptOli"" T0   
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<ReceiptOliModel>(ssql, id).Single();

            }

            return model;
        }

        public ReceiptOliModel NavFirst(int userId)
        {
            ReceiptOliModel model = null;


            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ReceiptOli");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ReceiptOli\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public ReceiptOliModel NavPrevious(int userId, long id = 0)
        {
            ReceiptOliModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ReceiptOli");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ReceiptOli\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public ReceiptOliModel NavNext(int userId, long id = 0)
        {
            ReceiptOliModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ReceiptOli");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ReceiptOli\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public ReceiptOliModel NavLast(int userId)
        {
            ReceiptOliModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "ReceiptOli");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_ReceiptOli\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
    }


    #endregion

}