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

using DevExpress.Web.Mvc;
using DevExpress.Web.ASPxGridView;

namespace Models.Transaction.SppHistoryOli
{
    #region Models

    public class SppHistoryOliModel
    {
        public string TransNo { get; set; }

        public string DriverName { get; set; }

        public string Unit { get; set; }

        public DateTime TransDate { get; set; }

        public string TransType { get; set; }

        public string ShippingType { get; set; }

        public List<SppHistoryOli_TransModel> ListTrans_ = new List<SppHistoryOli_TransModel>();

    }

    public class SppHistoryOli_TransModel
    {
        public long? Id { get; set; }

        public string TransType { get; set; }  //SO, AdjustmentOut

        public string TransNo { get; set; }

        public string SoNo { get; set; }

        public DateTime? TransDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string Owner { get; set; }

        public string SppOwner { get; set; }

        public string CardCode { get; set; }

        public string CardName { get; set; }

        public string Destination { get; set; }

        //public string SoType { get; set; }

        public string ShippingType { get; set; }

        public string ItemName { get; set; }

        public string DriverName { get; set; } //

        public string Unit { get; set; }

        //public string RemarksInternal { get; set; }

        public string JenisSo { get; set; }

        public string Transportir { get; set; }

        public string Warehouse { get; set; }

        public string IsNewRoute { get; set; }

        public string IsNewUnit { get; set; }

        public string Remarks { get; set; }

        public string Status { get; set; }

        public decimal? QtySpp { get; set; }

        public decimal? OpenQuantity { get; set; }

        public decimal? QtyLo { get; set; }

        public string OldSppRouteId { get; set; }

        public string OldSppRouteNo { get; set; }

        public string OldSppUnitId { get; set; }

        public string OldSppUnitNo { get; set; }
    }


    #endregion

    #region Services

    public class SppHistoryOliService
    {
        public SppHistoryOliModel GetNewModel(int userId)
        {
            SppHistoryOliModel model = new SppHistoryOliModel();
            model.ListTrans_ = this.SppHistoryOli_Transes();

            return model;
        }

        public SppHistoryOliModel GetListByParam(string TransNo, string DriverName, string Unit)
        {
            SppHistoryOliModel model = new SppHistoryOliModel();
            model.TransNo = TransNo;
            model.DriverName = DriverName;
            model.Unit = Unit;
            model.ListTrans_ = this.SppHistoryOli_Transes(TransNo, DriverName, Unit);

            return model;
        }

        //-------------------------------------
        //Detail  SppHistoryOli_TransModel
        //-------------------------------------
        public List<SppHistoryOli_TransModel> SppHistoryOli_Transes(string TransNo = "", string DriverName = "", string Unit = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return SppHistoryOli_Transes(CONTEXT, TransNo, DriverName, Unit);
            }
        }



        public List<SppHistoryOli_TransModel> SppHistoryOli_Transes(HANA_APP CONTEXT, string TransNo = "", string DriverName = "", string Unit = "")
        {
            string ssql = @"CALL ""SpSppHistoryOli_GetTranss"" (:p0, :p1, :p2) ";
            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return CONTEXT.Database.SqlQuery<SppHistoryOli_TransModel>(ssql, TransNo, DriverName, Unit).ToList();
        }


    }




    #endregion

}