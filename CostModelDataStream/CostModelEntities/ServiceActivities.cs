using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.CostModelEntities
{
   public class ServiceActivities
    {
        public int Id { get; set; }
        public string ServiceActivityDescription { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public string ServiceCategory { get; set; }
    }
}
