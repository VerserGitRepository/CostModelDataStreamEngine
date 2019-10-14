
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
namespace CostModelDataStream.CostModelEntities
{
    [Table("ServiceRevenue")]
    public partial class ServiceRevenue
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string ServiceDescription { get; set; }

        //[StringLength(50)]
        public decimal PricePerUnit { get; set; }

       // [StringLength(50)]
        public int Quantity { get; set; }

       // [StringLength(50)]
        public decimal TotalPrice { get; set; }

        public int OpportunityNumberID { get; set; }
        public int? ServiceActivityID { get; set; }
    }
}
