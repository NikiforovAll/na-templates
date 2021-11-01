// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Tests.Common;

using NikiforovAll.ES.Template.Domain.ProjectAggregate;

public class ProjectCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ToDoItem>(composer => composer
            .FromFactory(() => new ToDoItem(Guid.NewGuid(), 0, Guid.Empty))
            .With(x => x.ProjectNumber, () => 0)
            .With(x => x.ProjectId, () => Guid.Empty)
            .WithAutoProperties()
        );
    }
}
