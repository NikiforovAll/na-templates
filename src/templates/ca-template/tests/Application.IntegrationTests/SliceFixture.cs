// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.IntegrationTests;

using System.IO;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Nikiforovall.CA.Template.Api;
using Nikiforovall.CA.Template.Infrastructure.Persistence;
using Npgsql;
using Respawn;

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

        var provider = services.BuildServiceProvider();
        ScopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
        Checkpoint = new Checkpoint()
        {
            SchemasToInclude = new[] { "public" },
            TablesToIgnore = new[] { "__EFMigrationsHistory" },
            DbAdapter = DbAdapter.Postgres,
        };

        EnsureDatabase();
    }

    private static void EnsureDatabase()
    {
        using var scope = ScopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
    }

    public static async Task ResetCheckpointAsync()
    {
        using (var conn = new NpgsqlConnection(Configuration.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();

            await Checkpoint.Reset(conn);
        }
    }

    public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using (var scope = ScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                await dbContext.BeginTransactionAsync().ConfigureAwait(false);

                await action(scope.ServiceProvider).ConfigureAwait(false);

                await dbContext.CommitTransactionAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                dbContext.RollbackTransaction();
                throw;
            }
        }
    }

    public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using (var scope = ScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            try
            {
                await dbContext.BeginTransactionAsync().ConfigureAwait(false);

                var result = await action(scope.ServiceProvider).ConfigureAwait(false);

                await dbContext.CommitTransactionAsync().ConfigureAwait(false);

                return result;
            }
            catch (Exception)
            {
                dbContext.RollbackTransaction();
                throw;
            }
        }
    }

    public static Task ExecuteDbContextAsync(Func<ApplicationDbContext?, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>()));

    public static Task ExecuteDbContextAsync(Func<ApplicationDbContext?, IMediator, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>(), sp.GetService<IMediator>()));

    public static Task<T> ExecuteDbContextAsync<T>(Func<ApplicationDbContext?, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>()));

    public static Task<T> ExecuteDbContextAsync<T>(Func<ApplicationDbContext?, IMediator, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>(), sp.GetService<IMediator>()));

    public static Task InsertAsync<T>(params T[] entities) where T : class =>
        ExecuteDbContextAsync(db =>
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Add(entity);
            }
            return db.SaveChangesAsync();
        });

    public static Task InsertAsync<TEntity>(TEntity entity) where TEntity : class =>
        ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);

            return db.SaveChangesAsync();
        });

    public static Task InsertAsync<TEntity, TEntity2>(TEntity entity, TEntity2 entity2)
        where TEntity : class
        where TEntity2 : class =>
            ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);

                return db.SaveChangesAsync();
            });

    public static Task InsertAsync<TEntity, TEntity2, TEntity3>(TEntity entity, TEntity2 entity2, TEntity3 entity3)
        where TEntity : class
        where TEntity2 : class
        where TEntity3 : class =>
        ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);
            db.Set<TEntity3>().Add(entity3);

            return db.SaveChangesAsync();
        });

    public static Task InsertAsync<TEntity, TEntity2, TEntity3, TEntity4>(TEntity entity, TEntity2 entity2, TEntity3 entity3, TEntity4 entity4)
        where TEntity : class
        where TEntity2 : class
        where TEntity3 : class
        where TEntity4 : class =>
            ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);
                db.Set<TEntity3>().Add(entity3);
                db.Set<TEntity4>().Add(entity4);

                return db.SaveChangesAsync();
            });

    public static Task<T> FindAsync<T>(params object[] keyValues)
        where T : class => ExecuteDbContextAsync(db => db.Set<T>().FindAsync(keyValues).AsTask());

    // TBD: tests are not isolated, so it is really bad idea to check for count in any given point of time.
    //public static Task<int> CountAsync<TEntity>() where TEntity : class =>
    //    ExecuteDbContextAsync(db => db.Set<TEntity>().CountAsync());

    public static Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request) =>
        ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetService<IMediator>();

            return mediator.Send(request);
        });

    public static Task SendAsync(IRequest request) =>
        ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetService<IMediator>();

            return mediator.Send(request);
        });

    internal class HostingEnvironment : IWebHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Production;

        public string ApplicationName { get; set; }

        public string WebRootPath { get; set; }

        public IFileProvider WebRootFileProvider { get; set; }

        public string ContentRootPath { get; set; }

        public IFileProvider ContentRootFileProvider { get; set; }
    }
}
