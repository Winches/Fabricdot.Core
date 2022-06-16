using Fabricdot.Testing;

namespace Xunit;
public class AutoMockDataAttribute : AutoDataAttribute
{
    public AutoMockDataAttribute() : base(FixtureFactory.Create)
    {
    }
}
