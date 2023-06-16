using Fabricdot.Authorization.Permissions;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Fabricdot.PermissionGranting.AspNetCore.Tests.Authoriation;
using Fabricdot.PermissionGranting.Tests.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.PermissionGranting.Tests;

[Requires(typeof(PermissionGrantingTestModule))]
[Exports]
public class PermissionGrantingAspNetCoreTestModule : ModuleBase
{
    public override void ConfigureServices(ConfigureServiceContext context)
    {
        var services = context.Services;
        services.AddControllers();

        services
            .AddAuthentication(FakeAuthenticationHandler.AuthenticationSchema)
            .AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>(FakeAuthenticationHandler.AuthenticationSchema, _ => { });
    }

    public override Task OnStartingAsync(ApplicationStartingContext context)
    {
        var app = context.ServiceProvider.GetApplicationBuilder();
        ConfigureAsync(context.ServiceProvider).GetAwaiter().GetResult();

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return Task.CompletedTask;
    }

    private static async Task ConfigureAsync(IServiceProvider serviceProvider)
    {
        var fakeDataBuilder = serviceProvider.GetRequiredService<FakeDataBuilder>();
        var permissionManager = serviceProvider.GetRequiredService<IPermissionManager>();

        var permissionGroup = new PermissionGroup("default", "default group");
        permissionGroup.AddPermission("name1", "name1");
        permissionGroup.AddPermission("name2", "name2");
        foreach (var @object in FakeDataBuilder.GrantedObjects)
            permissionGroup.AddPermission(@object, @object);

        await permissionManager.AddGroupAsync(permissionGroup);
        await fakeDataBuilder.BuildAsync();
    }
}
