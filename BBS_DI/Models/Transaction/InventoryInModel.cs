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


namespace Models.Transaction.InventoryIn
{
    #region Models

    public class InventoryInModel
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

        public int? Series { get; set; }

        public string SeriesName { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime? TransDate { get; set; }
        
        public string ShipCode { get; set; }

        public string Status { get; set; }

        [Required(ErrorMessage = "required")]
        public Int32? SapPoId { get; set; }

        [Required(ErrorMessage = "required")]
        public string SapPoNo { get; set; }
        
        public Int32? SapGrpId { get; set; }
        
        public string SapGrpoNo { get; set; }

        [Required(ErrorMessage = "required")]
        public string VendorCode { get; set; }

        public string VendorName { get; set; }

        public string VendorRef { get; set; }

        [Required(ErrorMessage = "required")]
        public string WarehouseCode { get; set; }

        public string WarehouseName { get; set; }

        public string ReqType { get; set; }

        public string Bagian { get; set; }

        public string Remark { get; set; }

        public string Kategori { get; set; }

        public List<InventoryIn_DetailModel> ListDetails_ = new List<InventoryIn_DetailModel>();

        public InventoryIn_Details Details_ { get; set; }
    }

    public class InventoryIn_DetailModel
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

        public string ItemCodeKey { get; set; }

        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public decimal? QtyOrder { get; set; }

        public decimal? QtyReceipt { get; set; }

        public string UoMCode { get; set; }

        public int? UomEntry { get; set; }

        public int? ShipId { get; set; }

        public string ShipCode { get; set; }

        public string ShipName { get; set; }

        public int? MRId { get; set; }

        public string MRNum { get; set; }

        public string Status { get; set; }

        public string Batch { get; set; }

        public string BatchName { get; set; }

        public string Remark { get; set; }

        public int? SapPoId { get; set; }

        public string SapPoNo { get; set; }

        public string SapPoLineNum { get; set; }

        public int? SapGrpoId { get; set; }

        public string SapGrpoNo { get; set; }

        public string VANum { get; set; }

        public string ManBtchNum { get; set; }

        public Decimal? QtyOpen { get; set; }

        public List<InventoryInBatchDetailModel> ListBatchDetails_ = new List<InventoryInBatchDetailModel>();

        public InventoryInBatchDetails BatchDetails_ { get; set; }
    }

    public class InventoryIn_Details
    {
        public List<int> deletedRowKeys { get; set; }
        public List<InventoryIn_DetailModel> insertedRowValues { get; set; }
        public List<InventoryIn_DetailModel> modifiedRowValues { get; set; }
    }

    public class InventoryInBatchDetailModel
    {

        private FormModeEnum _FormModeEnum = FormModeEnum.New;

        public FormModeEnum _FormMode
        {
            get { return this._FormModeEnum; }
            set { this._FormModeEnum = value; }
        }

        public int _UserId { get; set; }

        public long Id { get; set; }

        public long? DetId { get; set; }
        public long? DetDetId { get; set; }
	    public string ItemCode { get; set; }
	    public int? BaseLine { get; set; }
	    public string BatchNum { get; set; }
	    public decimal? Quantity { get; set; }
        public DateTime? ExpDate { get; set; }
        public string Status { get; set; }
    }

    public class InventoryInBatchDetails
    {
        public List<int> deletedRowKeys { get; set; }
        public List<InventoryInBatchDetailModel> insertedRowValues { get; set; }
        public List<InventoryInBatchDetailModel> modifiedRowValues { get; set; }
    }

    #endregion

    #region Services

    public class InventoryInService
    {
        public InventoryInModel GetNewModel(int userId)
        {
            InventoryInModel model = new InventoryInModel();
            model.Status = "Draft";
            var CONTEXT = new HANA_APP();
            DateTime dateX = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
            string dateV = dateX.ToString("yyMM");
            string ssqlSeries = @"SELECT T0.""Series"" FROM ""{DbSap}"".NNM1 T0 WHERE T0.""ObjectCode"" = '20' AND RIGHT(T0.""SeriesName"",4) ='" + dateV + "' ";
            ssqlSeries = ssqlSeries.Replace("{DbSap}", DbProvider.dbSap_Name);
            model.Series = CONTEXT.Database.SqlQuery<int>(ssqlSeries).FirstOrDefault();
            return model;
        }
        public InventoryInModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }
        public InventoryInModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            InventoryInModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT T0.* 
                            FROM ""Tx_InventoryIn"" T0   
                            WHERE T0.""Id""=:p0 ";

                model = CONTEXT.Database.SqlQuery<InventoryInModel>(ssql, id).Single();

                model.ListDetails_ = this.InventoryIn_Details(CONTEXT, id);
            }

            return model;
        }
        public List<InventoryIn_DetailModel> InventoryIn_Details(long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return InventoryIn_Details(CONTEXT, id);
            }
        }
        public List<InventoryIn_DetailModel> InventoryIn_Details(HANA_APP CONTEXT, long id = 0)
        {
            return CONTEXT.Database.SqlQuery<InventoryIn_DetailModel>("SELECT T0.*, CAST(T0.\"DetId\" AS NVARCHAR(10)) AS \"ItemCodeKey\" FROM \"Tx_InventoryIn_Detail\" T0 WHERE T0.\"Id\"=:p0  ", id).ToList();
        }
        public InventoryInModel NavFirst(int userId)
        {
            InventoryInModel model = null;


            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "InventoryIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_InventoryIn\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;

        }
        public InventoryInModel NavPrevious(int userId, long id = 0)
        {
            InventoryInModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "InventoryIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_InventoryIn\" T0 WHERE T0.\"Id\"<:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC", id).FirstOrDefault();
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
        public InventoryInModel NavNext(int userId, long id = 0)
        {
            InventoryInModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "InventoryIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_InventoryIn\" T0 WHERE T0.\"Id\">:p0 " + sqlCriteria + " ORDER BY T0.\"Id\" ASC", id).FirstOrDefault();
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
        public InventoryInModel NavLast(int userId)
        {
            InventoryInModel model = null;

            using (var CONTEXT = new HANA_APP())
            {
                string sqlCriteria = "";
                var formAuthorizeSqlWhere = GeneralGetList.GetFormTransAuthorizeSqlWhere(CONTEXT, userId, "InventoryIn");
                if (!string.IsNullOrEmpty(formAuthorizeSqlWhere))
                {
                    sqlCriteria = " AND " + formAuthorizeSqlWhere;
                }

                long? Id = CONTEXT.Database.SqlQuery<long?>("SELECT TOP 1 T0.\"Id\" FROM \"Tx_InventoryIn\" T0 WHERE 1=1 " + sqlCriteria + " ORDER BY T0.\"Id\" DESC").FirstOrDefault();

                model = this.GetById(CONTEXT, userId, Id.HasValue ? Id.Value : 0);
            }

            return model;
        }
        //single PO
        public InventoryInModel GetPoByIdS(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetPoByIdS(CONTEXT, userId, id);
            }
        }
        public InventoryInModel GetPoByIdS(HANA_APP CONTEXT, int userId, long id = 0)
        {
            
            InventoryInModel model = null;
            if (id != 0)
            {
                string ssql = @"SELECT TOP 1 T0.""DocEntry"" AS ""SapPoId"",
                                        T0.""DocNum"" AS ""SapPoNo"",
                                        T0.""CardCode"" AS ""VendorCode"",
                                        T0.""CardName"" AS ""VendorName"",
                                        T0.""ToWhsCode"" AS ""WhsCode"",
                                        T0.""NumAtCard"" AS ""VendorRef"",
                                        T0.""U_IDU_WebRemark"" AS ""Remark"",
                                        T1.""Project"" AS ""ShipCode"",
                                        T0.""U_IDU_RequestType"" AS ""ReqType"",
                                        T0.""U_IDU_Bagian"" AS ""Bagian"",
                                        T0.""U_IDU_KATEGORIINV"" AS ""Kategori""
                            FROM ""{DbSap}"".""OPOR"" T0
                            LEFT JOIN ""{DbSap}"".""POR1"" T1 ON T0.""DocEntry"" = T1.""DocEntry"" 
                            WHERE T0.""DocEntry"" =:p0 ";
                ssql = ssql.Replace("{DbSap}", DbProvider.dbSap_Name);
                model = CONTEXT.Database.SqlQuery<InventoryInModel>(ssql, id.ToString()).Single();
                DateTime dateX = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                string dateV = dateX.ToString("yyMM");
                string ssqlSeries = @"SELECT T0.""Series"" FROM ""{DbSap}"".NNM1 T0 WHERE T0.""ObjectCode"" = '20' AND RIGHT(T0.""SeriesName"",4) ='" + dateV+"' ";
                ssqlSeries = ssqlSeries.Replace("{DbSap}", DbProvider.dbSap_Name);
                model.Series = CONTEXT.Database.SqlQuery<int>(ssqlSeries).FirstOrDefault();
                model.Status = "Draft";
                model.TransDate = dateX;
                model.ListDetails_ = this.InventoryInPoS_Details(CONTEXT, id);
            }

            return model;
        }
        public List<InventoryIn_DetailModel> InventoryInPoS_Details(long Id)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return InventoryInPoS_Details(CONTEXT, Id);
            }
        }
        public List<InventoryIn_DetailModel> InventoryInPoS_Details(HANA_APP CONTEXT,long Id)
        {

            string ssql = @"SELECT T0.""DocEntry"" AS ""SapPoId"",
                                   CONCAT(T0.""DocEntry"",T0.""LineNum"")  AS ""DetId"",
                                    T0.""LineNum"" AS ""SapPoLineNum"",
                                    T1.""DocNum"" AS ""SapPoNo"",
                                    T0.""ItemCode"" AS ""ItemCode"",
                                    T0.""Dscription"" AS ""ItemDescription"",
                                    T1.""DocDate"" AS ""TransDate"",
                                    T0.""Quantity"" AS ""QtyOrder"",
                                    T0.""Project"" AS ""ShipCode"",
                                    T0.""FreeTxt"" AS ""Remark"",
                                    T2.""TransNo"" AS ""VANum"",
                                    T2.""BaseTransNo"" AS ""MRNum"",
                                    T0.""UomEntry"" AS ""UomEntry"",
                                    T0.""UomCode"" AS ""UoMCode"",
                                    T2.""ShipId"" AS ""ShipId"",
                                    T2.""ShipName"" AS ""ShipName"",
                                    T4.""ManBtchNum"" AS ""ManBtchNum"",
                                    T0.""OpenQty"" AS ""QtyOpen"",
                                    T0.""U_IDU_Batch"" AS ""Batch"",
                                    T0.""U_IDU_BatchName"" AS ""BatchName""
                            FROM ""{DbSap}"".""POR1"" T0
                            LEFT JOIN ""{DbSap}"".""OPOR"" T1 ON T0.""DocEntry"" = T1.""DocEntry"" 
                            LEFT JOIN ""{DbApp}"".""Tx_ApprovalMR"" T2 ON T1.""U_IDU_WebTransId"" = T2.""Id""
                            LEFT JOIN ""{DbApp}"".""Tx_MaterialRequest"" T3 ON T2.""BaseTransId"" = T3.""Id""
                            LEFT JOIN ""{DbSap}"".""OITM"" T4 ON T0.""ItemCode"" = T4.""ItemCode""
                            WHERE T0.""DocEntry"" =:p0 AND T0.""OpenQty"" !=0";
            ssql = ssql.Replace("{DbSap}", DbProvider.dbSap_Name);
            ssql = ssql.Replace("{DbApp}", DbProvider.dbApp_Name);
            //ssql = ssql.Replace("{p0}", Id.ToString());
            return CONTEXT.Database.SqlQuery<InventoryIn_DetailModel>(ssql, Id).ToList();
        }

        //multiple PO
        public InventoryInModel GetPoByIdM(int userId, int[] data, long Id)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetPoByIdM(CONTEXT, userId, data, Id);
            }
        }
        public InventoryInModel GetPoByIdM(HANA_APP CONTEXT, int userId, int[] data, long Id)
        {
            String keyValue;
            keyValue = Id.ToString();

            //SpNotif.SpSysControllerTransNotif(UserId, "InventoryIn", CONTEXT, "before", "InventoryIn", "choosePurchaseOrder", "Id", keyValue);

            string sqlWhere;
            if (data == null)
            {
                sqlWhere = "";
            }
            else if (data.Length == 0)
            {
                sqlWhere = "";
            }
            else
            {
                var data1 = new string[data.Length];

                for (var i = 0; i < data.Length; i++)
                {
                    data1[i] = data[i].ToString();
                }

                sqlWhere = string.Join(",", data1);
            }
            InventoryInModel model = null;
            if (data != null)
            {
                string ssql = @"SELECT T0.""DocEntry"" AS ""SapPoId"",
                                        T0.""DocNum"" AS ""SapPoNo"",
                                        T0.""CardCode"" AS ""VendorCode"",
                                        T0.""CardName"" AS ""VendorName"",
                                        T0.""ToWhsCode"" AS ""WhsCode"",
                                        T0.""NumAtCard"" AS ""VendorRef"",
                                        T0.""U_IDU_KATEGORIINV"" AS ""Kategori""
                            FROM ""{DbSap}"".""OPOR"" T0   
                            WHERE T0.""DocEntry"" IN({p0}) ";
                ssql = ssql.Replace("{DbSap}", DbProvider.dbSap_Name);
                ssql = ssql.Replace("{p0}", sqlWhere);
                model = CONTEXT.Database.SqlQuery<InventoryInModel>(ssql).Single();
                DateTime dateX = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                string dateV = dateX.ToString("yyMM");
                string ssqlSeries = @"SELECT T0.""Series"" FROM ""{DbSap}"".NNM1 T0 WHERE T0.""ObjectCode"" = 20 AND RIGHT(T0.""SeriesName"",4) ='" + dateV + "' ";
                ssqlSeries = ssqlSeries.Replace("{DbSap}", DbProvider.dbSap_Name);
                model.Series = CONTEXT.Database.SqlQuery<int>(ssqlSeries).FirstOrDefault();
                model.Status = "Draft";
                model.TransDate = dateX;
                model.ListDetails_ = this.InventoryInPo_Details(CONTEXT, sqlWhere, Id);
            }

            return model;
        }
        public List<InventoryIn_DetailModel> InventoryInPo_Details(string data, long Id)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return InventoryInPo_Details(CONTEXT, data, Id);
            }
        }
        public List<InventoryIn_DetailModel> InventoryInPo_Details(HANA_APP CONTEXT, string data, long Id)
        {
            
            string ssql = @"SELECT T0.""DocEntry"" AS ""SapPoId"",
                                   CONCAT(T0.""DocEntry"",T0.""LineNum"")  AS ""DetId"",
                                    T0.""LineNum"" AS ""SapPoLineNum"",
                                    T1.""DocNum"" AS ""SapPoNo"",
                                    T0.""ItemCode"" AS ""ItemCode"",
                                    T0.""Dscription"" AS ""ItemDescription"",
                                    T1.""DocDate"" AS ""TransDate"",
                                    T0.""Quantity"" AS ""QtyOrder"",
                                    T0.""Project"" AS ""ShipCode"",
                                    T0.""Project"" AS ""ShipName"",
                                    T0.""FreeTxt"" AS ""Remark"",
                                    T2.""TransNo"" AS ""VANum"",
                                    T2.""BaseTransNo"" AS ""MRNum"",
                                    T0.""UomEntry"" AS ""UomEntry"",
                                    T0.""UomCode"" AS ""UoMCode"",
                                    T2.""ShipId"" AS ""ShipId"",
                                    T2.""ShipName"" AS ""ShipName"",
                                    T4.""ManBtchNum"" AS ""ManBtchNum""
                            FROM ""{DbSap}"".""POR1"" T0
                            LEFT JOIN ""{DbSap}"".""OPOR"" T1 ON T0.""DocEntry"" = T1.""DocEntry"" 
                            LEFT JOIN ""{DbApp}"".""Tx_ApprovalMR"" T2 ON T1.""U_IDU_WebTransId"" = T2.""Id""
                            LEFT JOIN ""{DbApp}"".""Tx_MaterialRequest"" T3 ON T2.""BaseTransId"" = T3.""Id""
                            LEFT JOIN ""{DbSap}"".""OITM"" T4 ON T0.""ItemCode"" = T4.""ItemCode""
                            WHERE T0.""DocEntry"" IN({p0}) ";
            ssql = ssql.Replace("{DbSap}", DbProvider.dbSap_Name);
            ssql = ssql.Replace("{DbApp}", DbProvider.dbApp_Name);
            ssql = ssql.Replace("{p0}", data.ToString());
            return CONTEXT.Database.SqlQuery<InventoryIn_DetailModel>(ssql).ToList();
        }
        public long Add(InventoryInModel model)
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

                            Tx_InventoryIn Tx_InventoryIn = new Tx_InventoryIn();
                            CopyProperty.CopyProperties(model, Tx_InventoryIn, false);

                            DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                            Tx_InventoryIn.TransType = "InventoryIn";
                            Tx_InventoryIn.CreatedDate = dtModified;
                            Tx_InventoryIn.CreatedUser = model._UserId;
                            Tx_InventoryIn.ModifiedDate = dtModified;
                            Tx_InventoryIn.ModifiedUser = model._UserId;
                            
                            string dateX = model.TransDate.Value.ToString("yyyy-MM-dd");
                            string dateV = model.TransDate.Value.ToString("YY/MM");
                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'InventoryIn','" + dateX + "','') ").SingleOrDefault();
                            Tx_InventoryIn.TransNo = transNo;
                            
                            CONTEXT.Tx_InventoryIn.Add(Tx_InventoryIn);
                            CONTEXT.SaveChanges();
                            Id = Tx_InventoryIn.Id;

                            String keyValue;
                            keyValue = Tx_InventoryIn.Id.ToString();
                            if (model.Details_ != null)
                            {

                                if (model.Details_.modifiedRowValues != null)
                                {
                                    foreach (var detail in model.Details_.modifiedRowValues)
                                    {
                                        Detail_Add(CONTEXT, detail, Id, model._UserId);
                                    }
                                }


                            }



                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_InventoryIn", "add", "Id", keyValue);
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
        public void Update(InventoryInModel model)
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


                                Tx_InventoryIn Tx_InventoryIn = CONTEXT.Tx_InventoryIn.Find(model.Id);
                                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                                Tx_InventoryIn.ModifiedDate = dtModified;
                                Tx_InventoryIn.ModifiedUser = model._UserId;
                                if (Tx_InventoryIn != null)
                                {
                                    var exceptColumns = new string[] { "Id", "TransNo", "CreatedUser" };
                                    CopyProperty.CopyProperties(model, Tx_InventoryIn, false, exceptColumns);
                                    Tx_InventoryIn.ModifiedDate = dtModified;
                                    Tx_InventoryIn.ModifiedUser = model._UserId;
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
                                                InventoryIn_DetailModel detailModel = new InventoryIn_DetailModel();
                                                detailModel.DetId = detId;
                                                Detail_Delete(CONTEXT, detailModel);
                                            }
                                        }
                                    }


                                    SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", " Tx_InventoryIn", "update", "Id", keyValue);

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
        public long Detail_Add(HANA_APP CONTEXT, InventoryIn_DetailModel model, long Id, int UserId)
        {
            long DetId = 0;

            if (model != null)
            {

                Tx_InventoryIn_Detail Tx_InventoryIn_Detail = new Tx_InventoryIn_Detail();

                CopyProperty.CopyProperties(model, Tx_InventoryIn_Detail, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_InventoryIn_Detail.Id = Id;
                Tx_InventoryIn_Detail.CreatedDate = dtModified;
                Tx_InventoryIn_Detail.CreatedUser = UserId;
                Tx_InventoryIn_Detail.ModifiedDate = dtModified;
                Tx_InventoryIn_Detail.ModifiedUser = UserId;

                CONTEXT.Tx_InventoryIn_Detail.Add(Tx_InventoryIn_Detail);
                CONTEXT.SaveChanges();
                DetId = Tx_InventoryIn_Detail.DetId;

            }

            return DetId;

        }
        public void Detail_Update(HANA_APP CONTEXT, InventoryIn_DetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_InventoryIn_Detail Tx_InventoryIn_Detail = CONTEXT.Tx_InventoryIn_Detail.Find(model.DetId);

                if (Tx_InventoryIn_Detail != null)
                {
                    var exceptColumns = new string[] { "DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_InventoryIn_Detail, false, exceptColumns);


                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();

                    Tx_InventoryIn_Detail.ModifiedDate = dtModified;
                    Tx_InventoryIn_Detail.ModifiedUser = UserId;

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void Detail_Delete(HANA_APP CONTEXT, InventoryIn_DetailModel model)
        {
            if (model.DetId != null)
            {
                if (model.DetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_InventoryIn_Detail\"  WHERE \"DetId\"=:p0", model.DetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
        public void Post(int userId, long Id)
        {

            String keyValue;
            keyValue = Id.ToString();

            SAPbobsCOM.Company oCompany = SAPCachedCompany.GetCompany();
            try
            {
                oCompany.StartTransaction();
                SpNotif.SpSysControllerTransNotif(userId, "InventoryIn", oCompany, "before", "InventoryIn", "post", "Id", keyValue);

                AddGRPO(oCompany, userId, Id);

                var sql1 = "UPDATE T0 SET   "
                        + " T0.\"Status\"='Posted',"
                        + " T0.\"IsAfterPosted\"='Y',"
                        + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                        + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP, "
                        + " T0.\"SapGrpoNo\"= T1.\"DocNum\" "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_InventoryIn\" T0 "
                        + " LEFT JOIN \"OPDN\" T1 ON T0.\"SapGrpoId\" = T1.\"DocEntry\" "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);

                SpNotif.SpSysControllerTransNotif(userId, "InventoryIn", oCompany, "after", "InventoryIn", "post", "Id", keyValue);

                oCompany.EndTransaction(BoWfTransOpt.wf_Commit);

            }

            catch (Exception ex)
            {

                if (oCompany.InTransaction)
                {
                    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                }

                throw ex;
            }
            finally
            {
                SAPCachedCompany.Release(oCompany);
            }

        }
        public void Cancel(int userId, long Id)
        {
            String keyValue;
            keyValue = Id.ToString();


            SAPbobsCOM.Company oCompany = SAPCachedCompany.GetCompany();

            try
            {
                oCompany.StartTransaction();

                SpNotif.SpSysControllerTransNotif(userId, "InventoryIn", oCompany, "before", "InventoryIn", "cancel", "Id", keyValue);

                CancelGRPO(oCompany, userId, Id);

                string sql1 = "UPDATE T0 SET   "
                       + " T0.\"Status\"='Cancel',"
                       + " T0.\"ModifiedUser\"=" + userId.ToString() + ","
                       + " T0.\"ModifiedDate\"=CURRENT_TIMESTAMP "
                       + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_InventoryIn\" T0 "
                       + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sql1);

                SpNotif.SpSysControllerTransNotif(userId, "InventoryIn", oCompany, "after", "InventoryIn", "cancel", "Id", keyValue);

                oCompany.EndTransaction(BoWfTransOpt.wf_Commit);

            }

            catch (Exception ex)
            {

                if (oCompany.InTransaction)
                {
                    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                }

                throw ex;
            }
            finally
            {
                SAPCachedCompany.Release(oCompany);
            }

        }
        public bool AddGRPO(SAPbobsCOM.Company oCompany, int userId, long Id)
        {
            int nErr;
            string errMsg;

            string ssql = "SELECT TOP 1 T0.\"DocEntry\" FROM \"OPDN\" T0 WHERE T0.\"U_IDU_WebTransId\"='{0}' AND T0.\"U_IDU_WebTransType\"='{1}'";
            ssql = string.Format(ssql, Id, "InventoryIn");
            string tempId = _Utils.SapCompany.RetRstField(oCompany, ssql);
            if (!string.IsNullOrEmpty(tempId))
            {
                return false;
            }

            //ADD GRPO
            SAPbobsCOM.Recordset rsGrpo = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsGrpo = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpInventoryIn_SapAddGrpo\" ('" + Id.ToString() + "')");

            if (!rsGrpo.EoF)
            {
                SAPbobsCOM.Documents oGoodReceiptPO = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);
                if (rsGrpo.Fields.Item("Hdr_Series").Value.ToString() != "")
                {
                    oGoodReceiptPO.Series = int.Parse(rsGrpo.Fields.Item("Hdr_Series").Value.ToString());
                }
                oGoodReceiptPO.CardCode = rsGrpo.Fields.Item("Hdr_CardCode").Value.ToString();
                oGoodReceiptPO.NumAtCard = rsGrpo.Fields.Item("Hdr_RefNo").Value.ToString();
                string ControlAccount = rsGrpo.Fields.Item("Hdr_ControlAccount").Value.ToString();
                if (!string.IsNullOrEmpty(ControlAccount))
                {
                    ControlAccount = ControlAccount.Replace("-", "");
                    oGoodReceiptPO.ControlAccount = SapCompany.RetCoaCode(oCompany, ControlAccount);
                }

                oGoodReceiptPO.DocDate = (DateTime)rsGrpo.Fields.Item("Hdr_DocDate").Value;
                oGoodReceiptPO.DocDueDate = (DateTime)rsGrpo.Fields.Item("Hdr_DocDueDate").Value;

                oGoodReceiptPO.DocCurrency = rsGrpo.Fields.Item("Hdr_DocCurrency").Value.ToString();
                oGoodReceiptPO.DocRate = double.Parse(rsGrpo.Fields.Item("Hdr_DocRate").Value.ToString());

                if (rsGrpo.Fields.Item("Hdr_PaymentGroupCode").Value.ToString() != "")
                {
                    oGoodReceiptPO.PaymentGroupCode = int.Parse(rsGrpo.Fields.Item("Hdr_PaymentGroupCode").Value.ToString());
                }

                if (rsGrpo.Fields.Item("Hdr_DocType").Value.ToString() == "I")
                {
                    oGoodReceiptPO.DocType = BoDocumentTypes.dDocument_Items;
                }
                else
                {
                    oGoodReceiptPO.DocType = BoDocumentTypes.dDocument_Service;
                }

                oGoodReceiptPO.UserFields.Fields.Item("U_IDU_WebTransType").Value = rsGrpo.Fields.Item("Hdr_U_IDU_WebTransType").Value.ToString();
                oGoodReceiptPO.UserFields.Fields.Item("U_IDU_WebTransNo").Value = rsGrpo.Fields.Item("Hdr_U_IDU_WebTransNo").Value.ToString();
                oGoodReceiptPO.UserFields.Fields.Item("U_IDU_WebTransId").Value = rsGrpo.Fields.Item("Hdr_U_IDU_WebTransId").Value.ToString();
                oGoodReceiptPO.UserFields.Fields.Item("U_IDU_WebUserId").Value = userId.ToString();
                oGoodReceiptPO.UserFields.Fields.Item("U_IDU_WebTransDate").Value = (DateTime)rsGrpo.Fields.Item("Hdr_DocDueDate").Value;
                oGoodReceiptPO.UserFields.Fields.Item("U_IDU_WebRemark").Value = rsGrpo.Fields.Item("Hdr_Remark").Value.ToString();

                
                SAPbobsCOM.Recordset rsGrpoFreight = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                rsGrpoFreight = _Utils.SapCompany.GetRs(oCompany, "CALL \"" + DbProvider.dbApp_Name + "\".\"SpInventoryIn_SapAddGrpo_Freight\" ('" + Id.ToString() + "')");
                int frItem = 0;
                while (!rsGrpoFreight.EoF)
                {
                    if (rsGrpoFreight.Fields.Item("ObjType").Value.ToString() != "")
                    {
                        oGoodReceiptPO.Expenses.BaseDocEntry = int.Parse(rsGrpoFreight.Fields.Item("DocEntry").Value.ToString());
                        oGoodReceiptPO.Expenses.BaseDocLine = int.Parse(rsGrpoFreight.Fields.Item("LineNum").Value.ToString());
                        oGoodReceiptPO.Expenses.BaseDocType = int.Parse(rsGrpoFreight.Fields.Item("ObjType").Value.ToString());
                        oGoodReceiptPO.Expenses.Add();
                        frItem += 1;
                        rsGrpoFreight.MoveNext();
                    }
                }
                
                
                int lineItem = 0;
                while (!rsGrpo.EoF)
                {
                    if (rsGrpo.Fields.Item("Hdr_DocType").Value.ToString() == "I")
                    {
                        
                        oGoodReceiptPO.Lines.BaseType = 22;
                        if (rsGrpo.Fields.Item("Hdr_U_IDU_KATEGORIINV").Value.ToString() == "Docking")
                        {
                            oGoodReceiptPO.Lines.AccountCode = "122.1";
                        }else if (rsGrpo.Fields.Item("Hdr_U_IDU_KATEGORIINV").Value.ToString() == "Kapal Baru")
                        {
                            oGoodReceiptPO.Lines.AccountCode = "122.2";
                        }
                        else if (rsGrpo.Fields.Item("Hdr_U_IDU_KATEGORIINV").Value.ToString() == "Inventory")
                        {
                            oGoodReceiptPO.Lines.AccountCode = "122.3";
                        }
                        oGoodReceiptPO.Lines.BaseEntry = Int32.Parse(rsGrpo.Fields.Item("Det_BaseEntry").Value.ToString());
                        oGoodReceiptPO.Lines.BaseLine = Int32.Parse(rsGrpo.Fields.Item("Det_BaseLine").Value.ToString());
                        oGoodReceiptPO.Lines.UserFields.Fields.Item("U_IDU_WebDetId").Value = rsGrpo.Fields.Item("Det_DetId").Value.ToString();
                        oGoodReceiptPO.Lines.Quantity = double.Parse(rsGrpo.Fields.Item("Det_Quantity").Value.ToString());
                        if (rsGrpo.Fields.Item("Det_WarehouseCode").Value.ToString() != "")
                        {
                            oGoodReceiptPO.Lines.WarehouseCode = rsGrpo.Fields.Item("Det_WarehouseCode").Value.ToString();
                        }

                       
                    }
                    
                    if (rsGrpo.Fields.Item("Det_ManBtchNum").Value.ToString() == "Y")
                    {

                        
                        oGoodReceiptPO.Lines.BatchNumbers.BatchNumber = rsGrpo.Fields.Item("Det_Batch").Value.ToString();
                        oGoodReceiptPO.Lines.BatchNumbers.Quantity = double.Parse(rsGrpo.Fields.Item("Det_Quantity").Value.ToString());
                        oGoodReceiptPO.Lines.BatchNumbers.Add();
                        
                    }
                    oGoodReceiptPO.Lines.Add();
                    lineItem += 1;
                    rsGrpo.MoveNext();

                }

                if (oGoodReceiptPO.Add() != 0)
                {

                    nErr = oCompany.GetLastErrorCode();
                    errMsg = oCompany.GetLastErrorDescription();
                    throw new Exception("[VALIDATION] - Add Good Receipt | " + nErr.ToString() + "|" + errMsg);

                }

                string docEntry;
                docEntry = oCompany.GetNewObjectKey();

                string sqlUpdateSO;
                sqlUpdateSO = "UPDATE T0 SET   "
                        + " T0.\"SapGrpoId\"=" + docEntry + " "
                        + " FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_InventoryIn\" T0 "
                        + " WHERE T0.\"Id\"=" + Id.ToString();

                SapCompany.ExecuteQuery(oCompany, sqlUpdateSO);
            }

            //END ADD DELIVERY

            //throw new Exception("[VALIDATION] - Lagi test jangan di save dulu");

            return true;
        }
        
        public bool CancelGRPO(SAPbobsCOM.Company oCompany, int userId, long Id)
        {
            int nErr;
            string errMsg;

            SAPbobsCOM.Recordset rsGrpo = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rsGrpo = _Utils.SapCompany.GetRs(oCompany, "SELECT * FROM \"" + DbProvider.dbApp_Name + "\".\"Tx_InventoryIn\" T0 WHERE T0.\"Id\" ='" + Id.ToString() + "' AND IFNULL(T0.\"SapGrpoId\",0)>0 ");

            if (!rsGrpo.EoF)
            {
                SAPbobsCOM.Documents oGrpo = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);

                oGrpo.GetByKey(int.Parse(rsGrpo.Fields.Item("SapGrpoId").Value.ToString()));

                SAPbobsCOM.Documents oCancelDoc = oGrpo.CreateCancellationDocument();
              
                oCancelDoc.DocDate = (DateTime)rsGrpo.Fields.Item("TransDate").Value;

                if (oCancelDoc.Add() != 0)
                {
                    nErr = oCompany.GetLastErrorCode();
                    errMsg = oCompany.GetLastErrorDescription();
                    throw new Exception("[VALIDATION] - Cancel GRPO - " + nErr.ToString() + "|" + errMsg);
                }

            }

            return true;
        }


    }
    public class InventoryInBatchDetailService
    {
        //public InventoryInBatchDetailModel GetNewModel(int userId)
        //{
        //    InventoryInBatchDetailModel model = new InventoryInBatchDetailModel();

        //    return model;
        //}
        //public InventoryIn_DetailModel GetByIdNewBatchDetail(long id = 0)
        //{
        //    InventoryIn_DetailModel model = new InventoryIn_DetailModel();
        //    model.Id = id;
        //    model.DetId = id;
        //    return model;
        //}
        public InventoryIn_DetailModel GetById(int userId, long detId = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, detId);
            }
        }
        public InventoryIn_DetailModel GetById(HANA_APP CONTEXT, int userId, long detId = 0)
        {
            InventoryIn_DetailModel model = null;
            if (detId != 0)
            {
                
                string ssql = "SELECT T0.*, T1.\"Status\" FROM \"Tx_InventoryIn_Detail\" T0 LEFT JOIN \"Tx_InventoryIn\" T1 ON T0.\"Id\" = T1.\"Id\" WHERE T0.\"DetId\"=:p0";
                model = CONTEXT.Database.SqlQuery<InventoryIn_DetailModel>(ssql, detId).Single();

                if (model != null)
                {
                    model.ListBatchDetails_ = this.InventoryInBatchDetailModel(detId);
                }
            }
            return model;
        }
        public List<InventoryInBatchDetailModel> InventoryInBatchDetailModel(long detId = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                
                return InventoryInBatchDetails(CONTEXT, detId);
            }
        }
        public List<InventoryInBatchDetailModel> InventoryInBatchDetails(HANA_APP CONTEXT, long detId = 0)
        {
            return CONTEXT.Database.SqlQuery<InventoryInBatchDetailModel>("SELECT T0.*, T1.\"Status\" FROM \"Tx_InventoryIn_Detail_Batch\" T0 LEFT JOIN \"Tx_InventoryIn\" T1 ON T0.\"Id\" = T1.\"Id\" WHERE T0.\"DetId\"=:p0  ", detId).ToList();
        }
        public long Add(InventoryIn_DetailModel model)
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
                            
                            Tx_InventoryIn_Detail Tx_InventoryIn_Detail = CONTEXT.Tx_InventoryIn_Detail.Find(model.DetId);
                           
                                
                            if (Tx_InventoryIn_Detail != null)
                            {

                                if (model.BatchDetails_ != null)
                                {
                                    if (model.BatchDetails_.insertedRowValues != null)
                                    {
                                        foreach (var detail in model.BatchDetails_.insertedRowValues)
                                        {
                                            BatchDetail_Add(CONTEXT, detail, model.Id, model.DetId, model._UserId);
                                        }
                                    }

                                    if (model.BatchDetails_.modifiedRowValues != null)
                                    {
                                        foreach (var detail in model.BatchDetails_.modifiedRowValues)
                                        {
                                            BatchDetail_Update(CONTEXT, detail, model._UserId);
                                        }
                                    }

                                    if (model.BatchDetails_.deletedRowKeys != null)
                                    {
                                        foreach (var detDetId in model.BatchDetails_.deletedRowKeys)
                                        {
                                            InventoryInBatchDetailModel detailBatchModel = new InventoryInBatchDetailModel();
                                            detailBatchModel.DetDetId = detDetId;
                                            BatchDetail_Delete(CONTEXT, detailBatchModel);
                                        }
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
            return model.DetId;

        }
        public void Delete(int detid = 0)
        {
            if (detid != 0)
            {
                using (var CONTEXT = new HANA_APP())
                {

                    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
                    {
                        try
                        {
                            string keyValue = detid.ToString();
                            CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_InventoryIn_Detail_Batch\" WHERE \"DetId\"=:p0 ", detid);
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

        public long BatchDetail_Add(HANA_APP CONTEXT, InventoryInBatchDetailModel model, long Id, long DetId, int UserId)
        {
            long DetDetId = 0;

            if (model != null)
            {

                Tx_InventoryIn_Detail_Batch Tx_InventoryIn_Detail_Batch = new Tx_InventoryIn_Detail_Batch();

                CopyProperty.CopyProperties(model, Tx_InventoryIn_Detail_Batch, false);

                DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                Tx_InventoryIn_Detail_Batch.Id = Id;
                Tx_InventoryIn_Detail_Batch.DetId = DetId;
                CONTEXT.Tx_InventoryIn_Detail_Batch.Add(Tx_InventoryIn_Detail_Batch);
                CONTEXT.SaveChanges();
                DetDetId = Tx_InventoryIn_Detail_Batch.DetDetId;

            }

            return DetId;

        }
        public void BatchDetail_Update(HANA_APP CONTEXT, InventoryInBatchDetailModel model, int UserId)
        {
            if (model != null)
            {

                Tx_InventoryIn_Detail_Batch Tx_InventoryIn_Detail_Batch = CONTEXT.Tx_InventoryIn_Detail_Batch.Find(model.DetDetId);

                if (Tx_InventoryIn_Detail_Batch != null)
                {
                    var exceptColumns = new string[] { "DetDetId","DetId", "Id" };
                    CopyProperty.CopyProperties(model, Tx_InventoryIn_Detail_Batch, false, exceptColumns);
                    

                    DateTime dtModified = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
                    

                    CONTEXT.SaveChanges();

                }


            }

        }
        public void BatchDetail_Delete(HANA_APP CONTEXT, InventoryInBatchDetailModel model)
        {
            if (model.DetDetId != null)
            {
                if (model.DetDetId != 0)
                {

                    CONTEXT.Database.ExecuteSqlCommand("DELETE FROM \"Tx_InventoryIn_Detail_Batch\"  WHERE \"DetDetId\"=:p0", model.DetDetId);

                    CONTEXT.SaveChanges();


                }
            }

        }
    }

    #endregion

}