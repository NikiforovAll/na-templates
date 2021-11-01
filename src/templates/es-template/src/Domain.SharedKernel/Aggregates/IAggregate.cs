// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Domain.SharedKernel.Aggregates;

using NikiforovAll.ES.Template.Domain.SharedKernel.Events;
using NikiforovAll.ES.Template.Domain.SharedKernel.Projections;

public interface IAggregate : IAggregate<Guid> { }

public interface IAggregate<out T> : IProjection
{
    T Id { get; }

    int Version { get; }

    IEvent[] DequeueUncommittedEvents();
}
