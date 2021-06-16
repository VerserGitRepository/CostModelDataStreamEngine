
using CostModelDataStream.CostModelEntities;
using System.Linq;

namespace CostModelDataStream.ServiceImplementations
{
    public class ServiceCostService
    {
        public void CreateServiceCost(ServiceCost serviceCostData)
        {
            bool savechange = false;
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                //&& c.serviceactivity_ID == serviceCostData.serviceactivity_ID
                var _serviceCostLine = db.ServiceCosts.Where(c => c.OpportunityNumberID == serviceCostData.OpportunityNumberID && c.CostCategory == serviceCostData.CostCategory 
                && c.CostPerUnit == serviceCostData.CostPerUnit && c.PMCostPerUnit == serviceCostData.PMCostPerUnit && c.TravelCostPerUnit == serviceCostData.TravelCostPerUnit
                && c.LabourCostPerUnit == serviceCostData.LabourCostPerUnit && c.TotalCost == serviceCostData.TotalCost ).FirstOrDefault();
                if (_serviceCostLine != null)
                {
                    if (_serviceCostLine.serviceactivity_ID >0 && _serviceCostLine.serviceactivity_ID != serviceCostData.serviceactivity_ID)
                    {
                        _serviceCostLine.serviceactivity_ID = serviceCostData.serviceactivity_ID;
                        savechange = true;
                    }
                    if (_serviceCostLine.CostPerUnit != null && _serviceCostLine.CostPerUnit != serviceCostData.CostPerUnit)
                    {
                        _serviceCostLine.CostPerUnit = serviceCostData.CostPerUnit;
                        savechange = true;
                    }
                    if (_serviceCostLine.TravelCostHoursPerunit != null && _serviceCostLine.TravelCostHoursPerunit != serviceCostData.TravelCostHoursPerunit)
                    {
                        _serviceCostLine.TravelCostHoursPerunit = serviceCostData.TravelCostHoursPerunit;
                        savechange = true;
                    }
                    if (_serviceCostLine.TravelCostPerUnit != null && _serviceCostLine.TravelCostPerUnit != serviceCostData.TravelCostPerUnit)
                    {
                        _serviceCostLine.TravelCostPerUnit = serviceCostData.TravelCostPerUnit;
                        savechange = true;
                    }
                    if (_serviceCostLine.LabourCostPerUnit != null && _serviceCostLine.LabourCostPerUnit != serviceCostData.LabourCostPerUnit)
                    {
                        _serviceCostLine.LabourCostPerUnit = serviceCostData.LabourCostPerUnit;
                        savechange = true;
                    }
                    if (_serviceCostLine.VariableCostPerUnit != null && _serviceCostLine.VariableCostPerUnit != serviceCostData.VariableCostPerUnit)
                    {
                        _serviceCostLine.VariableCostPerUnit = serviceCostData.VariableCostPerUnit;
                        savechange = true;
                    }
                    if (_serviceCostLine.PMCostPerUnit != null && _serviceCostLine.PMCostPerUnit != serviceCostData.PMCostPerUnit)
                    {
                        _serviceCostLine.PMCostPerUnit = serviceCostData.PMCostPerUnit;
                        savechange = true;
                    }
                    if (_serviceCostLine.PMCostHoursPerUnit != null && _serviceCostLine.PMCostHoursPerUnit != serviceCostData.PMCostHoursPerUnit)
                    {
                        _serviceCostLine.PMCostHoursPerUnit = serviceCostData.PMCostHoursPerUnit;
                        savechange = true;
                    }
                    if (_serviceCostLine.TechnicianHourlyRate != null && _serviceCostLine.TechnicianHourlyRate != serviceCostData.TechnicianHourlyRate)
                    {
                        _serviceCostLine.TechnicianHourlyRate = serviceCostData.TechnicianHourlyRate;
                        savechange = true;
                    }
                    if (_serviceCostLine.TotalCost != null && _serviceCostLine.TotalCost != serviceCostData.TotalCost)
                    {
                        _serviceCostLine.TotalCost = serviceCostData.TotalCost;
                        savechange = true;
                    }
                    if (_serviceCostLine.ProfitPerUnit != null && _serviceCostLine.ProfitPerUnit != serviceCostData.ProfitPerUnit)
                    {
                        _serviceCostLine.ProfitPerUnit = serviceCostData.ProfitPerUnit;
                        savechange = true;
                    }
                    if (_serviceCostLine.TotalProfit != null && _serviceCostLine.TotalProfit != serviceCostData.TotalProfit)
                    {
                        _serviceCostLine.TotalProfit = serviceCostData.TotalProfit;
                        savechange = true;
                    }
                }
                else
                {
                    db.ServiceCosts.Add(serviceCostData);
                    savechange = true;
                    CostModelLogger.InfoLogger($"New  CostCategory Added {serviceCostData.CostCategory}");
                   // System.Console.WriteLine($"New  CostCategory Added {serviceCostData.CostCategory}" );
                }
                if (savechange)
                {
                    db.SaveChanges();

                }
            }
        }
    }
}
