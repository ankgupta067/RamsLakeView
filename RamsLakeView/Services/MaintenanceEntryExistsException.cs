using RamsLakeView.Models;
using System;
using System.Runtime.Serialization;

namespace RamsLakeView.Services
{
    [Serializable]
    internal class MaintenanceEntryExistsException : Exception
    {
        public MaintenanceEntryEntity MaintenanceEntry { get; set; }
        public MaintenanceEntryExistsException(MaintenanceEntryEntity entry)
        {
            MaintenanceEntry = entry;
        }

        public MaintenanceEntryExistsException(string message) : base(message)
        {
        }

        public MaintenanceEntryExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MaintenanceEntryExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}