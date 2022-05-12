using System;
using Fabricdot.Core.Modularity;

namespace Fabricdot.Core.Tests.Modules
{
    internal class FakeCustomModule : IModule
    {
        public void ConfigureServices(ConfigureServiceContext context)
        {
            throw new NotSupportedException("This method should not be invoke.");
        }
    }
}