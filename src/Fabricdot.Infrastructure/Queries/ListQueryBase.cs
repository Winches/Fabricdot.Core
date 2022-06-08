using System.Collections.Generic;

namespace Fabricdot.Infrastructure.Queries;

public abstract class ListQueryBase<TResult> : IQuery<IList<TResult>>
{
}