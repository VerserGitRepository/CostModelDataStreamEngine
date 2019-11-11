using CostModelDataStream.CostModelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.ServiceImplementations
{
   public class ProjectManagerService
    {
        public static int CreateProjectManager(string PM)
        {
            string[] PMFNLN = PM.Split(',');
             int returnID = 0;
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                var list = db.ProjectManagers;
                foreach (ProjectManagers pm in list)
                {
                    if (pm.ProjectManagerName.Contains(PMFNLN[0]))
                    {
                        return pm.Id;
                    }
                }
               
                var add = new ProjectManagers() {
                    ProjectManagerName=PM,
                    IsActive = true
                };
                var Project = db.ProjectManagers.Add(add);
                db.SaveChanges();
                returnID = Project.Id;
                
            }
            return returnID;             
        }

    }
}
