using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.Core.Boot
{
    public interface IApplication : IDisposable
    {
        IServiceProvider Services { get; }

        Task StartAsync(CancellationToken cancellationToken = default);

        Task StopAsync(CancellationToken cancellationToken = default);
    }
}