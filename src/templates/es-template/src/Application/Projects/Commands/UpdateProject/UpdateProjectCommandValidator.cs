// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.Projects.Commands.UpdateProject;

using FluentValidation;
using Nikiforovall.ES.Template.Application.Projects.Models;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        this.RuleFor(x => x.Name).ValidProjectName();
    }
}
