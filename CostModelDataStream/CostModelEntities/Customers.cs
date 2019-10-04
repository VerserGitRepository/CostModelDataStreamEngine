using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.CostModelEntities
{
   [Table("Customers")]
   public class Customers
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int ProjectId { get; set; }
        public int OpportunityId { get; set; }
        public bool IsActive { get; set; }
    }
}
