// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Domain.ProjectAggregate.Specifications;

using Ardalis.Specification;
using Nikiforovall.CA.Template.Domain.ProjectAggregate;

public class ItemsSearchSpecification : Specification<ToDoItem>
{
    public ItemsSearchSpecification(string searchString) =>
        this.Query.Where(item => item.Title.Contains(searchString)
            || item.Description!.Contains(searchString));
}
