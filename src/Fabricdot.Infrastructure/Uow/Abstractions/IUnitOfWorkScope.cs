using System;
using System.Collections.Generic;

namespace Fabricdot.Infrastructure.Uow.Abstractions
{
    public interface IUnitOfWorkScope : IDisposable
    {
        IDictionary<object, object> Items { get; }

        IServiceProvider ServiceProvider { get; }

        IUnitOfWorkFacade Facade { get; }

        event EventHandler Disposed;
    }
}