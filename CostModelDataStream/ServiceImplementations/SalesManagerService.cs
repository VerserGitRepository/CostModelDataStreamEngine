using CostModelDataStream.CostModelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.ServiceImplementations
{
   public class SalesManagerService
    {
        public static int AddSalesManager(string salesManager)
        {           
            salesManager = salesManager.Trim();
            int returnID = 0;
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                var _iSsalemanEXIST = db.SalesManagers.Where(x =>x.SalesManagerName.Contains(salesManager)).FirstOrDefault();
                if (_iSsalemanEXIST == null)
                {
                    var add = new SalesManagers()
                    {
                        SalesManagerName = salesManager,
                        IsActive = true
                    };
                    var salesmanId = db.SalesManagers.Add(add);
                    db.SaveChanges();
                    returnID = salesmanId.Id;
                }
                else
                {
                    returnID = _iSsalemanEXIST.Id;
                }
            } 
            return returnID;             
        }

    }
}
