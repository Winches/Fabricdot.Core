using Ardalis.GuardClauses;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Core.Uow
{
    public static class UnitOfWorkExtensions
    {
        public static bool IsReservedFor(this IUnitOfWork unitOfWork, string name)
        {
            Guard.Against.Null(unitOfWork, nameof(unitOfWork));
            return unitOfWork.ReservationName != null && unitOfWork.ReservationName == name;
        }
    }
}