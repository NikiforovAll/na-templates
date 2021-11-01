// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.Projects.Commands.DeleteProject;

using MediatR;
using Microsoft.EntityFrameworkCore;
using NikiforovAll.CA.Template.Application.Interfaces;
using NikiforovAll.CA.Template.Application.SharedKernel.Exceptions;
using NikiforovAll.CA.Template.Domain.ProjectAggregate;

public class DeleteProjectCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IApplicationDbContext context;

    public DeleteProjectCommandHandler(IApplicationDbContext context) =>
        this.context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await this.context
            .Projects
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        _ = entity ?? throw new NotFoundException(nameof(Project), request.Id);

        this.context.Projects.Remove(entity);

        await this.context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
