using CostModelDataStream.CostModelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.ServiceImplementations
{
  public  class salesManagerService
    {
        public static int CreateSalesManager(string SM)
        {
            int returnID = 0;
            try
            {
                using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
                {
                    var IsExist = db.SalesManagers.Where(x => x.SalesManagerName == SM).FirstOrDefault();
                    if (IsExist == null)
                    {
                        var add = new SalesManagers()
                        {
                            SalesManagerName = SM,
                            IsActive = true
                        };
                        var _Sm = db.SalesManagers.Add(add);
                        db.SaveChanges();
                        //  Console.WriteLine($"Creating New Sales Manager{SM}");
                        CostModelLogger.InfoLogger($"Creating New Sales Manager{SM}");
                        returnID = _Sm.Id;
                    }
                    else
                    {
                        returnID = IsExist.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                CostModelLogger.InfoLogger($"Error Occured while Creating sales Manager, {ex.Message}");
            }           
            return returnID;
        }
    }
}
