using System.Threading.Tasks;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.WebApi.Tests.Uow
{
    public class UnitOfWorkActionFilterTests : AspNetCoreTestsBase<Startup>
    {
        [Fact]
        public async Task UnitOfWorkActionFilter_SendGetRequest_BeginNoTransactionalUow()
        {
            var response = await HttpClient.GetAsync("api/fake-uow/GetWithUow");
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UnitOfWorkActionFilter_SendNonGetRequest_BeginTransactionalUow()
        {
            var response = await HttpClient.PostAsync("api/fake-uow/CreateWithUow", null);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UnitOfWorkActionFilter_AnnotateUowIsTransactional_BeginTransactionalUow()
        {
            var response = await HttpClient.GetAsync("api/fake-uow/GetWithTransactionalUow");
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UnitOfWorkActionFilter_AnnotateUowIsDisabled_NoAvailableUow()
        {
            var response = await HttpClient.GetAsync("api/fake-uow/GetWithoutUow");
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UnitOfWorkActionFilter_WhenErrorOccurred_DisposeUow()
        {
            IUnitOfWork uow = null;
            ServiceProvider.GetRequiredService<ActionMiddlewareProvider>()
                .ExecutingAction = context =>
                {
                    var serviceProvider = context.RequestServices;
                    var ambientUnitOfWork = serviceProvider.GetRequiredService<IAmbientUnitOfWork>();
                    uow = ambientUnitOfWork.UnitOfWork;
                    return Task.CompletedTask;
                };
            var response = await HttpClient.PostAsync("api/fake-uow/ThrowException", null);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.False(uow.IsActive);
        }
    }
}