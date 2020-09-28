using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fabricdot.WebApi.Core.Validation
{
    public interface IModelStateValidator
    {
        void Validate(ModelStateDictionary modelState);
    }
}