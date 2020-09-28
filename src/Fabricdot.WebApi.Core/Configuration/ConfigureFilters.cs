using Fabricdot.WebApi.Core.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Fabricdot.WebApi.Core.Configuration
{
    public static class ConfigureFilters
    {
        public static void AddActionFilters(this MvcOptions options)
        {
            options.Filters.AddService<ValidationActionFilter>();
            options.Filters.AddService<ExceptionFilter>();
            options.Filters.Add<ResponseFilter>();
        }
    }
}