// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Application.Projects.Commands.MarkComplete;

using MediatR;
using NikiforovAll.ES.Template.Application.SharedKernel.Repositories;
using NikiforovAll.ES.Template.Domain.ProjectAggregate;

public class MarkCompleteCommand : IRequest
{
    public Guid ProjectId { get; set; }

    public int ItemId { get; set; }
}

public class MarkCompleteCommandHandler : IRequestHandler<MarkCompleteCommand>
{
    private readonly IRepository<Project> repository;

    public MarkCompleteCommandHandler(IRepository<Project> repository) =>
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public Task<Unit> Handle(
        MarkCompleteCommand request, CancellationToken cancellationToken) =>
            this.repository.GetAndUpdateAsync(request.ProjectId,
                project => project.MarkComplete(request.ItemId),
                cancellationToken);
}
