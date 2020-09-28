using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValidationException = Fabricdot.WebApi.Core.Exceptions.ValidationException;

namespace Fabricdot.WebApi.Core.Validation
{
    public class ModelStateValidator : IModelStateValidator
    {
        public virtual void Validate(ModelStateDictionary modelState)
        {
            if (modelState.IsValid) return;

            var errors = new List<ValidationResult>();

            foreach (var state in modelState)
            foreach (var error in state.Value.Errors)
                errors.Add(new ValidationResult(error.ErrorMessage, new[] {state.Key}));

            if (errors.Any()) throw new ValidationException("ModelState is not valid!", errors);
        }
    }
}