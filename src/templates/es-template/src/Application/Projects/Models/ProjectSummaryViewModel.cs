// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.Projects.Models;

using AutoMapper;
using Nikiforovall.ES.Template.Application.SharedKernel.Mappings;
using Nikiforovall.ES.Template.Domain.ProjectAggregate;

public class ProjectSummaryViewModel : IMapFrom<Project>
{
    public Guid Id { get; private set; }

    public string? Name { get; private set; }

    public ProjectStatus Status { get; private set; }

    public void Mapping(Profile profile) => profile.CreateMap<Project, ProjectSummaryViewModel>()
        .ForMember(p => p.Status, opt => opt.MapFrom(p => p.Status));
}
