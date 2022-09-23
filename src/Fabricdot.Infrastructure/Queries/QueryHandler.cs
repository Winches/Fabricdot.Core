using AutoMapper;

namespace Fabricdot.Infrastructure.Queries;

public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    protected IMapper Mapper { get; }

    protected QueryHandler(IMapper mapper)
    {
        Mapper = mapper;
    }

    public abstract Task<TResult> Handle(TQuery request, CancellationToken cancellationToken);
}