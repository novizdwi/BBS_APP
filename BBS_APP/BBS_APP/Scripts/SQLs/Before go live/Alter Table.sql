ALTER TABLE "Tm_Item_Warehouse_Tag_Log"
ADD (
    "NewItemCode" NVARCHAR(50) ,
    "NewItemName" NVARCHAR(200)
    )


ALTER TABLE "Tx_TransferSummaryOut_Item_Tag" ADD("ErrorMessage" NVARCHAR(200) );
ALTER TABLE "Tx_TransferSummaryIn_Item_Tag" ADD("ErrorMessage" NVARCHAR(200) );
ALTER TABLE "Tx_StockSummaryOpname_Item_Tag" ADD("ErrorMessage" NVARCHAR(200) );
ALTER TABLE "Tx_GoodsReceiptPO_Item_Tag" ADD("ErrorMessage" NVARCHAR(200) );
ALTER TABLE "Tx_AdjustmentIn_Item_Tag" ADD("ErrorMessage" NVARCHAR(200) );
ALTER TABLE "Tx_AdjustmentOut_Item_Tag" ADD("ErrorMessage" NVARCHAR(200) );
