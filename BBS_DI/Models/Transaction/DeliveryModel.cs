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


namespace Models.Transaction.Delivery
{

    #region Models

    public class DeliveryModel
    {
        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long Id { get; set; }

        public string TransNo { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime? TransDate { get; set; } 

        public string Status { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShippingType { get; set; }

        public long? SuratJalanId { get; set; }

        [Required(ErrorMessage = "required")]
        public string SuratJalanNo { get; set; }

        public String Remarks { get; set; }

        public List<Delivery_ActivityModel> ListActivitys_ = new List<Delivery_ActivityModel>();
    }

    public class Delivery_Activitys
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Delivery_ActivityModel> insertedRowValues { get; set; }
        public List<Delivery_ActivityModel> modifiedRowValues { get; set; }
    }


    public class Delivery_ActivityModel
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

        [Required(ErrorMessage = "required")]
        public Int32? ActivityId { get; set; }

        public string ActivityName { get; set; }

        public DateTime? ActivityDate { get; set; }

        public DateTime? ActivityTime { get; set; }

        public string Description { get; set; }
    }


    #endregion

    #region Services

    public class DeliveryService
    {

        public long Add(DeliveryModel model)
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

                            Tx_Delivery tx_Delivery = new Tx_Delivery();
                            CopyProperty.CopyProperties(model, tx_Delivery, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_Delivery.TransType = "Delivery";
                            tx_Delivery.CreatedDate = dtModified;
                            tx_Delivery.CreatedUser = model._UserId;
                            tx_Delivery.ModifiedDate = dtModified;
                            tx_Delivery.ModifiedUser = model._UserId;

                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'Delivery','" + model.TransDate.Value.ToString("yyyy-MM-dd") + "') ").SingleOrDefault();
                            tx_Delivery.TransNo = transNo;

                            CONTEXT.Tx_Delivery.Add(tx_Delivery);
                            CONTEXT.SaveChanges();
                            Id = tx_Delivery.Id;

                            String keyValue;
                            keyValue = tx_Delivery.Id.ToString();

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_Delivery", "add", "Id", keyValue);

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

        public void Update(DeliveryModel model)
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

                                SpNotif.SpSysTransNotif(model._UserId,CONTEXT, "before", "Tx_Delivery", "update", "Id", keyValue);


                                Tx_Delivery tx_Delivery = CONTEXT.Tx_Delivery.Find(model.Id);
                                if (tx_Delivery != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransType", "Status" };
                                    CopyProperty.CopyProperties(model, tx_Delivery, false, exceptColumns);

                                    CONTEXT.SaveChanges();

                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_Delivery", "update", "Id", keyValue);

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

                        SpNotif.SpSysTransNotif(userId, CONTEXT, "before", "Tx_Delivery", "post", "Id", keyValue);

                        Tx_Delivery tx_Delivery = CONTEXT.Tx_Delivery.Find(Id);
                        if (tx_Delivery != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_Delivery.Status = "Posted";
                            tx_Delivery.ModifiedDate = dtModified;
                            tx_Delivery.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }
                        SpNotif.SpSysTransNotif(userId, CONTEXT, "after", "Tx_Delivery", "post", "Id", keyValue);



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

                        SpNotif.SpSysTransNotif(userId, CONTEXT, "before", "Tx_Delivery", "cancel", "Id", keyValue);

                        Tx_Delivery tx_Delivery = CONTEXT.Tx_Delivery.Find(Id);
                        if (tx_Delivery != null)
                        {
                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            tx_Delivery.Status = "Cancel";
                            tx_Delivery.ModifiedDate = dtModified;
                            tx_Delivery.ModifiedUser = userId;

                            CONTEXT.SaveChanges();
                        }
                        SpNotif.SpSysTransNotif(userId, CONTEXT, "after", "Tx_Delivery", "cancel", "Id", keyValue);



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

         

        public DeliveryModel GetNewModel(int userId)
        {
            DeliveryModel model = new DeliveryModel();
            model.Status = "Draft";
            return model;
        }

        public DeliveryModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public DeliveryModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            DeliveryModel model = null;
            if (id != 0)
            {
//                string ssql = @"SELECT T0.*,T1.""WhsName"" AS ""Branch_"" 
//                            FROM ""Tx_Delivery"" T0 
//                            LEFT JOIN ""{0}"".""OWHS"" T1 ON T0.""CreatedBranch""=T1.""WhsCode""  
//                            WHERE T0.""Id""=:p0 ";

                string ssql = "CALL \"SpDelivery_GetId\"(:p0,:p1) "; 

                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                model = CONTEXT.Database.SqlQuery<DeliveryModel>(ssql, userId, id).Single();

                model.ListActivitys_ = this.GetDelivery_Activitys(CONTEXT, id);
            }

            return model;
        }

        public DeliveryModel NavFirst(int userId)
        {
            DeliveryModel model = null;


            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Delivery");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Delivery\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }

        public DeliveryModel NavPrevious(int userId, long id = 0)
        {
            DeliveryModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Delivery");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Delivery\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public DeliveryModel NavNext(int userId, long id = 0)
        {
            DeliveryModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Delivery");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Delivery\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public DeliveryModel NavLast(int userId)
        {
            DeliveryModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Delivery");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Delivery\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }



        //-------------------------------------
        //Detail  Delivery_ActivityModel
        //-------------------------------------
        public Delivery_ActivityModel Activity_GetById(int userId, long detId = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Activity_GetById(CONTEXT, userId, detId);
            }
        }
        public Delivery_ActivityModel Activity_GetById(HANA_APP CONTEXT, int userId, long detId = 0)
        {
            Delivery_ActivityModel model = null;
            if (detId != 0)
            {
                string ssql = @"SELECT T0.*
                            FROM ""Tx_Delivery_Activity"" T0
                            WHERE T0.""DetId""=:p0 ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                model = CONTEXT.Database.SqlQuery<Delivery_ActivityModel>(ssql, detId).Single();
            }
            return model;
        }

        public Delivery_ActivityModel Activity_GetNewModel(int userId, long id)
        {
            Delivery_ActivityModel model = new Delivery_ActivityModel();
            model.Id = id;
            return model;
        }

        public List<Delivery_ActivityModel> GetDelivery_Activitys(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDelivery_Activitys(CONTEXT, id);
            }
        }

        public List<Delivery_ActivityModel> GetDelivery_Activitys(HANA_APP CONTEXT, long id = 0)
        {
            return CONTEXT.Database.SqlQuery<Delivery_ActivityModel>("SELECT T0.* FROM \"Tx_Delivery_Activity\" T0 WHERE T0.\"Id\"=:p0  ", id).ToList();
        }

        public long Detail_Add(Delivery_ActivityModel model, long Id, int UserId)
        {
            long detId = 0;
            using (var CONTEXT = new HANA_APP())
            {
                using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                {
                    try
                    {
                        detId = Detail_Add(CONTEXT, model, Id, UserId);
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
                return detId;
            }
        }

        public long Detail_Add(HANA_APP CONTEXT, Delivery_ActivityModel model, long Id, int UserId)
        {
            long detId = 0;

            if (model != null)
            {
                Tx_Delivery_Activity tx_Delivery_Activity = new Tx_Delivery_Activity();

                CopyProperty.CopyProperties(model, tx_Delivery_Activity, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_Delivery_Activity.Id = Id;
                tx_Delivery_Activity.CreatedDate = dtModified;
                tx_Delivery_Activity.CreatedUser = UserId;
                tx_Delivery_Activity.ModifiedDate = dtModified;
                tx_Delivery_Activity.ModifiedUser = UserId;

                String keyValue;
                CONTEXT.Tx_Delivery_Activity.Add(tx_Delivery_Activity);
                CONTEXT.SaveChanges();
                keyValue = tx_Delivery_Activity.DetId.ToString();
                detId = tx_Delivery_Activity.DetId;

                SpNotif.SpSysTransNotif(UserId, CONTEXT, "after", "Tx_Delivery_Activity", "add", "DetId", keyValue);
            }

            return detId;

        }

        public void Detail_Update(Delivery_ActivityModel model, int UserId)
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

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "before", "Tx_Delivery_Activity", "update", "DetId", keyValue);


                            Tx_Delivery_Activity tx_Delivery_Activity = CONTEXT.Tx_Delivery_Activity.Find(model.DetId);
                            if (tx_Delivery_Activity != null)
                            {
                                var exceptColumns = new string[] { "Id", "DetId", };
                                CopyProperty.CopyProperties(model, tx_Delivery_Activity, false, exceptColumns);
                                CONTEXT.SaveChanges();

                                SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_Delivery", "update", "Id", keyValue);

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

        public void Detail_Delete(int detId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                Detail_Delete(CONTEXT, detId);
            }
        }

        public void Detail_Delete(HANA_APP CONTEXT, int detId)
        {
            if (detId != null)
            {
                if (detId != 0)
                {
                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_Delivery_Activity\"  WHERE \"DetId\"=:p0", detId);
                    CONTEXT.SaveChanges();
                }
            }
        }


    }


    #endregion

}