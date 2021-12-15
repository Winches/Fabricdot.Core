namespace Fabricdot.WebApi.Models
{
    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {
    }
}