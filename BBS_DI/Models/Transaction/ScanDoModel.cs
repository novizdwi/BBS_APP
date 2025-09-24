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


namespace Models.Transaction.ScanDo
{

    #region Models

    public class ScanDoModel
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

        public string DeliveryNo { get; set; }


    }


    #endregion

    #region Services

    public class ScanDoService
    {

        public long Add(ScanDoModel model)
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
                            tx_Receipt.SuratJalanNo = model.DeliveryNo;


                            string transNo = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (" + model._UserId.ToString() + ",'Receipt','" + DateTime.Now.ToString("yyyy-MM-dd") + "') ").SingleOrDefault();
                            tx_Receipt.TransNo = transNo;

                            CONTEXT.Tx_Receipt.Add(tx_Receipt);
                            CONTEXT.SaveChanges();
                            Id = tx_Receipt.Id;

                            String keyValue;
                            keyValue = tx_Receipt.Id.ToString();

                            //SpNotif.SpSysControllerTransNotif(model._UserId, "Receipt", CONTEXT, "after", "Tx_ScanDo", "add", "Id", keyValue);
                            SpNotif.SpSysTransNotif(model._UserId, CONTEXT, "after", "Tx_ScanDo", "add", "Id", keyValue);

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

        public ScanDoModel GetNewModel(int userId)
        {
            ScanDoModel model = new ScanDoModel();
            return model;
        }

        public ScanDoModel GetById(int userId, long id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetById(CONTEXT, userId, id);
            }
        }

        public ScanDoModel GetById(HANA_APP CONTEXT, int userId, long id = 0)
        {
            ScanDoModel model = null;
            if (id != 0)
            {

                //string ssql = "CALL \"SpScanDo_GetDeliveryNo\"(:p0,:p1) "; 

                //ssql = string.Format(ssql, DbProvider.dbSap_Name);
                //model = CONTEXT.Database.SqlQuery<ScanDoModel>(ssql, userId, id).Single();
            }

            return model;
        }

    }


    #endregion

}