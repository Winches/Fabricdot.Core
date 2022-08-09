using System.Security.Claims;
using Fabricdot.PermissionGranting.Domain;
using Fabricdot.PermissionGranting.Tests.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.PermissionGranting.Tests;

public class PermissionGrantingManagerTests : PermissionGrantingTestBase
{
    protected IPermissionGrantingManager PermissionGrantingManager { get; }

    protected IGrantedPermissionRepository GrantedPermissionRepository { get; }

    public PermissionGrantingManagerTests()
    {
        PermissionGrantingManager = ServiceProvider.GetRequiredService<IPermissionGrantingManager>();
        GrantedPermissionRepository = ServiceProvider.GetRequiredService<IGrantedPermissionRepository>();
    }

    [AutoData]
    [Theory]
    public async Task GrantAsync_GivenInput_Correctly(string @object)
    {
        var subject = FakeDataBuilder.Subject;
        await PermissionGrantingManager.GrantAsync(subject, @object);
        var isGranted = await GrantedPermissionRepository.AnyAsync(subject, @object);

        isGranted.Should().BeTrue();
    }

    [Fact]
    public async Task GrantAsync_InvokeTwice_IgnoreDuplicated()
    {
        var subject = FakeDataBuilder.Subject;
        var @object = FakeDataBuilder.GrantedObjects[0];
        await PermissionGrantingManager.GrantAsync(subject, @object);
        var grantedPermissions = await GrantedPermissionRepository.ListAsync(subject);

        grantedPermissions.Should().ContainSingle(v => v.Object == @object);
    }

    [AutoData]
    [Theory]
    public async Task RevokeAsync_GivenUngrantedObject_Ignore(string @object)
    {
        var subject = FakeDataBuilder.Subject;

        var isGranted = await GrantedPermissionRepository.AnyAsync(subject, @object);
        isGranted.Should().BeFalse();

        await PermissionGrantingManager.RevokeAsync(subject, @object);
    }

    [Fact]
    public async Task RevokeAsync_GivenInput_Correctly()
    {
        var subject = FakeDataBuilder.Subject;
        var @object = FakeDataBuilder.GrantedObjects[0];
        await PermissionGrantingManager.RevokeAsync(subject, @object);
        var isGranted = await GrantedPermissionRepository.AnyAsync(subject, @object);

        isGranted.Should().BeFalse();
    }

    [AutoData]
    [Theory]
    public async Task SetAsync_GivenInput_Correctly(string @object)
    {
        var subject = FakeDataBuilder.Subject;
        var objects = new[] { FakeDataBuilder.GrantedObjects[0], @object };

        await PermissionGrantingManager.SetAsync(subject, objects);
        var grantedPermissions = await GrantedPermissionRepository.ListAsync(subject);

        grantedPermissions.Should().HaveCount(objects.Length);
        grantedPermissions.Select(v => v.Object).Should().BeEquivalentTo(objects);
    }

    [Fact]
    public async Task ListAsync_GivenSubject_ReturnCorrectly()
    {
        var subject = FakeDataBuilder.Subject;
        var grantedPermissions = await PermissionGrantingManager.ListAsync(subject);

        grantedPermissions.Should().Contain(v => FakeDataBuilder.GrantedObjects.Contains(v.Object));
        grantedPermissions.Should().OnlyContain(v => v.Subject == subject.Value);
    }

    [Fact]
    public async Task ListAsync_GivenSubjects_ReturnCorrectly()
    {
        var subject = FakeDataBuilder.Subject;
        var grantedPermissions = await PermissionGrantingManager.ListAsync(new[] { subject });

        grantedPermissions.Should().Contain(v => FakeDataBuilder.GrantedObjects.Contains(v.Object));
        grantedPermissions.Should().OnlyContain(v => v.Subject == subject.Value);
    }

    [Fact]
    public async Task ListAsync_GivenClaimPrincipal_ReturnCorrectly()
    {
        var subject = FakeDataBuilder.Subject;
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, subject.Value) };
        var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var grantedPermissions = await PermissionGrantingManager.ListAsync(claimPrincipal);

        grantedPermissions.Should().Contain(v => FakeDataBuilder.GrantedObjects.Contains(v.Object));
        grantedPermissions.Should().OnlyContain(v => v.Subject == subject.Value);
    }
}