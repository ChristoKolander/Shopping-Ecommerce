using Microsoft.AspNetCore.Diagnostics;
using System.Net;


namespace Shopping.Api.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseConfigureExceptionHandler(this IApplicationBuilder app)
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