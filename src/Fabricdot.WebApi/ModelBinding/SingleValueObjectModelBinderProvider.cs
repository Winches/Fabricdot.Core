using Fabricdot.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fabricdot.WebApi.ModelBinding;

internal class SingleValueObjectModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        var modelType = context.Metadata.ModelType;

        return typeof(ISingleValueObject).IsAssignableFrom(modelType)
            ? new SingleValueObjectModelBinder()
            : null;
    }
}
