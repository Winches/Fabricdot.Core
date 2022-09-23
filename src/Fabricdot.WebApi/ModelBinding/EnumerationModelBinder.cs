using System.Reflection;
using System.Runtime.ExceptionServices;
using Fabricdot.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fabricdot.WebApi.ModelBinding;

internal class EnumerationModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelType = bindingContext.ModelType;
        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }
        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
        var value = valueProviderResult.FirstValue ?? string.Empty;

        object result;
        try
        {
            var method = typeof(Enumeration).GetMethod(nameof(Enumeration.FromValue), BindingFlags.Static | BindingFlags.Public)!
                                            .MakeGenericMethod(modelType);
            result = method.Invoke(null, new object[] { int.Parse(value) }) ?? throw new InvalidOperationException();
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