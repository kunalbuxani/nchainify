using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chainify.Extensions;
using NUnit.Framework;

namespace Chainify.Tests.Unit
{
    [TestFixture]
    public class StringExtensionsToChainLinkShould
    {
        [Test]
        public void Return_ChainLink_FromTitleAndPubDateExtractedFromRss()
        {
            var list = new List<(string title, string pubDate)>
            {
                ("7981. Otis Redding – You Don’t Miss Your Water", "Sat, 26 Oct 2019 08:23:52 +0000"),
                ("7980. Free – Mr. Big", "Sat, 26 Oct 2019 07:17:50 +0000"),
                ("7979. Mr. Mister – Broken Wings", "Sat, 25 Oct 2019 07:17:50 +0000"),
                ("7972. Earl King – Come On – Part I", "Sat, 24 Oct 2019 07:17:50 +0000")
            }.ToImmutableList();

            Assert.That(list.Select(r => r.ToChainLink()), Is.EquivalentTo(new List<ChainLink>
            {
                new ChainLink
                {
                    RowKey = "7981",
                    PublishedDate = "Sat, 26 Oct 2019 08:23:52 +0000",
                    Artist = "Otis Redding",
                    Position = 7981,
                    PartitionKey = "ChainLinks",
                    Track = "You Don’t Miss Your Water",
                },
                new ChainLink
                {
                    RowKey = "7980",
                    PublishedDate = "Sat, 26 Oct 2019 07:17:50 +0000",
                    Artist = "Free",
                    Position = 7980,
                    PartitionKey = "ChainLinks",
                    Track = "Mr. Big",
                },
                new ChainLink
                {
                    RowKey = "7979",
                    PublishedDate = "Sat, 25 Oct 2019 07:17:50 +0000",
                    Artist = "Mr. Mister",
                    Position = 7979,
                    PartitionKey = "ChainLinks",
                    Track = "Broken Wings",
                },
                new ChainLink
                {
                    RowKey = "7972",
                    PublishedDate = "Sat, 24 Oct 2019 07:17:50 +0000",
                    Artist = "Earl King",
                    Position = 7972,
                    PartitionKey = "ChainLinks",
                    Track = "Come On – Part I",
                },
            })
                .Using(new Func<ChainLink, ChainLink, bool>((actual, expected) =>
                  actual.RowKey == expected.RowKey && actual.PublishedDate == expected.PublishedDate && actual.Artist == expected.Artist &&
                  actual.Position == expected.Position && actual.PartitionKey == expected.PartitionKey && actual.Track == expected.Track)));
        }
    }
}
