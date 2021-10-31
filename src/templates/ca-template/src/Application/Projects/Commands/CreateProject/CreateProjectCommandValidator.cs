// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.Projects.Commands.CreateProject;

using FluentValidation;
using Nikiforovall.CA.Template.Application.Projects.Models;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        this.RuleFor(x => x.Name).ValidProjectName();
        this.RuleFor(x => x.Colour).NotNull();
    }
}
