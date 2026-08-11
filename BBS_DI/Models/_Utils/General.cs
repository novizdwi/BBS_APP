using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Web.Mvc;
using DevExpress.Web;
using Sap.Data.Hana;

namespace Models._Utils
{

    public class SapDocResult
    {
        public long DocEntry { get; set; }
        public string DocNum { get; set; }
    }

    public class ReconcileLineResult
    {
        public long DetId { get; set; }
        public string ItemCode { get; set; }
        public decimal QuantityPosted { get; set; }
        public decimal QuantitySAP { get; set; }
        public string IsMatch { get; set; }
    }

    public static class General
    {
        public static void GridViewFilterExpression(MVCxGridView gridView, GridViewSettings settings)
        {
            int i = -1;
            foreach (GridViewDataColumn column in gridView.DataColumns)
            {

                i++;
                if (column.FilterExpression.StartsWith("StartsWith"))
                {
                    column.Settings.AutoFilterCondition = AutoFilterCondition.BeginsWith;
                }
                else if (column.FilterExpression.StartsWith("Contains"))
                {
                    column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                }
                else if (column.FilterExpression.StartsWith("Not Contains"))
                {
                    column.Settings.AutoFilterCondition = AutoFilterCondition.DoesNotContain;
                }
                else if (column.FilterExpression.StartsWith("EndsWith"))
                {
                    column.Settings.AutoFilterCondition = AutoFilterCondition.EndsWith;
                }
                else if (column.FilterExpression.Contains("] <>"))
                {
                    column.Settings.AutoFilterCondition = AutoFilterCondition.NotEqual;
                }
                else if (column.FilterExpression.Contains("] ="))
                {
                    column.Settings.AutoFilterCondition = AutoFilterCondition.Equals;
                }
                else
                {

                    column.Settings.AutoFilterCondition = ((GridViewDataColumn)settings.Columns[i]).Settings.AutoFilterCondition;
                }

            }
        }



    }
}
