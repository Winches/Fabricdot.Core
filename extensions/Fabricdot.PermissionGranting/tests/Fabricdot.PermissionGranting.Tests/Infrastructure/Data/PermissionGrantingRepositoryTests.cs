using Fabricdot.Authorization;
using Fabricdot.PermissionGranting.Domain;
using Fabricdot.PermissionGranting.Tests.Data;
using Microsoft.Extensions.DependencyInjection;

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

    [AutoData]
    [Theory]
    public async Task ListAsync_GivenSubjectWithObjects_ReturnCorrectly(string[] ungrantedObjects)
    {
        var grantedObjects = FakeDataBuilder.GrantedObjects;
        var objects = grantedObjects.Union(ungrantedObjects).ToArray();
        var grantedPermissions = await GrantedPermissionRepository.ListAsync(FakeDataBuilder.Subject, objects);

        grantedPermissions.Should().Contain(v => grantedObjects.Contains(v.Object));
        grantedPermissions.Should().NotContain(v => ungrantedObjects.Contains(v.Object));
    }

    [AutoData]
    [Theory]
    public async Task ListAsync_GivenSubjects_ReturnCorrectly(List<GrantSubject> subjects)
    {
        subjects.Add(FakeDataBuilder.Subject);
        var grantedPermissions = await GrantedPermissionRepository.ListAsync(subjects);

        grantedPermissions.Select(v => v.Object)
                          .Should()
                          .BeEquivalentTo(FakeDataBuilder.GrantedObjects);
    }
}
