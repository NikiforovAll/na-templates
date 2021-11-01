// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Common.ServiceCollectionExtensions;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/// <summary>
/// <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TOptions : class, new()
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return services
            .Configure<TOptions>(configuration)
            .AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="configureBinder">Used to configure the binder options.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<BinderOptions> configureBinder)
        where TOptions : class, new()
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return services
            .Configure<TOptions>(configuration, configureBinder)
            .AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// Also runs data annotation validation.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TOptions : class, new()
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        services
            .AddOptions<TOptions>()
            .Bind(configuration)
            .ValidateDataAnnotations();
        return services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// Also runs data annotation validation.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="configureBinder">Used to configure the binder options.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<BinderOptions> configureBinder)
        where TOptions : class, new()
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        services
            .AddOptions<TOptions>()
            .Bind(configuration, configureBinder)
            .ValidateDataAnnotations();
        return services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// Also runs data annotation validation and custom validation using the default failure message.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="validation">The validation function.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<TOptions, bool> validation)
        where TOptions : class, new()
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (validation is null)
        {
            throw new ArgumentNullException(nameof(validation));
        }

        services
            .AddOptions<TOptions>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .Validate(validation);
        return services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// Also runs data annotation validation and custom validation using the default failure message.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="validation">The validation function.</param>
    /// <param name="configureBinder">Used to configure the binder options.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<TOptions, bool> validation,
        Action<BinderOptions> configureBinder)
        where TOptions : class, new()
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (validation is null)
        {
            throw new ArgumentNullException(nameof(validation));
        }

        services
            .AddOptions<TOptions>()
            .Bind(configuration, configureBinder)
            .ValidateDataAnnotations()
            .Validate(validation);
        return services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// Also runs data annotation validation and custom validation.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="validation">The validation function.</param>
    /// <param name="failureMessage">The failure message to use when validation fails.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<TOptions, bool> validation,
        string failureMessage)
        where TOptions : class, new()
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (validation is null)
        {
            throw new ArgumentNullException(nameof(validation));
        }

        if (failureMessage is null)
        {
            throw new ArgumentNullException(nameof(failureMessage));
        }

        services
            .AddOptions<TOptions>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .Validate(validation, failureMessage);
        return services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }

    /// <summary>
    /// Registers <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
    /// Also runs data annotation validation and custom validation.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="validation">The validation function.</param>
    /// <param name="failureMessage">The failure message to use when validation fails.</param>
    /// <param name="configureBinder">Used to configure the binder options.</param>
    /// <returns>The same services collection.</returns>
    public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<TOptions, bool> validation,
        string failureMessage,
        Action<BinderOptions> configureBinder)
        where TOptions : class, new()
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (validation is null)
        {
            throw new ArgumentNullException(nameof(validation));
        }

        if (failureMessage is null)
        {
            throw new ArgumentNullException(nameof(failureMessage));
        }

        services
            .AddOptions<TOptions>()
            .Bind(configuration, configureBinder)
            .ValidateDataAnnotations()
            .Validate(validation, failureMessage);
        return services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
    }
}
