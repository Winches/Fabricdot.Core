namespace Fabricdot.Testing.AutoFixture;

public class AutoCustomization : CompositeCustomization
{
    public AutoCustomization() : base(
        new AutoMoqCustomization(),
        new CommonCustomization())
    {
    }
}
