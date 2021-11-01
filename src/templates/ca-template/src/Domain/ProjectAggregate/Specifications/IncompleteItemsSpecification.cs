// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Domain.ProjectAggregate.Specifications;

using Ardalis.Specification;
using NikiforovAll.CA.Template.Domain.ProjectAggregate;

public class IncompleteItemsSpecification : Specification<ToDoItem>
{
    public IncompleteItemsSpecification() => this.Query.Where(item => !item.IsDone);
}
