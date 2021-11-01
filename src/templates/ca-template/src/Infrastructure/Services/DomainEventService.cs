// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Infrastructure.Services;

using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Nikiforovall.CA.Template.Application.SharedKernel;
using Nikiforovall.CA.Template.Application.SharedKernel.Interfaces;
using Nikiforovall.CA.Template.Application.SharedKernel.Models;
using Nikiforovall.CA.Template.Domain.SharedKernel;

public class DomainEventService : IDomainEventService
{
    private readonly ILogger<DomainEventService> logger;
    private readonly IPublisher mediator;

    public DomainEventService(ILogger<DomainEventService> logger, IPublisher mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task PublishAsync(DomainEvent domainEvent)
    {
        this.logger.LogDomainEventPublished(domainEvent.GetType().Name);

        var notification = GetNotificationCorrespondingToDomainEvent(domainEvent);

        if (notification is not null)
        {
            await this.mediator.Publish(notification);
        }
    }

    private static INotification? GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent) =>
        Activator.CreateInstance(typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent) as INotification;
}
