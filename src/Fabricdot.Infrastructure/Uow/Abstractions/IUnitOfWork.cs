using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.Infrastructure.Uow.Abstractions;

public interface IUnitOfWork : IUnitOfWorkScope
{
    Guid Id { get; }

    bool IsActive { get; }

    UnitOfWorkOptions Options { get; }

    string? ReservationName { get; }

    void Reserve(string name);

    void Initialize(UnitOfWorkOptions options);

    Task CommitChangesAsync(CancellationToken cancellationToken = default);
}