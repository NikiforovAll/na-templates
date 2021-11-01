// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Infrastructure.Ids;

using Marten.Schema.Identity;
using NikiforovAll.ES.Template.Application.SharedKernel.Interfaces.IdGeneration;

public class MartenIdGenerator : IIdGenerator
{
    public Guid New() => CombGuidIdGeneration.NewGuid();
}
