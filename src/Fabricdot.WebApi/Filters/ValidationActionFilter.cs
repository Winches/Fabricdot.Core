using System.Threading.Tasks;
using Fabricdot.WebApi.Endpoint;
using Fabricdot.WebApi.Validation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fabricdot.WebApi.Filters
{
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
}