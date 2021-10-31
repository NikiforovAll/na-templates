﻿// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Domain.ProjectAggregate.Events;
using Nikiforovall.ES.Template.Domain.ProjectAggregate;
using Nikiforovall.ES.Template.Domain.SharedKernel;

public class ToDoItemCompletedEvent : DomainEvent
{
    public ToDoItem CompletedItem { get; set; }

    public ToDoItemCompletedEvent(ToDoItem completedItem) => this.CompletedItem = completedItem;
}
