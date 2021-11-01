// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Application.Projects.Commands.DeleteProject;

using MediatR;
using NikiforovAll.ES.Template.Application.SharedKernel.Repositories;
using NikiforovAll.ES.Template.Domain.ProjectAggregate;

public class DeleteProjectCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IRepository<Project> repository;

    public DeleteProjectCommandHandler(IRepository<Project> repository) =>
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<Unit> Handle(
        DeleteProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = await this.repository.GetAsync(request.Id, cancellationToken);
        project.Archieve();
        await this.repository.DeleteAsync(project, cancellationToken);

        return Unit.Value;
    }
}
