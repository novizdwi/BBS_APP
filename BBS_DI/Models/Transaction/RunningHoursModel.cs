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



namespace Models.Transaction.RunningHours
{
    #region Models

    public class RunningHoursModel
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

        [Required(ErrorMessage = "required")]
        public int? ShipId { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipName { get; set; }

        public string PIC { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipCode { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }

        public List<RunningHours_DailyModel> ListDailys_ = new List<RunningHours_DailyModel>();

        public RunningHours_Daily Dailys_ { get; set;}

        public List<RunningHours_TotalModel> ListTotals_ = new List<RunningHours_TotalModel>();

        public RunningHours_Total Totals_ { get; set; }

        public List<RunningHours_RunPumpModel> ListRunPumps_ = new List<RunningHours_RunPumpModel>();

        public RunningHours_RunPump RunPumps_ { get; set; }
    }
    public class RunningHours_Daily    {
        public List<long> deletedRowKeys { get; set; }
        public List<RunningHours_DailyModel> insertedRowValues { get; set; }
        public List<RunningHours_DailyModel> modifiedRowValues { get; set; }
    }
    public class RunningHours_DailyModel
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
        
        public decimal? MEPS { get; set; }

        public decimal? MESB { get; set; }

        public decimal? AE1 { get; set; }

        public decimal? AE2 { get; set; }

        public decimal? GBME { get; set; }

        public decimal? GBAE { get; set; }

        public string Remark { get; set; }
    }
    public class RunningHours_Total
    {
        public List<long> deletedRowKeys { get; set; }
        public List<RunningHours_TotalModel> insertedRowValues { get; set; }
        public List<RunningHours_TotalModel> modifiedRowValues { get; set; }
    }
    public class RunningHours_TotalModel
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

        public string Engine { get; set; }

        public decimal? RunningHours { get; set; }

        public decimal? LubOil { get; set; }

        public decimal? LoFilter { get; set; }

        public decimal? LoRacor { get; set; }

        public decimal? FoFilter { get; set; }

        public string Remark { get; set; }
    }
    public class RunningHours_RunPump
    {
        public List<long> deletedRowKeys { get; set; }
        public List<RunningHours_RunPumpModel> insertedRowValues { get; set; }
        public List<RunningHours_RunPumpModel> modifiedRowValues { get; set; }
    }
    public class RunningHours_RunPumpModel
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

        public string Name { get; set; }

        public decimal? Hours { get; set; }

        public string Remark { get; set; }
    }
    #endregion

    #region Services

    public class RunningHoursService
    {

        public RunningHoursModel GetNewModel(int userId)
        {
            RunningHoursModel model = new RunningHoursModel();
            model.Status = "Open";
            return model;
        }
        public RunningHoursModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public RunningHoursModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            RunningHoursModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, T1.""FirstName"" AS ""UserName"" 
                            FROM ""Tx_RunningHours"" T0
                            LEFT JOIN ""Tm_User"" T1 ON T0.""ModifiedUser"" = T1.""Id""
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<RunningHoursModel>(ssql, id).Single();

                model.ListDailys_ = this.RunningHours_Dailys(CONTEXT, id);
                model.ListTotals_ = this.RunningHours_Totals(CONTEXT, id);
                model.ListRunPumps_ = this.RunningHours_RunPumps(CONTEXT, id);
            }

            return model;
        }
        public List<RunningHours_DailyModel> RunningHours_Dailys(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return RunningHours_Dailys(CONTEXT, id);
            }

        }

        public List<RunningHours_DailyModel> RunningHours_Dailys(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<RunningHours_DailyModel>("SELECT * FROM \"Tx_RunningHours_Daily\" WHERE \"Id\" =:p0", id).ToList();
        }
        public List<RunningHours_TotalModel> RunningHours_Totals(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<RunningHours_TotalModel>("SELECT * FROM \"Tx_RunningHours_Total\" WHERE \"Id\" =:p0", id).ToList();
        }
        public List<RunningHours_RunPumpModel> RunningHours_RunPumps(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<RunningHours_RunPumpModel>("SELECT * FROM \"Tx_RunningHours_RunPump\" WHERE \"Id\" =:p0", id).ToList();
        }
        public RunningHoursModel NavFirst(int userId)
        {
            RunningHoursModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_RunningHours\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public RunningHoursModel NavPrevious(int userId, long id = 0)
        {
            RunningHoursModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_RunningHours\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public RunningHoursModel NavNext(int userId, long id = 0)
        {
            RunningHoursModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_RunningHours\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public RunningHoursModel NavLast(int userId)
        {
            RunningHoursModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_RunningHours\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        public long Add(RunningHoursModel model)
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

                            Tx_RunningHours Tx_RunningHours = new Tx_RunningHours();
                            CopyProperty.CopyProperties(model, Tx_RunningHours, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_RunningHours.TransType = "RunningHours";
                            Tx_RunningHours.CreatedDate = dtModified;
                            Tx_RunningHours.CreatedUser = model._UserId;
                            Tx_RunningHours.ModifiedDate = dtModified;
                            Tx_RunningHours.ModifiedUser = model._UserId;
                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'RunningHours','" + dateX + "','') ").SingleOrDefault();
                            Tx_RunningHours.TransNo = transNo;
                            CONTEXT.Tx_RunningHours.Add(Tx_RunningHours);
                            CONTEXT.SaveChanges();
                            Id = Tx_RunningHours.Id;

                            String keyValue;
                            keyValue = Tx_RunningHours.Id.ToString();
                            String ssql = "SELECT A1.\"DetName\" AS \"Name\" FROM \"Tm_Ship\" A LEFT JOIN \"Tm_Ship_Engine\" A1 ON A.\"Id\" = A1.\"Id\" WHERE A.\"ShipCode\" = :p0";
                            var engine = CONTEXT.Database.SqlQuery<RunningHours_RunPumpModel>(ssql,Tx_RunningHours.ShipCode).ToList();
                            foreach(var runPumps in engine)
                            {
                                RunPump_Add(CONTEXT, runPumps, Id, model._UserId);
                            }

                            String ssql2 = "SELECT A1.\"DetName\" AS \"Engine\" FROM \"Tm_Ship\" A LEFT JOIN \"Tm_Ship_Engine\" A1 ON A.\"Id\" = A1.\"Id\" WHERE A.\"ShipCode\" = :p0";
                            var engine2 = CONTEXT.Database.SqlQuery<RunningHours_TotalModel>(ssql2, Tx_RunningHours.ShipCode).ToList();
                            foreach (var totals in engine2)
                            {
                                Total_Add(CONTEXT, totals, Id, model._UserId);
                            }
                            if (model.Dailys_ != null)
                            {
                                if (model.Dailys_.insertedRowValues != null)
                                {
                                    foreach (var daily in model.Dailys_.insertedRowValues)
                                    {
                                        Daily_Add(CONTEXT, daily, Id, model._UserId);
                                    }
                                }

                                if (model.Dailys_.modifiedRowValues != null)
                                {
                                    foreach (var daily in model.Dailys_.modifiedRowValues)
                                    {
                                        Daily_Update(CONTEXT, daily, model._UserId);
                                    }
                                }

                                if (model.Dailys_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.Dailys_.deletedRowKeys)
                                    {
                                        RunningHours_DailyModel dailyModel = new RunningHours_DailyModel();
                                        dailyModel.DetId = detId;
                                        Daily_Delete(CONTEXT, dailyModel);
                                    }
                                }
                            }

                            if (model.Totals_ != null)
                            {
                                if (model.Totals_.insertedRowValues != null)
                                {
                                    foreach (var total in model.Totals_.insertedRowValues)
                                    {
                                        Total_Add(CONTEXT, total, Id, model._UserId);
                                    }
                                }

                                if (model.Totals_.modifiedRowValues != null)
                                {
                                    foreach (var total in model.Totals_.modifiedRowValues)
                                    {
                                        Total_Update(CONTEXT, total, model._UserId);
                                    }
                                }

                                if (model.Totals_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.Totals_.deletedRowKeys)
                                    {
                                        RunningHours_TotalModel totalModel = new RunningHours_TotalModel();
                                        totalModel.DetId = detId;
                                        Total_Delete(CONTEXT, totalModel);
                                    }
                                }
                            }

                            if (model.RunPumps_ != null)
                            {
                                if (model.RunPumps_.insertedRowValues != null)
                                {
                                    foreach (var runPump in model.RunPumps_.insertedRowValues)
                                    {
                                        RunPump_Add(CONTEXT, runPump, Id, model._UserId);
                                    }
                                }

                                if (model.RunPumps_.modifiedRowValues != null)
                                {
                                    foreach (var runPump in model.RunPumps_.modifiedRowValues)
                                    {
                                        RunPump_Update(CONTEXT, runPump, model._UserId);
                                    }
                                }

                                if (model.RunPumps_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.RunPumps_.deletedRowKeys)
                                    {
                                        RunningHours_RunPumpModel runPumpModel = new RunningHours_RunPumpModel();
                                        runPumpModel.DetId = detId;
                                        RunPump_Delete(CONTEXT, runPumpModel);
                                    }
                                }
                            }



                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_RunningHours", "add", "Id", keyValue);


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
        public void Update(RunningHoursModel model)
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


                                Tx_RunningHours Tx_RunningHours = CONTEXT.Tx_RunningHours.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_RunningHours.ModifiedDate = dtModified;
                                Tx_RunningHours.ModifiedUser = model._UserId;
                                if (Tx_RunningHours != null)
                                {
                                    var exceptColumns = new string[] { "Id","TransNo","CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_RunningHours, false, exceptColumns);
                                    Tx_RunningHours.ModifiedDate = dtModified;
                                    Tx_RunningHours.ModifiedUser = model._UserId;
                                    CONTEXT.SaveChanges();

                                    if (model.Dailys_ != null)
                                    {
                                        if (model.Dailys_.insertedRowValues != null)
                                        {
                                            foreach (var daily in model.Dailys_.insertedRowValues)
                                            {
                                                Daily_Add(CONTEXT, daily, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.Dailys_.modifiedRowValues != null)
                                        {
                                            foreach (var daily in model.Dailys_.modifiedRowValues)
                                            {
                                                Daily_Update(CONTEXT, daily, model._UserId);
                                            }
                                        }

                                        if (model.Dailys_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.Dailys_.deletedRowKeys)
                                            {
                                                RunningHours_DailyModel dailyModel = new RunningHours_DailyModel();
                                                dailyModel.DetId = detId;
                                               Daily_Delete(CONTEXT, dailyModel);
                                            }
                                        }
                                    }
                                    if (model.Totals_ != null)
                                    {
                                        if (model.Totals_.insertedRowValues != null)
                                        {
                                            foreach (var total in model.Totals_.insertedRowValues)
                                            {
                                                Total_Add(CONTEXT, total, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.Totals_.modifiedRowValues != null)
                                        {
                                            foreach (var total in model.Totals_.modifiedRowValues)
                                            {
                                                Total_Update(CONTEXT, total, model._UserId);
                                            }
                                        }

                                        if (model.Totals_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.Totals_.deletedRowKeys)
                                            {
                                                RunningHours_TotalModel totalModel = new RunningHours_TotalModel();
                                                totalModel.DetId = detId;
                                                Total_Delete(CONTEXT, totalModel);
                                            }
                                        }
                                    }

                                    if (model.RunPumps_ != null)
                                    {
                                        if (model.RunPumps_.insertedRowValues != null)
                                        {
                                            foreach (var runPump in model.RunPumps_.insertedRowValues)
                                            {
                                                RunPump_Add(CONTEXT, runPump, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.RunPumps_.modifiedRowValues != null)
                                        {
                                            foreach (var runPump in model.RunPumps_.modifiedRowValues)
                                            {
                                                RunPump_Update(CONTEXT, runPump, model._UserId);
                                            }
                                        }

                                        if (model.RunPumps_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.RunPumps_.deletedRowKeys)
                                            {
                                                RunningHours_RunPumpModel runPumpModel = new RunningHours_RunPumpModel();
                                                runPumpModel.DetId = detId;
                                                RunPump_Delete(CONTEXT, runPumpModel);
                                            }
                                        }
                                    }

                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", " Tx_RunningHours", "update", "Id", keyValue);

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
        public long Daily_Add(HANA_APP CONTEXT, RunningHours_DailyModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_RunningHours_Daily Tx_RunningHours_Daily = new Tx_RunningHours_Daily();

                CopyProperty.CopyProperties(model, Tx_RunningHours_Daily, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_RunningHours_Daily.Id = Id;
                Tx_RunningHours_Daily.CreatedDate = dtModified;
                Tx_RunningHours_Daily.CreatedUser = UserId;
                Tx_RunningHours_Daily.ModifiedDate = dtModified;
                Tx_RunningHours_Daily.ModifiedUser = UserId;

                CONTEXT.Tx_RunningHours_Daily.Add(Tx_RunningHours_Daily);
                CONTEXT.SaveChanges();
                DetId = Tx_RunningHours_Daily.DetId;

            }

            return DetId;

        }
        public void Daily_Update(HANA_APP CONTEXT, RunningHours_DailyModel model, int UserId)
        {
            if (model != null)
            {

                Tx_RunningHours_Daily Tx_RunningHours_Daily = CONTEXT.Tx_RunningHours_Daily.Find(model.DetId);

                if (Tx_RunningHours_Daily != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_RunningHours_Daily, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_RunningHours_Daily.ModifiedDate = dtModified;
                    Tx_RunningHours_Daily.ModifiedUser = UserId;
                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Daily_Delete(HANA_APP CONTEXT, RunningHours_DailyModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_RunningHours_Daily\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }

        public long Total_Add(HANA_APP CONTEXT, RunningHours_TotalModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_RunningHours_Total Tx_RunningHours_Total = new Tx_RunningHours_Total();

                CopyProperty.CopyProperties(model, Tx_RunningHours_Total, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_RunningHours_Total.Id = Id;
                Tx_RunningHours_Total.CreatedDate = dtModified;
                Tx_RunningHours_Total.CreatedUser = UserId;
                Tx_RunningHours_Total.ModifiedDate = dtModified;
                Tx_RunningHours_Total.ModifiedUser = UserId;

                CONTEXT.Tx_RunningHours_Total.Add(Tx_RunningHours_Total);
                CONTEXT.SaveChanges();
                DetId = Tx_RunningHours_Total.DetId;

            }

            return DetId;

        }
        public void Total_Update(HANA_APP CONTEXT, RunningHours_TotalModel model, int UserId)
        {
            if (model != null)
            {

                Tx_RunningHours_Total Tx_RunningHours_Total = CONTEXT.Tx_RunningHours_Total.Find(model.DetId);

                if (Tx_RunningHours_Total != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_RunningHours_Total, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_RunningHours_Total.ModifiedDate = dtModified;
                    Tx_RunningHours_Total.ModifiedUser = UserId;
                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Total_Delete(HANA_APP CONTEXT, RunningHours_TotalModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_RunningHours_Total\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }

        public long RunPump_Add(HANA_APP CONTEXT, RunningHours_RunPumpModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_RunningHours_RunPump Tx_RunningHours_RunPump = new Tx_RunningHours_RunPump();

                CopyProperty.CopyProperties(model, Tx_RunningHours_RunPump, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_RunningHours_RunPump.Id = Id;
                Tx_RunningHours_RunPump.CreatedDate = dtModified;
                Tx_RunningHours_RunPump.CreatedUser = UserId;
                Tx_RunningHours_RunPump.ModifiedDate = dtModified;
                Tx_RunningHours_RunPump.ModifiedUser = UserId;

                CONTEXT.Tx_RunningHours_RunPump.Add(Tx_RunningHours_RunPump);
                CONTEXT.SaveChanges();
                DetId = Tx_RunningHours_RunPump.DetId;

            }

            return DetId;

        }
        public void RunPump_Update(HANA_APP CONTEXT, RunningHours_RunPumpModel model, int UserId)
        {
            if (model != null)
            {

                Tx_RunningHours_RunPump Tx_RunningHours_RunPump = CONTEXT.Tx_RunningHours_RunPump.Find(model.DetId);

                if (Tx_RunningHours_RunPump != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_RunningHours_RunPump, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_RunningHours_RunPump.ModifiedDate = dtModified;
                    Tx_RunningHours_RunPump.ModifiedUser = UserId;
                    CONTEXT.SaveChanges();

                }


            }

        }
        public void RunPump_Delete(HANA_APP CONTEXT, RunningHours_RunPumpModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_RunningHours_RunPump\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }

    }


    #endregion

}