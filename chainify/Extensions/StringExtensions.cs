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
            var artistRegex = new Regex(@"(?<=[0-9]{4}\. ).*(?= –)");

            var link = new ChainLink
            {
                Position = int.Parse(rawChainLink.title.Split('.').First()),
                Artist = artistRegex.Match(rawChainLink.title).Value,
                Track = rawChainLink.title.Split('\u2013').Last().Trim(),
                PublishedDate = rawChainLink.pubDate,
                RowKey = int.Parse(rawChainLink.title.Split('.').First()).ToString()
            };
            return link;
        }
    }
}
