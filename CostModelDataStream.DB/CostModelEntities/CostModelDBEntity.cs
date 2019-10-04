using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.DB.CostModelEntities
{
    public partial class CostModelDBEntity : DbContext
    {
         public CostModelDBEntity() : base("name=CostModelTimeSheetDB") { }

         public  DbSet<ProjectDetails> ProjectDetails { get; set; }
         public  DbSet<ServiceCost> ServiceCost { get; set; }
         public  DbSet<ServiceRevenue> ServiceRevenue { get; set; }
    }
}
