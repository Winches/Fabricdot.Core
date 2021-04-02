using Fabricdot.Core.DependencyInjection;

namespace Fabricdot.Core.Randoms
{
    public interface IRandomProvider : ISingletonDependency
    {
        int Next();
        int Next(int min, int max);
    }
}