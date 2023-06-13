using Fabricdot.Authorization;
using Fabricdot.PermissionGranting.Tests.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.PermissionGranting.Tests.Domain;

public class PermissionGrantingServiceTests : PermissionGrantingTestBase
{
    protected IPermissionGrantingService PermissionGrantingService { get; }

    public PermissionGrantingServiceTests()
    {
        PermissionGrantingService = ServiceProvider.GetRequiredService<IPermissionGrantingService>();
    }

    [AutoData]
    [Theory]
    public async Task IsGrantedAsync_GivenInvalidInput_Throw(GrantSubject subject)
    {
        await Awaiting(() => PermissionGrantingService.IsGrantedAsync(subject, null!))
                           .Should()
                           .ThrowAsync<ArgumentNullException>();
        await Awaiting(() => PermissionGrantingService.IsGrantedAsync(subject, Array.Empty<string>()))
                           .Should()
                           .ThrowAsync<ArgumentException>();
    }

    [AutoData]
    [Theory]
    public async Task IsGrantedAsync_GivenInput_ReturnCorrectly(string[] ungrantedObjects)
    {
        var subject = FakeDataBuilder.Subject;
        var grantedObjects = FakeDataBuilder.GrantedObjects;
        var objects = grantedObjects.Union(ungrantedObjects).ToArray();
        var grantResults = await PermissionGrantingService.IsGrantedAsync(subject, objects);

        grantResults.Should().HaveCount(objects.Length);
        grantResults.Should().Contain(v => v.IsGranted && grantedObjects.Contains(v.Object));
        grantResults.Should().Contain(v => !v.IsGranted && ungrantedObjects.Contains(v.Object));
    }
}
