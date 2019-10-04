
using CostModelDataStream.CostModelEntities;

namespace CostModelDataStream.ServiceImplementations
{
    public class ServiceCostService
    {       
        public void CreateServiceCost(ServiceCost serviceCostData)
        {
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {                
                db.ServiceCosts.Add(serviceCostData);
                db.SaveChanges();
            }
        }
    }
}
