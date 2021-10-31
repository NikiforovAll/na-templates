// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Worker;

using Nikiforovall.ES.Template.Application.SharedKernel.Interfaces;

public class WorkerUserService : ICurrentUserService
{
    public string? UserId => default;
}
