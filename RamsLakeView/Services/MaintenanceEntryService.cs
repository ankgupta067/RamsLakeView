using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using RamsLakeView.Interfaces;
using RamsLakeView.Models;
using RamsLakeView.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RamsLakeView.Services
{
    public class MaintenanceEntryService : IMaintenanceEntryService
    {
        Dictionary<string,MaintenanceEntryEntity> entriesCache = 
            new Dictionary<string, MaintenanceEntryEntity>();
        CloudTable _table;
        private IConfiguration _configuration;
        private string partitionKey = "RLMPartitionKey";

        public MaintenanceEntryService(IConfiguration configuration){
            _configuration = configuration;
            var sa = CloudStorageAccount.Parse(configuration["StorageConnectionString"]);
            var tc = sa.CreateCloudTableClient();
            var table = tc.GetTableReference("MaintenanceEntries");
            CreateOrFetchTableEntry(table);
        }

        private async void CreateOrFetchTableEntry(CloudTable table)
        {
            if (await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("table created");
            }
            else
            {
                Console.WriteLine("table exists");
            }
            _table = table;
        }
        public async Task<bool> AddEntry(MaintenanceEntry entry)
        {
            if (entriesCache.Keys.Contains(entry.TransactionId))
            {
                throw new MaintenanceEntryExistsException(entriesCache[entry.TransactionId]);
            }
            var entity = new MaintenanceEntryEntity()
            {
                Amount = entry.Amount,
                Block = entry.Block.ToString(),
                EntryDateTime = DateTime.Now,
                FlatNumber = entry.FlatNumber,
                TransactionId = entry.TransactionId
            };
            entity.PartitionKey = partitionKey;
            entity.RowKey = entry.TransactionId;
            // upload on azure 
            var operation = TableOperation.InsertOrMerge(entity);
            var result = await _table.ExecuteAsync(operation);
            if (result.HttpStatusCode == 204){
                entriesCache.Add(entry.TransactionId, result.Result as MaintenanceEntryEntity);
            }
            else{
                Console.WriteLine(result.HttpStatusCode);
                throw new ApplicationException($"Something went wrong " +
                    $"please retry");
            }
            return true;
        }

        public async Task<IList<MaintenanceEntryFetchViewModel>> GetEntries()
        {
            var entries = new List<MaintenanceEntryFetchViewModel>();

            TableContinuationToken token = null;
            do
            {
                var qr = await _table.ExecuteQuerySegmentedAsync(new TableQuery<MaintenanceEntryEntity>(), token);
                var results = qr.Select(x => new MaintenanceEntryFetchViewModel()
                {
                    Amount = x.Amount,
                    Block = x.Block,
                    EntryDateTime = x.EntryDateTime,
                    FlatNumber = x.FlatNumber,
                    TransactionId = x.TransactionId
                });
                entries.AddRange(results);

            } while (token != null);
            return entries;
        }
    }
}
