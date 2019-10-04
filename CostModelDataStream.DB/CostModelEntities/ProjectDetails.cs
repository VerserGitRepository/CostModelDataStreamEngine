using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.DB.CostModelEntities
{
    [Table("ProjectDetails")]
    public class ProjectDetails
    {
        public int Id { get; set; }
        public string OpportunityNumber { get; set; }
        public string Customer { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SalesManager { get; set; }
        public string ProjectManager { get; set; }
        public string CustomerContactName { get; set; }
        public string SiteAddress { get; set; }
        public string Approver { get; set; }
        public string VerserBranch { get; set; }
    }
}
