using RamsLakeView.Models;
using RamsLakeView.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RamsLakeView.Interfaces
{
    public interface IMaintenanceEntryService {
        Task<bool> AddEntry(MaintenanceEntry entry);
        Task<IList<MaintenanceEntryFetchViewModel>> GetEntries();

    }
}
