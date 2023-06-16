using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Tests.Modelbinding;

public abstract class ModelBinderTestBase : WebApplicationTestBase<WebApiTestModule>
{
    public ModelBinderTestBase(TestWebApplicationFactory<WebApiTestModule> webAppFactory) : base(webAppFactory)
    {
        WebAppFactory = WebAppFactory.WithWebHostBuilder(v =>
        {
            v.ConfigureServices(s => s.Configure<ApiBehaviorOptions>(opts => opts.SuppressModelStateInvalidFilter = false));
        });
    }
}
