using Fabricdot.WebApi.Core.ExceptionHanding;
using Fabricdot.WebApi.Core.Filters;
using Fabricdot.WebApi.Core.Uow;
using Microsoft.AspNetCore.Mvc;

namespace Fabricdot.WebApi.Core.Configuration
{
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
}