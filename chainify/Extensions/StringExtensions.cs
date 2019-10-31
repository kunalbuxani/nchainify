using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chainify.Extensions
{
    public static class StringExtensions
    {
        public static ChainLink ToChainLink(this (string title, string pubDate) rawChainLink)
        {
            var artistRegex = new Regex(@"(?<=[0-9]{4}\. ).*");

            var link = new ChainLink();
            link.Position = int.Parse(rawChainLink.title.Split('.').First());
            link.Artist = artistRegex.Match(rawChainLink.title).Value.Split("–").First().Trim();
            link.Track = rawChainLink.title.Replace($"{link.Position}. {link.Artist} – ", string.Empty).Trim();
            link.PublishedDate = rawChainLink.pubDate;
            link.RowKey = link.Position.ToString();
            return link;
        }
    }
}
