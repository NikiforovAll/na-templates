// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.ToDoItems.Queries.SearchToDoItem;

using FluentValidation;
using NikiforovAll.CA.Template.Application.SharedKernel.Models;

public class SearchTodoItemQueryValidator : AbstractValidator<SearchTodoItemQuery>
{
    public SearchTodoItemQueryValidator()
    {
        this.RuleFor(x => x.PageNumber).ValidPageNumber();

        this.RuleFor(x => x.PageSize).ValidPageSize();

        this.RuleFor(x => x.SearchTerm).MinimumLength(4);
    }
}
