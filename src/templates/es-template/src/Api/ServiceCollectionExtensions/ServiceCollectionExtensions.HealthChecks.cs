// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Api;

using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds health check to the application.
    /// </summary>
    /// <param name="services">The services. </param>
    /// <returns>The services with health check services added.</returns>
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
    {
        // Add health checks for external dependencies here.
        // See https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks

        services
            .AddHealthChecks();

        return services;
    }
}
