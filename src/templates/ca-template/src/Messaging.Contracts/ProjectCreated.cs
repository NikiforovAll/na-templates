// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Messaging.Contracts;

public interface ProjectCreated
{
    public string Name { get; }

    public DateTime CreatedAt { get; }
}
