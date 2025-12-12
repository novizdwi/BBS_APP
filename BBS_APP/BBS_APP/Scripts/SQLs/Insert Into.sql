INSERT INTO "Ts_FormatNumbering" VALUES('AdjustmentIn', 'Adjustment In', 'ADIN-','YYMM',4);
INSERT INTO "Ts_FormatNumbering" VALUES('AdjustmentOut', 'Adjustment Out', 'ADOU-','YYMM',4);

INSERT INTO "Ts_List" VALUES('RFIDStatus', '01', 'A', 'Active', '');
INSERT INTO "Ts_List" VALUES('RFIDStatus', '02', 'I', 'Inactive', '');
INSERT INTO "Ts_List" VALUES('RFIDStatus', '03', 'P', 'Pending', '');

INSERT INTO "Ts_List" VALUES('ItemTagStatus', '01', 'Active', 'Active', '');
INSERT INTO "Ts_List" VALUES('ItemTagStatus', '02', 'Inactive', 'Inactive', '');
INSERT INTO "Ts_List" VALUES('ItemTagStatus', '03', 'Pending', 'Pending', '');

INSERT INTO "Ts_List" VALUES('RFIDTransType', '01', 'TransferSummaryOut', 'Transfer Summary Out', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '02', 'GoodsReceiptPO', 'GoodsReceipt PO', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '03', 'TransferSummaryIn', 'Transfer Summary In', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '04', 'StockSummaryOpname', 'Stock Summary Opname', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '05', 'AdjustmentIn', 'Adjustment In', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '06', 'AdjustmentOut', 'Adjustment Out', '');

INSERT INTO "Ts_List" VALUES('RFIDTransType', '07', 'DeactiveTags', 'Deactive Tags', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '08', 'ReactiveTags', 'Reactive Tags', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '09', 'ReplaceTags', 'Replace Tags', ''); 