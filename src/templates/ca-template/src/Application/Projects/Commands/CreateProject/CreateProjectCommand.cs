// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.Projects.Commands.CreateProject;

using AutoMapper;
using MediatR;
using Nikiforovall.CA.Template.Application.Interfaces;
using Nikiforovall.CA.Template.Application.Projects.Models;
using Nikiforovall.CA.Template.Domain.ProjectAggregate;
using Nikiforovall.CA.Template.Domain.ValueObjects;

public class CreateProjectCommand : IRequest<ProjectViewModel>
{
    public string Name { get; set; } = default!;

    public Colour Colour { get; set; } = default!;
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectViewModel>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public CreateProjectCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ProjectViewModel> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = MapFrom(request);
        this.context.Projects.Add(entity);
        await this.context.SaveChangesAsync(cancellationToken);
        return this.mapper.Map<ProjectViewModel>(entity);
    }

    public static Project MapFrom(CreateProjectCommand command) =>
        new(command.Name, command.Colour);
}
