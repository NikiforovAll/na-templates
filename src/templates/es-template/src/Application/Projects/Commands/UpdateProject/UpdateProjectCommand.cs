// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Application.Projects.Commands.UpdateProject;

using MediatR;
using NikiforovAll.ES.Template.Application.SharedKernel.Repositories;
using NikiforovAll.ES.Template.Domain.ProjectAggregate;

public class UpdateProjectCommand : IRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
}

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IRepository<Project> repository;

    public UpdateProjectCommandHandler(IRepository<Project> repository) =>
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public Task<Unit> Handle(
        UpdateProjectCommand request, CancellationToken cancellationToken) =>
            this.repository.GetAndUpdateAsync(request.Id,
                project => project.UpdateName(request.Name),
                cancellationToken);
}
