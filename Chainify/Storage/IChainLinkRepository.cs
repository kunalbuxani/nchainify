using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Chainify.Storage
{
    public interface IChainLinkRepository
    {
        Task UpdateChainLinks(IEnumerable<ChainLink> chainLinks);
        Task<ImmutableList<ChainLink>> GetAll();
    }
}