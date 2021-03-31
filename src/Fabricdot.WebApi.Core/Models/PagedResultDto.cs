using System.Collections.Generic;

namespace Fabricdot.WebApi.Core.Models
{
    public class PagedResultDto<T> : IPagedResult<T>
    {
        /// <inheritdoc />
        public ICollection<T> List { get; set; }

        /// <inheritdoc />
        public long Total { get; set; }
    }
}