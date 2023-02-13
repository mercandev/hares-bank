using System;
using System.Net;
using Firebase.Auth;
using Firebase.Auth.Providers;
using HB.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HB.Infrastructure.Engine
{
	public static class ExceptionHandlerRegister
	{
        public static WebApplication UseExceptionHandlerRegister(this WebApplication app)
        {
            return (WebApplication)app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exception = context.Features.Get<IExceptionHandlerFeature>();

                    if (exception != null)
                    {
                        if (exception.Error is HbBusinessException || exception.Error is Exception)
                        {
                            await context.Response.WriteAsync(
                                new CustomExceptionResponse
                                {
                                    Status = HttpStatusCode.InternalServerError,
                                    ErrorMessage = exception.Error.Message

                                }.ToString());
                        }
                    }
                });
            });
        }
    }
}

