using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Transactions;
using Models._Utils;
using Models._Ef;
using BBS_DI.Models._EF;

using SAPbobsCOM;
using Models._Sap;

namespace Models.Api
{

    #region Models

    public class Delivery_Mutex
    {
        private static System.Threading.Mutex Delivery_TransactionLock = new System.Threading.Mutex();

        public static void wait()
        {
            Delivery_TransactionLock.WaitOne();
        }

        public static void release()
        {
            Delivery_TransactionLock.ReleaseMutex();
        }

    }

    public class DeliveryApiModel
    {

        public long? Id { get; set; }

        public long? DetId { get; set; }

        public string TransType { get; set; }

        public string TransNo { get; set; }

        public DateTime DocumentDate { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string Remarks { get; set; }

        public string SppOwner { get; set; }

        public long? SppId { get; set; }

        public string SyncDesc { get; set; }

        public List<DeliveryApi_ItemModel> Lines { get; set; } //= new List<Delivery_ItemModel>();

    }

    public class DeliveryApi_ItemModel
    {
        public string ItemCode { get; set; }

        public double Quantity { get; set; }

        public double Capacity { get; set; }

        public double Price { get; set; }

        public Int32 BinAbsEntry { get; set; }

        public string WarehouseCode { get; set; }

        public Int32? BaseType { get; set; }

        public Int32? BaseEntry { get; set; }

        public Int32? BaseLine { get; set; }
    }

    public class DeliveryResultModel
    {
        public string NewDocEntry { get; set; }
        public string NewDocNum { get; set; }
    }

    #endregion

    #region Services

    public class DeliveryApiServices
    {
        //APP
        public List<DeliveryApiModel> CheckLOSyncDeliveryBiayaKirim()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return CheckLOSyncDeliveryBiayaKirim(CONTEXT);
            }
        }

        //APP
        public List<DeliveryApiModel> CheckLOSyncDeliveryBiayaKirim(HANA_APP CONTEXT)
        {
            List<DeliveryApiModel> result = null;
            string ssql = @"SELECT T0.""Id"", T0.""SppOwner"", T0.""SppId"", IFNULL(T1.""SyncDlvryBiayaAngkutError"",'') AS ""SyncDesc""
                        FROM ""Tx_LoadingOrder"" T0
                        WHERE  IFNULL(T0.""IsSyncDlvryBiayaAngkut"", 'N') = 'N' AND T0.""Status"" IN ('Delivered') 
                        AND IFNULL(T0.""SppId"", 0) <> 0
                        ";
            //ssql = string.Format(ssql, DbProvider.dbSap_Name);
            result = CONTEXT.Database.SqlQuery<DeliveryApiModel>(ssql).ToList();

            return result;
        }

        //APP
        public DeliveryApiModel GetByDocEntry(long? id = 0)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetByDocEntry(CONTEXT, id);
            }
        }

        //APP
        public DeliveryApiModel GetByDocEntry(HANA_APP CONTEXT, long? id = 0)
        {
            DeliveryApiModel model = null;
            if (id != 0)
            {

                string ssql = @"SELECT T0.""Id"", T1.""DetId"", T0.""TransType"", T0.""TransNo"", T0.""TransDate"" AS ""DocumentDate"", T0.""Remarks"" AS ""Remarks"",
                        T4.""CardCode"" AS ""CustomerCode""
                        FROM ""Tx_Fpfs"" T0
                        INNER JOIN ""Tx_Fpfs_SalesOrder"" T1 ON T0.""Id"" = T1.""Id""
                        INNER JOIN ""{1}"".""ORDR"" T3 ON T3.""DocEntry"" = T1.""SapSalesOrderId""
                        INNER JOIN ""{0}"".""ORDR"" T4 ON T3.""U_SODocEntry"" = T4.""DocEntry"" AND IFNULL(T4.""CANCELED"",'N') = 'N'
                        WHERE T0.""Id""=:p0 AND T1.""DetId"" =:p1 ";
                ssql = string.Format(ssql, DbProvider.dbSap_Name2, DbProvider.dbSap_Name);
                model = CONTEXT.Database.SqlQuery<DeliveryApiModel>(ssql, id).Single();
                model.Lines = this.Detail_Lines(CONTEXT, id);
            }

            return model;
        }

        //SADP_II
        public List<DeliveryApi_ItemModel> Detail_Lines(HANA_APP CONTEXT, long? id = 0)
        {
            string ssql = @"SELECT T5.""ItemCode"" AS ""ItemCode"", 
                        CASE WHEN T1.""OpenQuantitySalesOrder"" > T0.""UnitCapacity"" THEN T0.""UnitCapacity"" ELSE T1.""OpenQuantitySalesOrder"" END AS ""Quantity"",
                        T5.""Price"" AS ""Price"", 
                        T2.""WhsCode"" AS ""WarehouseCode"", T2.""AbsEntry"" AS ""BinAbsEntry"",
                        T5.""ObjType"" AS ""BaseType"",T5.""DocEntry"" AS ""BaseEntry"",T5.""LineNum"" AS ""BaseLine""
                        FROM ""Tx_Fpfs"" T0
                        INNER JOIN ""Tx_Fpfs_SalesOrder"" T1 ON T0.""Id"" = T1.""Id""
                        INNER JOIN  ""{0}"".""OBIN"" T2 ON T1.""BinAbsEntry"" = T2.""AbsEntry""
                        INNER JOIN ""{1}"".""ORDR"" T3 ON T3.""DocEntry"" = T1.""SapSalesOrderId""
                        INNER JOIN ""{0}"".""ORDR"" T4 ON T3.""U_SODocEntry"" = T4.""DocEntry"" AND IFNULL(T4.""CANCELED"",'N') = 'N'
                        INNER JOIN ""{0}"".""RDR1"" T5 ON T5.""DocEntry"" = T4.""DocEntry""
                        WHERE T0.""Id""=:p0 AND T1.""DetId""=:p1  ";
            ssql = string.Format(ssql, DbProvider.dbSap_Name2, DbProvider.dbSap_Name);
            return CONTEXT.Database.SqlQuery<DeliveryApi_ItemModel>(ssql, id).ToList();
        }

    }

    #endregion

}