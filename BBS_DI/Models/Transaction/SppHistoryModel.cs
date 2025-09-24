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

namespace Models.Transaction.SppHistory
{
    #region Models

    public class SppHistoryModel
    {
        public string TransNo { get; set; }

        public string DriverName { get; set; }

        public string Unit { get; set; }

        public DateTime TransDate { get; set; }

        public string TransType { get; set; }

        public string ShippingType { get; set; }

        public List<SppHistory_TransModel> ListTrans_ = new List<SppHistory_TransModel>();

    }

    public class SppHistory_TransModel
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

    public class SppHistoryService
    {
        public SppHistoryModel GetNewModel(int userId)
        {
            SppHistoryModel model = new SppHistoryModel();
            model.ListTrans_ = this.SppHistory_Transes();

            return model;
        }

        public SppHistoryModel GetListByParam(string TransNo, string DriverName, string Unit)
        {
            SppHistoryModel model = new SppHistoryModel();
            model.TransNo = TransNo;
            model.DriverName = DriverName;
            model.Unit = Unit;
            model.ListTrans_ = this.SppHistory_Transes(TransNo, DriverName, Unit);

            return model;
        }

        //-------------------------------------
        //Detail  SppHistory_TransModel
        //-------------------------------------
        public List<SppHistory_TransModel> SppHistory_Transes(string TransNo = "", string DriverName = "", string Unit = "")
        {
            using (var CONTEXT = new HANA_APP())
            {
                return SppHistory_Transes(CONTEXT, TransNo, DriverName, Unit);
            }
        }



        public List<SppHistory_TransModel> SppHistory_Transes(HANA_APP CONTEXT, string TransNo = "", string DriverName = "", string Unit = "")
        {
            string ssql = @"CALL ""SpSppHistory_GetTranss"" (:p0, :p1, :p2) ";
            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return CONTEXT.Database.SqlQuery<SppHistory_TransModel>(ssql, TransNo, DriverName, Unit).ToList();
        }


    }




    #endregion

}