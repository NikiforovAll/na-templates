// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Infrastructure.Persistence.Repository;

using Marten;

public class MartenDocumentStore : Application.SharedKernel.Repositories.IDocumentStore
{
    private readonly IQuerySession querySession;

    public MartenDocumentStore(IQuerySession querySession) =>
        this.querySession = querySession
            ?? throw new ArgumentNullException(nameof(querySession));

    public Task<T?> LoadAsync<T>(Guid id, CancellationToken token = default) where T : notnull =>
        this.querySession.LoadAsync<T>(id, token);

    public IQueryable<T> Query<T>() => this.querySession.Query<T>();
}
