using System.Linq.Expressions;

namespace Fabricdot.Identity.Domain.Repositories;

/// <summary>
///     Make sure aggregate is fully loaded.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface ISupportExplicitLoading<TEntity> //where TEntity : class
{
    Task LoadReferenceAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        CancellationToken cancellationToken = default) where TProperty : class;

    Task LoadCollectionAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken = default) where TProperty : class;
}
