using System;
using System.Threading.Tasks;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;

namespace Fabricdot.Core.Tests.Modules;

internal class FakeRuntimeErrorModule : ModuleBase
{
    internal const string ConfigureServicesErrorMessage = nameof(ConfigureServicesErrorMessage);

    internal const string OnStartingErrorMessage = nameof(OnStartingErrorMessage);

    internal const string OnStoppingErrorMessage = nameof(OnStoppingErrorMessage);

    public override void ConfigureServices(ConfigureServiceContext context)
    {
        throw new Exception(ConfigureServicesErrorMessage);
    }

    public override Task OnStartingAsync(ApplicationStartingContext context)
    {
        throw new Exception(OnStartingErrorMessage);
    }

    public override Task OnStoppingAsync(ApplicationStoppingContext context)
    {
        throw new Exception(OnStoppingErrorMessage);
    }
}