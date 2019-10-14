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
            //using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            //{               
            //    db.ServiceRevenues.Add(servicerevenueData);
            //    db.SaveChanges();
            //}
            int returnID = 0;
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                var IsExist = db.ServiceRevenues.Where(x => x.ServiceDescription == servicerevenueData.ServiceDescription).FirstOrDefault();
                if (IsExist == null)
                {
                    db.ServiceRevenues.Add(servicerevenueData);
                    db.SaveChanges();
                }
                else
                {
                    returnID = IsExist.Id;
                }
            }
           // return returnID;
        }
    }
}
