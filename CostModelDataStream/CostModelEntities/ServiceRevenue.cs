
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
namespace CostModelDataStream.CostModelEntities
{
    [Table("ServiceRevenue")]
    public partial class ServiceRevenue
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string ServiceDescription { get; set; }
        public string PricePerUnit { get; set; }
        public string Quantity { get; set; }
        public string TotalPrice { get; set; }
        public int OpportunityNumberID { get; set; }
        public int? ServiceActivityID { get; set; }
        [StringLength(500)]
        public string CostCategory { get; set; }
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

    }
}
