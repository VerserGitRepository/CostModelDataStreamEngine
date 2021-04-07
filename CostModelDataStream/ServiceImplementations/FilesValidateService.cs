using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CostModelDataStream.CostModelEntities;

namespace CostModelDataStream.ServiceImplementations
{
   public class FilesValidateService
    {
        public ReturnEntityModel IsFileExists(string FileName)
        {
            ReturnEntityModel ReturnValues = new ReturnEntityModel();
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
               var isvalid= db.FilesProcessed.Where(y => y.FileName == FileName && y.IsFileProcessSuccess == true).FirstOrDefault();
                if (isvalid !=null)
                {
                    ReturnValues.FileName = isvalid.FileName;
                    ReturnValues.IsFileProcessSuccess = true;
                }
            }
            return ReturnValues;
        }

        public void AddNewFile(string FileName, string OpportunityNumber)
        {
            using (CostModelTimeSheetDB db = new CostModelTimeSheetDB())
            {
                var isvalid = db.FilesProcessed.Where(y => y.FileName == FileName).FirstOrDefault();
                if (isvalid ==null)
                {
                    FilesProcessed fileadd = new FilesProcessed
                    {
                        FileName = FileName,
                        OpportunityNumber = OpportunityNumber,
                        IsFileProcessSuccess = true,
                        DateProcessed = DateTime.Now
                    };
                    db.FilesProcessed.Add(fileadd);
                    db.SaveChanges();
                }              
            }
        }
    }
}
