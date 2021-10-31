// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Api;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures the ForwardedHeadersOptions.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The services with options services added.</returns>
    public static IServiceCollection AddHostingOptions(
        this IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto;
            // Only loopback proxies are allowed by default.
            // Clear that restriction because forwarders are enabled by explicit
            // configuration.
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });
        return services;
    }

    /// <summary>
    /// Adds custom routing settings which determines how URL's are generated.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The services with routing services added.</returns>
    public static IServiceCollection AddCustomRouting(this IServiceCollection services) =>
        services.AddRouting(options => options.LowercaseUrls = true);

    /// <summary>
    /// Adds custom versioning settings and format.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services) => services
        .AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new HeaderApiVersionReader("X-Version");
        })
        .AddVersionedApiExplorer(x => x.GroupNameFormat = "'v'VVV"); // Version format: 'v'major[.minor][-status]

    /// <summary>
    /// Adds cross-origin resource sharing (CORS) services and configures named CORS policies. See
    /// https://docs.asp.net/en/latest/security/cors.html
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="policyName"></param>
    /// <param name="configuration"></param>
    /// <returns>The services with CORS services added.</returns>
    public static IServiceCollection AddCustomCors(this IServiceCollection services, string policyName, IConfiguration configuration)
    {
        // Create named CORS policies here which you can consume using application.UseCors("PolicyName")
        // or a [EnableCors("PolicyName")] attribute on your controller or action.

        var origins = configuration.GetSection("AllowedOrigins").Get<string[]>();
        return services.AddCors(builder =>
            builder.AddPolicy(policyName,
                x => x
                    .WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("content-disposition")));
    }
}
