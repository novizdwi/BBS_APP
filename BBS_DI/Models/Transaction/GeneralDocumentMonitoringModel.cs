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


namespace Models.Transaction.GeneralDocumentMonitoring
{

    #region Models




    public class GeneralDocumentMonitoringModel
    {
        public int? Type { get; set; }

        public string Status { get; set; }

        public DateTime? ExpiredDateFrom { get; set; }

        public DateTime? ExpiredDateTo { get; set; }

        public string IncludeInactive { get; set; }

        public List<GeneralDocumentMonitoring_ReferenceModel> ListReferences_ = new List<GeneralDocumentMonitoring_ReferenceModel>();
    }


    public class GeneralDocumentMonitoring_ReferenceModel
    {

        public int Id { get; set; }

        public string DocumentName { get; set; }

        public string TypeName { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public string Status { get; set; }
    }


    #endregion

    #region Services

    public class GeneralDocumentMonitoringService
    {

        public GeneralDocumentMonitoringModel GetNewModel(int userId)
        {
            GeneralDocumentMonitoringModel model = new GeneralDocumentMonitoringModel();
            model.Type = -1;
            model.Status = "ALL";
            model.IncludeInactive = "N";

            model.ListReferences_ = GeneralDocumentMonitoring_GetReferences(-1, "ALL", "N");

            return model;
        }


        //-------------------------------------
        //Detail  GeneralDocumentMonitoring_Reference
        //-------------------------------------
        public GeneralDocumentMonitoringModel GetListByParam(int? Type, string Status, string IncludeInactive, DateTime? ExpiredDateFrom, DateTime? ExpiredDateTo)
        {
            GeneralDocumentMonitoringModel model = new GeneralDocumentMonitoringModel();
            model.Type = Type;
            model.Status = Status;
            model.IncludeInactive = IncludeInactive;
            model.ExpiredDateFrom = ExpiredDateFrom;
            model.ExpiredDateTo = ExpiredDateTo;
            model.ListReferences_ = this.GeneralDocumentMonitoring_GetReferences(Type, Status, IncludeInactive, ExpiredDateFrom, ExpiredDateTo);

            return model;
        }

        //-------------------------------------
        //Detail  GeneralDocumentMonitoring_Reference
        //-------------------------------------
        public List<GeneralDocumentMonitoring_ReferenceModel> GeneralDocumentMonitoring_GetReferences(int? Type = -1, string Status = "", string IncludeInactive = "", DateTime? ExpiredDateFrom = null, DateTime? ExpiredDateTo = null)
        {
            using (var CONTEXT = new HANA_APP())
            {
                return GeneralDocumentMonitoring_GetReferences(CONTEXT, Type, Status, IncludeInactive, ExpiredDateFrom, ExpiredDateTo);
            }
        }


        public List<GeneralDocumentMonitoring_ReferenceModel> GeneralDocumentMonitoring_GetReferences(HANA_APP CONTEXT, int? Type = -1, string Status = "", string IncludeInactive = "", DateTime? ExpiredDateFrom = null, DateTime? ExpiredDateTo = null)
        {
            string ssql = @"CALL ""SpGeneralDocumentMonitoring_GetReferences"" (:p0, :p1, :p2, :p3, :p4) ";
            ssql = string.Format(ssql, DbProvider.dbSap_Name);
            return CONTEXT.Database.SqlQuery<GeneralDocumentMonitoring_ReferenceModel>(ssql, Type, Status, IncludeInactive, ExpiredDateFrom, ExpiredDateTo).ToList();
        }



        ////-------------------------------------
        ////Detail  GeneralDocumentMonitoring_Reference
        ////-------------------------------------
        //public List<GeneralDocumentMonitoring_ReferenceModel> GetGeneralDocumentMonitoring_References()
        //{
        //    using (var CONTEXT = new HANA_APP())
        //    {
        //        string ssql =
        //         "SELECT T0.\"Id\", "
        //         + "       T0.\"DocumentName\", "
        //         + "       T0.\"VesselId\",  "
        //         + "       T0.\"VesselNo\",  "
        //         + "       T0.\"VesselName\",  "
        //         + "       T0.\"ExpiredDate\", "
        //         + "       CASE WHEN Now() >= T0.\"ExpiredDate\" THEN 'Expired' "
        //         + "            WHEN Now() < T0.\"ExpiredDate\" AND Now() >= T0.\"WarningDate\" THEN 'Warning' "
        //         + "            WHEN Now() < T0.\"ExpiredDate\" AND Now() < T0.\"WarningDate\" THEN 'Normal' "
        //         + "       ELSE NULL END AS \"Status\" "
        //         + "FROM \"Tm_DocGeneral\" T0 ";

        //        return CONTEXT.Database.SqlQuery<GeneralDocumentMonitoring_ReferenceModel>(ssql).ToList();
        //    }
        //}

        //public List<GeneralDocumentMonitoring_ReferenceModel> GetGeneralDocumentMonitoring_ReferencesByFilter(string Category, int? Type, String Status,DateTime? ExpiredFrom,DateTime? ExpiredTo, String IncludeInactive)
        //{
        //    using (var CONTEXT = new HANA_APP())
        //    {
        //        string sFilter = "";
        //        if (!string.IsNullOrEmpty(Category) && Category != "ALL")
        //        {
        //            sFilter += string.Format(" AND T0.\"Category\" = '{0}' ", Category);
        //        }
        //        if (Type != null && Type != -1)
        //        {
        //            sFilter += string.Format(" AND T0.\"Type\" = {0} ", Type);
        //        }
        //        if (ExpiredFrom != null)
        //        {
        //            sFilter += string.Format(" AND T0.\"ExpiredDate\" >= TO_DATE( '{0}', 'MM/DD/YYYY' ) ", ExpiredFrom.Value.ToString("MM/dd/yyyy"));
        //        }
        //        if (ExpiredTo != null)
        //        {
        //            sFilter += string.Format(" AND T0.\"ExpiredDate\" <= TO_DATE( '{0}', 'MM/DD/YYYY' ) ", ExpiredTo.Value.ToString("MM/dd/yyyy"));
        //        }

        //        if (!string.IsNullOrEmpty(Status) && Status != "ALL")
        //        {
        //            if (Status == "Normal")
        //            {
        //                sFilter += string.Format(" AND Now() < T0.\"ExpiredDate\" AND Now() < T0.\"WarningDate\" ");
        //            }else if(Status == "Warning")
        //            {
        //                sFilter += string.Format(" AND Now() < T0.\"ExpiredDate\" AND Now() >= T0.\"WarningDate\" ");
        //            }else if(Status == "Expired")
        //            {
        //                sFilter += string.Format(" AND Now() >= T0.\"ExpiredDate\" ");
        //            }
        //           // sFilter += string.Format(" AND T0.\"Category\" = {0} ", Category);
        //        }

        //        if (IncludeInactive == "N")
        //        {
        //            sFilter += string.Format(" AND IFNULL(T0.\"IsActive\", 'N') = 'Y' ");
        //        }

        //        string ssql =
        //         "SELECT T0.\"Id\", "
        //         + "       T0.\"DocumentName\", "
        //         + "       T0.\"VesselId\",  "
        //         + "       T0.\"VesselNo\",  "
        //         + "       T0.\"VesselName\",  "
        //         + "       T0.\"ExpiredDate\", "
        //         + "       CASE WHEN Now() >= T0.\"ExpiredDate\" THEN 'Expired' " 
        //         + "            WHEN Now() < T0.\"ExpiredDate\" AND Now() >= T0.\"WarningDate\" THEN 'Warning' "
        //         + "            WHEN Now() < T0.\"ExpiredDate\" AND Now() < T0.\"WarningDate\" THEN 'Normal' "
        //         + "       ELSE NULL END AS \"Status\" "
        //         + "FROM \"Tm_DocGeneral\" T0 "
        //         + "WHERE 1=1 "
        //         + sFilter
        //         ;

        //        return CONTEXT.Database.SqlQuery<GeneralDocumentMonitoring_ReferenceModel>(ssql).ToList();
        //    }
        //}


    }


     



    #endregion

}