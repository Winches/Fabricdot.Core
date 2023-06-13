using Fabricdot.Core.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.WebApi.Tests;

public class ActionMiddlewareProvider : ISingletonDependency
{
    public RequestDelegate? ExecutingAction { get; set; }

    public RequestDelegate? ExecutedAction { get; set; }
}