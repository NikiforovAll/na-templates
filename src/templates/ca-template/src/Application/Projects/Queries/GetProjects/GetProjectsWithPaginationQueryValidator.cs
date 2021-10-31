// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.Projects.Queries.GetProjects;

using FluentValidation;
using Nikiforovall.CA.Template.Application.SharedKernel.Models;

public class GetProjectsWithPaginationQueryValidator : AbstractValidator<GetProjectsWithPaginationQuery>
{
    public GetProjectsWithPaginationQueryValidator()
    {
        this.RuleFor(x => x.PageNumber).ValidPageNumber();

        this.RuleFor(x => x.PageSize).ValidPageSize();
    }
}
