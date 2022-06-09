using Fabricdot.Core.Modularity;
using Fabricdot.Domain;

namespace Mall.Domain.Shared;

[Requires(typeof(FabricdotDomainModule))]
[Exports]
public class MallDomainSharedModule : ModuleBase
{
}
