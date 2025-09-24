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


namespace Models.Transaction.Receipt
{
    #region Models

    public class ReceiptModel
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

        public DateTime? ReceiptTime { get; set; }

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

        public decimal? QtyReceipt { get; set; }

        public decimal? FlowMeterStart { get; set; }

        public decimal? FlowMeterEnd { get; set; }

        public String Remarks { get; set; }

        public DateTime? PASReceiptDate { get; set; }

        public string PASPic { get; set; }


    }


    #endregion

    #region Services

    public class ReceiptService
    {

        public long Add(ReceiptModel model)
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

                            Tx_Receipt tx_Receipt = new Tx_Receipt();
                            CopyProperty.CopyProperties(model, tx_Receipt, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_Receipt.TransType = "Receipt";
                            tx_Receipt.CreatedDate = dtModified;
                            tx_Receipt.CreatedUser = model._UserId;
                            tx_Receipt.ModifiedDate = dtModified;
                            tx_Receipt.ModifiedUser = model._UserId;


                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'Receipt','" + dateX + "','') ").SingleOrDefault();
                            tx_Receipt.TransNo = transNo;

                            CONTEXT.Tx_Receipt.Add(tx_Receipt);
                            CONTEXT.SaveChanges();
                            Id = tx_Receipt.Id;

                            String keyValue;
                            keyValue = tx_Receipt.Id.ToString();

                            SpNotif.SpSysControllerTransNotif(model._UserId, "Receipt", CONTEXT, "after", "Tx_Receipt", "add", "Id", keyValue);

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

        public void Update(ReceiptModel model)
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

                                SpNotif.SpSysControllerTransNotif(model._UserId, "Receipt", CONTEXT, "before", "Tx_Receipt", "update", "Id", keyValue);

                                Tx_Receipt tx_Receipt = CONTEXT.Tx_Receipt.Find(model.Id);
                                if (tx_Receipt != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransType", "Status", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, tx_Receipt, false, exceptColumns);

                                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                    tx_Receipt.ModifiedDate = dtModified;
                                    tx_Receipt.ModifiedUser = model._UserId;

                                    CONTEXT.SaveChanges();

                                    SpNotif.SpSysControllerTransNotif(model._UserId, "Receipt", CONTEXT, "after", "Tx_Receipt", "update", "Id", keyValue);
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

                        SpNotif.SpSysControllerTransNotif(userId, "Receipt", CONTEXT, "before", "Tx_Receipt", "post", "Id", keyValue);

                        Tx_Receipt tx_Receipt = CONTEXT.Tx_Receipt.Find(Id);
                        if (tx_Receipt != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_Receipt.Status = "Posted";
                            tx_Receipt.ModifiedDate = dtModified;
                            tx_Receipt.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "Receipt", CONTEXT, "after", "Tx_Receipt", "post", "Id", keyValue);

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

                        SpNotif.SpSysControllerTransNotif(userId, "Receipt", CONTEXT, "before", "Tx_Receipt", "cancel", "Id", keyValue);

                        Tx_Receipt tx_Receipt = CONTEXT.Tx_Receipt.Find(Id);
                        if (tx_Receipt != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_Receipt.Status = "Cancel";
                            tx_Receipt.ModifiedDate = dtModified;
                            tx_Receipt.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }

                        SpNotif.SpSysControllerTransNotif(userId, "Receipt", CONTEXT, "after", "Tx_Receipt", "cancel", "Id", keyValue);


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

         

        public ReceiptModel GetNewModel(int userId)
        {
            ReceiptModel model = new ReceiptModel();
            model.Status = "Draft";
            return model;
        }

        public ReceiptModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public ReceiptModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            ReceiptModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.* 
                            FROM ""Tx_Receipt"" T0   
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<ReceiptModel>(ssql, id).Single();

            }

            return model;
        }

        public ReceiptModel NavFirst(int userId)
        {
            ReceiptModel model = null;


            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Receipt");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Receipt\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public ReceiptModel NavPrevious(int userId, long id = 0)
        {
            ReceiptModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Receipt");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Receipt\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public ReceiptModel NavNext(int userId, long id = 0)
        {
            ReceiptModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Receipt");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Receipt\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public ReceiptModel NavLast(int userId)
        {
            ReceiptModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Receipt");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Receipt\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
    }


    #endregion

}