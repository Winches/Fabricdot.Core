using Fabricdot.Core.DependencyInjection;
using Fabricdot.WebApi.ExceptionHanding;
using Fabricdot.WebApi.Filters;
using Fabricdot.WebApi.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Configuration;

[ServiceContract(typeof(IConfigureOptions<MvcOptions>))]
[Dependency(ServiceLifetime.Singleton)]
public class ConfigureMvcOptions : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        var filters = options.Filters;

        filters.AddService<ValidationActionFilter>();
        filters.AddService<UnitOfWorkActionFilter>();
        filters.AddService<ExceptionHandlingFilter>();
        filters.AddService<ResultFilter>();
    }
}