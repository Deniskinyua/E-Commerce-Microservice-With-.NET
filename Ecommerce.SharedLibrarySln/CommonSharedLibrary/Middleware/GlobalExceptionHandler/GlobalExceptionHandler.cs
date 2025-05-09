
using System.Text.Json;
using CommonSharedLibrary.CommonLogger;
using CommonSharedLibrary.Middleware.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommonSharedLibrary.Middleware.GlobalExceptionHandler
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }
        /// <summary>
        /// Handles all exceptions globally
        /// </summary>
        /// <param name="httpContext"></param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                //Handle specific status codes
                switch (httpContext.Response.StatusCode)
                {
                    case StatusCodes.Status429TooManyRequests:
                        await ModifyHeader(httpContext, ExceptionConstants.TooManyRequestsTitle, ExceptionConstants.TooManyRequestsMessage,
                            StatusCodes.Status429TooManyRequests);
                        break;
                    case StatusCodes.Status401Unauthorized:
                        await ModifyHeader(httpContext, ExceptionConstants.UnauthorizedTitle,
                            ExceptionConstants.UnauthorizedMessage, StatusCodes.Status401Unauthorized);
                        break;
                    case StatusCodes.Status403Forbidden:
                        await ModifyHeader(httpContext, ExceptionConstants.ForbiddenTitle,
                            ExceptionConstants.ForbiddenMessage, StatusCodes.Status403Forbidden);
                        break;
                }
            }
            catch (Exception exception)
            {
                //Log original exceptions --> Console, File & Debugger
                LogException.LogExceptions(exception);
                if(exception is TaskCanceledException || exception is TimeoutException)
                    await ModifyHeader(httpContext, ExceptionConstants.TimeoutTitle, ExceptionConstants.TimeoutMessage,
                        StatusCodes.Status408RequestTimeout);
                        
                await ModifyHeader(httpContext, ExceptionConstants.ErrorTitle, ExceptionConstants.InternalServerErrorMessage,
                    StatusCodes.Status408RequestTimeout);
            }
        }

        private static async Task ModifyHeader(HttpContext httpContext, string errorTitle, string message, int statusCode)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;
            var problemDetails = new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = errorTitle
            };
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problemDetails), CancellationToken.None);
        }
    } 
}