using System;

namespace Fabricdot.Infrastructure.Uow.Abstractions
{
    public interface IUnitOfWorkScope : IDisposable
    {
        IServiceProvider ServiceProvider { get; }
        IUnitOfWorkFacade Facade { get; }

        event EventHandler Disposed;
    }
}