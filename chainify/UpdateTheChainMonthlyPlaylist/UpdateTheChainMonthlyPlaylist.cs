using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chainify.Extensions;
using Chainify.Storage;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace Chainify.UpdateTheChainMonthlyPlaylist
{
    public static class UpdateTheChainMonthlyPlaylist
    {
        [FunctionName("UpdateTheChainMonthlyPlaylist")]
        public static async Task Run(
                [TimerTrigger("0 * * * * *")] TimerInfo timer,
                [Table("chainLinks", Connection = "AzureWebJobsStorage")] CloudTable chainLinksCloudTable,
                ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            log.LogInformation("Getting chain links...");
            
            var lastMonthsChainLinks = (await new ChainLinkRepository(chainLinksCloudTable)
                .GetAll())
                .GetLastMonthsChainLinks(DateTime.Today);
            
            log.LogInformation($"Got chain links {lastMonthsChainLinks.Min(l => l.Position)} to {lastMonthsChainLinks.Max(l => l.Position)}.");


        }
    }
}
