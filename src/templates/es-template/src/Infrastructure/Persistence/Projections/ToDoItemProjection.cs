// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Infrastructure.Persistence.Projections;

using Marten;
using Marten.Events.Projections;
using Nikiforovall.ES.Template.Domain.ProjectAggregate;
using static Nikiforovall.ES.Template.Domain.ProjectAggregate.Events.Events.V1;

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
#pragma warning disable CA1822 // Mark members as static
public class ToDoItemProjection : EventProjection
{
    public ToDoItem Create(NewItemAdded item) => new(item.ItemId, item.ProjectNumber, item.ProjectId)
    {
        Description = item.Description,
        Title = item.Title,
    };

    public async Task Project(ToDoItemCompleted @event, IDocumentOperations ops)
    {
        var todo = await ops.LoadAsync<ToDoItem>(@event.ItemId);
        if (todo == null)
        {
            return;
        }
        todo.MarkComplete();
        ops.Update(todo);
    }
}
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
