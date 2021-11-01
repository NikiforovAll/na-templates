// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NikiforovAll.CA.Template.Domain.ProjectAggregate;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .OwnsOne(b => b.Colour);

        builder.Ignore(e => e.DomainEvents);
    }
}
