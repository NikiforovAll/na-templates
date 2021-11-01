// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Domain.ProjectAggregate;

public class ToDoItem
{
    public Guid Id { get; set; }

    public int ProjectNumber { get; set; }

    public Guid ProjectId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsDone { get; private set; }

    public ToDoItem(Guid id, int projectNumber, Guid projectId)
    {
        this.ProjectNumber = projectNumber;
        this.ProjectId = projectId;
        this.Id = id;
    }

    public void MarkComplete() => this.IsDone = true;

    public override string ToString()
    {
        var status = this.IsDone ? "Done!" : "Not done.";
        return $"{this.ProjectNumber}: Status: {status} - {this.Title} - {this.Description}";
    }
}
