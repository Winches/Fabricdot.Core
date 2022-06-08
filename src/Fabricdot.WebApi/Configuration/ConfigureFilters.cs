using Fabricdot.WebApi.ExceptionHanding;
using Fabricdot.WebApi.Filters;
using Fabricdot.WebApi.Uow;
using Microsoft.AspNetCore.Mvc;

namespace Fabricdot.WebApi.Configuration;

public static class ConfigureFilters
{
    public static void AddActionFilters(this MvcOptions options)
    {
        options.Filters.AddService<ValidationActionFilter>();
        options.Filters.AddService<UnitOfWorkActionFilter>();
        options.Filters.AddService<ExceptionHandlingFilter>();
        options.Filters.AddService<ResultFilter>();
    }
}