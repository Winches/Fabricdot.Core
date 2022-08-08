using Fabricdot.Core.Boot;
using Fabricdot.Core.Tests.Modules;
using Fabricdot.Core.Tests.Modules.Exports;
using Fabricdot.Core.Tests.Modules.Exports.Core;
using Fabricdot.Core.Tests.Modules.Exports.Core.Subdomain;

namespace Fabricdot.Core.Tests.Boot;

public class Bootstrapper_ExportsTests : TestFor<IBootstrapperBuilder>
{
    [Fact]
    public void BootstrapModules_ExportAll_RegisterServices()
    {
        var services = Sut.Services;
        Sut.AddModules(typeof(FakeStartupModule));

        services.Should().ContainSingle<FakeService>();
        services.Should().ContainSingle<FakeCoreService>();
    }

    [Fact]
    public void BootstrapModules_ExportNamespace_RegisterServices()
    {
        var services = Sut.Services;
        Sut.AddModules(typeof(FakeCoreModule));

        services.Should().ContainSingle<FakeCoreService>();
        services.Should().ContainSingle<FakeCoreSubDomainService>();
        services.Should().NotContain<FakeService>();
    }

    [Fact]
    public void BootstrapModules_AddModulesTwice_RegisterCorrectly()
    {
        var moduleType = typeof(FakeStartupModule);
        Sut.AddModules(moduleType)
           .AddModules(moduleType);

        Sut.Services.Should().ContainSingle<FakeService>();
    }

    [Fact]
    public void BootstrapModules_ModuleWithoutIConfigureService_IgnoreMethod()
    {
        Sut.AddModules(typeof(FakeCustomModule));
    }

    protected override IBootstrapperBuilder CreateSut()
    {
        var options = Create<BootstrapperBuilderOptions>();
        return Bootstrapper.CreateBuilder(options);
    }
}