using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chainify
{
    public class ChainLinkRepository
    {
        private readonly CloudTable _cloudTable;

        public ChainLinkRepository(CloudTable cloudTable)
        {
            _cloudTable = cloudTable;
        }

        public async Task UpdateChainLinks(IEnumerable<ChainLink> chainLinks)
        {
            await _cloudTable.CreateIfNotExistsAsync();
            var batchOperation = new TableBatchOperation();
            
            foreach (var chainLink in chainLinks) 
                batchOperation.InsertOrMerge(chainLink);

            await _cloudTable.ExecuteBatchAsync(batchOperation);
        }
    }
}
