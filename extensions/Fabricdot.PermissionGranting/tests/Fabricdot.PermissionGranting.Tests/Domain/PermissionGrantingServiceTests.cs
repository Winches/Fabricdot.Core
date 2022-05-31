using System;
using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Authorization;
using Fabricdot.PermissionGranting.Tests.Data;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.PermissionGranting.Tests.Domain;

public class PermissionGrantingServiceTests : PermissionGrantingTestBase
{
    protected IPermissionGrantingService PermissionGrantingService { get; }

    public PermissionGrantingServiceTests()
    {
        PermissionGrantingService = ServiceProvider.GetRequiredService<IPermissionGrantingService>();
    }

    [Fact]
    public async Task IsGrantedAsync_GivenInvalidInput_Throw()
    {
        var subject = new GrantSubject("type", "a");

        await FluentActions.Awaiting(() => PermissionGrantingService.IsGrantedAsync(subject, null))
                           .Should()
                           .ThrowAsync<ArgumentNullException>();
        await FluentActions.Awaiting(() => PermissionGrantingService.IsGrantedAsync(subject, Array.Empty<string>()))
                           .Should()
                           .ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task IsGrantedAsync_GivenInput_ReturnCorrectly()
    {
        var subject = FakeDataBuilder.Subject;
        var grantedObjects = FakeDataBuilder.GrantedObjects;
        var ungrantedObjects = new[] { "ungranted1", "ungranted2" };
        var objects = grantedObjects.Union(ungrantedObjects).ToArray();
        var grantResults = await PermissionGrantingService.IsGrantedAsync(subject, objects);

        grantResults.Should().HaveCount(objects.Length);
        grantResults.Should().Contain(v => v.IsGranted && grantedObjects.Contains(v.Object));
        grantResults.Should().Contain(v => !v.IsGranted && ungrantedObjects.Contains(v.Object));
    }
}