using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Chainify.Extensions;
using NUnit.Framework;


namespace Chainify.Tests.Unit
{
    [TestFixture]
    public class XDocumentExtensionsExtractTitlesAndDatesShould
    {
        [Test]
        public void Return_RssTitleAndDateAsTuple_When_RssXDocumentIsInput()
        {

            var xdoc = XDocument.Parse(File.ReadAllText(@"Unit\mockfeed.xml"));

            Assert.That(xdoc.ExtractTitlesAndDates(), Is.EquivalentTo(new List<(string, string)>
            {
                ("7981. Otis Redding – You Don’t Miss Your Water", "Sat, 26 Oct 2019 08:23:52 +0000"),
                ("7980. Free – Mr. Big", "Sat, 26 Oct 2019 07:17:50 +0000"),
            }));
        }
    }
}
