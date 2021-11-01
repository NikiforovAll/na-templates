// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Infrastructure.Persistence.Projections;

using Marten;
using Marten.Events.Projections;
using NikiforovAll.ES.Template.Domain.ProjectAggregate;
using static NikiforovAll.ES.Template.Domain.ProjectAggregate.Events.Events.V1;

#pragma warning disable CA1822 // Mark members as static
public class ProjectArchivedProjection : EventProjection
{
    public void Project(ProjectArchieved @event, IDocumentOperations ops)
        => ops.Delete<Project>(@event.ProjectId);
}
#pragma warning restore CA1822 // Mark members as static
