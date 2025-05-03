using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;

namespace CommonSharedLibrary.Middleware.ApiGatewayListener;

public class ApiGatewayListener(RequestDelegate next)
{
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
            await next(context);
        }
    }
}