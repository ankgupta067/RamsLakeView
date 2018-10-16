using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RamsLakeView.ViewModels
{
    public class FailureMessageEntryExistsViewModel
    {

        public string TransactionId { get; set; }

        public double Amount { get; set; }

        public DateTime dateTime { get; set; }
    }
}
