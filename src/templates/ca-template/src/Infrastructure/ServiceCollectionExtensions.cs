// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Infrastructure;

using System.Reflection;
using System.Security.Authentication;
using Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NikiforovAll.CA.Template.Application.Interfaces;
using NikiforovAll.CA.Template.Application.SharedKernel.Interfaces;
using NikiforovAll.CA.Template.Infrastructure.Options;
using NikiforovAll.CA.Template.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }

    /// <summary>
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The services.</returns>
    public static IServiceCollection AddApplicationMessageBroker(
        this IServiceCollection services, IConfiguration configuration, Assembly? assembly)
    {
        var options = configuration
                .GetSection(RabbitMQConfiguration.Options)
                .Get<RabbitMQConfiguration>();

        services.Configure<RabbitMQConfiguration>(
            configuration.GetSection(RabbitMQConfiguration.Options));

        var connectionString = new Uri(options?.ToConnectionString());

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
