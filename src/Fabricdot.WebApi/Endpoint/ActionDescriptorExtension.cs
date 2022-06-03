using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Fabricdot.WebApi.Endpoint
{
    public static class ActionDescriptorExtensions
    {
        public static List<Type> ObjectResultTypes { get; } = new List<Type>
        {
            typeof(JsonResult),
            typeof(ObjectResult),
            typeof(NoContentResult)
        };

        static ActionDescriptorExtensions()
        {
        }

        public static ControllerActionDescriptor AsControllerActionDescriptor(this ActionDescriptor actionDescriptor)
        {
            if (!actionDescriptor.IsControllerAction())
            {
                throw new InvalidOperationException(
                    $"{nameof(actionDescriptor)} should be type of {typeof(ControllerActionDescriptor).AssemblyQualifiedName}");
            }

            return (ControllerActionDescriptor)actionDescriptor;
        }

        public static MethodInfo GetMethodInfo(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.AsControllerActionDescriptor().MethodInfo;
        }

        public static Type GetReturnType(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetMethodInfo().ReturnType;
        }

        public static bool HasObjectResult(this ActionDescriptor actionDescriptor)
        {
            var returnType = actionDescriptor.GetReturnType();
            if (returnType == typeof(Task))
                returnType = typeof(void);
            else if (returnType.GetTypeInfo().IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                returnType = returnType.GenericTypeArguments[0];

            if (!typeof(IActionResult).IsAssignableFrom(returnType))
                return true;

            return ObjectResultTypes.Any(t => t.IsAssignableFrom(returnType));
        }

        public static bool IsControllerAction(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor is ControllerActionDescriptor;
        }
    }
}