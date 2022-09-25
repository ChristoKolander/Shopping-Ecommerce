using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Shopping.Api.LoggerService;
using Shopping.Api.Models;
using System.Net;


namespace Shopping.Api.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";


                    var contextfeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextfeature != null)
                    {
                        logger.LogError($"Error from Middleware, something went wrong: {contextfeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error"
                        }.ToString());

                    }

                });

            });
        }
    }
}