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



namespace Models.Transaction.Overhaul
{
    #region Models

    public class OverhaulModel
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

        [Required(ErrorMessage = "required")]
        public string ShipCode { get; set; }

        public string MEKiri { get; set; }

        public string MEKanan { get; set; }

        public string AEKiri { get; set; }

        public string AEKanan { get; set; }

        public string GBKiri { get; set; }

        public string GBKanan { get; set; }

        public string PICOVH { get; set; }

        public decimal? EstTime { get; set; }

        public decimal? EstBudget { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Status { get; set; }

        public string Status2 { get; set; }

        public string Remark { get; set; }


        public List<Overhaul_MEKiriModel> ListMEKiris_ = new List<Overhaul_MEKiriModel>();

        public Overhaul_MEKiri MEKiris_ { get; set;}

        public List<Overhaul_MEKananModel> ListMEKanans_ = new List<Overhaul_MEKananModel>();

        public Overhaul_MEKanan MEKanans_ { get; set; }

        public List<Overhaul_AEKiriModel> ListAEKiris_ = new List<Overhaul_AEKiriModel>();

        public Overhaul_AEKiri AEKiris_ { get; set; }

        public List<Overhaul_AEKananModel> ListAEKanans_ = new List<Overhaul_AEKananModel>();

        public Overhaul_AEKanan AEKanans_ { get; set; }

        public List<Overhaul_GBKiriModel> ListGBKiris_ = new List<Overhaul_GBKiriModel>();

        public Overhaul_GBKiri GBKiris_ { get; set; }

        public List<Overhaul_GBKananModel> ListGBKanans_ = new List<Overhaul_GBKananModel>();

        public Overhaul_GBKanan GBKanans_ { get; set; }
    }
    public class Overhaul_MEKiri    {
        public List<long> deletedRowKeys { get; set; }
        public List<Overhaul_MEKiriModel> insertedRowValues { get; set; }
        public List<Overhaul_MEKiriModel> modifiedRowValues { get; set; }
    }
    public class Overhaul_MEKiriModel
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
        
        public int? RowNum { get; set; }

        public string Item { get; set; }

        public string SubItem { get; set; }

        public string PartNumber { get; set; }

        [Required(ErrorMessage = "required")]
        public decimal? QtyOrder { get; set; }

        public string Unit { get; set; }
        
        public string Cyl1 { get; set; }
        
        public string Cyl2 { get; set; }

        public string Cyl3 { get; set; }

        public string Cyl4 { get; set; }

        public string Cyl5 { get; set; }

        public string Cyl6 { get; set; }

        public string Cyl7 { get; set; }

        public string Cyl8 { get; set; }

        public string Cyl9 { get; set; }

        public string Cyl10 { get; set; }

        public string Cyl11 { get; set; }

        public string Cyl12 { get; set; }

        public string Cyl13 { get; set; }

        public string Cyl14 { get; set; }

        public string Cyl15 { get; set; }

        public string Cyl16 { get; set; }

        public decimal? TotalRepair { get; set; }

        public decimal? TotalReplace { get; set; }

        public decimal? QtySisa { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }
    }
    public class Overhaul_MEKanan
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Overhaul_MEKananModel> insertedRowValues { get; set; }
        public List<Overhaul_MEKananModel> modifiedRowValues { get; set; }
    }
    public class Overhaul_MEKananModel
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

        public int? RowNum { get; set; }

        public string Item { get; set; }

        public string SubItem { get; set; }

        public string PartNumber { get; set; }

        [Required(ErrorMessage = "required")]
        public decimal? QtyOrder { get; set; }

        public string Unit { get; set; }

        public string Cyl1 { get; set; }

        public string Cyl2 { get; set; }

        public string Cyl3 { get; set; }

        public string Cyl4 { get; set; }

        public string Cyl5 { get; set; }

        public string Cyl6 { get; set; }

        public string Cyl7 { get; set; }

        public string Cyl8 { get; set; }

        public string Cyl9 { get; set; }

        public string Cyl10 { get; set; }

        public string Cyl11 { get; set; }

        public string Cyl12 { get; set; }

        public string Cyl13 { get; set; }

        public string Cyl14 { get; set; }

        public string Cyl15 { get; set; }

        public string Cyl16 { get; set; }

        public decimal? TotalRepair { get; set; }

        public decimal? TotalReplace { get; set; }

        public decimal? QtySisa { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }
    }
    public class Overhaul_AEKiri
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Overhaul_AEKiriModel> insertedRowValues { get; set; }
        public List<Overhaul_AEKiriModel> modifiedRowValues { get; set; }
    }
    public class Overhaul_AEKiriModel
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

        public int? RowNum { get; set; }

        public string Item { get; set; }

        public string SubItem { get; set; }

        public string PartNumber { get; set; }

        [Required(ErrorMessage = "required")]
        public decimal? QtyOrder { get; set; }

        public string Unit { get; set; }

        public string Cyl1 { get; set; }

        public string Cyl2 { get; set; }

        public string Cyl3 { get; set; }

        public string Cyl4 { get; set; }

        public string Cyl5 { get; set; }

        public string Cyl6 { get; set; }

        public string Cyl7 { get; set; }

        public string Cyl8 { get; set; }

        public string Cyl9 { get; set; }

        public string Cyl10 { get; set; }

        public string Cyl11 { get; set; }

        public string Cyl12 { get; set; }

        public string Cyl13 { get; set; }

        public string Cyl14 { get; set; }

        public string Cyl15 { get; set; }

        public string Cyl16 { get; set; }

        public decimal? TotalRepair { get; set; }

        public decimal? TotalReplace { get; set; }

        public decimal? QtySisa { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }
    }
    public class Overhaul_AEKanan
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Overhaul_AEKananModel> insertedRowValues { get; set; }
        public List<Overhaul_AEKananModel> modifiedRowValues { get; set; }
    }
    public class Overhaul_AEKananModel
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

        public int? RowNum { get; set; }

        public string Item { get; set; }

        public string SubItem { get; set; }

        public string PartNumber { get; set; }

        [Required(ErrorMessage = "required")]
        public decimal? QtyOrder { get; set; }

        public string Unit { get; set; }

        public string Cyl1 { get; set; }

        public string Cyl2 { get; set; }

        public string Cyl3 { get; set; }

        public string Cyl4 { get; set; }

        public string Cyl5 { get; set; }

        public string Cyl6 { get; set; }

        public string Cyl7 { get; set; }

        public string Cyl8 { get; set; }

        public string Cyl9 { get; set; }

        public string Cyl10 { get; set; }

        public string Cyl11 { get; set; }

        public string Cyl12 { get; set; }

        public string Cyl13 { get; set; }

        public string Cyl14 { get; set; }

        public string Cyl15 { get; set; }

        public string Cyl16 { get; set; }

        public decimal? TotalRepair { get; set; }

        public decimal? TotalReplace { get; set; }

        public decimal? QtySisa { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }
    }
    public class Overhaul_GBKiri
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Overhaul_GBKiriModel> insertedRowValues { get; set; }
        public List<Overhaul_GBKiriModel> modifiedRowValues { get; set; }
    }
    public class Overhaul_GBKiriModel
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

        public int? RowNum { get; set; }

        public string Item { get; set; }

        public string SubItem { get; set; }

        public string PartNumber { get; set; }

        public decimal? QtyOrder { get; set; }

        public string Unit { get; set; }

        public string Cyl1 { get; set; }

        public string Cyl2 { get; set; }

        public string Cyl3 { get; set; }

        public string Cyl4 { get; set; }

        public string Cyl5 { get; set; }

        public string Cyl6 { get; set; }

        public string Cyl7 { get; set; }

        public string Cyl8 { get; set; }

        public string Cyl9 { get; set; }

        public string Cyl10 { get; set; }

        public string Cyl11 { get; set; }

        public string Cyl12 { get; set; }

        public string Cyl13 { get; set; }

        public string Cyl14 { get; set; }

        public string Cyl15 { get; set; }

        public string Cyl16 { get; set; }

        public decimal? TotalRepair { get; set; }

        public decimal? TotalReplace { get; set; }

        public decimal? QtySisa { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }
    }
    public class Overhaul_GBKanan
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Overhaul_GBKananModel> insertedRowValues { get; set; }
        public List<Overhaul_GBKananModel> modifiedRowValues { get; set; }
    }
    public class Overhaul_GBKananModel
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

        public int? RowNum { get; set; }

        public string Item { get; set; }

        public string SubItem { get; set; }

        public string PartNumber { get; set; }

        public decimal? QtyOrder { get; set; }

        public string Unit { get; set; }

        public string Cyl1 { get; set; }

        public string Cyl2 { get; set; }

        public string Cyl3 { get; set; }

        public string Cyl4 { get; set; }

        public string Cyl5 { get; set; }

        public string Cyl6 { get; set; }

        public string Cyl7 { get; set; }

        public string Cyl8 { get; set; }

        public string Cyl9 { get; set; }

        public string Cyl10 { get; set; }

        public string Cyl11 { get; set; }

        public string Cyl12 { get; set; }

        public string Cyl13 { get; set; }

        public string Cyl14 { get; set; }

        public string Cyl15 { get; set; }

        public string Cyl16 { get; set; }

        public decimal? TotalRepair { get; set; }

        public decimal? TotalReplace { get; set; }

        public decimal? QtySisa { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }
    }
    #endregion

    #region Services

    public class OverhaulService
    {

        public OverhaulModel GetNewModel(int userId)
        {
            OverhaulModel model = new OverhaulModel();
            model.Status = "Open";
            return model;
        }
        public OverhaulModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public OverhaulModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            OverhaulModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT *, T1.""FirstName"" AS ""UserName"" 
                            FROM ""Tx_Overhaul"" T0
                            LEFT JOIN ""Tm_User"" T1 ON T0.""ModifiedUser"" = T1.""Id""
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<OverhaulModel>(ssql, id).Single();

                model.ListMEKiris_ = this.Overhaul_MEKiris(CONTEXT, id);
                model.ListMEKanans_ = this.Overhaul_MEKanans(CONTEXT, id);
                model.ListAEKiris_ = this.Overhaul_AEKiris(CONTEXT, id);
                model.ListAEKanans_ = this.Overhaul_AEKanans(CONTEXT, id);
                model.ListGBKiris_ = this.Overhaul_GBKiris(CONTEXT, id);
                model.ListGBKanans_ = this.Overhaul_GBKanans(CONTEXT, id);

            }

            return model;
        }
        public List<Overhaul_MEKiriModel> Overhaul_MEKiris(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Overhaul_MEKiris(CONTEXT, id);
            }

        }

        public List<Overhaul_MEKiriModel> Overhaul_MEKiris(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Overhaul_MEKiriModel>("SELECT * FROM \"Tx_Overhaul_MEKiri\" WHERE \"Id\" =:p0", id).ToList();
        }
        public List<Overhaul_MEKananModel> Overhaul_MEKanans(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Overhaul_MEKanans(CONTEXT, id);
            }

        }

        public List<Overhaul_MEKananModel> Overhaul_MEKanans(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Overhaul_MEKananModel>("SELECT * FROM \"Tx_Overhaul_MEKanan\" WHERE \"Id\" =:p0", id).ToList();
        }
        public List<Overhaul_AEKiriModel> Overhaul_AEKiris(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Overhaul_AEKiris(CONTEXT, id);
            }

        }

        public List<Overhaul_AEKiriModel> Overhaul_AEKiris(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Overhaul_AEKiriModel>("SELECT * FROM \"Tx_Overhaul_AEKiri\" WHERE \"Id\" =:p0", id).ToList();
        }
        public List<Overhaul_AEKananModel> Overhaul_AEKanans(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Overhaul_AEKanans(CONTEXT, id);
            }

        }

        public List<Overhaul_AEKananModel> Overhaul_AEKanans(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Overhaul_AEKananModel>("SELECT * FROM \"Tx_Overhaul_AEKanan\" WHERE \"Id\" =:p0", id).ToList();
        }
        public List<Overhaul_GBKiriModel> Overhaul_GBKiris(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Overhaul_GBKiris(CONTEXT, id);
            }

        }

        public List<Overhaul_GBKiriModel> Overhaul_GBKiris(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Overhaul_GBKiriModel>("SELECT * FROM \"Tx_Overhaul_GBKiri\" WHERE \"Id\" =:p0", id).ToList();
        }
        public List<Overhaul_GBKananModel> Overhaul_GBKanans(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Overhaul_GBKanans(CONTEXT, id);
            }

        }

        public List<Overhaul_GBKananModel> Overhaul_GBKanans(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Overhaul_GBKananModel>("SELECT * FROM \"Tx_Overhaul_GBKanan\" WHERE \"Id\" =:p0", id).ToList();
        }
        public OverhaulModel NavFirst(int userId)
        {
            OverhaulModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Overhaul\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public OverhaulModel NavPrevious(int userId, long id = 0)
        {
            OverhaulModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Overhaul\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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

        public OverhaulModel NavNext(int userId, long id = 0)
        {
            OverhaulModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Overhaul\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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

        public OverhaulModel NavLast(int userId)
        {
            OverhaulModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_Overhaul\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        public long Add(OverhaulModel model)
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

                            Tx_Overhaul Tx_Overhaul = new Tx_Overhaul();
                            CopyProperty.CopyProperties(model, Tx_Overhaul, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_Overhaul.TransType = "Overhaul";
                            Tx_Overhaul.CreatedDate = dtModified;
                            Tx_Overhaul.CreatedUser = model._UserId;
                            Tx_Overhaul.ModifiedDate = dtModified;
                            Tx_Overhaul.ModifiedUser = model._UserId;

                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'Overhaul','" + dateX + "','') ").SingleOrDefault();
                            Tx_Overhaul.TransNo = transNo;

                            CONTEXT.Tx_Overhaul.Add(Tx_Overhaul);
                            CONTEXT.SaveChanges();
                            Id = Tx_Overhaul.Id;

                            String keyValue;
                            keyValue = Tx_Overhaul.Id.ToString();

                            if (model.MEKiris_ != null)
                            {
                                if (model.MEKiris_.insertedRowValues != null)
                                {
                                    foreach (var MEKiri in model.MEKiris_.insertedRowValues)
                                    {
                                        MEKiri_Add(CONTEXT, MEKiri, Id, model._UserId);
                                    }
                                }

                                if (model.MEKiris_.modifiedRowValues != null)
                                {
                                    foreach (var MEKiri in model.MEKiris_.modifiedRowValues)
                                    {
                                        MEKiri_Update(CONTEXT, MEKiri, model._UserId);
                                    }
                                }

                                if (model.MEKiris_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.MEKiris_.deletedRowKeys)
                                    {
                                        Overhaul_MEKiriModel MEKiriModel = new Overhaul_MEKiriModel();
                                        MEKiriModel.DetId = detId;
                                        MEKiri_Delete(CONTEXT, MEKiriModel);
                                    }
                                }
                            }

                            if (model.MEKanans_ != null)
                            {
                                if (model.MEKanans_.insertedRowValues != null)
                                {
                                    foreach (var MEKanan in model.MEKanans_.insertedRowValues)
                                    {
                                        MEKanan_Add(CONTEXT, MEKanan, Id, model._UserId);
                                    }
                                }

                                if (model.MEKanans_.modifiedRowValues != null)
                                {
                                    foreach (var MEKanan in model.MEKanans_.modifiedRowValues)
                                    {
                                        MEKanan_Update(CONTEXT, MEKanan, model._UserId);
                                    }
                                }

                                if (model.MEKanans_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.MEKanans_.deletedRowKeys)
                                    {
                                        Overhaul_MEKananModel MEKananModel = new Overhaul_MEKananModel();
                                        MEKananModel.DetId = detId;
                                        MEKanan_Delete(CONTEXT, MEKananModel);
                                    }
                                }
                            }

                            if (model.AEKiris_ != null)
                            {
                                if (model.AEKiris_.insertedRowValues != null)
                                {
                                    foreach (var AEKiri in model.AEKiris_.insertedRowValues)
                                    {
                                        AEKiri_Add(CONTEXT, AEKiri, Id, model._UserId);
                                    }
                                }

                                if (model.AEKiris_.modifiedRowValues != null)
                                {
                                    foreach (var AEKiri in model.AEKiris_.modifiedRowValues)
                                    {
                                        AEKiri_Update(CONTEXT, AEKiri, model._UserId);
                                    }
                                }

                                if (model.AEKiris_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.AEKiris_.deletedRowKeys)
                                    {
                                        Overhaul_AEKiriModel AEKiriModel = new Overhaul_AEKiriModel();
                                        AEKiriModel.DetId = detId;
                                        AEKiri_Delete(CONTEXT, AEKiriModel);
                                    }
                                }
                            }

                            if (model.AEKanans_ != null)
                            {
                                if (model.AEKanans_.insertedRowValues != null)
                                {
                                    foreach (var AEKanan in model.AEKanans_.insertedRowValues)
                                    {
                                        AEKanan_Add(CONTEXT, AEKanan, Id, model._UserId);
                                    }
                                }

                                if (model.AEKanans_.modifiedRowValues != null)
                                {
                                    foreach (var AEKanan in model.AEKanans_.modifiedRowValues)
                                    {
                                        AEKanan_Update(CONTEXT, AEKanan, model._UserId);
                                    }
                                }

                                if (model.AEKanans_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.AEKanans_.deletedRowKeys)
                                    {
                                        Overhaul_AEKananModel AEKananModel = new Overhaul_AEKananModel();
                                        AEKananModel.DetId = detId;
                                        AEKanan_Delete(CONTEXT, AEKananModel);
                                    }
                                }
                            }

                            if (model.GBKiris_ != null)
                            {
                                if (model.GBKiris_.insertedRowValues != null)
                                {
                                    foreach (var GBKiri in model.GBKiris_.insertedRowValues)
                                    {
                                        GBKiri_Add(CONTEXT, GBKiri, Id, model._UserId);
                                    }
                                }

                                if (model.GBKiris_.modifiedRowValues != null)
                                {
                                    foreach (var GBKiri in model.GBKiris_.modifiedRowValues)
                                    {
                                        GBKiri_Update(CONTEXT, GBKiri, model._UserId);
                                    }
                                }

                                if (model.GBKiris_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.GBKiris_.deletedRowKeys)
                                    {
                                        Overhaul_GBKiriModel GBKiriModel = new Overhaul_GBKiriModel();
                                        GBKiriModel.DetId = detId;
                                        GBKiri_Delete(CONTEXT, GBKiriModel);
                                    }
                                }
                            }

                            if (model.GBKanans_ != null)
                            {
                                if (model.GBKanans_.insertedRowValues != null)
                                {
                                    foreach (var GBKanan in model.GBKanans_.insertedRowValues)
                                    {
                                        GBKanan_Add(CONTEXT, GBKanan, Id, model._UserId);
                                    }
                                }

                                if (model.GBKanans_.modifiedRowValues != null)
                                {
                                    foreach (var GBKanan in model.GBKanans_.modifiedRowValues)
                                    {
                                        GBKanan_Update(CONTEXT, GBKanan, model._UserId);
                                    }
                                }

                                if (model.GBKanans_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.GBKanans_.deletedRowKeys)
                                    {
                                        Overhaul_GBKananModel GBKananModel = new Overhaul_GBKananModel();
                                        GBKananModel.DetId = detId;
                                        GBKanan_Delete(CONTEXT, GBKananModel);
                                    }
                                }
                            }

                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_Overhaul", "add", "Id", keyValue);


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
        public void Update(OverhaulModel model)
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


                                Tx_Overhaul Tx_Overhaul = CONTEXT.Tx_Overhaul.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_Overhaul.ModifiedDate = dtModified;
                                Tx_Overhaul.ModifiedUser = model._UserId;
                                if (Tx_Overhaul != null)
                                {
                                    var exceptColumns = new string[] { "Id","TransNo","CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_Overhaul, false, exceptColumns);
                                    Tx_Overhaul.ModifiedDate = dtModified;
                                    Tx_Overhaul.ModifiedUser = model._UserId;
                                    CONTEXT.SaveChanges();

                                    if (model.MEKiris_ != null)
                                    {
                                        if (model.MEKiris_.insertedRowValues != null)
                                        {
                                            foreach (var MEKiri in model.MEKiris_.insertedRowValues)
                                            {
                                                MEKiri_Add(CONTEXT, MEKiri, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.MEKiris_.modifiedRowValues != null)
                                        {
                                            foreach (var MEKiri in model.MEKiris_.modifiedRowValues)
                                            {
                                                MEKiri_Update(CONTEXT, MEKiri, model._UserId);
                                            }
                                        }

                                        if (model.MEKiris_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.MEKiris_.deletedRowKeys)
                                            {
                                                Overhaul_MEKiriModel MEKiriModel = new Overhaul_MEKiriModel();
                                                MEKiriModel.DetId = detId;
                                               MEKiri_Delete(CONTEXT, MEKiriModel);
                                            }
                                        }
                                    }
                                    if (model.MEKanans_ != null)
                                    {
                                        if (model.MEKanans_.insertedRowValues != null)
                                        {
                                            foreach (var MEKanan in model.MEKanans_.insertedRowValues)
                                            {
                                                MEKanan_Add(CONTEXT, MEKanan, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.MEKanans_.modifiedRowValues != null)
                                        {
                                            foreach (var MEKanan in model.MEKanans_.modifiedRowValues)
                                            {
                                                MEKanan_Update(CONTEXT, MEKanan, model._UserId);
                                            }
                                        }

                                        if (model.MEKanans_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.MEKanans_.deletedRowKeys)
                                            {
                                                Overhaul_MEKananModel MEKananModel = new Overhaul_MEKananModel();
                                                MEKananModel.DetId = detId;
                                                MEKanan_Delete(CONTEXT, MEKananModel);
                                            }
                                        }
                                    }

                                    if (model.AEKiris_ != null)
                                    {
                                        if (model.AEKiris_.insertedRowValues != null)
                                        {
                                            foreach (var AEKiri in model.AEKiris_.insertedRowValues)
                                            {
                                                AEKiri_Add(CONTEXT, AEKiri, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.AEKiris_.modifiedRowValues != null)
                                        {
                                            foreach (var AEKiri in model.AEKiris_.modifiedRowValues)
                                            {
                                                AEKiri_Update(CONTEXT, AEKiri, model._UserId);
                                            }
                                        }

                                        if (model.AEKiris_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.AEKiris_.deletedRowKeys)
                                            {
                                                Overhaul_AEKiriModel AEKiriModel = new Overhaul_AEKiriModel();
                                                AEKiriModel.DetId = detId;
                                                AEKiri_Delete(CONTEXT, AEKiriModel);
                                            }
                                        }
                                    }

                                    if (model.AEKanans_ != null)
                                    {
                                        if (model.AEKanans_.insertedRowValues != null)
                                        {
                                            foreach (var AEKanan in model.AEKanans_.insertedRowValues)
                                            {
                                                AEKanan_Add(CONTEXT, AEKanan, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.AEKanans_.modifiedRowValues != null)
                                        {
                                            foreach (var AEKanan in model.AEKanans_.modifiedRowValues)
                                            {
                                                AEKanan_Update(CONTEXT, AEKanan, model._UserId);
                                            }
                                        }

                                        if (model.AEKanans_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.AEKanans_.deletedRowKeys)
                                            {
                                                Overhaul_AEKananModel AEKananModel = new Overhaul_AEKananModel();
                                                AEKananModel.DetId = detId;
                                                AEKanan_Delete(CONTEXT, AEKananModel);
                                            }
                                        }
                                    }

                                    if (model.GBKiris_ != null)
                                    {
                                        if (model.GBKiris_.insertedRowValues != null)
                                        {
                                            foreach (var GBKiri in model.GBKiris_.insertedRowValues)
                                            {
                                                GBKiri_Add(CONTEXT, GBKiri, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.GBKiris_.modifiedRowValues != null)
                                        {
                                            foreach (var GBKiri in model.GBKiris_.modifiedRowValues)
                                            {
                                                GBKiri_Update(CONTEXT, GBKiri, model._UserId);
                                            }
                                        }

                                        if (model.GBKiris_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.GBKiris_.deletedRowKeys)
                                            {
                                                Overhaul_GBKiriModel GBKiriModel = new Overhaul_GBKiriModel();
                                                GBKiriModel.DetId = detId;
                                                GBKiri_Delete(CONTEXT, GBKiriModel);
                                            }
                                        }
                                    }

                                    if (model.GBKanans_ != null)
                                    {
                                        if (model.GBKanans_.insertedRowValues != null)
                                        {
                                            foreach (var GBKanan in model.GBKanans_.insertedRowValues)
                                            {
                                                GBKanan_Add(CONTEXT, GBKanan, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.GBKanans_.modifiedRowValues != null)
                                        {
                                            foreach (var GBKanan in model.GBKanans_.modifiedRowValues)
                                            {
                                                GBKanan_Update(CONTEXT, GBKanan, model._UserId);
                                            }
                                        }

                                        if (model.GBKanans_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.GBKanans_.deletedRowKeys)
                                            {
                                                Overhaul_GBKananModel GBKananModel = new Overhaul_GBKananModel();
                                                GBKananModel.DetId = detId;
                                                GBKanan_Delete(CONTEXT, GBKananModel);
                                            }
                                        }
                                    }


                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", " Tx_Overhaul", "update", "Id", keyValue);

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
        public long MEKiri_Add(HANA_APP CONTEXT, Overhaul_MEKiriModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_Overhaul_MEKiri Tx_Overhaul_MEKiri = new Tx_Overhaul_MEKiri();

                CopyProperty.CopyProperties(model, Tx_Overhaul_MEKiri, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_Overhaul_MEKiri.Id = Id;
                Tx_Overhaul_MEKiri.CreatedDate = dtModified;
                Tx_Overhaul_MEKiri.CreatedUser = UserId;
                Tx_Overhaul_MEKiri.ModifiedDate = dtModified;
                Tx_Overhaul_MEKiri.ModifiedUser = UserId;


                CONTEXT.Tx_Overhaul_MEKiri.Add(Tx_Overhaul_MEKiri);
                CONTEXT.SaveChanges();
                DetId = Tx_Overhaul_MEKiri.DetId;
                if (DetId != null)
                {
                    string ssql = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Replace', '')) / 6 AS ""REPAIR""
                    FROM ""Tx_Overhaul_MEKiri"" WHERE ""DetId"" = :p0 ;";
                    decimal Repair = CONTEXT.Database.SqlQuery<Decimal>(ssql, DetId).FirstOrDefault();

                    string ssql2 = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Repair', '')) / 7 AS ""Replace""
                    FROM ""Tx_Overhaul_MEKiri"" WHERE ""DetId"" = :p0 ;";
                    decimal Replace = CONTEXT.Database.SqlQuery<Decimal>(ssql2, DetId).FirstOrDefault();
                    if (Tx_Overhaul_MEKiri != null)
                    {
                        var exceptColumns = new string[] { "DetId", "Id" };
                        CopyProperty.CopyProperties(model, Tx_Overhaul_MEKiri, false, exceptColumns);
                        Tx_Overhaul_MEKiri.TotalReplace = Replace;
                        Tx_Overhaul_MEKiri.TotalRepair = Repair;
                        Tx_Overhaul_MEKiri.QtySisa = model.QtyOrder - Replace;
                        CONTEXT.SaveChanges();

                    }
                }
            }

            return DetId;

        }
        public void MEKiri_Update(HANA_APP CONTEXT, Overhaul_MEKiriModel model, int UserId)
        {
            if (model != null)
            {

                Tx_Overhaul_MEKiri Tx_Overhaul_MEKiri = CONTEXT.Tx_Overhaul_MEKiri.Find(model.DetId);

                if (Tx_Overhaul_MEKiri != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_Overhaul_MEKiri, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    
                    Tx_Overhaul_MEKiri.ModifiedDate = dtModified;
                    Tx_Overhaul_MEKiri.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                    string ssql = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Replace', '')) / 6 AS ""REPAIR""
                    FROM ""Tx_Overhaul_MEKiri"" WHERE ""DetId"" = :p0 ;";
                    decimal Repair = CONTEXT.Database.SqlQuery<Decimal>(ssql, model.DetId).FirstOrDefault();

                    string ssql2 = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Repair', '')) / 7 AS ""REPLACE""
                    FROM ""Tx_Overhaul_MEKiri"" WHERE ""DetId"" = :p0 ;";
                    decimal Replace = CONTEXT.Database.SqlQuery<Decimal>(ssql2, model.DetId).FirstOrDefault();
                    Tx_Overhaul_MEKiri.TotalReplace = Replace;
                    Tx_Overhaul_MEKiri.TotalRepair = Repair;
                    Tx_Overhaul_MEKiri.QtySisa = model.QtyOrder - Replace;
                    CONTEXT.SaveChanges();
                }


            }

        }
        public void MEKiri_Delete(HANA_APP CONTEXT, Overhaul_MEKiriModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_Overhaul_MEKiri\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }

        public long MEKanan_Add(HANA_APP CONTEXT, Overhaul_MEKananModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_Overhaul_MEKanan Tx_Overhaul_MEKanan = new Tx_Overhaul_MEKanan();

                CopyProperty.CopyProperties(model, Tx_Overhaul_MEKanan, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_Overhaul_MEKanan.Id = Id;
                Tx_Overhaul_MEKanan.CreatedDate = dtModified;
                Tx_Overhaul_MEKanan.CreatedUser = UserId;
                Tx_Overhaul_MEKanan.ModifiedDate = dtModified;
                Tx_Overhaul_MEKanan.ModifiedUser = UserId;


                CONTEXT.Tx_Overhaul_MEKanan.Add(Tx_Overhaul_MEKanan);
                CONTEXT.SaveChanges();
                DetId = Tx_Overhaul_MEKanan.DetId;
                if (DetId != null)
                {
                    string ssql = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Replace', '')) / 6 AS ""REPAIR""
                    FROM ""Tx_Overhaul_MEKanan"" WHERE ""DetId"" = :p0 ;";
                    decimal Repair = CONTEXT.Database.SqlQuery<Decimal>(ssql, DetId).FirstOrDefault();

                    string ssql2 = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Repair', '')) / 7 AS ""Replace""
                    FROM ""Tx_Overhaul_MEKanan"" WHERE ""DetId"" = :p0 ;";
                    decimal Replace = CONTEXT.Database.SqlQuery<Decimal>(ssql2, DetId).FirstOrDefault();
                    if (Tx_Overhaul_MEKanan != null)
                    {
                        var exceptColumns = new string[] { "DetId", "Id" };
                        CopyProperty.CopyProperties(model, Tx_Overhaul_MEKanan, false, exceptColumns);
                        Tx_Overhaul_MEKanan.TotalReplace = Replace;
                        Tx_Overhaul_MEKanan.TotalRepair = Repair;
                        Tx_Overhaul_MEKanan.QtySisa = model.QtyOrder - Replace;
                        CONTEXT.SaveChanges();

                    }
                }
            }

            return DetId;

        }
        public void MEKanan_Update(HANA_APP CONTEXT, Overhaul_MEKananModel model, int UserId)
        {
            if (model != null)
            {

                Tx_Overhaul_MEKanan Tx_Overhaul_MEKanan = CONTEXT.Tx_Overhaul_MEKanan.Find(model.DetId);

                if (Tx_Overhaul_MEKanan != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_Overhaul_MEKanan, false, exceptColumns);

                    

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    Tx_Overhaul_MEKanan.ModifiedDate = dtModified;
                    Tx_Overhaul_MEKanan.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();
                    string ssql = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Replace', '')) / 6 AS ""REPAIR""
                    FROM ""Tx_Overhaul_MEKanan"" WHERE ""DetId"" = :p0 ;";
                    decimal Repair = CONTEXT.Database.SqlQuery<Decimal>(ssql, model.DetId).FirstOrDefault();

                    string ssql2 = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Repair', '')) / 7 AS ""REPLACE""
                    FROM ""Tx_Overhaul_MEKanan"" WHERE ""DetId"" = :p0 ;";
                    decimal Replace = CONTEXT.Database.SqlQuery<Decimal>(ssql2, model.DetId).FirstOrDefault();
                    Tx_Overhaul_MEKanan.TotalReplace = Replace;
                    Tx_Overhaul_MEKanan.TotalRepair = Repair;
                    Tx_Overhaul_MEKanan.QtySisa = model.QtyOrder - Replace;

                    CONTEXT.SaveChanges();



                }


            }

        }
        public void MEKanan_Delete(HANA_APP CONTEXT, Overhaul_MEKananModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_Overhaul_MEKanan\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }

        public long AEKiri_Add(HANA_APP CONTEXT, Overhaul_AEKiriModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_Overhaul_AEKiri Tx_Overhaul_AEKiri = new Tx_Overhaul_AEKiri();

                CopyProperty.CopyProperties(model, Tx_Overhaul_AEKiri, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_Overhaul_AEKiri.Id = Id;
                Tx_Overhaul_AEKiri.CreatedDate = dtModified;
                Tx_Overhaul_AEKiri.CreatedUser = UserId;
                Tx_Overhaul_AEKiri.ModifiedDate = dtModified;
                Tx_Overhaul_AEKiri.ModifiedUser = UserId;


                CONTEXT.Tx_Overhaul_AEKiri.Add(Tx_Overhaul_AEKiri);
                CONTEXT.SaveChanges();
                DetId = Tx_Overhaul_AEKiri.DetId;
                if (DetId != null)
                {
                    string ssql = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Replace', '')) / 6 AS ""REPAIR""
                    FROM ""Tx_Overhaul_AEKiri"" WHERE ""DetId"" = :p0 ;";
                    decimal Repair = CONTEXT.Database.SqlQuery<Decimal>(ssql, DetId).FirstOrDefault();

                    string ssql2 = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Repair', '')) / 7 AS ""Replace""
                    FROM ""Tx_Overhaul_AEKiri"" WHERE ""DetId"" = :p0 ;";
                    decimal Replace = CONTEXT.Database.SqlQuery<Decimal>(ssql2, DetId).FirstOrDefault();
                    if (Tx_Overhaul_AEKiri != null)
                    {
                        var exceptColumns = new string[] { "DetId", "Id" };
                        CopyProperty.CopyProperties(model, Tx_Overhaul_AEKiri, false, exceptColumns);
                        Tx_Overhaul_AEKiri.TotalReplace = Replace;
                        Tx_Overhaul_AEKiri.TotalRepair = Repair;
                        Tx_Overhaul_AEKiri.QtySisa = model.QtyOrder - Replace;
                        CONTEXT.SaveChanges();

                    }
                }
            }

            return DetId;

        }
        public void AEKiri_Update(HANA_APP CONTEXT, Overhaul_AEKiriModel model, int UserId)
        {
            if (model != null)
            {

                Tx_Overhaul_AEKiri Tx_Overhaul_AEKiri = CONTEXT.Tx_Overhaul_AEKiri.Find(model.DetId);

                if (Tx_Overhaul_AEKiri != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_Overhaul_AEKiri, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    
                    Tx_Overhaul_AEKiri.ModifiedDate = dtModified;
                    Tx_Overhaul_AEKiri.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                    string ssql = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Replace', '')) / 6 AS ""REPAIR""
                    FROM ""Tx_Overhaul_AEKiri"" WHERE ""DetId"" = :p0 ;";
                    decimal Repair = CONTEXT.Database.SqlQuery<Decimal>(ssql, model.DetId).FirstOrDefault();

                    string ssql2 = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Repair', '')) / 7 AS ""REPLACE""
                    FROM ""Tx_Overhaul_AEKiri"" WHERE ""DetId"" = :p0 ;";
                    decimal Replace = CONTEXT.Database.SqlQuery<Decimal>(ssql2, model.DetId).FirstOrDefault();
                    Tx_Overhaul_AEKiri.TotalReplace = Replace;
                    Tx_Overhaul_AEKiri.TotalRepair = Repair;
                    Tx_Overhaul_AEKiri.QtySisa = model.QtyOrder - Replace;
                    CONTEXT.SaveChanges();
                }


            }

        }
        public void AEKiri_Delete(HANA_APP CONTEXT, Overhaul_AEKiriModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_Overhaul_AEKiri\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }

        public long AEKanan_Add(HANA_APP CONTEXT, Overhaul_AEKananModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_Overhaul_AEKanan Tx_Overhaul_AEKanan = new Tx_Overhaul_AEKanan();

                CopyProperty.CopyProperties(model, Tx_Overhaul_AEKanan, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_Overhaul_AEKanan.Id = Id;
                Tx_Overhaul_AEKanan.CreatedDate = dtModified;
                Tx_Overhaul_AEKanan.CreatedUser = UserId;
                Tx_Overhaul_AEKanan.ModifiedDate = dtModified;
                Tx_Overhaul_AEKanan.ModifiedUser = UserId;


                CONTEXT.Tx_Overhaul_AEKanan.Add(Tx_Overhaul_AEKanan);
                CONTEXT.SaveChanges();
                DetId = Tx_Overhaul_AEKanan.DetId;
                if (DetId != null)
                {
                    string ssql = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Replace', '')) / 6 AS ""REPAIR""
                    FROM ""Tx_Overhaul_AEKanan"" WHERE ""DetId"" = :p0 ;";
                    decimal Repair = CONTEXT.Database.SqlQuery<Decimal>(ssql, DetId).FirstOrDefault();

                    string ssql2 = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Repair', '')) / 7 AS ""Replace""
                    FROM ""Tx_Overhaul_AEKanan"" WHERE ""DetId"" = :p0 ;";
                    decimal Replace = CONTEXT.Database.SqlQuery<Decimal>(ssql2, DetId).FirstOrDefault();
                    if (Tx_Overhaul_AEKanan != null)
                    {
                        var exceptColumns = new string[] { "DetId", "Id" };
                        CopyProperty.CopyProperties(model, Tx_Overhaul_AEKanan, false, exceptColumns);
                        Tx_Overhaul_AEKanan.TotalReplace = Replace;
                        Tx_Overhaul_AEKanan.TotalRepair = Repair;
                        Tx_Overhaul_AEKanan.QtySisa = model.QtyOrder - Replace;
                        CONTEXT.SaveChanges();

                    }
                }
            }

            return DetId;

        }
        public void AEKanan_Update(HANA_APP CONTEXT, Overhaul_AEKananModel model, int UserId)
        {
            if (model != null)
            {

                Tx_Overhaul_AEKanan Tx_Overhaul_AEKanan = CONTEXT.Tx_Overhaul_AEKanan.Find(model.DetId);

                if (Tx_Overhaul_AEKanan != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_Overhaul_AEKanan, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    
                    Tx_Overhaul_AEKanan.ModifiedDate = dtModified;
                    Tx_Overhaul_AEKanan.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                    string ssql = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Replace', '')) / 6 AS ""REPAIR""
                    FROM ""Tx_Overhaul_AEKanan"" WHERE ""DetId"" = :p0 ;";
                    decimal Repair = CONTEXT.Database.SqlQuery<Decimal>(ssql, model.DetId).FirstOrDefault();

                    string ssql2 = @"SELECT 
                    LENGTH(REPLACE(IFNULL(""Cyl1"", '')
                    || IFNULL(""Cyl2"", '')
                    || IFNULL(""Cyl3"", '')
                    || IFNULL(""Cyl4"", '')
                    || IFNULL(""Cyl5"", '')
                    || IFNULL(""Cyl6"", '')
                    || IFNULL(""Cyl7"", '')
                    || IFNULL(""Cyl8"", '')
                    || IFNULL(""Cyl9"", '')
                    || IFNULL(""Cyl10"", '')
                    || IFNULL(""Cyl11"", '')
                    || IFNULL(""Cyl12"", '')
                    || IFNULL(""Cyl13"", '')
                    || IFNULL(""Cyl14"", '')
                    || IFNULL(""Cyl15"", '')
                    || IFNULL(""Cyl16"", ''), 'Repair', '')) / 7 AS ""REPLACE""
                    FROM ""Tx_Overhaul_AEKanan"" WHERE ""DetId"" = :p0 ;";
                    decimal Replace = CONTEXT.Database.SqlQuery<Decimal>(ssql2, model.DetId).FirstOrDefault();
                    Tx_Overhaul_AEKanan.TotalReplace = Replace;
                    Tx_Overhaul_AEKanan.TotalRepair = Repair;
                    Tx_Overhaul_AEKanan.QtySisa = model.QtyOrder - Replace;
                    CONTEXT.SaveChanges();
                }


            }

        }
        public void AEKanan_Delete(HANA_APP CONTEXT, Overhaul_AEKananModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_Overhaul_AEKanan\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }

        public long GBKiri_Add(HANA_APP CONTEXT, Overhaul_GBKiriModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_Overhaul_GBKiri Tx_Overhaul_GBKiri = new Tx_Overhaul_GBKiri();

                CopyProperty.CopyProperties(model, Tx_Overhaul_GBKiri, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_Overhaul_GBKiri.Id = Id;
                Tx_Overhaul_GBKiri.CreatedDate = dtModified;
                Tx_Overhaul_GBKiri.CreatedUser = UserId;
                Tx_Overhaul_GBKiri.ModifiedDate = dtModified;
                Tx_Overhaul_GBKiri.ModifiedUser = UserId;


                CONTEXT.Tx_Overhaul_GBKiri.Add(Tx_Overhaul_GBKiri);
                CONTEXT.SaveChanges();
                DetId = Tx_Overhaul_GBKiri.DetId;

            }

            return DetId;

        }
        public void GBKiri_Update(HANA_APP CONTEXT, Overhaul_GBKiriModel model, int UserId)
        {
            if (model != null)
            {

                Tx_Overhaul_GBKiri Tx_Overhaul_GBKiri = CONTEXT.Tx_Overhaul_GBKiri.Find(model.DetId);

                if (Tx_Overhaul_GBKiri != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_Overhaul_GBKiri, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_Overhaul_GBKiri.ModifiedDate = dtModified;
                    Tx_Overhaul_GBKiri.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void GBKiri_Delete(HANA_APP CONTEXT, Overhaul_GBKiriModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_Overhaul_GBKiri\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }

        public long GBKanan_Add(HANA_APP CONTEXT, Overhaul_GBKananModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_Overhaul_GBKanan Tx_Overhaul_GBKanan = new Tx_Overhaul_GBKanan();

                CopyProperty.CopyProperties(model, Tx_Overhaul_GBKanan, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_Overhaul_GBKanan.Id = Id;
                Tx_Overhaul_GBKanan.CreatedDate = dtModified;
                Tx_Overhaul_GBKanan.CreatedUser = UserId;
                Tx_Overhaul_GBKanan.ModifiedDate = dtModified;
                Tx_Overhaul_GBKanan.ModifiedUser = UserId;


                CONTEXT.Tx_Overhaul_GBKanan.Add(Tx_Overhaul_GBKanan);
                CONTEXT.SaveChanges();
                DetId = Tx_Overhaul_GBKanan.DetId;

            }

            return DetId;

        }
        public void GBKanan_Update(HANA_APP CONTEXT, Overhaul_GBKananModel model, int UserId)
        {
            if (model != null)
            {

                Tx_Overhaul_GBKanan Tx_Overhaul_GBKanan = CONTEXT.Tx_Overhaul_GBKanan.Find(model.DetId);

                if (Tx_Overhaul_GBKanan != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_Overhaul_GBKanan, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_Overhaul_GBKanan.ModifiedDate = dtModified;
                    Tx_Overhaul_GBKanan.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void GBKanan_Delete(HANA_APP CONTEXT, Overhaul_GBKananModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_Overhaul_GBKanan\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
    }


    #endregion

}