using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.DB.CostModelEntities
{
    [Table("ServiceRevenue")]
    public  class ServiceRevenue
    {
        public int Id { get; set; }
        public string ServiceDescription { get; set; }
        public string PricePerUnit { get; set; }
        public string Quantity { get; set; }
        public string TotalPrice { get; set; }
        public string OpportunityNumber { get; set; }
    }
}
