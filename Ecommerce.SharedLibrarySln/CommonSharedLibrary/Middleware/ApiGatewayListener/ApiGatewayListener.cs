using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;

namespace CommonSharedLibrary.Middleware.ApiGatewayListener;

public class ApiGatewayListener
{
    private readonly RequestDelegate _next;

    public ApiGatewayListener(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }
    /// <summary>
    /// InvokeAsync
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        //Extract specific header from the request
        var signedHeader = context.Request.Headers["Api-Gateway"];
        
        //Null means, the request is NOT coming from the API Gateway
        if (signedHeader.FirstOrDefault() is null)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("Sorry, service is unavailable.");
        }
        else
        {
            await _next(context);
        }
    }
}