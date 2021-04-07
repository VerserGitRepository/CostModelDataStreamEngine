using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.CostModelEntities
{
   public class ServiceCostRevenueViewModel
    {
       
        public string PricePerUnit { get; set; }
        public string Quantity { get; set; }
        public string TotalPrice { get; set; }
        public int OpportunityNumberID { get; set; }
        public int? ServiceActivityID { get; set; }
        public string ServiceDescription { get; set; }
        public  string CostCategory { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal TravelCostPerUnit { get; set; }
        public decimal LabourCostPerUnit { get; set; }
        public decimal VariableCostPerUnit { get; set; }
        public decimal PMCostPerUnit { get; set; }
        public decimal TechnicianHourlyRate { get; set; }
        public decimal TravelCostHoursPerunit { get; set; }
        public decimal LabourCostHoursPerUnit { get; set; }
        public decimal PMCostHoursPerUnit { get; set; }
        public decimal VariableCostPerUnitNA { get; set; }
        public decimal TotalCost { get; set; }
        public decimal ProfitPerUnit { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal ActualMarginOnOverHead { get; set; }
        public int serviceactivity_ID { get; set; }
    }
}
