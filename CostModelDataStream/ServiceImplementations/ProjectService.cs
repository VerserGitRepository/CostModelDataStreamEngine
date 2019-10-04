using CostModelDataStream.CostModelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.ServiceImplementations
{
   public class ProjectService
    {
        public static int CreateProject(string ProjectName)
        {
            int returnID = 0;
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                var IsExist = db.Projects.Where(x => x.ProjectName == ProjectName).FirstOrDefault();
                if (IsExist == null)
                {
                    var add = new Projects()
                    {
                        ProjectName = ProjectName,
                        IsActive = true
                    };
                    var Project = db.Projects.Add(add);
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
