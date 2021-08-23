namespace Fabricdot.Infrastructure.Core.Uow.Abstractions
{
    public interface IUnitOfWorkManager
    {
        IUnitOfWork Available { get; }

        IUnitOfWork Begin(UnitOfWorkOptions options = null, bool requireNew = false);

        IUnitOfWork Reserve(string name, bool requireNew = false);

        void BeginReserved(string name, UnitOfWorkOptions options = null);
        
        bool TryBeginReserved(string name, UnitOfWorkOptions options = null);
    }
}