using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.CostModelEntities
{
    public class ReturnEntityModel
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; } = false;        
        public string  OpportunityNumber { get; set; }
        public int OpportunityNumberID { get; set; } = 0;
        public int ProjectID { get; set; } = 0;
        public int ProjectManagerID { get; set; } = 0;     
        public int ServiceActivityID { get; set; } = 0;
        public string FileName { get; set; }
        public bool IsFileProcessSuccess { get; set; } = false;

    }
}
