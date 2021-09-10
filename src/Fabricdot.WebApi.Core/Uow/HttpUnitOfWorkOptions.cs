using System.Collections.Generic;

namespace Fabricdot.WebApi.Core.Uow
{
    public class HttpUnitOfWorkOptions
    {
        public List<string> IgnoredUrls { get; } = new();
    }
}