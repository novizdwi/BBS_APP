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

namespace Models.Master.Ship
{
    #region Models
    public class ShipModel
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
        public string ShipCode { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipName { get; set; }

        [Required(ErrorMessage = "required")]
        public string Alias { get; set; }

        [Required(ErrorMessage = "required")]
        public string ShipType { get; set; }

        [Required(ErrorMessage = "required")]
        public string Builder { get; set; }

        [Required(ErrorMessage = "required")]
        public string YearOfBuild { get; set; }

        public string Flag { get; set; }

        public string Class { get; set; }

        public string CallSign { get; set; }

        public string MarkTonnage { get; set; }

        public string RegMark { get; set; }

        public string RegPlace { get; set; }

        public decimal? LOA { get; set; }

        public decimal? LBP { get; set; }

        public decimal? BreadthMoulded { get; set; }

        public decimal? DepthDraught { get; set; }

        public decimal? GrossTonnage { get; set; }

        public decimal? NetTonnage { get; set; }

        public decimal? DraftDesign { get; set; }

        public string Owner { get; set; }

        public string OwnerAddress { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string UserName { get; set; }

        public string MarkOfTonageCer { get; set; }

        public List<Ship_EngineModel> ListEngines_ = new List<Ship_EngineModel>();

        public Ship_Engines Engines_ { get; set; }

        public List<Ship_NavEqModel> ListNavEqs_ = new List<Ship_NavEqModel>();

        public Ship_NavEqs NavEqs_ { get; set; }

        public List<Ship_SafeEqModel> ListSafeEqs_ = new List<Ship_SafeEqModel>();

        public Ship_SafeEqs SafeEqs_ { get; set; }

        public List<Ship_AccoModel> ListAccos_ = new List<Ship_AccoModel>();

        public Ship_Accos Accos_ { get; set; }

        public List<Ship_AnchorModel> ListAnchors_ = new List<Ship_AnchorModel>();

        public Ship_Anchors Anchors_ { get; set; }

        public List<Ship_OperationModel> ListOperations_ = new List<Ship_OperationModel>();

        public Ship_Operations Operations_ { get; set; }

        public List<Ship_AttachmentModel> ListAttachments_ = new List<Ship_AttachmentModel>();

        public Ship_Attachments Attachments_ { get; set; }
    }
    public class Ship_Engines
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Ship_EngineModel> insertedRowValues { get; set; }
        public List<Ship_EngineModel> modifiedRowValues { get; set; }
    }

    public class Ship_EngineModel
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
        
        public string DetName { get; set; }
        
        public string Brand { get; set; }

        public string Power { get; set; }

        public string StdLO { get; set; }
        public string StdFilterLO { get; set; }
        public string StdFORacor { get; set; }
        public string StdFOFilter { get; set; }

    }
    public class Ship_NavEqs
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Ship_NavEqModel> insertedRowValues { get; set; }
        public List<Ship_NavEqModel> modifiedRowValues { get; set; }
    }
    
    public class Ship_NavEqModel
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

        public string DetName { get; set; }

        public string Brand { get; set; }

        public string Remark { get; set; }
        

    }
    public class Ship_SafeEqs
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Ship_SafeEqModel> insertedRowValues { get; set; }
        public List<Ship_SafeEqModel> modifiedRowValues { get; set; }
    }
    public class Ship_SafeEqModel
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

        public string DetName { get; set; }

        public string Brand { get; set; }

        public string Remark { get; set; }


    }
    public class Ship_Accos
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Ship_AccoModel> insertedRowValues { get; set; }
        public List<Ship_AccoModel> modifiedRowValues { get; set; }
    }
    public class Ship_AccoModel
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

        public string DetName { get; set; }

        public string Remark { get; set; }


    }
    public class Ship_Anchors
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Ship_AnchorModel> insertedRowValues { get; set; }
        public List<Ship_AnchorModel> modifiedRowValues { get; set; }
    }
    public class Ship_AnchorModel
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

        public string DetName { get; set; }

        public string Brand { get; set; }

        public string Remark { get; set; }


    }
    public class Ship_Attachments
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Ship_AttachmentModel> insertedRowValues { get; set; }
        public List<Ship_AttachmentModel> modifiedRowValues { get; set; }
    }
    public class Ship_Operations
    {
        public List<long> deletedRowKeys { get; set; }
        public List<Ship_OperationModel> insertedRowValues { get; set; }
        public List<Ship_OperationModel> modifiedRowValues { get; set; }
    }
    public class Ship_OperationModel
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

        public string DetName { get; set; }

        public string Remark { get; set; }


    }
    public class Ship_AttachmentModel
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

    #region Service
    public class ShipService
    {
        public long Add(ShipModel model)
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

                            Tm_Ship Tm_Ship = new Tm_Ship();
                            CopyProperty.CopyProperties(model, Tm_Ship, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                            Tm_Ship.CreatedDate = dtModified;
                            Tm_Ship.CreatedUser = model._UserId;
                            Tm_Ship.ModifiedDate = dtModified;
                            Tm_Ship.ModifiedUser = model._UserId;


                            CONTEXT.Tm_Ship.Add(Tm_Ship);
                            CONTEXT.SaveChanges();
                            Id = Tm_Ship.Id;

                            String keyValue;
                            keyValue = Tm_Ship.Id.ToString();

                            if (model.Engines_ != null)
                            {
                                if (model.Engines_.insertedRowValues != null)
                                {
                                    foreach (var engine in model.Engines_.insertedRowValues)
                                    {
                                        Engine_Add(CONTEXT, engine, Id, model._UserId);
                                    }
                                }

                                if (model.Engines_.modifiedRowValues != null)
                                {
                                    foreach (var engine in model.Engines_.modifiedRowValues)
                                    {
                                        Engine_Update(CONTEXT, engine, model._UserId);
                                    }
                                }

                                if (model.Engines_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.Engines_.deletedRowKeys)
                                    {
                                        Ship_EngineModel shipModel = new Ship_EngineModel();
                                        shipModel.DetId = detId;
                                        Engine_Delete(CONTEXT, shipModel);
                                    }
                                }
                            }

                            if (model.NavEqs_ != null)
                            {
                                if (model.NavEqs_.insertedRowValues != null)
                                {
                                    foreach (var naveq in model.NavEqs_.insertedRowValues)
                                    {
                                        NavEq_Add(CONTEXT, naveq, Id, model._UserId);
                                    }
                                }

                                if (model.NavEqs_.modifiedRowValues != null)
                                {
                                    foreach (var naveq in model.NavEqs_.modifiedRowValues)
                                    {
                                        NavEq_Update(CONTEXT, naveq, model._UserId);
                                    }
                                }

                                if (model.NavEqs_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.NavEqs_.deletedRowKeys)
                                    {
                                        Ship_NavEqModel shipNavEqModel = new Ship_NavEqModel();
                                        shipNavEqModel.DetId = detId;
                                        NavEq_Delete(CONTEXT, shipNavEqModel);
                                    }
                                }
                            }
                            if (model.SafeEqs_ != null)
                            {
                                if (model.SafeEqs_.insertedRowValues != null)
                                {
                                    foreach (var safeeq in model.SafeEqs_.insertedRowValues)
                                    {
                                        SafeEq_Add(CONTEXT, safeeq, model.Id, model._UserId);
                                    }
                                }

                                if (model.SafeEqs_.modifiedRowValues != null)
                                {
                                    foreach (var safeeq in model.SafeEqs_.modifiedRowValues)
                                    {
                                        SafeEq_Update(CONTEXT, safeeq, model._UserId);
                                    }
                                }

                                if (model.SafeEqs_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.SafeEqs_.deletedRowKeys)
                                    {
                                        Ship_SafeEqModel shipSafeEqModel = new Ship_SafeEqModel();
                                        shipSafeEqModel.DetId = detId;
                                        SafeEq_Delete(CONTEXT, shipSafeEqModel);
                                    }
                                }
                            }
                            if (model.Accos_ != null)
                            {
                                if (model.Accos_.insertedRowValues != null)
                                {
                                    foreach (var acco in model.Accos_.insertedRowValues)
                                    {
                                        Acco_Add(CONTEXT, acco, model.Id, model._UserId);
                                    }
                                }

                                if (model.Accos_.modifiedRowValues != null)
                                {
                                    foreach (var safeeq in model.Accos_.modifiedRowValues)
                                    {
                                        Acco_Update(CONTEXT, safeeq, model._UserId);
                                    }
                                }

                                if (model.Accos_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.Accos_.deletedRowKeys)
                                    {
                                        Ship_AccoModel shipAccoModel = new Ship_AccoModel();
                                        shipAccoModel.DetId = detId;
                                        Acco_Delete(CONTEXT, shipAccoModel);
                                    }
                                }
                            }
                            if (model.Anchors_ != null)
                            {
                                if (model.Anchors_.insertedRowValues != null)
                                {
                                    foreach (var anchor in model.Anchors_.insertedRowValues)
                                    {
                                        Anchor_Add(CONTEXT, anchor, model.Id, model._UserId);
                                    }
                                }

                                if (model.Anchors_.modifiedRowValues != null)
                                {
                                    foreach (var anchor in model.Anchors_.modifiedRowValues)
                                    {
                                        Anchor_Update(CONTEXT, anchor, model._UserId);
                                    }
                                }

                                if (model.Anchors_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.Anchors_.deletedRowKeys)
                                    {
                                        Ship_AnchorModel shipAnchorModel = new Ship_AnchorModel();
                                        shipAnchorModel.DetId = detId;
                                        Anchor_Delete(CONTEXT, shipAnchorModel);
                                    }
                                }
                            }
                            if (model.Operations_ != null)
                            {
                                if (model.Operations_.insertedRowValues != null)
                                {
                                    foreach (var operation in model.Operations_.insertedRowValues)
                                    {
                                        Operation_Add(CONTEXT, operation, model.Id, model._UserId);
                                    }
                                }

                                if (model.Operations_.modifiedRowValues != null)
                                {
                                    foreach (var operation in model.Operations_.modifiedRowValues)
                                    {
                                        Operation_Update(CONTEXT, operation, model._UserId);
                                    }
                                }

                                if (model.Operations_.deletedRowKeys != null)
                                {
                                    foreach (var detId in model.Operations_.deletedRowKeys)
                                    {
                                        Ship_OperationModel shipOperationModel = new Ship_OperationModel();
                                        shipOperationModel.DetId = detId;
                                        Operation_Delete(CONTEXT, shipOperationModel);
                                    }
                                }
                            }


                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tm_Ship", "add", "Id", keyValue);


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
        public void Update(ShipModel model)
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


                                Tm_Ship Tm_Ship = CONTEXT.Tm_Ship.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tm_Ship.ModifiedDate = dtModified;
                                Tm_Ship.ModifiedUser = model._UserId;
                                if (Tm_Ship != null)
                                {
                                    var exceptColumns = new string[] { "Id" };
                                    CopyProperty.CopyProperties(model, Tm_Ship, false, exceptColumns);
                                    Tm_Ship.ModifiedDate = dtModified;
                                    Tm_Ship.ModifiedUser = model._UserId;
                                    CONTEXT.SaveChanges();

                                    if (model.Engines_ != null)
                                    {
                                        if (model.Engines_.insertedRowValues != null)
                                        {
                                            foreach (var engine in model.Engines_.insertedRowValues)
                                            {
                                                Engine_Add(CONTEXT, engine, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.Engines_.modifiedRowValues != null)
                                        {
                                            foreach (var engine in model.Engines_.modifiedRowValues)
                                            {
                                                Engine_Update(CONTEXT, engine, model._UserId);
                                            }
                                        }

                                        if (model.Engines_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.Engines_.deletedRowKeys)
                                            {
                                                Ship_EngineModel shipModel = new Ship_EngineModel();
                                                shipModel.DetId = detId;
                                                Engine_Delete(CONTEXT, shipModel);
                                            }
                                        }
                                    }

                                    if (model.NavEqs_ != null)
                                    {
                                        if (model.NavEqs_.insertedRowValues != null)
                                        {
                                            foreach (var naveq in model.NavEqs_.insertedRowValues)
                                            {
                                                NavEq_Add(CONTEXT, naveq, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.NavEqs_.modifiedRowValues != null)
                                        {
                                            foreach (var reference in model.NavEqs_.modifiedRowValues)
                                            {
                                                NavEq_Update(CONTEXT, reference, model._UserId);
                                            }
                                        }

                                        if (model.NavEqs_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.NavEqs_.deletedRowKeys)
                                            {
                                                Ship_NavEqModel shipNavEqModel = new Ship_NavEqModel();
                                                shipNavEqModel.DetId = detId;
                                                NavEq_Delete(CONTEXT, shipNavEqModel);
                                            }
                                        }
                                    }
                                    if (model.SafeEqs_ != null)
                                    {
                                        if (model.SafeEqs_.insertedRowValues != null)
                                        {
                                            foreach (var safeeq in model.SafeEqs_.insertedRowValues)
                                            {
                                                SafeEq_Add(CONTEXT, safeeq, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.SafeEqs_.modifiedRowValues != null)
                                        {
                                            foreach (var safeeq in model.SafeEqs_.modifiedRowValues)
                                            {
                                                SafeEq_Update(CONTEXT, safeeq, model._UserId);
                                            }
                                        }

                                        if (model.SafeEqs_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.SafeEqs_.deletedRowKeys)
                                            {
                                                Ship_SafeEqModel shipSafeEqModel = new Ship_SafeEqModel();
                                                shipSafeEqModel.DetId = detId;
                                                SafeEq_Delete(CONTEXT, shipSafeEqModel);
                                            }
                                        }
                                    }
                                    if (model.Accos_ != null)
                                    {
                                        if (model.Accos_.insertedRowValues != null)
                                        {
                                            foreach (var acco in model.Accos_.insertedRowValues)
                                            {
                                                Acco_Add(CONTEXT, acco, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.Accos_.modifiedRowValues != null)
                                        {
                                            foreach (var safeeq in model.Accos_.modifiedRowValues)
                                            {
                                                Acco_Update(CONTEXT, safeeq, model._UserId);
                                            }
                                        }

                                        if (model.Accos_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.Accos_.deletedRowKeys)
                                            {
                                                Ship_AccoModel shipAccoModel = new Ship_AccoModel();
                                                shipAccoModel.DetId = detId;
                                                Acco_Delete(CONTEXT, shipAccoModel);
                                            }
                                        }
                                    }
                                    if (model.Anchors_ != null)
                                    {
                                        if (model.Anchors_.insertedRowValues != null)
                                        {
                                            foreach (var anchor in model.Anchors_.insertedRowValues)
                                            {
                                                Anchor_Add(CONTEXT, anchor, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.Anchors_.modifiedRowValues != null)
                                        {
                                            foreach (var anchor in model.Anchors_.modifiedRowValues)
                                            {
                                                Anchor_Update(CONTEXT, anchor, model._UserId);
                                            }
                                        }

                                        if (model.Anchors_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.Anchors_.deletedRowKeys)
                                            {
                                                Ship_AnchorModel shipAnchorModel = new Ship_AnchorModel();
                                                shipAnchorModel.DetId = detId;
                                                Anchor_Delete(CONTEXT, shipAnchorModel);
                                            }
                                        }
                                    }
                                    if (model.Operations_ != null)
                                    {
                                        if (model.Operations_.insertedRowValues != null)
                                        {
                                            foreach (var operation in model.Operations_.insertedRowValues)
                                            {
                                                Operation_Add(CONTEXT, operation, model.Id, model._UserId);
                                            }
                                        }

                                        if (model.Operations_.modifiedRowValues != null)
                                        {
                                            foreach (var operation in model.Operations_.modifiedRowValues)
                                            {
                                                Operation_Update(CONTEXT, operation, model._UserId);
                                            }
                                        }

                                        if (model.Operations_.deletedRowKeys != null)
                                        {
                                            foreach (var detId in model.Operations_.deletedRowKeys)
                                            {
                                                Ship_OperationModel shipOperationModel = new Ship_OperationModel();
                                                shipOperationModel.DetId = detId;
                                                Operation_Delete(CONTEXT, shipOperationModel);
                                            }
                                        }
                                    }
                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", " Tm_Ship", "update", "Id", keyValue);

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
        public ShipModel GetNewModel(int userId)
        {
            ShipModel model = new ShipModel();
            return model;
        }
        public ShipModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }
        public ShipModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            ShipModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.*, T1.""UserName"" AS ""UserName""
                            FROM ""Tm_Ship"" T0
                            LEFT JOIN ""Tm_User"" T1 ON T0.""ModifiedUser"" = T1.""Id""
                            WHERE T0.""Id""=:p0 ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name);
                model = CONTEXT.Database.SqlQuery<ShipModel>(ssql, id).Single();

                //add dulu dari master yang belum ada
                //CONTEXT.Database.ExecuteSqlCommand("CALL \"SpShip_AddNewReference\"(:p0, :p1)", userId, id);

                model.ListEngines_ = this.Ship_Engines(CONTEXT, id);
                model.ListNavEqs_ = this.Ship_NavEqs(CONTEXT, id);
                model.ListSafeEqs_ = this.Ship_SafeEqs(CONTEXT, id);
                model.ListAccos_ = this.Ship_Accos(CONTEXT, id);
                model.ListAnchors_ = this.Ship_Anchors(CONTEXT, id);
                model.ListOperations_ = this.Ship_Operations(CONTEXT, id);
                model.ListAttachments_ = this.Ship_Attachments(CONTEXT, id);
            }
            return model;


        }
        public ShipModel NavFirst(int userId)
        {
            ShipModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Ship\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"ModifiedDate\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public ShipModel NavPrevious(int userId, long id = 0)
        {
            ShipModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Ship\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + "  ORDER BY T0.\"ModifiedDate\" DESC", id).FirstOrDefault();
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
        public ShipModel NavNext(int userId, long id = 0)
        {
            ShipModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Ship\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + "  ORDER BY T0.\"ModifiedDate\" ASC", id).FirstOrDefault();
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
        public ShipModel NavLast(int userId)
        {
            ShipModel model = null;
            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "Ship");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tm_Ship\" T0 WHERE 1=1 " + sqlCriteria + "  ORDER BY T0.\"ModifiedDate\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        public List<Ship_EngineModel> Ship_Engines(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Ship_Engines(CONTEXT, id);
            }

        }
        public List<Ship_EngineModel> Ship_Engines(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Ship_EngineModel>("SELECT * FROM \"Tm_Ship_Engine\" WHERE \"Id\" =:p0", id).ToList();
        }
        public long Engine_Add(HANA_APP CONTEXT, Ship_EngineModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tm_Ship_Engine Tm_Ship_Engine = new Tm_Ship_Engine();

                CopyProperty.CopyProperties(model, Tm_Ship_Engine, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tm_Ship_Engine.Id = Id;
                Tm_Ship_Engine.CreatedDate = dtModified;
                Tm_Ship_Engine.CreatedUser = UserId;
                Tm_Ship_Engine.ModifiedDate = dtModified;
                Tm_Ship_Engine.Modifieduser = UserId;

                CONTEXT.Tm_Ship_Engine.Add(Tm_Ship_Engine);
                CONTEXT.SaveChanges();
                DetId = Tm_Ship_Engine.DetId;

            }

            return DetId;

        }
        public void Engine_Update(HANA_APP CONTEXT, Ship_EngineModel model, int UserId)
        {
            if (model != null)
            {

                Tm_Ship_Engine Tm_Ship_Engine = CONTEXT.Tm_Ship_Engine.Find(model.DetId);

                if (Tm_Ship_Engine != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tm_Ship_Engine, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tm_Ship_Engine.ModifiedDate = dtModified;
                    Tm_Ship_Engine.Modifieduser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Engine_Delete(HANA_APP CONTEXT, Ship_EngineModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Ship_Engine\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
        public List<Ship_NavEqModel> Ship_NavEqs(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Ship_NavEqs(CONTEXT, id);
            }

        }
        public List<Ship_NavEqModel> Ship_NavEqs(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Ship_NavEqModel>("SELECT * FROM \"Tm_Ship_NavEq\" WHERE \"Id\" =:p0", id).ToList();
        }
        public long NavEq_Add(HANA_APP CONTEXT, Ship_NavEqModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tm_Ship_NavEq Tm_Ship_NavEq = new Tm_Ship_NavEq();

                CopyProperty.CopyProperties(model, Tm_Ship_NavEq, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tm_Ship_NavEq.Id = Id;
                Tm_Ship_NavEq.CreatedDate = dtModified;
                Tm_Ship_NavEq.CreatedUser = UserId;
                Tm_Ship_NavEq.ModifiedDate = dtModified;
                Tm_Ship_NavEq.ModifiedUser = UserId;

                CONTEXT.Tm_Ship_NavEq.Add(Tm_Ship_NavEq);
                CONTEXT.SaveChanges();
                DetId = Tm_Ship_NavEq.DetId;

            }

            return DetId;

        }
        public void NavEq_Update(HANA_APP CONTEXT, Ship_NavEqModel model, int UserId)
        {
            if (model != null)
            {

                Tm_Ship_NavEq Tm_Ship_NavEq = CONTEXT.Tm_Ship_NavEq.Find(model.DetId);

                if (Tm_Ship_NavEq != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tm_Ship_NavEq, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tm_Ship_NavEq.ModifiedDate = dtModified;
                    Tm_Ship_NavEq.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void NavEq_Delete(HANA_APP CONTEXT, Ship_NavEqModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Ship_NavEq\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
        public List<Ship_SafeEqModel> Ship_SafeEqs(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Ship_SafeEqs(CONTEXT, id);
            }

        }
        public List<Ship_SafeEqModel> Ship_SafeEqs(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Ship_SafeEqModel>("SELECT * FROM \"Tm_Ship_SafeEq\" WHERE \"Id\" =:p0", id).ToList();
        }
        public long SafeEq_Add(HANA_APP CONTEXT, Ship_SafeEqModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tm_Ship_SafeEq Tm_Ship_SafeEq = new Tm_Ship_SafeEq();

                CopyProperty.CopyProperties(model, Tm_Ship_SafeEq, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tm_Ship_SafeEq.Id = Id;
                Tm_Ship_SafeEq.CreatedDate = dtModified;
                Tm_Ship_SafeEq.CreatedUser = UserId;
                Tm_Ship_SafeEq.ModifiedDate = dtModified;
                Tm_Ship_SafeEq.ModifiedUser = UserId;

                CONTEXT.Tm_Ship_SafeEq.Add(Tm_Ship_SafeEq);
                CONTEXT.SaveChanges();
                DetId = Tm_Ship_SafeEq.DetId;

            }

            return DetId;

        }
        public void SafeEq_Update(HANA_APP CONTEXT, Ship_SafeEqModel model, int UserId)
        {
            if (model != null)
            {

                Tm_Ship_SafeEq Tm_Ship_SafeEq = CONTEXT.Tm_Ship_SafeEq.Find(model.DetId);

                if (Tm_Ship_SafeEq != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tm_Ship_SafeEq, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tm_Ship_SafeEq.ModifiedDate = dtModified;
                    Tm_Ship_SafeEq.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void SafeEq_Delete(HANA_APP CONTEXT, Ship_SafeEqModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Ship_SafeEq\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
        public List<Ship_AccoModel> Ship_Accos(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Ship_Accos(CONTEXT, id);
            }

        }
        public List<Ship_AccoModel> Ship_Accos(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Ship_AccoModel>("SELECT * FROM \"Tm_Ship_Acco\" WHERE \"Id\" =:p0", id).ToList();
        }
        public long Acco_Add(HANA_APP CONTEXT, Ship_AccoModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tm_Ship_Acco Tm_Ship_Acco = new Tm_Ship_Acco();

                CopyProperty.CopyProperties(model, Tm_Ship_Acco, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tm_Ship_Acco.Id = Id;
                Tm_Ship_Acco.CreatedDate = dtModified;
                Tm_Ship_Acco.CreatedUser = UserId;
                Tm_Ship_Acco.ModifiedDate = dtModified;
                Tm_Ship_Acco.ModifiedUser = UserId;

                CONTEXT.Tm_Ship_Acco.Add(Tm_Ship_Acco);
                CONTEXT.SaveChanges();
                DetId = Tm_Ship_Acco.DetId;

            }

            return DetId;

        }
        public void Acco_Update(HANA_APP CONTEXT, Ship_AccoModel model, int UserId)
        {
            if (model != null)
            {

                Tm_Ship_Acco Tm_Ship_Acco = CONTEXT.Tm_Ship_Acco.Find(model.DetId);

                if (Tm_Ship_Acco != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tm_Ship_Acco, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tm_Ship_Acco.ModifiedDate = dtModified;
                    Tm_Ship_Acco.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Acco_Delete(HANA_APP CONTEXT, Ship_AccoModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Ship_Acco\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
        public List<Ship_AnchorModel> Ship_Anchors(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Ship_Anchors(CONTEXT, id);
            }

        }
        public List<Ship_AnchorModel> Ship_Anchors(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Ship_AnchorModel>("SELECT * FROM \"Tm_Ship_Anchor\" WHERE \"Id\" =:p0", id).ToList();
        }
        public long Anchor_Add(HANA_APP CONTEXT, Ship_AnchorModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tm_Ship_Anchor Tm_Ship_Anchor = new Tm_Ship_Anchor();

                CopyProperty.CopyProperties(model, Tm_Ship_Anchor, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tm_Ship_Anchor.Id = Id;
                Tm_Ship_Anchor.CreatedDate = dtModified;
                Tm_Ship_Anchor.CreatedUser = UserId;
                Tm_Ship_Anchor.ModifiedDate = dtModified;
                Tm_Ship_Anchor.ModifiedUser = UserId;

                CONTEXT.Tm_Ship_Anchor.Add(Tm_Ship_Anchor);
                CONTEXT.SaveChanges();
                DetId = Tm_Ship_Anchor.DetId;

            }

            return DetId;

        }
        public void Anchor_Update(HANA_APP CONTEXT, Ship_AnchorModel model, int UserId)
        {
            if (model != null)
            {

                Tm_Ship_Anchor Tm_Ship_Anchor = CONTEXT.Tm_Ship_Anchor.Find(model.DetId);

                if (Tm_Ship_Anchor != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tm_Ship_Anchor, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tm_Ship_Anchor.ModifiedDate = dtModified;
                    Tm_Ship_Anchor.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Anchor_Delete(HANA_APP CONTEXT, Ship_AnchorModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Ship_Anchor\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
        public List<Ship_OperationModel> Ship_Operations(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Ship_Operations(CONTEXT, id);
            }

        }
        public List<Ship_OperationModel> Ship_Operations(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Ship_OperationModel>("SELECT * FROM \"Tm_Ship_Operation\" WHERE \"Id\" =:p0", id).ToList();
        }
        public long Operation_Add(HANA_APP CONTEXT, Ship_OperationModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tm_Ship_Operation Tm_Ship_Operation = new Tm_Ship_Operation();

                CopyProperty.CopyProperties(model, Tm_Ship_Operation, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tm_Ship_Operation.Id = Id;
                Tm_Ship_Operation.CreatedDate = dtModified;
                Tm_Ship_Operation.CreatedUser = UserId;
                Tm_Ship_Operation.ModifiedDate = dtModified;
                Tm_Ship_Operation.ModifiedUser = UserId;

                CONTEXT.Tm_Ship_Operation.Add(Tm_Ship_Operation);
                CONTEXT.SaveChanges();
                DetId = Tm_Ship_Operation.DetId;

            }

            return DetId;

        }
        public void Operation_Update(HANA_APP CONTEXT, Ship_OperationModel model, int UserId)
        {
            if (model != null)
            {

                Tm_Ship_Operation Tm_Ship_Operation = CONTEXT.Tm_Ship_Operation.Find(model.DetId);

                if (Tm_Ship_Operation != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tm_Ship_Operation, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tm_Ship_Operation.ModifiedDate = dtModified;
                    Tm_Ship_Operation.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Operation_Delete(HANA_APP CONTEXT, Ship_OperationModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Ship_Operation\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
        public List<Ship_AttachmentModel> Ship_Attachments(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Ship_Attachments(CONTEXT, id);
            }

        }
        public List<Ship_AttachmentModel> Ship_Attachments(HANA_APP CONTEXT, long id = 0)
        {

            return CONTEXT.Database.SqlQuery<Ship_AttachmentModel>("SELECT T0.\"Id\", T0.\"DetId\", T0.\"FileName\" FROM \"Tm_Ship_Attachment\" T0 WHERE T0.\"Id\"=:p0 ORDER BY T0.\"DetId\" ", id).ToList();


        }
        public long Detail_Add(HANA_APP CONTEXT, Ship_AttachmentModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tm_Ship_Attachment tx_Ship_Attachment = new Tm_Ship_Attachment();

                CopyProperty.CopyProperties(model, tx_Ship_Attachment, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                tx_Ship_Attachment.Id = Id;
                tx_Ship_Attachment.CreatedDate = dtModified;
                tx_Ship_Attachment.CreatedUser = UserId;
                tx_Ship_Attachment.ModifiedDate = dtModified;
                tx_Ship_Attachment.ModifiedUser = UserId;

                CONTEXT.Tm_Ship_Attachment.Add(tx_Ship_Attachment);
                CONTEXT.SaveChanges();
                DetId = tx_Ship_Attachment.DetId;

            }

            return DetId;

        }
        public long Detail_Add(List<Ship_AttachmentModel> ListModel)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Detail_Add(CONTEXT, ListModel);
            }

        }
        public long Detail_Add(HANA_APP CONTEXT, List<Ship_AttachmentModel> ListModel)
        {
            long Id = 0;
            long DetId = 0;

            if (ListModel != null)
            {

                for (int i = 0; i < ListModel.Count; i++)
                {
                    Tm_Ship_Attachment tx_Ship_Attachment = new Tm_Ship_Attachment();
                    var model = ListModel[i];

                    CopyProperty.CopyProperties(model, tx_Ship_Attachment, false);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    tx_Ship_Attachment.Id = model.Id;
                    tx_Ship_Attachment.CreatedDate = dtModified;
                    tx_Ship_Attachment.CreatedUser = model._UserId;
                    tx_Ship_Attachment.ModifiedDate = dtModified;
                    tx_Ship_Attachment.ModifiedUser = model._UserId;

                    CONTEXT.Tm_Ship_Attachment.Add(tx_Ship_Attachment);
                    CONTEXT.SaveChanges();
                    DetId = tx_Ship_Attachment.DetId;
                }



            }

            return Id;

        }
        public void Detail_Delete(Ship_AttachmentModel model)
        {
            if (model.DetId != null)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    Detail_Delete(CONTEXT, model);
                }
            }

        }
        public void Detail_Delete(HANA_APP CONTEXT, Ship_AttachmentModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tm_Ship_Attachment\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
        public bool ChooseItem(int UserId, long Id, string[] data)
        {


            return true;

        }
    }
    #endregion
}
