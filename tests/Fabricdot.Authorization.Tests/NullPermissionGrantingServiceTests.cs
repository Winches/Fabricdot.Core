using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Authorization.Tests;

public class NullPermissionGrantingServiceTests
{
    [Fact]
    public async Task IsGrantedAsync_Should_AlwaysGranted()
    {
        var permissionGrantingService = new NullPermissionGrantingService();
        var grantResults = await permissionGrantingService.IsGrantedAsync(
            new GrantSubject(GrantTypes.User, "1"),
            new[] { "object1" });

        grantResults.Should().OnlyContain(v => v.IsGranted);
    }
}