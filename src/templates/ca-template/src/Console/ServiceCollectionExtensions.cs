// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Console;

using Microsoft.Extensions.DependencyInjection;
using NikiforovAll.CA.Template.Application.SharedKernel.Interfaces;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds CLI services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The services with CLI services added.</returns>
    public static IServiceCollection AddCliContainer(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, UserServiceStub>();
        return services;
    }

    internal class UserServiceStub : ICurrentUserService
    {
        public string? UserId => default;
    }
}
