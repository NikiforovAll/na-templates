// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.Projects.Commands.CreateProject;

using FluentValidation;
using NikiforovAll.CA.Template.Application.Projects.Models;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        this.RuleFor(x => x.Name).ValidProjectName();
        this.RuleFor(x => x.ColourCode).NotEmpty();
    }
}
