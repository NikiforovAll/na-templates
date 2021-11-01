// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Tests.Common;

using NikiforovAll.ES.Template.Domain.SharedKernel.Aggregates;
using NikiforovAll.ES.Template.Domain.SharedKernel.Events;

public static class AggregateExtensions
{
    public static T? PublishedEvent<T>(this IAggregate aggregate)
        where T : class, IEvent =>
            aggregate.DequeueUncommittedEvents().LastOrDefault() as T;
}
