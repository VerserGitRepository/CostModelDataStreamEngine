using CostModelDataStream.CostModelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.ServiceImplementations
{
    public class ServiceRevenueService
    {

        public void CreateServiceRevenue(ServiceRevenue servicerevenueData)
        {
            bool _Savechange = false;

            try
            {
                using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
                {
                    var _ServiceRevenueLine = db.ServiceRevenues.Where(r => r.OpportunityNumberID == servicerevenueData.OpportunityNumberID
                    && r.ServiceDescription == servicerevenueData.ServiceDescription).FirstOrDefault();
                    if (_ServiceRevenueLine != null)
                    {
                        if (servicerevenueData.CostPerUnit > 0 && _ServiceRevenueLine.CostPerUnit != servicerevenueData.CostPerUnit)
                        {
                            _ServiceRevenueLine.CostPerUnit = servicerevenueData.CostPerUnit;
                            _Savechange = true;
                        }
                        if (servicerevenueData.PricePerUnit != null && _ServiceRevenueLine.PricePerUnit != servicerevenueData.PricePerUnit)
                        {
                            _ServiceRevenueLine.PricePerUnit = servicerevenueData.PricePerUnit;
                            _Savechange = true;
                        }
                        if (servicerevenueData.Quantity != null && _ServiceRevenueLine.Quantity != servicerevenueData.Quantity)
                        {
                            _ServiceRevenueLine.Quantity = servicerevenueData.Quantity;
                            _Savechange = true;
                        }
                        if (servicerevenueData.TotalPrice != null && _ServiceRevenueLine.TotalPrice != servicerevenueData.TotalPrice)
                        {
                            _ServiceRevenueLine.TotalPrice = servicerevenueData.TotalPrice;
                            _Savechange = true;
                        }
                        if (servicerevenueData.TravelCostPerUnit > 0 && _ServiceRevenueLine.TravelCostPerUnit != servicerevenueData.TravelCostPerUnit)
                        {
                            _ServiceRevenueLine.TravelCostPerUnit = servicerevenueData.TravelCostPerUnit;
                            _Savechange = true;
                        }
                        if (servicerevenueData.LabourCostPerUnit > 0 && _ServiceRevenueLine.LabourCostPerUnit != servicerevenueData.LabourCostPerUnit)
                        {
                            _ServiceRevenueLine.LabourCostPerUnit = servicerevenueData.LabourCostPerUnit;
                            _Savechange = true;
                        }
                        if (servicerevenueData.PMCostPerUnit > 0 && _ServiceRevenueLine.PMCostPerUnit != servicerevenueData.PMCostPerUnit)
                        {
                            _ServiceRevenueLine.PMCostPerUnit = servicerevenueData.PMCostPerUnit;
                            _Savechange = true;
                        }
                        if (servicerevenueData.PMCostHoursPerUnit > 0 && _ServiceRevenueLine.PMCostHoursPerUnit != servicerevenueData.PMCostHoursPerUnit)
                        {
                            _ServiceRevenueLine.PMCostHoursPerUnit = servicerevenueData.PMCostHoursPerUnit;
                            _Savechange = true;
                        }
                        if (servicerevenueData.LabourCostHoursPerUnit > 0 && _ServiceRevenueLine.LabourCostHoursPerUnit != servicerevenueData.LabourCostHoursPerUnit)
                        {
                            _ServiceRevenueLine.LabourCostHoursPerUnit = servicerevenueData.LabourCostHoursPerUnit;
                            _Savechange = true;
                        }
                        if (servicerevenueData.TravelCostHoursPerunit > 0 && _ServiceRevenueLine.TravelCostHoursPerunit != servicerevenueData.TravelCostHoursPerunit)
                        {
                            _ServiceRevenueLine.TravelCostHoursPerunit = servicerevenueData.TravelCostHoursPerunit;
                            _Savechange = true;
                        }
                        if (servicerevenueData.VariableCostPerUnit > 0 && _ServiceRevenueLine.VariableCostPerUnit != servicerevenueData.VariableCostPerUnit)
                        {
                            _ServiceRevenueLine.VariableCostPerUnit = servicerevenueData.VariableCostPerUnit;
                            _Savechange = true;
                        }
                        if (servicerevenueData.TotalCost > 0 && _ServiceRevenueLine.TotalCost != servicerevenueData.TotalCost)
                        {
                            _ServiceRevenueLine.TotalCost = servicerevenueData.TotalCost;
                            _Savechange = true;
                        }
                        if (servicerevenueData.TechnicianHourlyRate > 0 && _ServiceRevenueLine.TechnicianHourlyRate != servicerevenueData.TechnicianHourlyRate)
                        {
                            _ServiceRevenueLine.TechnicianHourlyRate = servicerevenueData.TechnicianHourlyRate;
                            _Savechange = true;
                        }
                    }
                    else
                    {
                        db.ServiceRevenues.Add(servicerevenueData);
                     //   System.Console.WriteLine($"{servicerevenueData.CostCategory} processing...");
                        CostModelLogger.InfoLogger($"Creating New CostCategory {servicerevenueData.CostCategory}");
                        _Savechange = true;
                    }
                    if (_Savechange)
                    {
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                CostModelLogger.InfoLogger($"Error Occured While Creating New CostCategory, {ex.Message}");
            }       

        }
    }
}
