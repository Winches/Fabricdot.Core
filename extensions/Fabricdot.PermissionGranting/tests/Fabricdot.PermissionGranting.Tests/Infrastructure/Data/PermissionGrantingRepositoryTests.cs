using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Authorization;
using Fabricdot.PermissionGranting.Domain;
using Fabricdot.PermissionGranting.Tests.Data;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.PermissionGranting.Tests.Infrastructure.Data;

public class GrantedPermissionRepositoryTests : PermissionGrantingTestBase
{
    protected IGrantedPermissionRepository GrantedPermissionRepository { get; }

    public GrantedPermissionRepositoryTests()
    {
        GrantedPermissionRepository = ServiceProvider.GetRequiredService<IGrantedPermissionRepository>();
    }

    [Fact]
    public async Task AnyAsync_GivenInput_ReturnCorrectly()
    {
        var isExisted = await GrantedPermissionRepository.AnyAsync(
            FakeDataBuilder.Subject,
            FakeDataBuilder.GrantedObjects[0]);

        isExisted.Should().BeTrue();
    }

    [Fact]
    public async Task GetAsync_GivenInput_ReturnCorrectly()
    {
        var subject = FakeDataBuilder.Subject;
        var @object = FakeDataBuilder.GrantedObjects[0];
        var expected = new
        {
            GrantType = subject.Type,
            Subject = subject.Value,
            Object = @object
        };
        var grantedPermission = await GrantedPermissionRepository.GetAsync(subject, @object);

        grantedPermission.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ListAsync_GivenSubject_ReturnCorrectly()
    {
        var grantedPermissions = await GrantedPermissionRepository.ListAsync(FakeDataBuilder.Subject);

        grantedPermissions.Select(v => v.Object)
                          .Should()
                          .BeEquivalentTo(FakeDataBuilder.GrantedObjects);
    }

    [Fact]
    public async Task ListAsync_GivenSubjectWithObjects_ReturnCorrectly()
    {
        var ungrantedObjects = new[] { "ungranted1", "ungranted2" };
        var grantedObjects = FakeDataBuilder.GrantedObjects;
        var objects = grantedObjects.Union(ungrantedObjects).ToArray();
        var grantedPermissions = await GrantedPermissionRepository.ListAsync(FakeDataBuilder.Subject, objects);

        grantedPermissions.Should().Contain(v => grantedObjects.Contains(v.Object));
        grantedPermissions.Should().NotContain(v => ungrantedObjects.Contains(v.Object));
    }

    [Fact]
    public async Task ListAsync_GivenSubjects_ReturnCorrectly()
    {
        var subjects = new[] { FakeDataBuilder.Subject, new GrantSubject("type", "value") };
        var grantedPermissions = await GrantedPermissionRepository.ListAsync(subjects);

        grantedPermissions.Select(v => v.Object)
                          .Should()
                          .BeEquivalentTo(FakeDataBuilder.GrantedObjects);
    }
}