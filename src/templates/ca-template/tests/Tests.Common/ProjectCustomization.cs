// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Tests.Common;

using NikiforovAll.CA.Template.Domain.ProjectAggregate;

public class ProjectCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Project>(composer => composer
                .Without(x => x.Created)
                .Without(x => x.LastModified)
        );
    }
}
