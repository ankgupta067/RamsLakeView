using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RamsLakeView.Models
{
    public class MaintenanceEntry
    {
        [Required]
        public BlockNumber Block { get; set; }
        [Required]
        public int FlatNumber { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public double Amount { get; set; }

        public DateTime EntryDateTime { get; set; }
    }

}
