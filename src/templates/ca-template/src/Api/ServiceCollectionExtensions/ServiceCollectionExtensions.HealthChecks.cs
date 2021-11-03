// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Api;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NikiforovAll.CA.Template.Infrastructure.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;

internal static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds health check to the application.
    /// </summary>
    /// <param name="services">The services. </param>
    /// <returns>The services with health check services added.</returns>
    /// <param name="configuration"></param>
    public static IServiceCollection AddCustomHealthChecks(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Add health checks for external dependencies here.
        // See https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks

        var databaseConnectionString = configuration
            .GetConnectionString("DefaultConnection");

        var messageBrokerOptions = configuration
            .GetSection(RabbitMQConfiguration.Options)
            .Get<RabbitMQConfiguration>() ?? new RabbitMQConfiguration();

        var tags = new string[] { "services" };
        var timeout = TimeSpan.FromSeconds(1);
        services.AddHealthChecks()
            .AddNpgSql(
                databaseConnectionString,
                name: "postgres",
                failureStatus: HealthStatus.Degraded,
                timeout: timeout,
                tags: tags)
            .AddRabbitMQ(
                messageBrokerOptions.ToConnectionString(),
                name: "rabbitmq",
                failureStatus: HealthStatus.Degraded,
                timeout: timeout,
                tags: tags);

        return services;
    }
}
