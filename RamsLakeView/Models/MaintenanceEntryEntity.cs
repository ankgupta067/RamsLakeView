using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RamsLakeView.Models
{
    public class MaintenanceEntryEntity : TableEntity
    {
        public string Block { get; set; }
        public int FlatNumber { get; set; }

        public string TransactionId { get; set; }

        public double Amount { get; set; }

        public DateTime EntryDateTime { get; set; }
    }
}
