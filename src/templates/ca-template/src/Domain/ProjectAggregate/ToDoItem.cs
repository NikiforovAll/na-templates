// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Domain.ProjectAggregate;

using System.Collections.Generic;
using NikiforovAll.CA.Template.Domain.ProjectAggregate.Events;
using NikiforovAll.CA.Template.Domain.SharedKernel;

public class ToDoItem : IHasDomainEvent
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsDone { get; private set; }

    private readonly List<DomainEvent> domainEvents = new();

    public IEnumerable<DomainEvent> DomainEvents => this.domainEvents.AsReadOnly();

    public void MarkComplete()
    {
        this.IsDone = true;

        this.domainEvents.Add(new ToDoItemCompletedEvent(this));
    }

    public override string ToString()
    {
        var status = this.IsDone ? "Done!" : "Not done.";
        return $"{this.Id}: Status: {status} - {this.Title} - {this.Description}";
    }
}
