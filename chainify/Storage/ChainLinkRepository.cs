using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Chainify.Storage
{
    public class ChainLinkRepository : IChainLinkRepository
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

        public async Task<ImmutableList<ChainLink>> GetAll()
        {
            var queryResult = await _cloudTable.ExecuteQuerySegmentedAsync(new TableQuery(),
                GetChainLinkResolver(), null);

            var list = ImmutableList<ChainLink>.Empty;
            return list.AddRange(queryResult.Results);
        }

        private static EntityResolver<ChainLink> GetChainLinkResolver()
        {
            return (partitionKey, rowKey, timestamp, properties, etag) => new ChainLink
            {
                Artist = properties["Artist"].StringValue,
                Position = properties["Position"].Int32Value.Value,
                PublishedDate = properties["PublishedDate"].StringValue,
                Track = properties["Track"].StringValue,
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Timestamp = timestamp,
                ETag = etag
            };
        }
    }
}
