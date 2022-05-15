using Fabricdot.Core.Modularity;
using Fabricdot.Domain;

namespace Fabricdot.Identity.Domain
{
    [Requires(typeof(FabricdotDomainModule))]
    [Exports]
    public class FabricdotIdentityDomainModule : ModuleBase
    {
    }
}