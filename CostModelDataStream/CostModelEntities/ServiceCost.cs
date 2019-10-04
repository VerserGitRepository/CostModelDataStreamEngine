namespace CostModelDataStream.CostModelEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ServiceCost")]
    public partial class ServiceCost
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string CostCategory { get; set; }

        [StringLength(50)]
        public string CostPerUnit { get; set; }

        [StringLength(50)]
        public string TravelCostPerUnit { get; set; }

        [StringLength(50)]
        public string LabourCostPerUnit { get; set; }

        [StringLength(50)]
        public string VariableCostPerUnit { get; set; }

        [StringLength(50)]
        public string PMCostPerUnit { get; set; }

        [StringLength(50)]
        public string TechnicianHourlyRate { get; set; }

        [StringLength(50)]
        public string TravelCostHoursPerunit { get; set; }

        [StringLength(50)]
        public string LabourCostHoursPerUnit { get; set; }

        [StringLength(50)]
        public string PMCostHoursPerUnit { get; set; }

        [StringLength(50)]
        public string VariableCostPerUnitNA { get; set; }

        [StringLength(50)]
        public string TotalCost { get; set; }

        [StringLength(50)]
        public string ProfitPerUnit { get; set; }

        [StringLength(50)]
        public string TotalProfit { get; set; }

        [StringLength(50)]
        public string ActualMarginOnOverHead { get; set; }

        public int OpportunityNumberID_FK { get; set; }
    }
}
