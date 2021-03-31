using System.Collections.Generic;

namespace Fabricdot.WebApi.Core.Models
{
    public interface IListResult<T>
    {
        ICollection<T> List { get; set; }
    }
}