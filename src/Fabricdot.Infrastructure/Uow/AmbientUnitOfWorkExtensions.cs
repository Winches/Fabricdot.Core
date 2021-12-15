using System;
using Ardalis.GuardClauses;
using Fabricdot.Infrastructure.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Uow
{
    public static class AmbientUnitOfWorkExtensions
    {
        public static IUnitOfWork Find(this IAmbientUnitOfWork ambientUnitOfWork, Predicate<IUnitOfWork> predicate)
        {
            Guard.Against.Null(ambientUnitOfWork, nameof(ambientUnitOfWork));
            Guard.Against.Null(predicate, nameof(predicate));

            var uow = ambientUnitOfWork.UnitOfWork;
            while (uow != null && !predicate(uow))
            {
                uow = ambientUnitOfWork.GetOuter(uow);
            }

            return uow;
        }
    }
}