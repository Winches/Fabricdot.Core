namespace Fabricdot.Authorization.Tests;

public class NullPermissionGrantingServiceTests : TestFor<NullPermissionGrantingService>
{
    [AutoData]
    [Theory]
    public async Task IsGrantedAsync_Should_AlwaysGranted(
        GrantSubject subject,
        string[] permissions)
    {
        var grantResults = await Sut.IsGrantedAsync(subject, permissions);

        grantResults.Should().OnlyContain(v => v.IsGranted);
    }
}