using Ardalis.GuardClauses;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Core.Uow
{
    public class DefaultUnitOfWorkTransactionBehaviourProvider : IUnitOfWorkTransactionBehaviourProvider, ISingletonDependency
    {
        /// <inheritdoc />
        public virtual bool GetBehaviour(string actionName)
        {
            Guard.Against.NullOrEmpty(actionName, nameof(actionName));

            return !actionName.StartsWith("Get", System.StringComparison.InvariantCultureIgnoreCase);
        }
    }
}