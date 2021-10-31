// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nikiforovall.ES.Template.Domain.ProjectAggregate;

public class ToDoConfiguration : IEntityTypeConfiguration<ToDoItem>
{
    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.Property(t => t.Title)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Ignore(e => e.DomainEvents);
    }
}
