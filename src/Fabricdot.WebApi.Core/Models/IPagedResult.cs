namespace Fabricdot.WebApi.Core.Models
{
    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {
    }
}