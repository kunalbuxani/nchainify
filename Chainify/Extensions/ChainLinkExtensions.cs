using Chainify.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Chainify.Extensions
{
    public static class ChainLinkExtensions
    {
        public static ImmutableList<ChainLink> GetLastMonthsChainLinks(this IEnumerable<ChainLink> chainLinks, DateTime dateTime)
        {
            var list = ImmutableList<ChainLink>.Empty;
            var startOfLastMonth = new DateTime(dateTime.AddMonths(-1).Year, dateTime.AddMonths(-1).Month, 1);
            var startOfCurrentMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var positionsForLastMonth = chainLinks.Select(l => new
                {
                    l.Position,
                    PublishedDate = DateTime.ParseExact(l.PublishedDate, "ddd, dd MMM yyyy HH:mm:ss +0000", CultureInfo.CurrentCulture),
                })
                .Where(f => f.PublishedDate > startOfLastMonth && f.PublishedDate < startOfCurrentMonth)
                .Select(f => f.Position);

            return list.AddRange(chainLinks.Where(f => positionsForLastMonth.Contains(f.Position)));
        }
    }
}
