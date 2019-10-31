using Microsoft.WindowsAzure.Storage.Table;

namespace Chainify
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
    }
}