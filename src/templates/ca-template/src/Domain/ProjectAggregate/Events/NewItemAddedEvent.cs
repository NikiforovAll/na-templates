// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Domain.ProjectAggregate.Events;
using Nikiforovall.CA.Template.Domain.ProjectAggregate;
using Nikiforovall.CA.Template.Domain.SharedKernel;

public class NewItemAddedEvent : DomainEvent
{
    public ToDoItem NewItem { get; set; }
    public Project Project { get; set; }

    public NewItemAddedEvent(Project project,
        ToDoItem newItem)
    {
        this.Project = project;
        this.NewItem = newItem;
    }
}
