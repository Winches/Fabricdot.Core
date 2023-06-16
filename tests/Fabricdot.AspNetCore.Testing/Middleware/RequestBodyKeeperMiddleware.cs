using System.Text;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.AspNetCore.Testing.Middleware;

// https://devblogs.microsoft.com/dotnet/re-reading-asp-net-core-request-bodies-with-enablebuffering/
public class RequestBodyKeeperMiddleware : IMiddleware
{
    internal const string Key = "_RequestBody";

    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next)
    {
        context.Request.EnableBuffering();

        // Leave the body open so the next middleware can read it.
        using (var reader = new StreamReader(
            context.Request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true))
        {
            var body = await reader.ReadToEndAsync();
            context.Items.Add(Key, body);

            // Reset the request body stream position so the next middleware can read it
            context.Request.Body.Position = 0;
        }

        await next(context);
    }
}
