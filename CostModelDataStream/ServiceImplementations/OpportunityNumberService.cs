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
        public static int CreateOpportunityNumber(string Opportunity, int ProjectId)
        {
            int returnID = 0;
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                var IsExist = db.OpportunityNumbers.Where(x => x.OpportunityNumber == Opportunity && x.ProjectID== ProjectId).FirstOrDefault();
                if (IsExist == null)
                {
                    var add = new OpportunityNumbers()
                    {
                        OpportunityNumber = Opportunity,
                        ProjectID= ProjectId,
                        IsActive = true
                    };
                    var Project = db.OpportunityNumbers.Add(add);
                    db.SaveChanges();
                    returnID = Project.Id;
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
