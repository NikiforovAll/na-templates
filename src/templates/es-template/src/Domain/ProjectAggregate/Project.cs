// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Domain.ProjectAggregate;

using NikiforovAll.ES.Template.Domain.SharedKernel.Aggregates;
using NikiforovAll.ES.Template.Domain.ValueObjects;
using static NikiforovAll.ES.Template.Domain.ProjectAggregate.Events.Events.V1;

public class Project : Aggregate
{
    public string Name { get; private set; }

    public Colour Colour { get; private set; }


    public IList<ToDoItem> Items { get; private set; }

    //private readonly List<ToDoItem> items;
    // TODO: encapsulate collection properly, currently, it causes bug during deserialization (Marten)
    //public IEnumerable<ToDoItem> Items => this.items?.AsReadOnly() ?? Enumerable.Empty<ToDoItem>();

    public ProjectStatus Status => this.Items.All(i => i.IsDone) ? ProjectStatus.Complete : ProjectStatus.InProgress;

    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Marten required
    /// </summary>
    private Project() => this.Items = new List<ToDoItem>();

    public Project(Guid id, string name, Colour colour) : this()
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }
        var @event = ProjectCreated.Create(
            id,
            name,
            colour.Code,
            DateTime.UtcNow);

        this.Enqueue(@event);
        this.Apply(@event);
    }

    private void Apply(ProjectCreated @event)
    {
        this.Version++;

        this.Id = @event.Id;
        this.Name = @event.Name;
        this.Colour = (Colour)@event.Colour;
        this.CreatedAt = @event.CreatedAt;
    }

    public void AddItem(ToDoItem newItem)
    {
        if (newItem is null)
        {
            throw new ArgumentNullException(nameof(newItem));
        }

        var eventId = this.Items is { Count: 0 } ? 0 : this.Items.Max(i => i.ProjectNumber);
        eventId++;
        var @event = NewItemAdded.Create(
            newItem.Id,
            eventId,
            newItem.Title,
            newItem.Description,
            this.Id);

        this.Enqueue(@event);
        this.Apply(@event);
    }

    private void Apply(NewItemAdded @event)
    {
        this.Version++;

        var todoItem = new ToDoItem(@event.ItemId, @event.ProjectNumber, this.Id)
        {
            Description = @event.Description,
            Title = @event.Title,
        };

        this.Items.Add(todoItem);
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException($"'{nameof(newName)}' cannot be null or whitespace.", nameof(newName));
        }

        var @event = NameUpdated.Create(newName);

        this.Enqueue(@event);
        this.Apply(@event);
    }

    private void Apply(NameUpdated @event)
    {
        this.Version++;

        this.Name = @event.Name;
    }

    public void MarkComplete(int todoItemId)
    {
        var todoItem = this.Items.FirstOrDefault(i => i.ProjectNumber == todoItemId);

        if (todoItem is null)
        {
            return;
        }

        var @event = ToDoItemCompleted.Create(this.Id, todoItemId);

        this.Enqueue(@event);
        this.Apply(@event);
    }

    private void Apply(ToDoItemCompleted @event)
    {
        this.Version++;

        this.Items.FirstOrDefault(i => i.ProjectNumber == @event.ItemId)?.MarkComplete();
    }

    public void Archieve()
    {
        var @event = ProjectArchieved.Create(this.Id, DateTime.UtcNow);

        this.Enqueue(@event);
    }
}
