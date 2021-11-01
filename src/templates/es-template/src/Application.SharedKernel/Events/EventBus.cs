// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.SharedKernel.Events;

using MediatR;
using Nikiforovall.ES.Template.Application.SharedKernel.Events.External;
using Nikiforovall.ES.Template.Domain.SharedKernel.Events;

public class EventBus : IEventBus
{
    private readonly IMediator mediator;
    private readonly IExternalEventProducer externalEventProducer;

    public EventBus(
        IMediator mediator,
        IExternalEventProducer externalEventProducer
    )
    {
        this.mediator = mediator
            ?? throw new ArgumentNullException(nameof(mediator));
        this.externalEventProducer = externalEventProducer
            ?? throw new ArgumentNullException(nameof(externalEventProducer));
    }

    public async Task PublishAsync(params IEvent[] events)
    {
        foreach (var @event in events)
        {
            await this.mediator.Publish(@event);

            if (@event is IExternalEvent externalEvent)
            {
                await this.externalEventProducer.PublishAsync(externalEvent);
            }
        }
    }
}
