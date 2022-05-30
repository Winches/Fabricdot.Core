using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Fabricdot.Authorization.Permissions;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Fabricdot.Authorization.Tests
{
    public class AuthorizationTestBase : IntegrationTestBase<AuthorizationTestModule>
    {
        protected static Claim Superuser { get; } = new(ClaimTypes.NameIdentifier, "0");

        protected static Claim Superrole { get; } = new(ClaimTypes.Role, "role");

        protected static PermissionName[] GrantedPermissions { get; } = new[] { new PermissionName("name1"), new PermissionName("name2") };

        protected static PermissionName[] UngrantedPermissions { get; } = new[] { new PermissionName("name3"), new PermissionName("name4") };

        public AuthorizationTestBase()
        {
            var permissionManager = ServiceProvider.GetRequiredService<IPermissionManager>();
            var group = new PermissionGroup("group1", "group 1");
            GrantedPermissions.ForEach(v => group.AddPermission(v, v));
            UngrantedPermissions.ForEach(v => group.AddPermission(v, v));
            permissionManager.AddGroupAsync(group).GetAwaiter().GetResult();
        }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var mockGrantingService = new Mock<IPermissionGrantingService>();
            IReadOnlySet<GrantResult> isGranted(GrantSubject subject, IEnumerable<string> objects, CancellationToken __)
            {
                return objects.Select(v => new GrantResult(v, subject.Value == Superuser.Value
                                                              || (subject.Value == Superrole.Value && v != StandardPermissions.Superuser)
                                                              || GrantedPermissions.Contains(v)))
                              .ToHashSet();
            }
            mockGrantingService.Setup(v => v.IsGrantedAsync(It.IsAny<GrantSubject>(), It.IsAny<IEnumerable<string>>(), default))
                               .ReturnsAsync((Func<GrantSubject, IEnumerable<string>, CancellationToken, IReadOnlySet<GrantResult>>)isGranted);

            serviceCollection.Replace(ServiceDescriptor.Scoped(_ => mockGrantingService.Object));
        }
    }
}