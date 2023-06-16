using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Tests.Uow;

public class UnitOfWorkActionFilterTests : WebApplicationTestBase<WebApiTestModule>
{
    public UnitOfWorkActionFilterTests(TestWebApplicationFactory<WebApiTestModule> webAppFactory) : base(webAppFactory)
    {
    }

    [Fact]
    public async Task UnitOfWorkActionFilter_SendGetRequest_BeginNoTransactionalUow()
    {
        var response = await HttpClient.GetAsync("api/fake-uow/GetWithUow");

        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task UnitOfWorkActionFilter_SendNonGetRequest_BeginTransactionalUow()
    {
        var response = await HttpClient.PostAsync("api/fake-uow/CreateWithUow", null);

        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task UnitOfWorkActionFilter_AnnotateUowIsTransactional_BeginTransactionalUow()
    {
        var response = await HttpClient.GetAsync("api/fake-uow/GetWithTransactionalUow");

        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task UnitOfWorkActionFilter_AnnotateUowIsDisabled_NoAvailableUow()
    {
        var response = await HttpClient.GetAsync("api/fake-uow/GetWithoutUow");

        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task UnitOfWorkActionFilter_WhenErrorOccurred_DisposeUow()
    {
        IUnitOfWork? uow = null;
        ServiceProvider.GetRequiredService<ActionMiddlewareProvider>()
            .ExecutingAction = context =>
            {
                var serviceProvider = context.RequestServices;
                var ambientUnitOfWork = serviceProvider.GetRequiredService<IAmbientUnitOfWork>();
                uow = ambientUnitOfWork.UnitOfWork;
                return Task.CompletedTask;
            };
        var response = await HttpClient.PostAsync("api/fake-uow/ThrowException", null);

        response.Should().BeSuccessful();
        uow!.IsActive.Should().BeFalse();
    }
}
