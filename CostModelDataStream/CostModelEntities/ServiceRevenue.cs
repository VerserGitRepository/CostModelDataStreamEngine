
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
namespace CostModelDataStream.CostModelEntities
{
    [Table("ServiceRevenue")]
    public partial class ServiceRevenue
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string ServiceDescription { get; set; }

        //[StringLength(50)]
        public string PricePerUnit { get; set; }

       // [StringLength(50)]
        public string Quantity { get; set; }

       // [StringLength(50)]
        public string TotalPrice { get; set; }

        public int OpportunityNumberID { get; set; }
        public int? ServiceActivityID { get; set; }

        [StringLength(500)]
        public string CostCategory { get; set; }

        //[StringLength(50)]
        public decimal CostPerUnit { get; set; }

        //[StringLength(50)]
        public decimal TravelCostPerUnit { get; set; }

        //[StringLength(50)]
        public decimal LabourCostPerUnit { get; set; }

        //[StringLength(50)]
        public decimal VariableCostPerUnit { get; set; }

        //[StringLength(50)]
        public decimal PMCostPerUnit { get; set; }

        //[StringLength(50)]
        public decimal TechnicianHourlyRate { get; set; }

        //[StringLength(50)]
        public decimal TravelCostHoursPerunit { get; set; }

        //[StringLength(50)]
        public decimal LabourCostHoursPerUnit { get; set; }

        //[StringLength(50)]
        public decimal PMCostHoursPerUnit { get; set; }

        //[StringLength(50)]
        public decimal VariableCostPerUnitNA { get; set; }

        //[StringLength(50)]
        public decimal TotalCost { get; set; }

        //[StringLength(50)]
        public decimal ProfitPerUnit { get; set; }

        //[StringLength(50)]
        public decimal TotalProfit { get; set; }

        //[StringLength(50)]
        public decimal ActualMarginOnOverHead { get; set; }
    }
}
