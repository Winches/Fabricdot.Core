namespace Fabricdot.Infrastructure.Uow.Abstractions;

public interface IUnitOfWorkTransactionBehaviourProvider
{
    /// <summary>
    ///     Get transaction behaviour of unit-of-work
    /// </summary>
    /// <param name="actionName"></param>
    /// <returns>true if the behaviour is transactional; otherwise, false.</returns>
    bool GetBehaviour(string actionName);
}
