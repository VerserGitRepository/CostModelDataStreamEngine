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
          //  string[] PMFNLN = PM.Split(' ');
            PM = PM.Trim();

            int returnID = 0;
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                var _iSPMEXIST = db.ProjectManagers.Where(x => x.ProjectManagerName.Contains(PM)).FirstOrDefault();

                //foreach (ProjectManagers pm in list)
                //{
                //    if (pm.ProjectManagerName.Contains(PMFNLN[0]))
                //    {
                //        return pm.Id;
                //    }
                //}

                if (_iSPMEXIST == null)
                {
                    var add = new ProjectManagers()
                    {
                        ProjectManagerName = PM,
                        IsActive = true
                    };
                    var Project = db.ProjectManagers.Add(add);
                    db.SaveChanges();
                    returnID = Project.Id;
                }
                else
                {
                    returnID = _iSPMEXIST.Id;
                }
            }          
        
            return returnID;             
        }

    }
}
