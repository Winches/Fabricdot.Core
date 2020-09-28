using System.Threading.Tasks;

namespace Fabricdot.Domain.Core.Services
{
    public interface IIdGenerator
    {
        Task<string> NextAsync();
    }
}