
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
namespace CostModelDataStream.CostModelEntities
{
    public partial class ProjectDetails
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string OpportunityNumber { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
       
        public string SiteAddress { get; set; }
    
        public string Customer { get; set; }
        [StringLength(50)]
        public string Approver { get; set; }

        [StringLength(50)]
        public string ProjectManager { get; set; }

        [StringLength(50)]
        public string SalesManager { get; set; }
     
        public string CustomerContactName { get; set; }
       
        public string VerserBranch { get; set; }
    }
}
