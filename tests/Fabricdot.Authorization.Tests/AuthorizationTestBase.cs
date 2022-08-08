using System.Security.Claims;
using Fabricdot.Authorization.Permissions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Fabricdot.Authorization.Tests;

public class AuthorizationTestBase : IntegrationTestBase<AuthorizationTestModule>
{
    protected Mock<IPermissionGrantingService> PermissionGrantingServiceMock { get; }

    protected Claim Superuser { get; }

    protected Claim Superrole { get; }

    protected PermissionName[] GrantedPermissions { get; }

    protected PermissionName[] UngrantedPermissions { get; }

    protected PermissionName[] Permissions { get; }

    public AuthorizationTestBase()
    {
        Superuser = new(ClaimTypes.NameIdentifier, Create<string>());
        Superrole = new(ClaimTypes.Role, Create<string>());
        GrantedPermissions = Create<PermissionName[]>();
        UngrantedPermissions = Create<PermissionName[]>();
        Permissions = GrantedPermissions.Union(UngrantedPermissions).ToArray();

        var permissionManager = ServiceProvider.GetRequiredService<IPermissionManager>();
        var group = new PermissionGroup(Create<string>(), Create<string>());
        GrantedPermissions.ForEach(v => group.AddPermission(v, v));
        UngrantedPermissions.ForEach(v => group.AddPermission(v, v));
        permissionManager.AddGroupAsync(group).GetAwaiter().GetResult();

        PermissionGrantingServiceMock = Mock<IPermissionGrantingService>();
        Func<GrantSubject, IEnumerable<string>, CancellationToken, IReadOnlySet<GrantResult>> valueFunction = (subject, objects, _) =>
                                        objects.Select(v => new GrantResult(v, GrantedPermissions.Contains(v)
                                        || subject.Value == Superrole.Value
                                        || subject.Value == Superuser.Value))
                                               .ToHashSet();
        PermissionGrantingServiceMock.Setup(v => v.IsGrantedAsync(It.IsAny<GrantSubject>(), It.IsAny<IEnumerable<string>>(), default))
                           .ReturnsAsync(valueFunction);
    }

    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.Replace(ServiceDescriptor.Scoped(_ => PermissionGrantingServiceMock.Object));
    }
}