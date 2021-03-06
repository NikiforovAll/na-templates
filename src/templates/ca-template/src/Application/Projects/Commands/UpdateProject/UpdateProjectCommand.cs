// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.Projects.Commands.UpdateProject;

using MediatR;
using NikiforovAll.CA.Template.Application.Interfaces;
using NikiforovAll.CA.Template.Application.SharedKernel.Exceptions;
using NikiforovAll.CA.Template.Domain.ProjectAggregate;

public class UpdateProjectCommand : IRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
}

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Unit>
{
    private readonly IApplicationDbContext context;

    public UpdateProjectCommandHandler(IApplicationDbContext context) =>
        this.context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await this.context
            .Projects
            .FindAsync(new object[] { request.Id }, cancellationToken);

        _ = entity ?? throw new NotFoundException(nameof(Project), request.Id);

        entity.UpdateName(request.Name);

        await this.context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
