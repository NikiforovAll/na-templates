// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.SharedKernel.Mappings;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NikiforovAll.CA.Template.Application.SharedKernel.Models;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, int pageNumber, int pageSize) =>
            PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(
        this IQueryable queryable, IConfigurationProvider configuration) =>
            queryable.ProjectTo<TDestination>(configuration).ToListAsync();
}
