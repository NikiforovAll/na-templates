// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.SharedKernel.Mappings;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Marten;
using Nikiforovall.ES.Template.Application.SharedKernel.Models;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, int pageNumber, int pageSize) =>
            PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

    public static Task<IReadOnlyList<TDestination>> ProjectToListAsync<TDestination>(
        this IQueryable queryable, IConfigurationProvider configuration) =>
            queryable.ProjectTo<TDestination>(configuration).ToListAsync();
}
