using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.CostModelEntities
{
   public class Projects
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsActive { get; set; }
        public string UserID { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public int? JMSProjectID { get; set; }
        public int? ProjectManagerID { get; set; }
        public int? SalesManagerID { get; set; }
        public int? BranchID { get; set; }
        public bool? IsInternal { get; set; }    

    }
}
