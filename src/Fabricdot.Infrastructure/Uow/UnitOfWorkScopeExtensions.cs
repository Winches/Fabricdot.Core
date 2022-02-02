using System.Collections.Generic;
using Ardalis.GuardClauses;
using Fabricdot.Infrastructure.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Uow
{
    public static class UnitOfWorkScopeExtensions
    {
        public const string HardDeletedEntity = "HardDeletedEntities";

        public static ISet<object> GetHardDeletedSet(this IUnitOfWorkScope unitOfWorkScope)
        {
            Guard.Against.Null(unitOfWorkScope, nameof(unitOfWorkScope));

            return (HashSet<object>)unitOfWorkScope.Items.GetOrAdd(
                HardDeletedEntity,
                _ => new HashSet<object>());
        }

        public static void MarkHardDeletedEntity(
            this IUnitOfWorkScope unitOfWorkScope,
            object entity)
        {
            Guard.Against.Null(unitOfWorkScope, nameof(unitOfWorkScope));
            Guard.Against.Null(entity, nameof(entity));

            var set = unitOfWorkScope.GetHardDeletedSet();
            set.Add(entity);
        }
    }
}