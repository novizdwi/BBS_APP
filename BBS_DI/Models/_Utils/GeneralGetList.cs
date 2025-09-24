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


using Models._Sap;
using Models._Ef;
using BBS_DI.Models._EF;

namespace Models._Utils
{

    public static class GeneralGetList
    {
        //mendapatkan semua url : punya authorize ke form tertentu atau tidak
        public static DataTable GetMenuUrls(int userId)
        {
            string ssql = @"SELECT DISTINCT T3.""Url""
                            FROM ""Tm_User"" T0   
                            INNER JOIN ""Tm_Role"" T1   ON T0.""RoleId""=T1.""Id"" 
                            INNER JOIN ""Tm_Role_Auth"" T2   ON T1.""Id""=T2.""Id"" AND T2.""IsAccess""='Y' 
                            INNER JOIN ""Ts_Menu"" T3   ON T2.""MenuCode""=T3.""MenuCode"" AND T3.""Url"" LIKE '%/Detail' 
                            WHERE T0.""Id""=:p0";

            return EfIduHanaRsExtensionsApp.IduGetDataTable(ssql, userId);

        }

        public static string GetFormTransAuthorize(int userId, string formCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetFormTransAuthorize(CONTEXT, userId, formCode);
            }
        }

        public static string GetFormTransAuthorize(HANA_APP CONTEXT, int userId, string formCode)
        {

            int? roleId = GetValue<int?>(CONTEXT, "SELECT TOP 1 IFNULL(T0.\"RoleId\",0) AS \"RoleId\" FROM \"Tm_User\" T0   WHERE T0.\"Id\"=:p0", userId);
            string roleName = GetValue<string>(CONTEXT, "SELECT TOP 1 IFNULL(T0.\"RoleName\",'') AS \"RoleName\" FROM \"Tm_Role\" T0   WHERE T0.\"Id\"=:p0", roleId);

            if (GetIsAdmin(roleName) == "Y")
            {
                return "All";
            }

            //All Form
            if (GetValue<string>(CONTEXT, "SELECT T0.\"IsAccess\" FROM \"Tm_Role_Auth\" T0   WHERE T0.\"Id\"=:p0 AND T0.\"MenuCode\"=:p1 ", roleId, formCode + "/Detail#All") == "Y")
            {
                return "All";
            }
            //Branch
            else if (GetValue<string>(CONTEXT, "SELECT T0.\"IsAccess\" FROM \"Tm_Role_Auth\" T0   WHERE T0.\"Id\"=:p0 AND T0.\"MenuCode\"=:p1 ", roleId, formCode + "/Detail#Branch") == "Y")
            {
                return "Branch";
            }
            //User
            else if (GetValue<string>(CONTEXT, "SELECT T0.\"IsAccess\" FROM \"Tm_Role_Auth\" T0   WHERE T0.\"Id\"=:p0 AND T0.\"MenuCode\"=:p1 ", roleId, formCode + "/Detail#User") == "Y")
            {
                return "User";
            }
            else
            {
                return "";
            }
        }

        public static string GetFormTransAuthorizeSqlWhere(int userId, string formCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetFormTransAuthorizeSqlWhere(CONTEXT, userId, formCode);
            }
        }

        public static string GetFormTransAuthorizeSqlWhere(HANA_APP CONTEXT, int userId, string formCode)
        {
            string formAuthorize = GetFormTransAuthorize(CONTEXT, userId, formCode); 

            string ssql = "";

            if (formAuthorize == "All")
            {
                ssql = "";
            }
            else if (formAuthorize == "Branch")
            {
                string branch = GeneralGetList.GetAuthBranchCodeByUserId(CONTEXT, userId);

                if (!string.IsNullOrEmpty(branch))
                {
                    ssql = ssql + "  \"CreatedBranch\" IN (" + branch + ") ";
                }
                else
                {
                    ssql = ssql + "  1=0 ";
                }
            }
            else if (formAuthorize == "User")
            {
                ssql = ssql + "  \"CreatedUser\"=" + userId.ToString();
            }
            else
            {
                ssql = ssql + "  1=0 ";
            }

            return ssql;
        }

        public static string GetAuthBranchCodeByUserId(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAuthBranchCodeByUserId(CONTEXT, userId);
            }
        }

        public static string GetAuthBranchCodeByUserId(HANA_APP CONTEXT, int userId)
        {
//            string ssql = @" 
//	                        SELECT TOP 1 ''''||IFNULL(T0_.""WhsCode"",'')||'''' AS ""WhsCode"" 
//	                        FROM ""Tm_User"" T0_ 
//                            WHERE T0_.""Id""=:p0 ";

            string ssql = @"SELECT STRING_AGG(''''||T0.""BranchCode""||'''', ', ') AS IDU
                            FROM  ( 
	                            SELECT TOP 1 IFNULL(T0_.""WhsCode"",'') AS ""BranchCode"" 
	                            FROM ""Tm_User"" T0_ 
	                            WHERE T0_.""Id""=:p0
	                            UNION  
	                            SELECT IFNULL(T0_.""BranchCode"",'') AS ""BranchCode"" 
	                            FROM ""Tm_User_AuthBranch"" T0_ 
	                            WHERE T0_.""Id""=:p1 AND T0_.""IsTick""='Y'
                            ) T0";


            return GetValue<string>(CONTEXT, ssql, userId, userId);
        }

        public static string GetRegionalByUserId(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetRegionalByUserId(CONTEXT, userId);
            }
        }

        public static string GetRegionalByUserId(HANA_APP CONTEXT, int userId)
        {
            string ssql = @" 
	                        SELECT TOP 1 T0_.""Regional"" AS ""Regional"" 
	                        FROM ""Tm_User"" T0_ 
                            WHERE T0_.""Id""=:p0 ";

            return GetValue<string>(CONTEXT, ssql, userId);
        }

        public static DataTable GetDocumentType(string strType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT T0.""Id"", T0.""DocumentName""  FROM ""Tm_DocContent"" T0   WHERE T0.""Type""=:p0  ";
                return GetDataTable(CONTEXT, ssql, strType);
            }
        }

        public static DataTable GetDocumentTypePlusAll(string strType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT -1 AS ""Id"", 'ALL' AS ""DocumentName"" FROM DUMMY UNION ALL SELECT T0.""Id"", T0.""DocumentName""  FROM ""Tm_DocContent"" T0   WHERE T0.""Type""=:p0  ";
                return GetDataTable(CONTEXT, ssql, strType);
            }
        }

        public static DataTable GetDocumentBaseOnCategoryPlusAll(string strType, string strCategory)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT -1 AS ""Id"", 'ALL' AS ""DocumentName"" FROM DUMMY UNION ALL SELECT DISTINCT T0.""Id"", T0.""DocumentName""  FROM ""Tm_DocContent"" T0 INNER JOIN ""Tm_DocNonShipping"" T1 ON T1.""Type"" = T0.""Id""   WHERE T0.""Type""=:p0 AND T1.""Category"" =:p1  ";
                return GetDataTable(CONTEXT, ssql, strType, strCategory);
            }
        }



        public static DataTable GetList(string strType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetList(CONTEXT, strType);
            }
        }

        public static DataTable GetListPlusAll(string strType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                var ssql = @"SELECT 'ALL' AS ""Code"", 'ALL' AS ""Name"" FROM DUMMY UNION ALL SELECT T0.""Code"", T0.""Name""  FROM ""Ts_List"" T0   WHERE T0.""Type""=:p0 ";

                return GetDataTable(CONTEXT, ssql, strType);
            }
        }



        public static DataTable GetListPointEx(string strType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetListPointEx(CONTEXT, strType);
            }
        }

        public static DataTable GetList(HANA_APP CONTEXT, string strType)
        {
            var ssql = @"SELECT T0.""Code"", T0.""Name""  FROM ""Ts_List"" T0   WHERE T0.""Type""=:p0 ORDER BY T0.""Sort"" ASC ";

            return GetDataTable(CONTEXT, ssql, strType);
        }

        public static DataTable GetListPointEx(HANA_APP CONTEXT, string strType)
        {
            var ssql = @"SELECT T0.""Code"", T0.""Name""  FROM ""Ts_List"" T0   WHERE T0.""Type""=:p0  ";

            return GetDataTable(CONTEXT, ssql, strType);
        }

        public static DataTable GetList(string strType, string strCategory)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetList(CONTEXT, strType, strCategory);
            }
        }

        public static DataTable GetList(HANA_APP CONTEXT, string strType, string strCategory)
        {
            var ssql = @"SELECT T0.""Code"", T0.""Name""  FROM ""Ts_List"" T0   WHERE T0.""Type""=:p0  AND T0.""Category""=:p1 ORDER BY T0.""Sort"" ";

            return GetDataTable(CONTEXT, ssql, strType, strCategory);

        }

        public static string GetWarehouseCodeByUserId(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetWarehouseCodeByUserId(CONTEXT, userId);
            }
        }

        public static string GetWarehouseCodeByUserId(HANA_APP CONTEXT, int userId)
        {
            var ssql = @"SELECT T0.""WhsCode""  FROM ""Tm_User"" T0   WHERE T0.""Id""=:p0    ";

            return GetValue<string>(CONTEXT, ssql, userId);

        }

        public static string GetUserNameByUserId(int? userId)
        {
            if (userId.HasValue)
            {
                using (var CONTEXT = new HANA_APP())
                {
                    return GetUserNameByUserId(CONTEXT, userId);
                }
            }
            else 
            {
                return "";
            }
        }

        public static string GetUserNameByUserId(HANA_APP CONTEXT, int? userId)
        {
            var ssql = @"SELECT TOP 1 IFNULL(T0.""UserName"",'') AS ""UserName"" FROM ""Tm_User"" T0   WHERE T0.""Id""=:p0  ";

            return GetValue<string>(CONTEXT, ssql, userId);
        }

        public static DataTable GetPaymentTypes()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetPaymentTypes(CONTEXT);
            }
        }

        public static DataTable GetPaymentTypes(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT T0.""Code"", T0.""Name""
                            FROM ""{0}"".""@IDU_PAYMENT_TYPE"" T0 ORDER BY T0.""Name"" ASC ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }


        public static DataTable GetAreas()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAreas(CONTEXT);
            }
        }

        public static DataTable GetAreas(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT T0.""Code"", T0.""Name""
                            FROM ""{0}"".""@IDU_AREA"" T0 ORDER BY T0.""Name"" ASC ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetBPGroup(string GroupType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetBPGroup(CONTEXT, GroupType);
            }
        }

        public static DataTable GetBPGroup(HANA_APP CONTEXT, string GroupType)
        {
            string ssql = @"SELECT T0.""GroupCode"", T0.""GroupName""
                            FROM ""{0}"".""OCRG"" T0 WHERE T0.""GroupType"" = '{1}' ORDER BY T0.""GroupCode"" ASC ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name, GroupType);
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetCapacity(string strJenis)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetCapacity(CONTEXT, strJenis);
            }
        }

        public static DataTable GetCapacity(HANA_APP CONTEXT, string strJenis)
        {
            string ssql = @"SELECT TO_DECIMAL(T0.""U_Capacity"",10, 2) AS ""Capacity""
                            FROM ""{0}"".""@IDU_CAPACITY"" T0 WHERE T0.""U_Jenis"" = '{1}' ORDER BY T0.""U_Capacity"" ASC ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name, strJenis);
            return GetDataTable(CONTEXT, ssql);
        }

        public static T GetValue<T>(string ssql, params object[] parameters)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetValue<T>(CONTEXT, ssql, parameters);
            }
        }

        public static T GetValue<T>(HANA_APP CONTEXT, string ssql, params object[] parameters)
        {

            return CONTEXT.Database.SqlQuery<T>(ssql, parameters).FirstOrDefault();
        }


        //public static DataTable GetCurrencies()
        //{
        //    var ssql = PetaPoco.Sql.Builder
        //             .Append("SELECT CurrCode AS CurCode, CurrName AS CurName FROM [" + DbProvider.dbSap_Name + "].DBO.OCRN (NOLOCK)  "
        //         );
        //    return PetaPocoIduSqlRsExtensionsApp.IduGetDataTable(ssql);
        //}

        public static DataTable GetLayoutForms()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetLayoutForms(CONTEXT);
            }
        }

        public static DataTable GetLayoutForms(HANA_APP CONTEXT)
        {
            var ssql = @"SELECT T0.""LayoutFormCode"", T0.""LayoutFormName"" FROM ""Ts_LayoutForm"" T0   ORDER BY T0.""Sort"" ASC";

            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetRoles()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetRoles(CONTEXT);
            }
        }

        public static DataTable GetRoles(HANA_APP CONTEXT)
        {
            var ssql = "SELECT T0.\"Id\", T0.\"RoleName\" FROM \"Tm_Role\" T0   ORDER BY T0.\"RoleName\" ASC ";
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetEmployees()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetEmployees(CONTEXT);
            }
        }

        public static DataTable GetEmployees(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT T0.""empID"" AS ""empID"",T0.""ExtEmpNo"", T0.""firstName"" AS ""firstName"", T0.""lastName"" AS ""lastName"", T0.""middleName"" AS ""middleName"" 
                            FROM  ""{0}"".""OHEM"" T0       
                            ORDER BY T0.""firstName"", T0.""lastName"", T0.""middleName"" ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }

        //get Deck
        public static DataTable GetBagian()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetBagian(CONTEXT);
            }
        }
        public static DataTable GetBagian(HANA_APP CONTEXT)
        {
            var ssql = "SELECT T0.\"Id\", T0.\"Bagian\" FROM \"Tm_MasterSetting_Bagian\" T0   ORDER BY T0.\"Bagian\" ASC ";
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetItem()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetItem(CONTEXT);
            }
        }
        public static DataTable GetItem(HANA_APP CONTEXT)
        {
            var ssql = "SELECT T0.\"Id\", T0.\"Item\", T0.\"Bagian\" FROM \"Tm_MasterSetting_Item\" T0   ORDER BY T0.\"Item\" ASC ";
            return GetDataTable(CONTEXT, ssql);
        }
        public static DataTable GetItemsByBagian(string Bagian)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetItemsByBagian(CONTEXT, Bagian);
            }
        }
        public static DataTable GetItemsByBagian(HANA_APP CONTEXT, string Bagian)
        {
            var ssql = "SELECT T0.\"Id\", T0.\"Item\", T0.\"Bagian\" FROM \"Tm_MasterSetting_Item\" T0 WHERE T0.\"Bagian\" = "+ Bagian +"  ORDER BY T0.\"Item\" ASC ";
            return GetDataTable(CONTEXT, ssql);
        }
        public static DataTable GetSubItem()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetSubItem(CONTEXT);
            }
        }

        public static DataTable GetSubItem(HANA_APP CONTEXT)
        {
            var ssql = "SELECT T0.\"Id\", T0.\"SubItem\", T0.\"Item\", T0.\"Bagian\" FROM \"Tm_MasterSetting_SubItem\" T0   ORDER BY T0.\"SubItem\" ASC ";
            return GetDataTable(CONTEXT, ssql);
        }
        //get Engine
        public static DataTable GetBagianEngine()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetBagianEngine(CONTEXT);
            }
        }
        public static DataTable GetBagianEngine(HANA_APP CONTEXT)
        {
            var ssql = "SELECT T0.\"Id\", T0.\"Bagian\" FROM \"Tm_MasterSettingEngine_Bagian\" T0   ORDER BY T0.\"Bagian\" ASC ";
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetItemEngine()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetItemEngine(CONTEXT);
            }
        }
        public static DataTable GetItemEngine(HANA_APP CONTEXT)
        {
            var ssql = "SELECT T0.\"Id\", T0.\"Item\", T0.\"Bagian\" FROM \"Tm_MasterSettingEngine_Item\" T0   ORDER BY T0.\"Item\" ASC ";
            return GetDataTable(CONTEXT, ssql);
        }
        public static DataTable GetSubItemEngine()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetSubItemEngine(CONTEXT);
            }
        }
        public static DataTable GetSubItemEngine(HANA_APP CONTEXT)
        {
            var ssql = "SELECT T0.\"Id\", T0.\"SubItem\", T0.\"Item\", T0.\"Bagian\" FROM \"Tm_MasterSettingEngine_SubItem\" T0   ORDER BY T0.\"SubItem\" ASC ";
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetRegional()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetRegional(CONTEXT);
            }
        }



        public static DataTable GetRegional(HANA_APP CONTEXT)
        {
            //            string ssql = @"SELECT T0.""U_IDU_Regional"" AS ""Regional"", T1.""PrcName"" AS ""RegionalName""  
            //                            FROM  ""{0}"".""OWHS"" T0  
            //                            LEFT JOIN ""{0}"".""OPRC"" T1 ON T1.""U_IDU_whs"" = T0.""WhsCode"" 
            //                            AND T1.""DimCode"" = 2
            //                            ORDER BY T0.""WhsCode"" ";
            string ssql = @"SELECT T1.""PrcCode"" AS ""Regional"", T1.""PrcName"" AS ""RegionalName""  
                            FROM ""{0}"".""OPRC"" T1 
                            WHERE T1.""DimCode"" = 2 AND T1.""Locked"" = 'N'
                            ORDER BY T1.""PrcCode"" ASC ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetWarehouses()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetWarehouses(CONTEXT);
            }
        }

        public static DataTable GetWarehouses(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT T0.""WhsCode"", T0.""WhsName""  
                            FROM  ""{0}"".""OWHS"" T0       
                            ORDER BY T0.""WhsCode"" ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }
        //
        public static DataTable GetSeries(string ObjType)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetSeries(CONTEXT, ObjType);
            }
        }

        public static DataTable GetSeries(HANA_APP CONTEXT, string ObjType)
        {
            string ssql = @"SELECT T0.""Series"", T0.""SeriesName""  
                            FROM  ""{0}"".""NNM1"" T0  
                            WHERE T0.""ObjectCode"" = '" + ObjType + "' ORDER BY T0.\"ObjectCode\" DESC ";



            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }
        public static DataTable GetBatch(string ItemCode)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetBatch(CONTEXT, ItemCode);
            }
        }

        public static DataTable GetBatch(HANA_APP CONTEXT, string ItemCode)
        {
            string ssql = @"SELECT DISTINCT
                                A.""BatchNum"",
                                B.""U_IDU_BATCH_NAME"" AS ""BatchName""
                                FROM ""{0}"".""OIBT"" A
                                LEFT JOIN ""{0}"".""@IDU_BATCH"" B ON A.""BatchNum"" = B.""Code""
                                WHERE
                                A.""Quantity"" > 0
                                AND
                                A.""ItemCode"" = '" + ItemCode + "' ORDER BY A.\"BatchNum\" ASC";
           
            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }
        public static DataTable GetBatchUDT()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetBatchUDT(CONTEXT);
            }
        }

        public static DataTable GetBatchUDT(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT DISTINCT
                                A.""U_IDU_BATCH_NAME""
                                FROM ""{0}"".""@IDU_BATCH"" A
                                ORDER BY A.""U_IDU_BATCH_NAME"" ASC";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }
        public static string GetCustomerCode(string docEntry)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetCustomerCode(CONTEXT, docEntry);
            }
        }

        public static string GetCustomerCode(HANA_APP CONTEXT, string docEntry)
        {
            string ssql = @"SELECT T0.""CardCode""  
                            FROM  ""{0}"".""OINV"" T0   
                            WHERE T0.""DocEntry"" = " + docEntry + " ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetValue<string>(CONTEXT, ssql, docEntry);
        }

        public static DataTable GetSalesmans()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetSalesmans(CONTEXT);
            }
        }

        public static DataTable GetSalesmans(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT T0.""SlpCode"", T0.""SlpName"",T0.""U_SalesPersonCode""   
                            FROM  ""{0}"".""OSLP"" T0       
                            ORDER BY T0.""U_SalesPersonCode"" ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetDriver()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDriver(CONTEXT);
            }
        }

        public static DataTable GetDriver(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT T0.""Code"",T0.""U_IDU_DriverCode"" AS ""DriverCode"", T0.""U_IDU_DriverName"" AS ""DriverName""  
                            FROM  ""{0}"".""@IDU_DRIVER"" T0       
                            ORDER BY T0.""U_IDU_DriverCode""  ASC ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetVehicle()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetVehicle(CONTEXT);
            }
        }

        public static DataTable GetVehicle(HANA_APP CONTEXT)
        {
            string ssql = @"SELECT T0.""Code"" ,T0.""U_IDU_PoliceNo"" AS ""PoliceNo"", T0.""U_IDU_Type"" AS ""Vehicle""  
                            FROM  ""{0}"".""@IDU_VEHICLE"" T0       
                            ORDER BY T0.""U_IDU_PoliceNo""  ASC ";

            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return GetDataTable(CONTEXT, ssql);
        }


        public static DataTable GetReportGroups()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetReportGroups(CONTEXT);
            }
        }

        public static DataTable GetReportGroups(HANA_APP CONTEXT)
        {

            var ssql = @"SELECT T0.""Id"", T0.""GroupName"" FROM ""Tm_ReportGroup"" T0   ORDER BY T0.""SortCode"", T0.""Id"" ";

            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetQueryGroups()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetQueryGroups(CONTEXT);
            }
        }

        public static DataTable GetQueryGroups(HANA_APP CONTEXT)
        {
            var ssql = @"SELECT T0.""Id"", T0.""GroupName"" FROM ""Tm_QueryGroup"" T0   ORDER BY T0.""SortCode"", T0.""Id"" ";

            return GetDataTable(CONTEXT, ssql);
        }

        public static DataTable GetAlertGroups()
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAlertGroups(CONTEXT);
            }
        }

        public static DataTable GetAlertGroups(HANA_APP CONTEXT)
        {

            var ssql = @"SELECT T0.""Id"", T0.""GroupName"" FROM ""Tm_AlertGroup"" T0   ORDER BY T0.""SortCode"", T0.""Id"" ";

            return GetDataTable(CONTEXT, ssql);

        }

        public static string GetIsAdmin(int userId)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetIsAdmin(CONTEXT, userId);
            }
        }


        public static string GetIsAdmin(HANA_APP CONTEXT, int userId)
        {
            int? roleId = CONTEXT.Database.SqlQuery<int?>("SELECT TOP 1 IFNULL(T0.\"RoleId\",0) AS \"RoleId\" FROM \"Tm_User\" T0   WHERE T0.\"Id\"=:p0", userId).FirstOrDefault();
            string roleName = CONTEXT.Database.SqlQuery<string>("SELECT TOP 1 IFNULL(T0.\"RoleName\",'') AS \"RoleName\" FROM \"Tm_Role\" T0   WHERE T0.\"Id\"=:p0", roleId).FirstOrDefault();
            return GetIsAdmin(roleName);
        }


        public static string GetIsAdmin(string roleName)
        {
            if ((roleName.ToLower() == "admin") || (roleName.ToLower() == "administrator"))
            {
                return "Y";
            }
            else
            {
                return "N";
            }

        }

        public static bool GetAuthAction(int userId, string url)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetAuthAction(CONTEXT, userId, url);
            }
        }

        public static bool GetAuthAction(HANA_APP CONTEXT, int userId, string url)
        {
            string ssql = @"SELECT DISTINCT T0.""Id""
                            FROM ""Tm_User"" T0   
                            INNER JOIN ""Tm_Role"" T1   ON T0.""RoleId""=T1.""Id"" 
                            INNER JOIN ""Tm_Role_Auth"" T2   ON T1.""Id""=T2.""Id"" AND T2.""IsAccess""='Y' 
                            INNER JOIN ""Ts_Menu"" T3   ON T2.""MenuCode""=T3.""MenuCode"" AND T3.""Url"" =:p1 
                            WHERE T0.""Id""=:p0";


            int? id = CONTEXT.Database.SqlQuery<int?>(ssql, userId, url).FirstOrDefault();
            if (id.HasValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static DataTable GetDataTable(string sql, params object[] parameters)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDataTable(CONTEXT, sql, parameters);
            }
        }

        public static DataTable GetDataTable(HANA_APP CONTEXT, string sql, params object[] parameters)
        {
            var s = EfIduHanaRsExtensionsApp.IduGetDataTable(CONTEXT, sql, parameters);

            return s;
        }

        public static DataSet GetDataSet(string sql, params object[] parameters)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GetDataSet(CONTEXT, sql, parameters);
            }
        }

        public static DataSet GetDataSet(HANA_APP CONTEXT, string sql, params object[] parameters)
        {
            var s = EfIduHanaRsExtensionsApp.IduGetDataSet(CONTEXT, sql, parameters);

            return s;
        }

        public static DataTable ExpandoToDataTable(this IEnumerable<dynamic> items)
        {
            var data = items.ToArray();
            if (data.Count() == 0) return null;

            var dt = new DataTable();
            foreach (var key in ((IDictionary<string, object>)data[0]).Keys)
            {
                dt.Columns.Add(key);
            }
            foreach (var d in data)
            {
                dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            }
            return dt;
        }

    }


}
