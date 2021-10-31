// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.Interfaces;

using Microsoft.EntityFrameworkCore;
using Nikiforovall.ES.Template.Domain.ProjectAggregate;

public interface IApplicationDbContext
{
    public DbSet<ToDoItem> ToDoItems { get; set; }
    public DbSet<Project> Projects { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}