using System;
using System.Threading.Tasks;

namespace Fabricdot.Domain.Core.Services
{
    [Obsolete("using IGuidGenerator")]
    public interface IIdGenerator
    {
        Task<string> NextAsync();
    }
}