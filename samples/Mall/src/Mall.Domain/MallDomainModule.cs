using Fabricdot.Core.Modularity;
using Fabricdot.Domain;
using Mall.Domain.Shared;

namespace Mall.Domain;

[Requires(typeof(MallDomainSharedModule))]
[Requires(typeof(FabricdotDomainModule))]
[Exports]
public class MallDomainModule : ModuleBase
{
}