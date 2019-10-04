using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.CostModelEntities
{
    public class OpportunityNumbers
    {
        public int Id { get; set; }
        public string OpportunityNumber { get; set; }
        public int? ProjectID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public bool? IsActive { get; set; }
        public DateTime? Updated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
