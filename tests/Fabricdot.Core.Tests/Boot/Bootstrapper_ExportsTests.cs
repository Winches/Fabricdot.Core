using Fabricdot.Core.Boot;
using Fabricdot.Core.Tests.Modules;
using Fabricdot.Core.Tests.Modules.Exports;
using Fabricdot.Core.Tests.Modules.Exports.Core;
using Fabricdot.Core.Tests.Modules.Exports.Core.Subdomain;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.Boot
{
    public class Bootstrapper_ExportsTests
    {
        [Fact]
        public void BootstrapModules_ExportAll_RegisterServices()
        {
            var serviceType = typeof(FakeService);
            var options = new BootstrapperBuilderOptions();
            var builder = Bootstrapper.CreateBuilder(options).AddModules(typeof(FakeStartupModule));
            var services = builder.Services;

            services.Should()
                    .ContainSingle(v => v.ServiceType == serviceType);
            services.Should()
                    .ContainSingle(v => v.ServiceType == typeof(FakeCoreService));
        }

        [Fact]
        public void BootstrapModules_ExportNamespace_RegisterServices()
        {
            var options = new BootstrapperBuilderOptions();
            var builder = Bootstrapper.CreateBuilder(options).AddModules(typeof(FakeCoreModule));
            var services = builder.Services;

            services.Should().ContainSingle(v => v.ServiceType == typeof(FakeCoreService));
            services.Should().ContainSingle(v => v.ServiceType == typeof(FakeCoreSubDomainService));
            services.Should().NotContain(v => v.ServiceType == typeof(FakeService));
        }

        [Fact]
        public void BootstrapModules_AddModulesTwice_RegisterCorrectly()
        {
            var moduleType = typeof(FakeStartupModule);
            var bootstrapperBuilder = Bootstrapper.CreateBuilder(new BootstrapperBuilderOptions())
                                                  .AddModules(moduleType)
                                                  .AddModules(moduleType);

            bootstrapperBuilder.Services.Should().ContainSingle(v => v.ServiceType == typeof(FakeService));
        }

        [Fact]
        public void BootstrapModules_ModuleWithoutIConfigureService_IgnoreMethod()
        {
            var moduleType = typeof(FakeCustomModule);
            Bootstrapper.CreateBuilder(new BootstrapperBuilderOptions()).AddModules(moduleType);
        }
    }
}