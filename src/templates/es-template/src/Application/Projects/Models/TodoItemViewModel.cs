// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Application.Projects.Models;

using NikiforovAll.ES.Template.Application.SharedKernel.Mappings;
using NikiforovAll.ES.Template.Domain.ProjectAggregate;

public class TodoItemViewModel : IMapFrom<ToDoItem>
{
    public int ProjectNumber { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool IsDone { get; set; }
}
