namespace Fabricdot.Infrastructure.Core.Uow.Abstractions
{
    public enum UnitOfWorkState
    {
        Allocated = 0,
        Initialized,
        Performed,
        Disposed
    }
}