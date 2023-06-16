using Fabricdot.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fabricdot.WebApi.ModelBinding;

internal class EnumerationModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        var modelType = context.Metadata.ModelType;

        return typeof(Enumeration).IsAssignableFrom(modelType)
            ? new EnumerationModelBinder()
            : null;
    }
}
