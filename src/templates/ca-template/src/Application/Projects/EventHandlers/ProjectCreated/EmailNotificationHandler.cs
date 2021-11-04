// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.Projects.EventHandlers.ProjectCreated;

using MediatR;
using NikiforovAll.CA.Template.Application.Interfaces;
using NikiforovAll.CA.Template.Application.SharedKernel.Models;
using NikiforovAll.CA.Template.Domain.ProjectAggregate.Events;

public class EmailNotificationHandler
    : INotificationHandler<DomainEventNotification<ProjectCreatedEvent>>
{
    private readonly IEmailSender emailSender;

    public EmailNotificationHandler(IEmailSender emailSender)
        => this.emailSender = emailSender;

    public async Task Handle(
        DomainEventNotification<ProjectCreatedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        await this.emailSender.SendEmailAsync(
            "admin@ca.com", "system@ca.com",
            $"Project Created {domainEvent.Project.Name}", string.Empty);
    }
}
