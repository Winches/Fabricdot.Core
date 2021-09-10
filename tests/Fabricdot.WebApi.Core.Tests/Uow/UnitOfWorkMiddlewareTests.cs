using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Core.Uow;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;
using Fabricdot.WebApi.Core.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Fabricdot.WebApi.Core.Tests.Uow
{
    public class UnitOfWorkMiddlewareTests : AspNetCoreTestsBase<Startup>
    {
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
                    var isReserved = uow.IsReservedFor(UnitOfWorkManager.RESERVATION_NAME);
                    var isTransactional = uow.Options.IsTransactional;

                    Assert.True(isReserved);
                    return Task.CompletedTask;
                };

            using var request = new HttpRequestMessage(httpMethod, "/");
            var response = await HttpClient.SendAsync(request);
            async Task testCode() => await uow.CommitChangesAsync();
            await Assert.ThrowsAsync<InvalidOperationException>(testCode);
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
                    var uow = ambientUnitOfWork.UnitOfWork;

                    Assert.Null(uow);
                    return Task.CompletedTask;
                };

            var response = await HttpClient.GetAsync(url);
        }

        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            base.ConfigureServices(context, services);
            services.Configure<HttpUnitOfWorkOptions>(v => v.IgnoredUrls.Add("/book"));
        }
    }
}