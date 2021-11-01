// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.SharedKernel.Models;

using Marten.Pagination;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public long PageIndex { get; }
    public long TotalPages { get; }
    public long TotalCount { get; }

    public PaginatedList(List<T> items, long count, long pageIndex, long pageSize)
    {
        this.PageIndex = pageIndex;
        this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        this.TotalCount = count;
        this.Items = items;
    }

    public bool HasPreviousPage => this.PageIndex > 1;

    public bool HasNextPage => this.PageIndex < this.TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source, int pageIndex, int pageSize)
    {
        var paged = await source.ToPagedListAsync(pageIndex, pageSize);

        return new PaginatedList<T>(
            paged.ToList(),
            paged.TotalItemCount,
            paged.PageNumber,
            paged.PageSize);
    }
}
