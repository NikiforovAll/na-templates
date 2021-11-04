// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.Projects.EventHandlers.ProjectCreated;

using MediatR;
using NikiforovAll.CA.Template.Application.SharedKernel.Events.External;
using NikiforovAll.CA.Template.Application.SharedKernel.Models;
using NikiforovAll.CA.Template.Domain.ProjectAggregate.Events;
using NikiforovAll.CA.Template.Messaging.Contracts;

public class ExternalEventDispatcherHandler
    : INotificationHandler<DomainEventNotification<ProjectCreatedEvent>>
{
    private readonly IExternalEventProducer externalEventProducer;

    public ExternalEventDispatcherHandler(IExternalEventProducer externalEventProducer) =>
        this.externalEventProducer = externalEventProducer;

    public async Task Handle(
        DomainEventNotification<ProjectCreatedEvent> notification,
        CancellationToken cancellationToken)
    {
        var project = notification.DomainEvent.Project;
        var externalEvent = new ProjectCreatedReportingEvent(
            project.Name, project.Created);

        await this.externalEventProducer.PublishAsync(externalEvent);
    }

    private record ProjectCreatedReportingEvent(
        string Name, DateTime CreatedAt) : ProjectCreated
    {
    }
}
