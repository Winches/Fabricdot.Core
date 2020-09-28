using System;
using System.Threading.Tasks;
using Fabricdot.Domain.Core.Services;

namespace Fabricdot.Infrastructure.Core.Domain.Services
{
    public class GuidGenerator : IIdGenerator
    {
        public Task<string> NextAsync()
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }
    }
}