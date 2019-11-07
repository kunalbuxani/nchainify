using Microsoft.WindowsAzure.Storage.Table;

namespace Chainify.Storage
{
    public class ChainLink : TableEntity
    {
        public int Position { get; set; }
        public string Artist { get; set; }
        public string Track { get; set; }
        public string PublishedDate { get; set; }

        public ChainLink()
        {
            PartitionKey = "ChainLinks";
        }

        public override string ToString() => $"{PartitionKey}|{RowKey}|{Position}|{Artist}|{Track}|{PublishedDate}";
    }
}