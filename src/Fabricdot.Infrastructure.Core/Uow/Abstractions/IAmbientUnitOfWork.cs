namespace Fabricdot.Infrastructure.Core.Uow.Abstractions
{
    public interface IAmbientUnitOfWork
    {
        IUnitOfWork UnitOfWork { get; set; }

        void DropCurrent();

        IUnitOfWork GetOuter(IUnitOfWork unitOfWork);
    }
}