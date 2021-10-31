// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Tests.Common;

public class AutoProjectDataAttribute : AutoDataAttribute
{
    public AutoProjectDataAttribute()
        : base(() => new Fixture()
            .Customize(new ProjectCustomization())
        )
    { }
}
