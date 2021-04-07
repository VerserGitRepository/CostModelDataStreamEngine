using CostModelDataStream.CostModelEntities;
using CostModelDataStream.StreamEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.ServiceImplementations
{
    public class ProjectService
    {
        public static int CreateProject(string ProjectName, int JMSprojectid, int projectManagerID, int SalesManagerID)
        {
            int returnID = 0;
            try
            {
                using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
                {
                    var IsExist = db.Projects.Where(x => x.ProjectName == ProjectName).FirstOrDefault();
                    if (IsExist == null)
                    {
                        var add = new Projects()
                        {
                            ProjectName = ProjectName,
                            IsActive = true,
                            JMSProjectID = JMSprojectid,
                            ProjectManagerID = projectManagerID,
                            SalesManagerID = SalesManagerID,
                            Created = DateTime.Now
                        };
                        var Project = db.Projects.Add(add);
                        db.SaveChanges();
                        returnID = Project.Id;
                        // Console.WriteLine($"Creating New project{ProjectName}");
                        CostModelLogger.InfoLogger($"Creating New project{ProjectName}");
                    }
                    else
                    {
                        returnID = IsExist.Id;
                    }
                }
            }
            catch (Exception)
            {

                CostModelLogger.ErrorLogger($"Error Occured while Creating New project, {ProjectName}");
            }
            return returnID;
        }
    }
}
