using Chainify.Extensions;
using Chainify.Spotify;
using Chainify.Storage;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Environment;

namespace Chainify.UpdateTheChainMonthlyPlaylist
{
    public static class UpdateTheChainMonthlyPlaylist
    {
        [FunctionName("UpdateTheChainMonthlyPlaylist")]
        public static async Task Run(
                //[TimerTrigger("0 * * * * *")] TimerInfo timer,
                [TimerTrigger("0 0 0 1 * *")] TimerInfo timer,
                [Table("chainLinks", Connection = "AzureWebJobsStorage")] CloudTable chainLinksCloudTable,
                ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            log.LogInformation("Getting chain links...");

            var lastMonthsChainLinks = (await new ChainLinkRepository(chainLinksCloudTable)
                .GetAll())
                .GetLastMonthsChainLinks(DateTime.Today);

            log.LogInformation($"Got chain links {lastMonthsChainLinks.Min(l => l.Position)} to {lastMonthsChainLinks.Max(l => l.Position)}.");

            var client = new SpotifyClient(
                new SpotifyConfiguration(
                    GetEnvironmentVariable("SpotifyClientId"),
                    GetEnvironmentVariable("SpotifyClientSecret"),
                    GetEnvironmentVariable("SpotifyRefreshToken")),
                log);

            await client.RefreshPlaylist(lastMonthsChainLinks,
                new SpotifyPlaylist(
                    new Uri(GetEnvironmentVariable("SpotifyPlaylistUri")),
                    "The Chain Monthly", true, false,
                    $"{DateTime.Today.AddMonths(-1):MMMM} in The Chain, BBC Radio 6's listener-generated playlist of thematically linked songs. Check https://www.thechain.uk/ for more."
                    ));
        }
    }
}
