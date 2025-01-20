using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using LibraryAPI.Domain.Exceptions;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        var statusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        context.Response.StatusCode = statusCode;
                        context.Response.ContentType = "application/json";

                        logger.LogError(
                            $"Something went wrong. Method: {context.Request.Method}," +
                            $" Path: {context.Request.Path}, Error: {contextFeature.Error}");

                        var problemDetails = new ProblemDetails
                        {
                            Status = statusCode,
                            Title = "An error occurred while processing your request.",
                            Detail = contextFeature.Error.Message,
                            Instance = context.Request.Path
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                    }
                });
            });
        }
    }
}