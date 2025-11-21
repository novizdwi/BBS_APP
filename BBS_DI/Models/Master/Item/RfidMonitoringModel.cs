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


namespace Models.Master.Item.RfidMonitoring
{

    #region Models

    public class RfidMonitoringModel
    {
        public int UserId { get; set; }

        public string ItemCode { get; set; }

        public string WhsCode { get; set; }

        public string TagId { get; set; }

        public string Status { get; set; }

        public List<RfidMonitoring_ReferenceModel> ListReferences_ = new List<RfidMonitoring_ReferenceModel>();
    }


    public class RfidMonitoring_ReferenceModel
    {

        public int Id { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public string TagId { get; set; }

        public string Status { get; set; }

        public DateTime? CreatedDate { get; set; }
    }


    #endregion

    #region Services

    public class RfidMonitoringService
    {

        public RfidMonitoringModel GetNewModel(int userId)
        {
            RfidMonitoringModel model = new RfidMonitoringModel();
            model.UserId = -1;
            model.ItemCode = null;
            model.WhsCode = null;
            model.TagId = null;
            model.Status = null;

            model.ListReferences_ = RfidMonitoring_GetReferences(userId, null, null, null, null);

            return model;
        }


        //-------------------------------------
        //Detail  RfidMonitoring_Reference
        //-------------------------------------
        public RfidMonitoringModel GetListByParam(int userId, string itemCode, string whsCode, string tagId, string status)
        {
            RfidMonitoringModel model = new RfidMonitoringModel();
            model.UserId = userId;
            model.ItemCode = itemCode;
            model.WhsCode = whsCode;
            model.TagId = tagId;
            model.Status = status;

            model.ListReferences_ = this.RfidMonitoring_GetReferences(userId, itemCode, whsCode, tagId, status);

            return model;
        }

        //-------------------------------------
        //Detail  RfidMonitoring_Reference
        //-------------------------------------
        public List<RfidMonitoring_ReferenceModel> RfidMonitoring_GetReferences(int userId, string itemCode, string whsCode, string tagId, string status)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return RfidMonitoring_GetReferences(CONTEXT, userId, itemCode, whsCode, tagId, status);
            }
        }


        public List<RfidMonitoring_ReferenceModel> RfidMonitoring_GetReferences(HANA_APP CONTEXT, int? userId = -1, string itemCode = "", string whsCode = "", string tagId = "", string status = "")
        {
            string ssql = @"CALL ""SpRfidMonitoring_GetReferences"" (:p0, :p1, :p2, :p3, :p4) ";
            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return CONTEXT.Database.SqlQuery<RfidMonitoring_ReferenceModel>(ssql, userId, itemCode, whsCode, tagId, status).ToList();
        }

    }

    #endregion

}