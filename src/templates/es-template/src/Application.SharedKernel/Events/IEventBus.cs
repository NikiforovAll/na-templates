// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.SharedKernel.Events;

using Nikiforovall.ES.Template.Domain.SharedKernel.Events;

public interface IEventBus
{
    Task PublishAsync(params IEvent[] events);
}
