using CostModelDataStream.CostModelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.ServiceImplementations
{
  public  class ServiceActivityHelperService
    {
        public static int CreateServiceActivity(string ServiceActivity)
        {
            int returnID = 0;
           // ServiceActivity = ServiceActivity.Substring(0, 450);
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                var IsExist = db.ServiceActivities.Where(x => x.ServiceActivityDescription == ServiceActivity).FirstOrDefault();
                if (IsExist == null)
                {
                    var add = new ServiceActivities()
                    {
                        ServiceActivityDescription = ServiceActivity,
                        ServiceCategory = "Project",
                        IsActive = true
                    };
                    var ServiceActivitiesID = db.ServiceActivities.Add(add);
                    db.SaveChanges();
                    returnID = ServiceActivitiesID.Id;
                }
                else
                {
                    returnID = IsExist.Id;
                }
            }
            return returnID;
        }
    }
}
