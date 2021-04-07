using CostModelDataStream.CostModelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.ServiceImplementations
{
   public  class OpportunityNumberService
    {
        public static int CreateOpportunityNumber(int Opportunity, int ProjectId,int pmID,int SMId)
        {
            int returnID = 0;
            try
            {
                using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
                {
                    var IsExist = db.OpportunityNumbers.Where(x => x.OpportunityNumber == Opportunity).FirstOrDefault();
                    if (IsExist == null)
                    {
                        var add = new OpportunityNumbers()
                        {
                            OpportunityNumber = Convert.ToInt32(Opportunity),
                            ProjectID = ProjectId,
                            ProjectManagerID = pmID,
                            Created = DateTime.Now,
                            SalesManagerID = SMId,
                            IsActive = true
                        };
                        var Project = db.OpportunityNumbers.Add(add);
                        db.SaveChanges();
                      //  Console.WriteLine($"Creating New Opportunity {Opportunity}");
                        CostModelLogger.InfoLogger($"Creating New Opportunity {Opportunity}");
                        returnID = Project.Id;
                    }
                    else
                    {
                        returnID = IsExist.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                CostModelLogger.ErrorLogger($"Error Occured while Creating New Opportunity, {ex.Message}");
            }           
            return returnID;
        }
    }
}
