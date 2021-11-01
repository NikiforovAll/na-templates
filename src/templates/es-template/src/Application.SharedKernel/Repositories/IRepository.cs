// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.SharedKernel.Repositories;

using Nikiforovall.ES.Template.Domain.SharedKernel.Aggregates;

public interface IRepository<T> where T : IAggregate
{
    Task<T?> FindAsync(Guid id, CancellationToken cancellationToken);

    Task AddAsync(T aggregate, CancellationToken cancellationToken);

    Task UpdateAsync(T aggregate, CancellationToken cancellationToken);

    Task DeleteAsync(T aggregate, CancellationToken cancellationToken);
}
