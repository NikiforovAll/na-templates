// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.ToDoItems.Commands.MarkComplete;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nikiforovall.CA.Template.Application.Interfaces;
using Nikiforovall.CA.Template.Application.SharedKernel.Exceptions;
using Nikiforovall.CA.Template.Application.SharedKernel.Utils;
using Nikiforovall.CA.Template.Domain.ProjectAggregate.Specifications;

public class MarkCompleteCommand : IRequest
{
    public Guid ProjectId { get; set; }

    public int ItemId { get; set; }
}

public class MarkCompleteCommandHandler : IRequestHandler<MarkCompleteCommand>
{
    private readonly IApplicationDbContext context;

    public MarkCompleteCommandHandler(IApplicationDbContext context) =>
        this.context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<Unit> Handle(
        MarkCompleteCommand request,
        CancellationToken cancellationToken)
    {
        var spec = new ProjectByIdWithItemsSpecification(request.ProjectId);

        var project = await this.context.Projects
            .ApplySpecification(spec)
            .FirstOrDefaultAsync(cancellationToken);

        _ = project ?? throw new NotFoundException(nameof(Projects), request.ProjectId);

        project.Items.FirstOrDefault(i => i.Id == request.ItemId)?.MarkComplete();

        await this.context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
