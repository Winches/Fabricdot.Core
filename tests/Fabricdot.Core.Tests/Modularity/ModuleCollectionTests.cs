using Fabricdot.Core.Modularity;
using Fabricdot.Core.Tests.Modules;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.Modularity
{
    public class ModuleCollectionTests
    {
        [Fact]
        public void Build_CannotSolveDependency_ThrowException()
        {
            var modules = new ModuleCollection
            {
                new ModuleMetadata(typeof(FakeStartupModule), new FakeStartupModule())
            };

            FluentActions.Invoking(() => modules.Build())
                         .Should()
                         .Throw<ModularityException>();
        }
    }
}