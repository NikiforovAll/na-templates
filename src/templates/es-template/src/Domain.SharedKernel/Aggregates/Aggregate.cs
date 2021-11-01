// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Domain.SharedKernel.Aggregates;

using NikiforovAll.ES.Template.Domain.SharedKernel.Events;

public abstract class Aggregate : Aggregate<Guid>, IAggregate { }

public abstract class Aggregate<T> : IAggregate<T> where T : notnull
{
    public T Id { get; protected set; } = default!;

    public int Version { get; protected set; }

    [NonSerialized] private readonly Queue<IEvent> uncommittedEvents = new();

    public virtual void When(object @event) { }

    public IEvent[] DequeueUncommittedEvents()
    {
        var dequeuedEvents = this.uncommittedEvents.ToArray();

        this.uncommittedEvents.Clear();

        return dequeuedEvents;
    }

    protected void Enqueue(IEvent @event) => this.uncommittedEvents.Enqueue(@event);
}
