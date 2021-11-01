// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.IntegrationTests;

using Marten;
using Marten.Events.Projections;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Nikiforovall.ES.Template.Api;
using Nikiforovall.ES.Template.Application.SharedKernel.Repositories;
using Nikiforovall.ES.Template.Domain.ProjectAggregate;
using Nikiforovall.ES.Template.Domain.SharedKernel.Aggregates;
using Nikiforovall.ES.Template.Infrastructure;
using Nikiforovall.ES.Template.Infrastructure.Persistence;
using Nikiforovall.ES.Template.Infrastructure.Persistence.Projections;
using Npgsql;
using Respawn;
using IDocumentStore = Marten.IDocumentStore;

/// <summary>
/// Ref: https://github.com/jbogard/ContosoUniversityDotNetCore/blob/master/ContosoUniversity.IntegrationTests/SliceFixture.cs
/// </summary>
public class SliceFixture
{
    private static readonly Checkpoint Checkpoint;
    private static readonly IConfigurationRoot Configuration;
    private static readonly IServiceScopeFactory ScopeFactory;

    static SliceFixture()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();
        Configuration = builder.Build();

        var startup = new Startup(Configuration, new HostingEnvironment());
        var services = new ServiceCollection();

        services.AddLogging();

        startup.ConfigureServices(services);

        // ! REPLACES projections, used for testing purposes
        services.AddMarten(Configuration,
            options =>
            {
                options.Projections.SelfAggregate<Project>();
                options.Projections.Add<ToDoItemProjection>(ProjectionLifecycle.Inline);
                options.Projections.Add<ProjectArchivedProjection>(ProjectionLifecycle.Inline);
            });

        var provider = services.BuildServiceProvider();
        var martenConfig = Configuration
            .GetSection("EventStore")
            .Get<MartenConfiguration>();
        ScopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
        Checkpoint = new Checkpoint()
        {
            SchemasToInclude = new[] { martenConfig.ReadModelSchema, martenConfig.WriteModelSchema },
            TablesToIgnore = Array.Empty<string>(),
            DbAdapter = DbAdapter.Postgres,
        };
    }
    public static async Task ResetCheckpointAsync()
    {
        var martenConfig = Configuration
            .GetSection("EventStore")
            .Get<MartenConfiguration>();
        using (var conn = new NpgsqlConnection(martenConfig.ConnectionString))
        {
            await conn.OpenAsync();

            await Checkpoint.Reset(conn);
        }
    }

    public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using (var scope = ScopeFactory.CreateScope())
        {
            try
            {
                await action(scope.ServiceProvider).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using (var scope = ScopeFactory.CreateScope())
        {
            try
            {
                var result = await action(scope.ServiceProvider).ConfigureAwait(false);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public static Task ExecuteDocumentStoreAsync(Func<IDocumentStore, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetRequiredService<IDocumentStore>()));

    public static Task ExecuteDocumentStoreAsync(Func<IDocumentStore, IMediator, Task> action)
        => ExecuteScopeAsync(sp => action(
            sp.GetRequiredService<IDocumentStore>(), sp.GetRequiredService<IMediator>()));

    public static Task<T> ExecuteDocumentStoreAsync<T>(Func<IDocumentStore, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetRequiredService<IDocumentStore>()));

    public static Task<T> ExecuteDocumentStoreAsync<T>(Func<IDocumentStore, IMediator, Task<T>> action)
        => ExecuteScopeAsync(sp => action(
            sp.GetRequiredService<IDocumentStore>(), sp.GetRequiredService<IMediator>()));

    public static Task ExecuteDatabaseAsync<T>(Func<IRepository<T>, Task> action) where T : IAggregate
        => ExecuteScopeAsync(sp => action(sp.GetRequiredService<IRepository<T>>()));

    public static Task<T> ExecuteDatabaseAsync<T>(Func<IRepository<T>, Task<T>> action) where T : IAggregate
        => ExecuteScopeAsync(sp => action(sp.GetRequiredService<IRepository<T>>()));

    public static Task InsertAsync<T>(params T[] entities) where T : IAggregate =>
        ExecuteDatabaseAsync<T>(async db =>
        {
            foreach (var entity in entities)
            {
                await db.AddAsync(entity, CancellationToken.None);
            }
        });

    public static Task InsertDocumentsAsync<T>(params T[] entities) where T : class =>
        ExecuteDocumentStoreAsync(async db =>
        {
            using var session = db.LightweightSession();
            foreach (var entity in entities)
            {
                session.Store(entity);
            }
            await session.SaveChangesAsync();
        });

    public static Task<T?> FindAsync<T>(Guid id)
        where T : class, IAggregate => ExecuteDatabaseAsync<T>(db => db.FindAsync(id, CancellationToken.None));


    public static Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request) =>
        ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();

            return mediator.Send(request);
        });

    public static Task SendAsync(IRequest request) =>
        ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();

            return mediator.Send(request);
        });

    internal class HostingEnvironment : IWebHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Production;

        public string ApplicationName { get; set; } = default!;

        public string WebRootPath { get; set; } = default!;

        public IFileProvider WebRootFileProvider { get; set; } = default!;

        public string ContentRootPath { get; set; } = default!;

        public IFileProvider ContentRootFileProvider { get; set; } = default!;
    }
}
