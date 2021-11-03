// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Infrastructure;

using System.Reflection;
using System.Security.Authentication;
using Infrastructure.Services;
using Marten;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NikiforovAll.ES.Template.Application.SharedKernel.Events;
using NikiforovAll.ES.Template.Application.SharedKernel.Events.External;
using NikiforovAll.ES.Template.Application.SharedKernel.Interfaces;
using NikiforovAll.ES.Template.Application.SharedKernel.Interfaces.IdGeneration;
using NikiforovAll.ES.Template.Application.SharedKernel.Repositories;
using NikiforovAll.ES.Template.Domain.ProjectAggregate;
using NikiforovAll.ES.Template.Infrastructure.Ids;
using NikiforovAll.ES.Template.Infrastructure.Options;
using NikiforovAll.ES.Template.Infrastructure.Persistence;
using NikiforovAll.ES.Template.Infrastructure.Persistence.Repository;
using Weasel.Core;
using Weasel.Postgresql;
using IApplicationDocumentStore = Application.SharedKernel.Repositories.IDocumentStore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IIdGenerator, MartenIdGenerator>();
        services.AddMarten(configuration, options => options.ConfigureEventStoreSnapshoting());

        services.AddScoped<IRepository<Project>, MartenRepository<Project>>();
        services.AddScoped<IApplicationDocumentStore, MartenDocumentStore>();

        services.TryAddScoped<IEventBus, EventBus>();
        services.TryAddScoped<IExternalEventProducer, DummyExternalEventsProducer>();

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }

    private const string DefaultConfigKey = "EventStore";

    public static IServiceCollection AddMarten(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<StoreOptions>? configureOptions = default,
        string configKey = DefaultConfigKey)
    {
        var martenConfig = GetMartenConfig(configuration, configKey);

        var documentStore = services
            .AddMarten(options =>
            {
                options.Connection(martenConfig.ConnectionString);
                options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;

                options.Events.DatabaseSchemaName = martenConfig.WriteModelSchema;
                options.DatabaseSchemaName = martenConfig.ReadModelSchema;

                options.UseDefaultSerialization(
                    nonPublicMembersStorage: NonPublicMembersStorage.NonPublicSetters,
                    enumStorage: EnumStorage.AsString);
                options.Projections.AsyncMode = martenConfig.DaemonMode;

                options.Schema.Include<MartenApplicationRegistry>();

                configureOptions?.Invoke(options);
            })
            .UseLightweightSessions()
            .InitializeStore();

        // Migration
        MartenConfigurationExtensions.SetupSchema(documentStore, martenConfig, 1);

        return services;
    }

    public static void ConfigureEventStoreSnapshoting(this StoreOptions options) =>
        MartenApplicationRegistry.RegisterProjections(options);

    private static MartenConfiguration GetMartenConfig(
        IConfiguration configuration, string configKey = DefaultConfigKey) => configuration
            .GetSection(configKey)
            .Get<MartenConfiguration>();

    /// <summary>
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The services.</returns>
    public static IServiceCollection AddApplicationMessageBroker(
        this IServiceCollection services, IConfiguration configuration, Assembly? assembly)
    {
        var options = configuration
                .GetSection(RabbitMQConfiguration.Options)
                .Get<RabbitMQConfiguration>() ?? new RabbitMQConfiguration();

        services.Configure<RabbitMQConfiguration>(
            configuration.GetSection(RabbitMQConfiguration.Options));

        var connectionString = new Uri(
            options.ToConnectionString() ?? string.Empty);

        services.AddMassTransit(x =>
        {
            if (assembly != null)
            {
                x.AddConsumers(assembly);
            }

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(connectionString, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);

                    UseSSL(h, options);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddMassTransitHostedService();

        return services;

        static void UseSSL(MassTransit.RabbitMqTransport.IRabbitMqHostConfigurator h, RabbitMQConfiguration options)
        {
            if (options.SSL)
            {
                h.UseSsl(ssl => ssl.Protocol = SslProtocols.Tls12);
            }
        }
    }
}
