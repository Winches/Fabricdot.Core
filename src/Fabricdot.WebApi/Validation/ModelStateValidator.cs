using System.Linq;
using Fabricdot.Core.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fabricdot.WebApi.Validation
{
    public class ModelStateValidator : IModelStateValidator
    {
        public virtual void Validate(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
                return;

            var notification = CreateNotification(modelState);
            if (notification.Errors.Any())
                throw new ValidationFailedException("ModelState is not valid!", notification);
        }

        public Notification CreateNotification(ModelStateDictionary modelState)
        {
            var notification = new Notification();
            foreach (var (key, value) in modelState)
            {
                foreach (var error in value.Errors)
                    notification.Add(key, new Notification.Error(error.ErrorMessage));
            }

            return notification;
        }
    }
}