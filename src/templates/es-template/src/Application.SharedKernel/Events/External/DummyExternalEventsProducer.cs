// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.SharedKernel.Events.External;

using System.Threading.Tasks;
using Nikiforovall.ES.Template.Application.SharedKernel.Events;

public class DummyExternalEventsProducer : IExternalEventProducer
{
    public Task PublishAsync(IExternalEvent @event) => Task.CompletedTask;
}
