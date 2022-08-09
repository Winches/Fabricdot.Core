using Fabricdot.Core.DependencyInjection;
using Fabricdot.WebApi.ExceptionHanding;
using Fabricdot.WebApi.Filters;
using Fabricdot.WebApi.ModelBinding;
using Fabricdot.WebApi.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Configuration;

[ServiceContract(typeof(IConfigureOptions<MvcOptions>))]
[Dependency(ServiceLifetime.Singleton)]
public class ConfigureMvcOptions : IConfigureOptions<MvcOptions>
{
    public bool UseEnumerationModelBinding { get; set; } = true;

    public bool UseSingleValueObjectModelBinding { get; set; } = true;

    public void Configure(MvcOptions options)
    {
        var filters = options.Filters;
        var modelBinderProviders = options.ModelBinderProviders;

        filters.AddService<ValidationActionFilter>();
        filters.AddService<UnitOfWorkActionFilter>();
        filters.AddService<ExceptionHandlingFilter>();
        filters.AddService<ResultFilter>();

        if (UseEnumerationModelBinding)
            modelBinderProviders.Insert(0, new EnumerationModelBinderProvider());
        if (UseSingleValueObjectModelBinding)
            modelBinderProviders.Insert(0, new SingleValueObjectModelBinderProvider());
    }
}