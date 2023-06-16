using Fabricdot.Core.Modularity;
using Fabricdot.Core.Tests.Modules;

namespace Fabricdot.Core.Tests.Modularity;

public class ModuleMetadataTests : TestBase
{
    [Fact]
    public void Constructor_WhenInstanceTypeIsNotMatch_ThrowException()
    {
        Invoking(() => new ModuleMetadata(typeof(FakeStartupModule), new FakeInfrastructureModule()))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_GivenDependencies_Correctly()
    {
        var dependency = new ModuleMetadata(typeof(FakeInfrastructureModule), new FakeInfrastructureModule());
        var module = new ModuleMetadata(
            nameof(FakeStartupModule),
            typeof(FakeStartupModule),
            new FakeStartupModule(),
            dependency,
            dependency);

        module.Dependencies.Should().ContainSingle(dependency);
    }

    [Fact]
    public void Equals_GivenInput_ReturnCorrectly()
    {
        var module = new ModuleMetadata(
            nameof(FakeStartupModule),
            typeof(FakeStartupModule),
            new FakeStartupModule());
        var dependency = new ModuleMetadata(typeof(FakeInfrastructureModule), new FakeInfrastructureModule());

        module.Should()
              .NotBeNull().And
              .BeEquivalentTo(module).And
              .NotBeEquivalentTo(dependency);
    }
}
