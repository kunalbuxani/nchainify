using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chainify
{
    public class TheChainUkClient
    {
        public TheChainUkClient()
        {
            
        }

        public async Task<XDocument> GetFeed() =>
            await XDocument.LoadAsync(await new HttpClient().GetStreamAsync("https://www.thechain.uk/feed/"),
                LoadOptions.None, CancellationToken.None);
    }
}
