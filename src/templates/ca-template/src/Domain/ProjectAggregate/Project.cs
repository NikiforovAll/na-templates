// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Domain.ProjectAggregate;

using Nikiforovall.CA.Template.Domain.ProjectAggregate.Events;
using Nikiforovall.CA.Template.Domain.SharedKernel;
using Nikiforovall.CA.Template.Domain.ValueObjects;

public class Project : AuditableEntity, IHasDomainEvent, IAggregateRoot
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Colour Colour { get; private set; }

    private readonly List<ToDoItem> items;

    public IEnumerable<ToDoItem> Items => this.items?.AsReadOnly() ?? Enumerable.Empty<ToDoItem>();

    public ProjectStatus Status => this.items.All(i => i.IsDone) ? ProjectStatus.Complete : ProjectStatus.InProgress;

    public List<DomainEvent> DomainEvents { get; private set; } = new();

    /// <summary>
    /// EF required
    /// </summary>
    private Project() => this.items = new();

    public Project(string name, Colour colour) : this()
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }
        this.Name = name;
        this.Colour = colour;
    }

    public void AddItem(ToDoItem newItem)
    {
        if (newItem is null)
        {
            throw new ArgumentNullException(nameof(newItem));
        }

        this.items.Add(newItem);

        var newItemAddedEvent = new NewItemAddedEvent(this, newItem);
        this.DomainEvents.Add(newItemAddedEvent);
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException($"'{nameof(newName)}' cannot be null or whitespace.", nameof(newName));
        }

        this.Name = newName;
    }
}
