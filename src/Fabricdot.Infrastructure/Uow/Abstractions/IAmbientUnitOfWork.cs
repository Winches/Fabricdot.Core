namespace Fabricdot.Infrastructure.Uow.Abstractions
{
    public interface IAmbientUnitOfWork
    {
        IUnitOfWork UnitOfWork { get; set; }

        void DropCurrent();

        IUnitOfWork GetOuter(IUnitOfWork unitOfWork);
    }
}