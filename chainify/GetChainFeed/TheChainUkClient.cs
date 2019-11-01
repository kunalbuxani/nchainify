using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chainify.GetChainFeed
{
    public class TheChainUkClient
    {
        public async Task<XDocument> GetFeed() =>
            await XDocument.LoadAsync(await new HttpClient().GetStreamAsync("https://www.thechain.uk/feed/"),
                LoadOptions.None, CancellationToken.None);
    }
}
