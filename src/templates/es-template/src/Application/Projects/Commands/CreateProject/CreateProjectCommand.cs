// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.Projects.Commands.CreateProject;

using MediatR;
using Nikiforovall.ES.Template.Application.SharedKernel.Repositories;
using Nikiforovall.ES.Template.Domain.ProjectAggregate;
using Nikiforovall.ES.Template.Domain.ValueObjects;

public class CreateProjectCommand : IRequest<Guid>
{
    public string Name { get; set; } = default!;

    public Colour Colour { get; set; } = default!;
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IRepository<Project> repository;

    public CreateProjectCommandHandler(IRepository<Project> repository) =>
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<Guid> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = new Project(Guid.NewGuid(), request.Name, request.Colour);

        await this.repository.AddAsync(project, cancellationToken);

        return project.Id;
    }
}
