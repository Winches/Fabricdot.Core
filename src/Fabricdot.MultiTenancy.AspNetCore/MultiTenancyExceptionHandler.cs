using System.Net;
using Fabricdot.Core.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.MultiTenancy.AspNetCore;

[Dependency(ServiceLifetime.Singleton)]
public class MultiTenancyExceptionHandler : IMultiTenancyExceptionHandler
{
    public async Task HandleAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "text/html";
        var text = @$"
                <html>
                    <body>
                        <h3>{exception.Message}</h3>
                    </body>
                </html>";
        await context.Response.WriteAsync(text);
    }
}