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
            try
            {
                using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
                {
                    var IsExist = db.ServiceActivities.Where(x => x.ServiceActivityDescription == ServiceActivity).FirstOrDefault();
                    if (IsExist == null)
                    {
                        var add = new ServiceActivities()
                        {
                            ServiceActivityDescription = ServiceActivity,
                            ServiceCategory = "Project",
                            IsActive = true,
                            Created = DateTime.Now
                        };
                        var ServiceActivitiesID = db.ServiceActivities.Add(add);
                        db.SaveChanges();
                        //    Console.WriteLine($"Creating New ServiceActivity {ServiceActivity}");
                        CostModelLogger.InfoLogger($"Creating New ServiceActivity {ServiceActivity}");
                        returnID = ServiceActivitiesID.Id;
                    }
                    else
                    {
                        returnID = IsExist.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                CostModelLogger.ErrorLogger($"Error Occured While Creating New ServiceActivity, {ex.Message}");
            }           
            return returnID;
        }
    }
}
