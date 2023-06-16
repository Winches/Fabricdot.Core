using Fabricdot.Testing.AutoFixture;

namespace Fabricdot.Testing;

public static class FixtureFactory
{
    public static IFixture Create()
    {
        return new Fixture().Customize(new AutoCustomization());
    }
}
