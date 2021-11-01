// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Infrastructure.Ids;

using Marten.Schema.Identity;
using Nikiforovall.ES.Template.Application.SharedKernel.Interfaces.IdGeneration;

public class MartenIdGenerator : IIdGenerator
{
    public Guid New() => CombGuidIdGeneration.NewGuid();
}
