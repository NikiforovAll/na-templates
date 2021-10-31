// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.Projects.Queries.GetProjects;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nikiforovall.CA.Template.Application.Interfaces;
using Nikiforovall.CA.Template.Application.Projects.Models;
using Nikiforovall.CA.Template.Application.SharedKernel.Mappings;
using Nikiforovall.CA.Template.Application.SharedKernel.Models;

public class GetProjectsWithPaginationQuery : IRequest<PaginatedList<ProjectSummaryViewModel>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetProjectsWithPaginationQueryHandler
    : IRequestHandler<GetProjectsWithPaginationQuery, PaginatedList<ProjectSummaryViewModel>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetProjectsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PaginatedList<ProjectSummaryViewModel>> Handle(
        GetProjectsWithPaginationQuery request, CancellationToken cancellationToken) =>
            await this.context.Projects
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ProjectTo<ProjectSummaryViewModel>(this.mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
}
