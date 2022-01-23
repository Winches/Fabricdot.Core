using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.MultiTenancy.AspNetCore
{
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
}