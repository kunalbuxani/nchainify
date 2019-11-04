using Chainify.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chainify.Tests.Unit
{
    [TestFixture]
    public class ChainLinksRepositoryShould
    {
        [Test]
        public async Task InsertOrMergeChainLinks()
        {
            var chainLink1 = new ChainLink { RowKey = "1" };
            var chainLink2 = new ChainLink { RowKey = "2" };

            var cloudTableMock = new Mock<CloudTable>(new Uri("http://127.0.0.1:10002/devstoreaccount1/screenSettings"));
            cloudTableMock.Setup(d => d.ExecuteBatchAsync(It.IsAny<TableBatchOperation>()));
            cloudTableMock.Setup(d => d.CreateIfNotExistsAsync());

            var repo = new ChainLinkRepository(cloudTableMock.Object);
            await repo.UpdateChainLinks(new List<ChainLink> { chainLink1, chainLink2 });

            cloudTableMock.Verify(m => m.CreateIfNotExistsAsync(), Times.Once);
            cloudTableMock.Verify(m => m.ExecuteBatchAsync(It.IsAny<TableBatchOperation>()), Times.Once);
        }
    }
}
