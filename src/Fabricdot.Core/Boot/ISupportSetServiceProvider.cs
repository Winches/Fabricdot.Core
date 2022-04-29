using System;

namespace Fabricdot.Core.Boot
{
    public interface ISupportSetServiceProvider
    {
        IServiceProvider Services { get; }

        void SetServiceProvider(IServiceProvider serviceProvider);
    }
}