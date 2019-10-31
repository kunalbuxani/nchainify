using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Chainify;

namespace Chainify.Tests.Integration
{
    [TestFixture]
    public class TheChainUkClientShould
    {
        [Test]
        public async Task Return_XDocumentWithChainRssFeed()
        {
            var feed = await new TheChainUkClient().GetFeed();
            Assert.That(feed, Is.InstanceOf(typeof(XDocument)));
            Assert.That(feed.Element("rss").Element("channel").Element("title").Value, Is.EqualTo("The Chain"));
        }
    }
}
