// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Application.SharedKernel.Repositories;

using MediatR;
using NikiforovAll.ES.Template.Domain.SharedKernel.Aggregates;
using NikiforovAll.ES.Template.Domain.SharedKernel.Exceptions;

public static class RepositoryExtensions
{
    public static async Task<T> GetAsync<T>(
        this IRepository<T> repository,
        Guid id,
        CancellationToken cancellationToken = default) where T : IAggregate
    {
        var entity = await repository.FindAsync(id, cancellationToken);

        return entity ?? throw AggregateNotFoundException.For<T>(id);
    }

    public static async Task<Unit> GetAndUpdateAsync<T>(
        this IRepository<T> repository,
        Guid id,
        Action<T> action,
        CancellationToken cancellationToken = default) where T : IAggregate
    {
        var entity = await repository.GetAsync(id, cancellationToken);

        action(entity);

        await repository.UpdateAsync(entity, cancellationToken);

        return Unit.Value;
    }
}
