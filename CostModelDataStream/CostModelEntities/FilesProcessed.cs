using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostModelDataStream.CostModelEntities
{
    [Table("FilesProcessed")]
    public class FilesProcessed
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime? DateProcessed { get; set; }
        public string OpportunityNumber { get; set; }
        public bool IsFileProcessSuccess { get; set; }
      
    }
}
