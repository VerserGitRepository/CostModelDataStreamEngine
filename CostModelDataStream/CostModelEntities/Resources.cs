using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.CostModelEntities
{
    public class Resources
    {
        public int Id { get; set; }
        public string ResourceName { get; set; }
        public string State { get; set; }
        public string Warehouse { get; set; }
        public int? WarehouseID { get; set; }
        public bool IsActive { get; set; }       
    }
}
