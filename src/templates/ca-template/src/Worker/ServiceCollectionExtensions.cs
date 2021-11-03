// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Worker;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NikiforovAll.CA.Template.Application;
using NikiforovAll.CA.Template.Application.SharedKernel.Interfaces;
using NikiforovAll.CA.Template.Infrastructure;

public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Composition root of the application.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection ConfigureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddSingleton<ICurrentUserService, WorkerUserService>();
        services.AddApplicationMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}
