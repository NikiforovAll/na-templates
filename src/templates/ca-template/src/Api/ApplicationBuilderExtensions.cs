// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Api;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

internal static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Uses custom serilog request logging. Adds additional properties to each log.
    /// See https://github.com/serilog/serilog-aspnetcore.
    /// </summary>
    /// <param name="application">The application builder.</param>
    /// <returns>The application builder with the Serilog middleware configured.</returns>
    public static IApplicationBuilder UseCustomSerilogRequestLogging(this IApplicationBuilder application) =>
        application.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                var request = httpContext.Request;
                var response = httpContext.Response;
                var endpoint = httpContext.GetEndpoint();

                diagnosticContext.Set("Host", request.Host);
                diagnosticContext.Set("Protocol", request.Protocol);
                diagnosticContext.Set("Scheme", request.Scheme);

                if (request.QueryString.HasValue)
                {
                    diagnosticContext.Set("QueryString", request.QueryString.Value);
                }

                if (endpoint != null)
                {
                    diagnosticContext.Set("EndpointName", endpoint.DisplayName);
                }

                diagnosticContext.Set("ContentType", response.ContentType);
            };
            options.GetLevel = GetLevel;

            static LogEventLevel GetLevel(HttpContext httpContext, double elapsedMilliseconds, Exception exception)
            {
                if (exception == null && httpContext.Response.StatusCode <= 499)
                {
                    if (IsHealthCheckEndpoint(httpContext))
                    {
                        return LogEventLevel.Verbose;
                    }

                    return LogEventLevel.Information;
                }

                return LogEventLevel.Error;

                static bool IsHealthCheckEndpoint(HttpContext httpContext)
                {
                    var endpoint = httpContext.GetEndpoint();
                    if (endpoint != null)
                    {
                        return endpoint.DisplayName == "Health checks";
                    }

                    return false;
                }
            }
        });

    /// <summary>
    /// Register OpenAPI toolchain.
    /// </summary>
    /// <param name="application"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder application, IConfiguration configuration)
    {
        // https://github.com/RicoSuter/NSwag/wiki/AspNetCore-Middleware
        // https://github.com/RicoSuter/NSwag/blob/master/samples/WithMiddleware/Sample.AspNetCore21.Nginx/Startup.cs
        var swaggerDocumentPath = "api/swagger/{documentName}/swagger.json";
        application.UseOpenApi(options => options.Path = swaggerDocumentPath);
        application.UseSwaggerUi3(options =>
        {
            options.Path = string.Empty;
            options.DocumentPath = swaggerDocumentPath;
            options.CustomStylesheetPath = "api/swagger/theme-flattop.css";
            options.OAuth2Client = new NSwag.AspNetCore.OAuth2ClientSettings()
            {
                AppName = "",
                ClientId = configuration.GetValue<string>("IdentityProvider:ClientId"),
            };
        });
        return application;
    }
}
