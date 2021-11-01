// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Infrastructure.Persistence.Repository;

using Marten;
using Microsoft.Extensions.Logging;
using NikiforovAll.ES.Template.Application.SharedKernel.Events;
using NikiforovAll.ES.Template.Application.SharedKernel.Repositories;
using NikiforovAll.ES.Template.Domain.SharedKernel.Aggregates;

public class MartenRepository<T> : IRepository<T> where T : class, IAggregate
{
    private readonly IDocumentSession documentSession;
    private readonly IEventBus eventBus;
    private readonly ILogger<MartenRepository<T>> logger;

    public MartenRepository(
        IDocumentSession documentSession,
        IEventBus eventBus,
        ILogger<MartenRepository<T>> logger)
    {
        this.documentSession = documentSession
            ?? throw new ArgumentNullException(nameof(documentSession));
        this.eventBus = eventBus
            ?? throw new ArgumentNullException(nameof(eventBus));
        this.logger = logger;
    }

    public Task<T?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        this.documentSession
            .Events
            .AggregateStreamAsync<T>(id, token: cancellationToken);

    public async Task AddAsync(T aggregate, CancellationToken cancellationToken)
    {
        var events = aggregate.DequeueUncommittedEvents();
        if (events.Length == 0)
        {
            this.logger.LogAggregateWithoutEvents();
            return;
        }
        this.documentSession.Events.StartStream<T>(
            aggregate.Id,
            events
        );
        await this.documentSession.SaveChangesAsync(cancellationToken);
        await this.eventBus.PublishAsync(events);
    }

    public Task UpdateAsync(T aggregate, CancellationToken cancellationToken) =>
        this.StoreAsync(aggregate, cancellationToken);

    public Task DeleteAsync(T aggregate, CancellationToken cancellationToken) =>
        this.StoreAsync(aggregate, cancellationToken);

    private async Task StoreAsync(T aggregate, CancellationToken cancellationToken)
    {
        var events = aggregate.DequeueUncommittedEvents();
        if (events.Length == 0)
        {
            this.logger.LogAggregateWithoutEvents();
            return;
        }
        await this.documentSession.Events.AppendExclusive(
            aggregate.Id,
            events
        );
        await this.documentSession.SaveChangesAsync(cancellationToken);
        await this.eventBus.PublishAsync(events);
    }
}
