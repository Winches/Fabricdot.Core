using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fabricdot.WebApi.Validation;

public interface IModelStateValidator
{
    void Validate(ModelStateDictionary modelState);
}
