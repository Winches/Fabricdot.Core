using System.Collections.Generic;

namespace Fabricdot.WebApi.Models
{
    public class PagedResultDto<T> : IPagedResult<T>
    {
        /// <inheritdoc />
        public ICollection<T> List { get; set; }

        /// <inheritdoc />
        public long Total { get; set; }
    }
}