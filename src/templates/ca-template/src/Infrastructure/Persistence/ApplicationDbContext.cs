// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Infrastructure.Persistence;

using System.Data;
using System.Reflection;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nikiforovall.CA.Template.Application.Interfaces;
using Nikiforovall.CA.Template.Application.SharedKernel.Interfaces;
using Nikiforovall.CA.Template.Domain.ProjectAggregate;
using Nikiforovall.CA.Template.Domain.SharedKernel;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IDateTime dateTime;
    private readonly ICurrentUserService currentUserService;
    private readonly IDomainEventService domainEventService;

    private IDbContextTransaction? currentTransaction;

    public DbSet<ToDoItem> ToDoItems { get; set; }
    public DbSet<Project> Projects { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService,
        IDomainEventService domainEventService,
        IDateTime dateTime) : base(options)
    {
        this.currentUserService = currentUserService
            ?? throw new ArgumentNullException(nameof(currentUserService));
        this.domainEventService = domainEventService
            ?? throw new ArgumentNullException(nameof(domainEventService));
        this.dateTime = dateTime;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in this.ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = this.currentUserService.UserId;
                    entry.Entity.Created = this.dateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = this.currentUserService.UserId;
                    entry.Entity.LastModified = this.dateTime.Now;
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
                default:
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        await this.DispatchEventsAsync();

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        // specify DateTime.Kind = Utc on the way out.
        modelBuilder.ApplyUtcDateTimeConverter();
    }
    public async Task BeginTransactionAsync()
    {
        if (this.currentTransaction != null)
        {
            return;
        }

        this.currentTransaction = await this.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await this.SaveChangesAsync().ConfigureAwait(false);

            this.currentTransaction?.Commit();
        }
        catch
        {
            this.RollbackTransaction();
            throw;
        }
        finally
        {
            if (this.currentTransaction != null)
            {
                this.currentTransaction.Dispose();
                this.currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            this.currentTransaction?.Rollback();
        }
        finally
        {
            if (this.currentTransaction != null)
            {
                this.currentTransaction.Dispose();
                this.currentTransaction = null;
            }
        }
    }

    private async Task DispatchEventsAsync()
    {
        while (true)
        {
            var domainEventEntity = this.ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .FirstOrDefault(domainEvent => !domainEvent.IsPublished);

            if (domainEventEntity == null)
            {
                break;
            }

            domainEventEntity.IsPublished = true;
            await this.domainEventService.PublishAsync(domainEventEntity);
        }
    }
}
