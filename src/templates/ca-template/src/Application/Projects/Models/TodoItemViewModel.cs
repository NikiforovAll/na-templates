// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.Projects.Models;

using Nikiforovall.CA.Template.Application.SharedKernel.Mappings;
using Nikiforovall.CA.Template.Domain.ProjectAggregate;

public class TodoItemViewModel : IMapFrom<ToDoItem>
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool IsDone { get; set; }
}
