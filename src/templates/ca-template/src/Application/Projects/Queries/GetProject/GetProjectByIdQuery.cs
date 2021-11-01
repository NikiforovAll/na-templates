// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.Projects.Queries.GetProject;

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nikiforovall.CA.Template.Application.Interfaces;
using Nikiforovall.CA.Template.Application.Projects.Models;
using Nikiforovall.CA.Template.Application.SharedKernel.Exceptions;
using Nikiforovall.CA.Template.Application.SharedKernel.Utils;
using Nikiforovall.CA.Template.Domain.ProjectAggregate.Specifications;

public class GetProjectByIdQuery : IRequest<ProjectViewModel>
{
    public Guid Id { get; set; }
}

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectViewModel>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetProjectByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<ProjectViewModel> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProjectByIdWithItemsSpecification(request.Id);

        var project = await this.context.Projects
            .ApplySpecification(spec)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        _ = project ?? throw new NotFoundException(nameof(Projects), request.Id);

        return this.mapper.Map<ProjectViewModel>(project);
    }
}
