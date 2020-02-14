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
       
        public void CreateServiceCost(ServiceRevenue servicerevenueData)
        {
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                var IsExist = db.ServiceRevenues.Where(x => x.ServiceActivityID == servicerevenueData.ServiceActivityID 
                && x.OpportunityNumberID == servicerevenueData.OpportunityNumberID && x.Quantity== servicerevenueData.Quantity 
                && x.PricePerUnit == servicerevenueData.PricePerUnit.ToString()).FirstOrDefault();
                if (IsExist == null)
                {
                    db.ServiceRevenues.Add(servicerevenueData);
                    db.SaveChanges();

                }
                //else
                //{

                //}
            }          
        }
    }
}
