namespace Fabricdot.Infrastructure.Queries;

/// <summary>
///     query processor
/// </summary>
public interface IQueryProcessor
{
    Task<TResult> ProcessAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default);
}
