INSERT INTO "Ts_Menu" VALUES ('Master', 'Master', NULL, NULL, 10);
INSERT INTO "Ts_Menu" VALUES ('Item', 'Item', NULL, 'Master', 1001);
INSERT INTO "Ts_Menu" VALUES ('Transaction', 'Transaction', NULL, NULL, 20);
INSERT INTO "Ts_Menu" VALUES ('Web', 'Web', NULL, 'Transaction', 2001);
INSERT INTO "Ts_Menu" VALUES ('Inventory', 'Inventory', NULL, 'Web', 200101);

INSERT INTO "Ts_Menu" VALUES ('TransferRequest', 'Transfer Request', 'TransferRequest', 'Inventory', 20010101);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Detail#User', 'Detail - User', 'TransferRequest/Detail', 'TransferRequest', 2001010101);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Detail#All', 'Detail - All', 'TransferRequest/Detail', 'TransferRequest', 2001010102);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Add', 'Add', 'TransferRequest/Add', 'TransferRequest', 2001010103);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Update', 'Update', 'TransferRequest/Update', 'TransferRequest', 2001010104);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Post', 'Post', 'TransferRequest/Post', 'TransferRequest', 2001010105);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Cancel', 'Cancel', 'TransferRequest/Cancel', 'TransferRequest', 2001010106);

INSERT INTO "Ts_Menu" VALUES ('TransferIn', 'Transfer In', 'TransferIn', 'Inventory', 20010102);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Detail#User', 'Detail - User', 'TransferIn/Detail', 'TransferIn', 2001010201);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Detail#All', 'Detail - All', 'TransferIn/Detail', 'TransferIn', 2001010202);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Add', 'Add', 'TransferIn/Add', 'TransferIn', 2001010203);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Update', 'Update', 'TransferIn/Update', 'TransferIn', 2001010204);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Post', 'Post', 'TransferIn/Post', 'TransferIn', 2001010205);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Cancel', 'Cancel', 'TransferIn/Cancel', 'TransferIn', 2001010206);

INSERT INTO "Ts_Menu" VALUES ('TransferOut', 'Transfer Out', 'TransferOut', 'Inventory', 20010103);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Detail#User', 'Detail - User', 'TransferOut/Detail', 'TransferOut', 2001010301);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Detail#All', 'Detail - All', 'TransferOut/Detail', 'TransferOut', 2001010302);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Add', 'Add', 'TransferOut/Add', 'TransferOut', 2001010303);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Update', 'Update', 'TransferOut/Update', 'TransferOut', 2001010304);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Post', 'Post', 'TransferOut/Post', 'TransferOut', 2001010305);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Cancel', 'Cancel', 'TransferOut/Cancel', 'TransferOut', 2001010306);

INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname', 'Stock Opname', 'RequestStockOpname', 'Inventory', 20010104);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Detail#User', 'Detail - User', 'RequestStockOpname/Detail', 'RequestStockOpname', 2001010401);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Detail#All', 'Detail - All', 'RequestStockOpname/Detail', 'RequestStockOpname', 2001010402);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Add', 'Add', 'RequestStockOpname/Add', 'RequestStockOpname', 2001010403);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Update', 'Update', 'RequestStockOpname/Update', 'RequestStockOpname', 2001010404);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Post', 'Post', 'RequestStockOpname/Post', 'RequestStockOpname', 2001010405);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Cancel', 'Cancel', 'RequestStockOpname/Cancel', 'RequestStockOpname', 2001010406);

INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan', 'Stock Opname Scan List', 'StockOpnameScan', 'Inventory', 20010105);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Detail#User', 'Detail - User', 'StockOpnameScan/Detail', 'StockOpnameScan', 2001010501);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Detail#All', 'Detail - All', 'StockOpnameScan/Detail', 'StockOpnameScan', 2001010502);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Add', 'Add', 'StockOpnameScan/Add', 'StockOpnameScan', 2001010503);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Update', 'Update', 'StockOpnameScan/Update', 'StockOpnameScan', 2001010504);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Post', 'Post', 'StockOpnameScan/Post', 'StockOpnameScan', 2001010505);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Cancel', 'Cancel', 'StockOpnameScan/Cancel', 'StockOpnameScan', 2001010506);

INSERT INTO "Ts_Menu" VALUES ('StockOpnameSummary', 'Stock Opname Summary', 'StockOpnameSummary', 'Inventory', 20010106);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameSummary/Detail#User', 'Detail - User', 'StockOpnameSummary/Detail', 'StockOpnameSummary', 2001010601);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameSummary/Detail#All', 'Detail - All', 'StockOpnameSummary/Detail', 'StockOpnameSummary', 2001010602);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameSummary/Add', 'Add', 'StockOpnameSummary/Add', 'StockOpnameSummary', 2001010603);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameSummary/Update', 'Update', 'StockOpnameSummary/Update', 'StockOpnameSummary', 2001010604);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameSummary/Post', 'Post', 'StockOpnameSummary/Post', 'StockOpnameSummary', 2001010605);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameSummary/Cancel', 'Cancel', 'StockOpnameSummary/Cancel', 'StockOpnameSummary', 2001010606);

INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn', 'Transfer Summary In', 'TransferSummaryIn', 'Inventory', 20010105);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Detail#User', 'Detail - User', 'TransferSummaryIn/Detail', 'TransferSummaryIn', 2001010501);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Detail#All', 'Detail - All', 'TransferSummaryIn/Detail', 'TransferSummaryIn', 2001010502);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Add', 'Add', 'TransferSummaryIn/Add', 'TransferSummaryIn', 2001010503);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Update', 'Update', 'TransferSummaryIn/Update', 'TransferSummaryIn', 2001010504);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Post', 'Post', 'TransferSummaryIn/Post', 'TransferSummaryIn', 2001010505);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Cancel', 'Cancel', 'TransferSummaryIn/Cancel', 'TransferSummaryIn', 2001010506);

INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut', 'Transfer Summary Out', 'TransferSummaryOut', 'Inventory', 20010106);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Detail#User', 'Detail - User', 'TransferSummaryOut/Detail', 'TransferSummaryOut', 2001010601);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Detail#All', 'Detail - All', 'TransferSummaryOut/Detail', 'TransferSummaryOut', 2001010602);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Add', 'Add', 'TransferSummaryOut/Add', 'TransferSummaryOut', 2001010603);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Update', 'Update', 'TransferSummaryOut/Update', 'TransferSummaryOut', 2001010604);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Post', 'Post', 'TransferSummaryOut/Post', 'TransferSummaryOut', 2001010605);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Cancel', 'Cancel', 'TransferSummaryOut/Cancel', 'TransferSummaryOut', 2001010606);

INSERT INTO "Ts_Menu" VALUES ('Purchasing', 'Purchasing', NULL, 'Web', 200102);
INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan', 'PO Scan List', 'PurchaseOrderScan', 'Purchasing', 20010202);
INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan/Detail#User', 'Detail - User', 'PurchaseOrderScan/Detail', 'PurchaseOrderScan', 2001020201);
INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan/Detail#All', 'Detail - All', 'PurchaseOrderScan/Detail', 'PurchaseOrderScan', 2001020202);
INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan/Add', 'Add', 'PurchaseOrderScan/Add', 'PurchaseOrderScan', 2001020203);
INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan/Post', 'Post', 'PurchaseOrderScan/Post', 'PurchaseOrderScan', 2001020205);
INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan/Cancel', 'Cancel', 'PurchaseOrderScan/Cancel', 'PurchaseOrderScan', 2001020206);

INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO', 'Goods Receipt PO', 'GoodsReceiptPO', 'Purchasing', 20010203);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Detail#User', 'Detail - User', 'GoodsReceiptPO/Detail', 'GoodsReceiptPO', 2001020301);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Detail#All', 'Detail - All', 'GoodsReceiptPO/Detail', 'GoodsReceiptPO', 2001020302);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Add', 'Add', 'GoodsReceiptPO/Add', 'GoodsReceiptPO', 2001020303);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Post', 'Post', 'GoodsReceiptPO/Post', 'GoodsReceiptPO', 2001020305);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Cancel', 'Cancel', 'GoodsReceiptPO/Cancel', 'GoodsReceiptPO', 2001020306);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/RefreshItem', 'RefreshItem', 'GoodsReceiptPO/RefreshItem', 'GoodsReceiptPO', 2001020307);

INSERT INTO "Ts_Menu" VALUES ('Mobile', 'Mobile', NULL, 'Transaction', 2002);
INSERT INTO "Ts_Menu" VALUES ('Reports', 'Report', NULL, NULL, 30);
INSERT INTO "Ts_Menu" VALUES ('ReportCustom', 'Custom Report', NULL, 'Reports', 3003);
INSERT INTO "Ts_Menu" VALUES ('ReportCustom/Detail', 'Detail', 'ReportCustom/Detail', 'ReportCustom', 300301);

INSERT INTO "Ts_Menu" VALUES ('Setting', 'Setting', NULL, NULL, 40);
INSERT INTO "Ts_Menu" VALUES ('Setting-Report', 'Report', NULL, 'Setting', 4002);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup', 'Report Group', NULL, 'Setting-Report', 400202);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup/Detail', 'Detail', 'Report/Detail', 'ReportGroup', 40020201);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup/Add', 'Add', 'Report/Add', 'ReportGroup', 40020202);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup/Update', 'Update', 'Report/Update', 'ReportGroup', 40020203);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup/Delete', 'Delete', 'Report/Delete', 'ReportGroup', 40020204);

INSERT INTO "Ts_Menu" VALUES ('Report', 'Report', NULL, 'Setting-Report', 400203);
INSERT INTO "Ts_Menu" VALUES ('Report/Detail', 'Detail', 'Report/Detail', 'Report', 40020301);
INSERT INTO "Ts_Menu" VALUES ('Report/Add', 'Add', 'Report/Add', 'Report', 40020302);
INSERT INTO "Ts_Menu" VALUES ('Report/Update', 'Update', 'Report/Update', 'Report', 40020303);
INSERT INTO "Ts_Menu" VALUES ('Report/Delete', 'Delete', 'Report/Delete', 'Report', 40020304);

INSERT INTO "Ts_Menu" VALUES ('Setting-Approval', 'Approval', NULL, 'Setting', 4004);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStage', 'Approval Stage', NULL, 'Setting-Approval', 400401);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStage/Detail', 'Detail', 'ApprovalStage/Detail', 'ApprovalStage', 40040101);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStage/Add', 'Add', 'ApprovalStage/Add', 'ApprovalStage', 40040102);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStage/Update', 'Update', 'ApprovalStage/Update', 'ApprovalStage', 40040103);
INSERT INTO "Ts_Menu" VALUES ('ApprovalTemplate', 'Approval Template', NULL, 'Setting-Approval', 400402);
INSERT INTO "Ts_Menu" VALUES ('ApprovalTemplate/Detail', 'Detail', 'ApprovalTemplate/Detail', 'ApprovalTemplate', 40040201);
INSERT INTO "Ts_Menu" VALUES ('ApprovalTemplate/Add', 'Add', 'ApprovalTemplate/Add', 'ApprovalTemplate', 40040202);
INSERT INTO "Ts_Menu" VALUES ('ApprovalTemplate/Update', 'Update', 'ApprovalTemplate/Update', 'ApprovalTemplate', 40040203);

INSERT INTO "Ts_Menu" VALUES ('Authentication', 'Authentication', NULL, NULL, 50);
INSERT INTO "Ts_Menu" VALUES ('Role', 'Role', NULL, 'Authentication', 5001);
INSERT INTO "Ts_Menu" VALUES ('Role/Detail', 'Detail', 'Role/Detail', 'Role', 500101);
INSERT INTO "Ts_Menu" VALUES ('Role/Add', 'Add', 'Role/Add', 'Role', 500102);
INSERT INTO "Ts_Menu" VALUES ('Role/Update', 'Update', 'Role/Update', 'Role', 500103);
INSERT INTO "Ts_Menu" VALUES ('Role/Delete', 'Delete', 'Role/Delete', 'Role', 500104);
INSERT INTO "Ts_Menu" VALUES ('User', 'User', NULL, 'Authentication', 5002);
INSERT INTO "Ts_Menu" VALUES ('User/Detail', 'Detail', 'User/Detail', 'User', 500201);
INSERT INTO "Ts_Menu" VALUES ('User/Add', 'Add', 'User/Add', 'User', 500202);
INSERT INTO "Ts_Menu" VALUES ('User/Update', 'Update', 'User/Update', 'User', 500203);
INSERT INTO "Ts_Menu" VALUES ('User/Delete', 'Delete', 'User/Delete', 'User', 500204);
INSERT INTO "Ts_Menu" VALUES ('ChangePassword', 'Change Password', NULL, 'Authentication', 5003);
INSERT INTO "Ts_Menu" VALUES ('ChangePassword/Change', 'Change Password - Change', 'ChangePassword/Detail', 'ChangePassword', 500301);
INSERT INTO "Ts_Menu" VALUES ('ChangePassword/Detail', 'Change Password - Detail', 'ChangePassword/Change', 'ChangePassword', 500302);

INSERT INTO "Ts_Menu" VALUES ('Approval', 'Approval', NULL, NULL, 60);
INSERT INTO "Ts_Menu" VALUES ('ApprovalDecision', 'Approval Decision', NULL, 'Approval', 6001);
INSERT INTO "Ts_Menu" VALUES ('ApprovalDecision/Detail', 'Detail', 'ApprovalDecision/Detail', 'ApprovalDecision', 600101);
INSERT INTO "Ts_Menu" VALUES ('ApprovalDecision/Update', 'Update', 'ApprovalDecision/Update', 'ApprovalDecision', 600102);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStatus', 'Approval Status', NULL, 'Approval', 6002);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStatus/Detail#All', 'Detail - All', 'ApprovalStatus/Detail', 'ApprovalStatus', 600201);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStatus/Detail#User', 'Detail - User', 'ApprovalStatus/Detail', 'ApprovalStatus', 600202);
