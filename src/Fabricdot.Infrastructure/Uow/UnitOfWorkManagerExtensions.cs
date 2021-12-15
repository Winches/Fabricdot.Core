using System.Data;
using Ardalis.GuardClauses;
using Fabricdot.Infrastructure.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Uow
{
    public static class UnitOfWorkManagerExtensions
    {
        public static void Begin(
            this IUnitOfWorkManager unitOfWorkManager,
            bool requireNew = false,
            bool? isTransactional = null,
            IsolationLevel? isolationLevel = null)
        {
            Guard.Against.Null(unitOfWorkManager, nameof(unitOfWorkManager));
            var options = new UnitOfWorkOptions();
            if (isTransactional.HasValue)
                options.IsTransactional = isTransactional.Value;
            if (isolationLevel.HasValue)
                options.IsolationLevel = isolationLevel.Value;

            unitOfWorkManager.Begin(options, requireNew);
        }
    }
}