using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.WebApi.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Tests.Uow;

public class UnitOfWorkMiddlewareTests : WebApplicationTestBase<WebApiTestModule>
{
    public UnitOfWorkMiddlewareTests(TestWebApplicationFactory<WebApiTestModule> webAppFactory) : base(webAppFactory)
    {
        var options = ServiceProvider.GetRequiredService<IOptions<HttpUnitOfWorkOptions>>();
        options.Value.IgnoredUrls.Add("/book");
    }

    public static IEnumerable<object[]> GetHttpMethods()
    {
        yield return new[] { HttpMethod.Get };
        yield return new[] { HttpMethod.Post };
        yield return new[] { HttpMethod.Put };
        yield return new[] { HttpMethod.Delete };
    }

    [Theory]
    [MemberData(nameof(GetHttpMethods))]
    public async Task UnitOfWorkMiddleware_SendHttpRequest_ReserveUnitOfWork(HttpMethod httpMethod)
    {
        IUnitOfWork uow = null;
        ServiceProvider.GetRequiredService<ActionMiddlewareProvider>()
            .ExecutingAction = context =>
            {
                var serviceProvider = context.RequestServices;
                var ambientUnitOfWork = serviceProvider.GetRequiredService<IAmbientUnitOfWork>();
                uow = ambientUnitOfWork.UnitOfWork;

                uow.IsReservedFor(UnitOfWorkManager.RESERVATION_NAME).Should().BeTrue();

                return Task.CompletedTask;
            };

        using var request = new HttpRequestMessage(httpMethod, "/");
        await HttpClient.SendAsync(request);

        await FluentActions.Awaiting(() => uow.CommitChangesAsync())
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData("book/1")]
    [InlineData("book/1/tag")]
    public async Task UnitOfWorkMiddleware_SendHttpRequestWithIgnoredUrl_DoNothing(string url)
    {
        ServiceProvider.GetRequiredService<ActionMiddlewareProvider>()
            .ExecutingAction = context =>
            {
                var serviceProvider = context.RequestServices;
                var ambientUnitOfWork = serviceProvider.GetRequiredService<IAmbientUnitOfWork>();

                ambientUnitOfWork.UnitOfWork.Should().BeNull();

                return Task.CompletedTask;
            };

        await HttpClient.GetAsync(url);
    }
}