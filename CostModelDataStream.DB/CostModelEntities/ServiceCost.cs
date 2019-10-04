using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.DB.CostModelEntities
{
    [Table("ServiceCost")]
    public class ServiceCost
    {
        public int Id { get; set; }
        public string CostCategory { get; set; }
        public string CostPerUnit { get; set; }
        public string TravelCostPerUnit { get; set; }
        public string LabourCostPerUnit { get; set; }
        public string VariableCostPerUnit { get; set; }
        public string PMCostPerUnit { get; set; }
        public string TechnicianHourlyRate { get; set; }
        public string TravelCostHoursPerunit { get; set; }
        public string LabourCostHoursPerUnit { get; set; }
        public string PMCostHoursPerUnit { get; set; }
        public string VariableCostPerUnitNA { get; set; }
        public string TotalCost { get; set; }
        public string ProfitPerUnit { get; set; }
        public string TotalProfit { get; set; }
        public string ActualMarginOnOverHead { get; set; }
        public string OpportunityNumber { get; set; }
    }
}
