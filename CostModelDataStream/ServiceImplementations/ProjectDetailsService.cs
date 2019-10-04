
using CostModelDataStream.CostModelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.ServiceImplementations
{
   public class ProjectDetailsService
    {
        public ReturnEntityModel CreateProjectDetails(ProjectDetails ProjectData)
        {

            ReturnEntityModel ReturnValues= new ReturnEntityModel();            

            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
               var IsExist= db.ProjectDetails.Where(x => x.OpportunityNumber == ProjectData.OpportunityNumber && x.StartDate == ProjectData.StartDate).FirstOrDefault();
                if (IsExist ==null)
                {
                 var OpportunityId =   db.ProjectDetails.Add(ProjectData);
                    db.SaveChanges();                   
                    ReturnValues.IsSuccess = true;
                    ReturnValues.Message = "Sucess";
                }
                else
                {                   
                    ReturnValues.IsSuccess = true;
                    ReturnValues.Message = "Sucess";
                }                
            }
            return ReturnValues;
        }      

    }
}
