using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Linq;

namespace Chainify.Extensions
{
    public static class XDocumentExtensions
    {
        public static ImmutableList<(string title, string pubDate)> ExtractTitlesAndDates(this XDocument chainRssFeed)
        {
            return chainRssFeed
                .Descendants("item")
                .Select(item => (item.Element("title").Value, item.Element("pubDate").Value))
                .ToImmutableList();
        }
    }
}
