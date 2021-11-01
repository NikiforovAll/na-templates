// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.Projects.Queries.GetProjects;

using AutoMapper;
using MediatR;
using Nikiforovall.ES.Template.Application.Projects.Models;
using Nikiforovall.ES.Template.Application.SharedKernel.Mappings;
using Nikiforovall.ES.Template.Application.SharedKernel.Models;
using Nikiforovall.ES.Template.Application.SharedKernel.Repositories;
using Nikiforovall.ES.Template.Domain.ProjectAggregate;

public class GetProjectsWithPaginationQuery : IRequest<PaginatedList<ProjectSummaryViewModel>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetProjectsWithPaginationQueryHandler
    : IRequestHandler<GetProjectsWithPaginationQuery, PaginatedList<ProjectSummaryViewModel>>
{
    private readonly IDocumentStore store;
    private readonly IMapper mapper;

    public GetProjectsWithPaginationQueryHandler(
        IDocumentStore store, IMapper mapper)
    {
        this.store = store ?? throw new ArgumentNullException(nameof(store));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PaginatedList<ProjectSummaryViewModel>> Handle(
        GetProjectsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var query = this.store.Query<Project>();

        var paged = await query
            .OrderBy(p => p.Name)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        var mapped = this.mapper.Map<List<ProjectSummaryViewModel>>(paged.Items);
        return new PaginatedList<ProjectSummaryViewModel>(
            mapped,
            paged.TotalCount,
            paged.PageIndex,
            paged.TotalPages);
    }
}
