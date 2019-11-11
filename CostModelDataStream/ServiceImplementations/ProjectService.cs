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
        public static int CreateProject(string ProjectName,int JMSProjectId)
        {
            int returnID = 0;
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                var IsExist = db.Projects.Where(x => x.JMSProjectID== JMSProjectId).FirstOrDefault();
                if (IsExist == null)
                {
                    var add = new Projects()
                    {
                        Created = DateTime.Now,
//                        CreatedBy = "Kalyan Vedula"
                        ProjectName = ProjectName,
                        IsActive = true,
                        JMSProjectID = JMSProjectId
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
