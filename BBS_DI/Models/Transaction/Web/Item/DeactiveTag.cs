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
using Models;

namespace Models.Transaction.Web.Item
{
    #region Models

    public class DeactiveTagModel
    {
        public int _UserId { get; set; }

        public int Id { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string TagId { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public List<DeactiveTag_ItemModel> ListItems_ = new List<DeactiveTag_ItemModel>();

        public DeactiveTag_Items Items_ { get; set; }

        public string[] DocumentTicks_ { get; set; }
        public string Remarks { get; set; }
    }

    public class DeactiveTag_ItemModel
    {
        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string WhsCode { get; set; }

        public string WhsName { get; set; }

        public string TagId { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

    public class DeactiveTag_Items
    {
        public List<int> deletedRowKeys { get; set; }
        public List<DeactiveTag_ItemModel> insertedRowValues { get; set; }
        public List<DeactiveTag_ItemModel> modifiedRowValues { get; set; }
    }

    public class DeactiveTagRemarksModel
    {
        public string TagId { get; set; }

        public string DeactiveRemarks { get; set; }

    }

    #endregion Models

    #region Services
    public class DeactiveTagService
    {
        public DeactiveTagModel GetNewModel(int userId)
        {
            DeactiveTagModel model = new DeactiveTagModel(); 

            return model;
        }

        public List<DeactiveTag_ItemModel> Find(string TagId, string WhsCode, string WhsName, string ItemCode, string ItemName, DateTime? DateFrom, DateTime? DateTo)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return Find(TagId, WhsCode, WhsName, ItemCode, ItemName, DateFrom, DateTo);
            }
        }

        public List<DeactiveTag_ItemModel> Find(HANA_APP CONTEXT, string TagId, string WhsCode, string WhsName, string ItemCode, string ItemName, DateTime? DateFrom, DateTime? DateTo)
        {
            string ssql = @"CALL ""SpDeactiveTag_GetItems"" (:p0, :p1, :p2, :p3, :p4, :p5, :p6) ";
            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return CONTEXT.Database.SqlQuery<DeactiveTag_ItemModel>(TagId, WhsCode, WhsName, ItemCode, ItemName, DateFrom, DateTo).ToList();

        }

        public void Update(DeactiveTagModel model)
        {
            if (model.DocumentTicks_?.Length <= 0) return;

            var tx_DeactiveTag_Log = new List<Tx_DeactiveTag_Log>();
            int userId = model._UserId;

            using (var CONTEXT = new HANA_APP())
            {
                using (var trans = CONTEXT.Database.BeginTransaction())
                {
                    foreach (var tick in model.DocumentTicks_)
                    {
                        try
                        {
                            // --- Ambil item terkait ---
                            string tagId = "";
                            string itemCode = "";
                            string whsCode = "";
                            string remarks = "";

                            if (model.Items_?.modifiedRowValues != null)
                            {
                                var item = model.Items_.modifiedRowValues.FirstOrDefault(x => x.TagId.ToString() == tick);
                                if (item != null)
                                {
                                    tagId = item.TagId;
                                    itemCode = item.ItemCode;
                                    whsCode = item.WhsCode;
                                    remarks = item.Remarks;
                                }
                            }

                            // --- Validasi remarks ---
                            if (string.IsNullOrWhiteSpace(remarks))
                            {
                                remarks = model.Remarks;
                                if (string.IsNullOrWhiteSpace(remarks))
                                    throw new Exception("Remarks must not null");
                            }

                            // --- Update database ---
                            var tm_Item_Warehouse_Tag = CONTEXT.Tm_Item_Warehouse_Tag.Find(tagId);
                            if (tm_Item_Warehouse_Tag != null)
                            {
                                if (tm_Item_Warehouse_Tag.WhsCode != whsCode)
                                    throw new Exception($"WhsCode already changed, before:{whsCode}, after:{tm_Item_Warehouse_Tag.WhsCode}");
                                if (tm_Item_Warehouse_Tag.ItemCode != itemCode)
                                    throw new Exception($"ItemCode already changed, before:{itemCode}, after:{tm_Item_Warehouse_Tag.ItemCode}");

                                tm_Item_Warehouse_Tag.Status = "I";
                                tm_Item_Warehouse_Tag.ModifiedDate = DateTime.Now;
                                tm_Item_Warehouse_Tag.ModifiedUser = userId;
                            }
                        }
                        catch (Exception ex)
                        {
                            // --- Log error per tick ---
                            tx_DeactiveTag_Log.Add(new Tx_DeactiveTag_Log
                            {
                                Message = ex.Message.Length > 5000 ? ex.Message.Substring(0, 5000) : ex.Message,
                                CreatedDate = DateTime.Now,
                                CreatedUser = userId
                            });
                            continue;
                        }
                    }
                    
                    if (tx_DeactiveTag_Log.Any())
                    {
                        CONTEXT.Tx_DeactiveTag_Log.AddRange(tx_DeactiveTag_Log);
                    }
                    CONTEXT.SaveChanges();

                    trans.Commit();
                }
            }
        }


    }



    #endregion Services
}
