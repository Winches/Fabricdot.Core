using System.Security.Claims;

namespace Fabricdot.Testing.AutoFixture;

public class CommonCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        CustomizeFactory(fixture, f => new Claim(f.Create<string>(), f.Create<string>()));
        CustomizeFactory<GuardClauseAssertion>(fixture, f => new CommonGuardClauseAssertion(f));
    }

    private static void CustomizeFactory<T>(IFixture fixture, Func<IFixture, T> factory)
    {
        fixture.Customize<T>(sub => sub.FromFactory(() => factory(fixture)));
    }
}