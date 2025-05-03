using System.Net;
using System.Text.Json;
using CommonSharedLibrary.CommonLogger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommonSharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            string message = "sorry, internal server error occured. Kindly try again.";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string errorTitle = "Error";
            try
            {
                await next(httpContext);
                //check if exception is Too Many Request --> 429
                if (httpContext.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    errorTitle = "Warning";
                    message = "Too many requests made";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(httpContext, errorTitle, message, statusCode);
                }
                else if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    errorTitle = "Alert";
                    message = "You are not authorized to access.";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(httpContext, errorTitle, message, statusCode);
                }
                else if (httpContext.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    errorTitle = "Out of access";
                    message = "You are not required to access";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                    await ModifyHeader(httpContext, errorTitle, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                //Log original exceptions --> File, Console & Debugger
                LogException.LogExceptions(ex);
                //Check if exception is TimeOut
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    errorTitle = "Out of time";
                    message = "Request timeout... try again";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }
                await ModifyHeader(httpContext, errorTitle, message, statusCode);
            }
        }

        private static async Task ModifyHeader(HttpContext httpContext, string errorTitle, string message, int statusCode)
        {
            //display scary-free message to client
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = errorTitle,

            }), CancellationToken.None);
            return;
        }
    } 
}