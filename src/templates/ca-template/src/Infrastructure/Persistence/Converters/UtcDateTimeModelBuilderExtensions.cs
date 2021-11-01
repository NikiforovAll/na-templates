// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Infrastructure.Persistence.Converters;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public static class UtcDateTimeModelBuilderExtensions
{
    private static readonly ValueConverter<DateTime, DateTime> UtcConverter =
        new(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

    /// <summary>
    /// Make sure this is called after configuring all your entities.
    /// Sourced: <https://github.com/dotnet/efcore/issues/4711>.
    /// </summary>
    public static void ApplyUtcDateTimeConverter(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) ||
                        property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(UtcConverter);
                }
            }

        }
    }
}
