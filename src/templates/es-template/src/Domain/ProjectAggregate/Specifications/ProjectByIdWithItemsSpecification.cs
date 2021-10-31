// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Domain.ProjectAggregate.Specifications;

using Ardalis.Specification;

public class ProjectByIdWithItemsSpecification : Specification<Project>, ISingleResultSpecification
{
    public ProjectByIdWithItemsSpecification(Guid projectId) =>
        this.Query
            .Where(project => project.Id == projectId)
            .Include(project => project.Items);
}
