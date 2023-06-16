using Ardalis.GuardClauses;
using Fabricdot.Infrastructure.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Uow;

public static class UnitOfWorkExtensions
{
    public static bool IsReservedFor(this IUnitOfWork unitOfWork, string name)
    {
        Guard.Against.Null(unitOfWork, nameof(unitOfWork));
        return unitOfWork.ReservationName != null && unitOfWork.ReservationName == name;
    }
}
