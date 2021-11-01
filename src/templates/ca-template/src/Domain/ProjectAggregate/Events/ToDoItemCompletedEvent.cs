// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Domain.ProjectAggregate.Events;
using NikiforovAll.CA.Template.Domain.ProjectAggregate;
using NikiforovAll.CA.Template.Domain.SharedKernel;

public class ToDoItemCompletedEvent : DomainEvent
{
    public ToDoItem CompletedItem { get; set; }

    public ToDoItemCompletedEvent(ToDoItem completedItem) => this.CompletedItem = completedItem;
}
