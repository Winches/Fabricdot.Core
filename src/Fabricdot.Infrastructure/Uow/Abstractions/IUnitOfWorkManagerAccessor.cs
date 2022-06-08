namespace Fabricdot.Infrastructure.Uow.Abstractions;

public interface IUnitOfWorkManagerAccessor
{
    IUnitOfWorkManager UnitOfWorkManager { get; }
}