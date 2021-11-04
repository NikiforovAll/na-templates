// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Infrastructure.Services;

using System.Threading.Tasks;
using MassTransit;
using NikiforovAll.ES.Template.Application.SharedKernel.Events.External;
using NikiforovAll.ES.Template.Domain.SharedKernel.Events;

public class ExternalEventProducer : IExternalEventProducer
{
    private readonly IPublishEndpoint endpoint;

    public ExternalEventProducer(IPublishEndpoint endpoint)
    {
        this.endpoint = endpoint
            ?? throw new ArgumentNullException(nameof(endpoint));
    }

    public Task PublishAsync(IExternalEvent @event) =>
        this.endpoint.Publish(@event);
}
