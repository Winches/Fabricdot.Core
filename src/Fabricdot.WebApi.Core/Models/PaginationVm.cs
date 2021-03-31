using System;
using System.Collections.Generic;

namespace Fabricdot.WebApi.Core.Models
{
    [Obsolete("use PagedResultDto")]//todo:remove this class
    public class PaginationVm<T>
    {
        public IEnumerable<T> List { get; set; }

        public int Total { get; set; }
    }
}