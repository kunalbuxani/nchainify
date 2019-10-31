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

            var rawChainLinks = (await new TheChainUkClient().GetFeed()).ExtractTitlesAndDates();

            log.LogInformation("Got latest feed");
            
            var chainLinks = from chainLink in rawChainLinks
                             select new ChainLink
                             {
                                 Position = int.Parse(chainLink.title.Split('.').First()),
                                 Artist = chainLink.title.Split('.').Last().Split('\u2013').First().Trim(),
                                 Track = chainLink.title.Split('.').Last().Split('\u2013').Last().Trim(),
                                 PublishedDate = chainLink.pubDate,
                                 RowKey = int.Parse(chainLink.pubDate.Split('.').First()).ToString(),
                             };
            log.LogInformation("Updating data table...");

            await chainLinksCloudTable.CreateIfNotExistsAsync();

            Task.WaitAll(chainLinks.Select(chainLink => chainLinksCloudTable.ExecuteAsync(TableOperation.InsertOrMerge(chainLink))).ToArray());

            log.LogInformation("Updated data table.");
        }
    }
}
