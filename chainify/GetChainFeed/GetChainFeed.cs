using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Chainify.Extensions;
using Chainify.Storage;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace Chainify.GetChainFeed
{
    public static class GetChainFeed
    {
        [FunctionName("GetChainFeed")]
        public static async Task Run(
            [TimerTrigger("0 * * * * *")]TimerInfo myTimer,
            [Table("chainLinks", Connection = "AzureWebJobsStorage")] CloudTable chainLinksCloudTable,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            log.LogInformation($"Getting latest feed...");

            var chainLinks = (await new TheChainUkClient().GetFeed())
                .ExtractTitlesAndDates()
                .Select(c => c.ToChainLink());

            log.LogInformation("Got latest feed");
            
            log.LogInformation("Updating data table...");

            var repo = new ChainLinkRepository(chainLinksCloudTable);
            await repo.UpdateChainLinks(chainLinks.ToImmutableList());

            log.LogInformation("Updated data table.");
        }
    }
}
