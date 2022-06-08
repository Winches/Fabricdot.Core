using System.Threading.Tasks;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.WebApi.Endpoint;
using Fabricdot.WebApi.Validation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Filters;

[ServiceContract(typeof(ValidationActionFilter))]
[Dependency(ServiceLifetime.Scoped)]
public class ValidationActionFilter : IAsyncActionFilter
{
    private readonly IModelStateValidator _validator;

    public ValidationActionFilter(IModelStateValidator validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionDescriptor.IsControllerAction() ||
            !context.ActionDescriptor.HasObjectResult())
        {
            await next();
            return;
        }

        _validator.Validate(context.ModelState);
        await next();
    }
}