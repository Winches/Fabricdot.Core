using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Randoms;

[Dependency(ServiceLifetime.Singleton)]
public class DefaultRandomProvider : IRandomProvider
{
    private readonly Random _random;

    public DefaultRandomProvider()
    {
        _random = new Random();
    }

    /// <inheritdoc />
    public int Next()
    {
        lock (_random)
        {
            return _random.Next();
        }
    }

    /// <inheritdoc />
    public int Next(int min, int max)
    {
        lock (_random)
        {
            return _random.Next(min, max);
        }
    }
}
