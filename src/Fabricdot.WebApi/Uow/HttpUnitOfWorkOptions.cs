using System.Collections.Generic;

namespace Fabricdot.WebApi.Uow
{
    public class HttpUnitOfWorkOptions
    {
        public List<string> IgnoredUrls { get; } = new();
    }
}