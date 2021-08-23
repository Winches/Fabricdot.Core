using System;

namespace Fabricdot.Infrastructure.Core.Uow.Abstractions
{
    public interface IUnitOfWorkScope : IDisposable
    {
        IServiceProvider ServiceProvider { get; }
        IUnitOfWorkFacade Facade { get; }

        event EventHandler Disposed;
    }
}