// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.Projects.Models;

using AutoMapper;
using NikiforovAll.CA.Template.Application.SharedKernel.Mappings;
using NikiforovAll.CA.Template.Domain.ProjectAggregate;

public class ProjectViewModel : IMapFrom<Project>
{
    public Guid Id { get; private set; }

    public string? Name { get; private set; }

    public string DisplayName { get; private set; } = default!;

    public IEnumerable<TodoItemViewModel>? Items { get; private set; }

    public ProjectStatus Status { get; private set; }

    public void Mapping(Profile profile) => profile.CreateMap<Project, ProjectViewModel>()
        .ForMember(p => p.DisplayName, opt => opt.MapFrom(p => $"{p.Name}[{p.Colour}]"))
        .ForMember(p => p.Status, opt => opt.MapFrom(p => p.Status));
}
