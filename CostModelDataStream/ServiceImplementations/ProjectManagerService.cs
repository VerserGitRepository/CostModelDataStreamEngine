﻿using CostModelDataStream.CostModelEntities;
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
            int returnID = 0;
            try
            {
                using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
                {
                    var IsExist = db.ProjectManagers.Where(x => x.ProjectManagerName == PM).FirstOrDefault();
                    if (IsExist == null)
                    {
                        var add = new ProjectManagers()
                        {
                            ProjectManagerName = PM,
                            IsActive = true
                        };
                        var Project = db.ProjectManagers.Add(add);
                        db.SaveChanges();
                        // Console.WriteLine($"Creating New project Manager{PM}");
                        CostModelLogger.InfoLogger($"Creating New project Manager{PM}");
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
                CostModelLogger.InfoLogger($"Error Occured while Creating Project Manager, {ex.Message}");
            }
            return returnID;
        }
    }
}
