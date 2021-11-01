// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.Projects.Queries.GetProject;

using AutoMapper;
using MediatR;
using Nikiforovall.ES.Template.Application.Projects.Models;
using Nikiforovall.ES.Template.Application.SharedKernel.Exceptions;
using Nikiforovall.ES.Template.Application.SharedKernel.Repositories;
using Nikiforovall.ES.Template.Domain.ProjectAggregate;

public class GetProjectByIdQuery : IRequest<ProjectViewModel>
{
    public Guid Id { get; set; }
}

public class GetProjectByIdQueryHandler
    : IRequestHandler<GetProjectByIdQuery, ProjectViewModel>
{
    private readonly IDocumentStore repository;
    private readonly IMapper mapper;

    public GetProjectByIdQueryHandler(
        IDocumentStore store,
        IMapper mapper)
    {
        this.repository = store ?? throw new ArgumentNullException(nameof(store));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ProjectViewModel> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await this.repository.LoadAsync<Project>(request.Id, cancellationToken);

        _ = entity ?? throw new NotFoundException(nameof(Project), request.Id);
        var project = this.mapper.Map<ProjectViewModel>(entity);

        return project;
    }
}
