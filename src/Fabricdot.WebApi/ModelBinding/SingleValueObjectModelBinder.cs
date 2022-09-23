using System.ComponentModel;
using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fabricdot.WebApi.ModelBinding;

internal class SingleValueObjectModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelType = bindingContext.ModelType;
        var constructor = modelType.GetConstructors().Single();
        var parameterType = constructor.GetParameters().Single().ParameterType;
        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        var value = valueProviderResult.FirstValue;
        object result;
        try
        {
            if (parameterType == typeof(string))
            {
                result = constructor.Invoke(new object?[] { value });
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(parameterType);
                if (!converter.CanConvertFrom(typeof(string)))
                {
                    return Task.CompletedTask;
                }

                var argument = new[] { converter.ConvertFrom(null, valueProviderResult.Culture, value!) };
                result = constructor.Invoke(argument);
            }
        }
        catch (Exception ex)
        {
            if (ex is not FormatException && ex.InnerException is not null)
                ex = ExceptionDispatchInfo.Capture(ex.InnerException).SourceException;

            bindingContext.ModelState.TryAddModelError(
                bindingContext.ModelName,
                ex,
                bindingContext.ModelMetadata);
            return Task.CompletedTask;
        }

        bindingContext.ModelState.MarkFieldValid(modelName);
        bindingContext.Result = ModelBindingResult.Success(result);

        return Task.CompletedTask;
    }
}
