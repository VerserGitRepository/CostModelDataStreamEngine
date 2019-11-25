
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
        public static ReturnEntityModel CreateProjectDetails(ProjectDetails ProjectData)
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

        public static ReturnEntityModel AddCustomer(ProjectDetails project)
        {
            var ReturnValues = new ReturnEntityModel();           
            ReturnValues.IsSuccess = false;
            ReturnValues.Message = "Invalid parameters";

            //if (customer ==null  || _opportunity == null)
            //{
            //    ReturnValues.IsSuccess = false;
            //    ReturnValues.Message = "Invalid parameters";
            //    return ReturnValues;
            //}
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                try
                {
                    int _opportunityid = Convert.ToInt32(project.OpportunityNumber);
                    bool insert = true;
                    var _opportunityId = db.OpportunityNumbers.Where(x => x.OpportunityNumber == _opportunityid).FirstOrDefault();
                    if (_opportunityId != null)
                    {
                        var IsExist = db.Customers.Where(x => x.CustomerName == project.Customer && x.ProjectId == _opportunityId.ProjectID && x.OpportunityId == _opportunityId.Id).FirstOrDefault();
                        if (IsExist != null)
                        {
                            insert = false;
                        }
                    }
                    if (insert)
                    {
                        var _a = new Customers();
                        _a.CustomerName = project.Customer; _a.ProjectId =  project.Id; _a.OpportunityId = Convert.ToInt32(project.OpportunityNumber); _a.IsActive = true;
                        db.Customers.Add(_a);
                        db.SaveChanges();
                        ReturnValues.IsSuccess = true;
                        ReturnValues.Message = "Sucess";
                    }
                }
                catch (Exception ex)
                {

                   //Log.Error($"Error Occured At Customer Insert Class {ex.Message}");
                }
            }
            return ReturnValues;
        }
    }
}
