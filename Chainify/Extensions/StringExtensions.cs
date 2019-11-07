using System.Linq;
using System.Text.RegularExpressions;
using Chainify.Storage;

namespace Chainify.Extensions
{
    public static class StringExtensions
    {
        public static ChainLink ToChainLink(this (string title, string pubDate) rawChainLink)
        {
            var artistRegex = new Regex(@"(?<=[0-9]{4}\. ).*");

            var link = new ChainLink
            {
                Position = int.Parse(rawChainLink.title.Split('.').First()),
                Artist = artistRegex.Match(rawChainLink.title).Value.Split("–").First().Trim()
            };
            link.Track = rawChainLink.title.Replace($"{link.Position}. {link.Artist} – ", string.Empty).Trim();
            link.PublishedDate = rawChainLink.pubDate;
            link.RowKey = link.Position.ToString();
            return link;
        }
    }
}
