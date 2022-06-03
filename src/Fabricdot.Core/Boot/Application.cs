using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.Core.Boot
{
    public abstract class Application : IApplication
    {
        private bool _disposedValue;

        public IServiceProvider Services { get; protected set; } = null!;

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public virtual Task StartAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public virtual Task StopAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    (Services as IDisposable)?.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}