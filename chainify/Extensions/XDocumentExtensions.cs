using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
