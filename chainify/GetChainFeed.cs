using Chainify.Extensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace Chainify
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

            await chainLinksCloudTable.CreateIfNotExistsAsync();

            Task.WaitAll(chainLinks.Select(chainLink => chainLinksCloudTable.ExecuteAsync(TableOperation.InsertOrMerge(chainLink))).ToArray());

            log.LogInformation("Updated data table.");
        }
    }
}
