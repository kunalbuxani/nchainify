using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Xml.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
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

            var client = new HttpClient();
            var chainRssFeed = await XDocument.LoadAsync(await client.GetStreamAsync("https://www.thechain.uk/feed/"), LoadOptions.None, CancellationToken.None);
            var rawChainLinks = (from item in chainRssFeed.Descendants("item")
                                 select new
                                 {
                                     PositionArtistTrack = item.Element("title")?.Value,
                                     PublishedDate = item.Element("pubDate")?.Value
                                 }).ToImmutableList();

            var chainLinks = from chainLink in rawChainLinks
                             select new ChainLink
                             {
                                 Position = int.Parse(chainLink.PositionArtistTrack.Split('.').First()),
                                 Artist = chainLink.PositionArtistTrack.Split('.').Last().Split('\u2013').First().Trim(),
                                 Track = chainLink.PositionArtistTrack.Split('.').Last().Split('\u2013').Last().Trim(),
                                 PublishedDate = chainLink.PublishedDate,
                                 RowKey = int.Parse(chainLink.PositionArtistTrack.Split('.').First()).ToString(),
                             };

            await chainLinksCloudTable.CreateIfNotExistsAsync();

            Task.WaitAll(chainLinks.Select(chainLink => chainLinksCloudTable.ExecuteAsync(TableOperation.InsertOrMerge(chainLink))).ToArray());
        }
    }
}
