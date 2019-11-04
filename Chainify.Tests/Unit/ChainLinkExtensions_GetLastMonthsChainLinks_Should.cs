using Chainify.Extensions;
using Chainify.Storage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chainify.Tests.Unit
{
    [TestFixture]
    public class ChainLinkExtensionsGetLastMonthsChainLinksShould
    {
        [Test]
        public static void Return_ChainLinksPublishedLastMonth_BasedOnInputDate()
        {
            var septemberChainLink = new ChainLink { Position = 1, PublishedDate = "Sat, 28 Sep 2019 08:23:52 +0000" };
            var novemberChainLink = new ChainLink { Position = 4, PublishedDate = "Mon, 04 Nov 2019 08:23:52 +0000" };
            var chainLinks = new List<ChainLink>
            {
                septemberChainLink,
                new ChainLink {Position = 2, PublishedDate = "Sat, 26 Oct 2019 08:23:52 +0000"},
                new ChainLink {Position = 3, PublishedDate = "Sat, 26 Oct 2019 07:17:50 +0000"},
                novemberChainLink,
            };

            Assert.That(chainLinks.GetLastMonthsChainLinks(new DateTime(2019, 11, 02)),
                Is.EquivalentTo(chainLinks.Except(chainLinks.Where(l => l == septemberChainLink || l == novemberChainLink))));
        }
    }
}
