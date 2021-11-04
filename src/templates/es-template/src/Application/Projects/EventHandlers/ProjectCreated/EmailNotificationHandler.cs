// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Application.Projects.EventHandlers.ProjectCreated;

using MediatR;
using NikiforovAll.ES.Template.Application.Interfaces;
using static NikiforovAll.ES.Template.Domain.ProjectAggregate.Events.Events.V1;

public class EmailNotificationHandler
    : INotificationHandler<ProjectCreated>
{
    private readonly IEmailSender emailSender;

    public EmailNotificationHandler(IEmailSender emailSender)
        => this.emailSender = emailSender;

    public async Task Handle(
        ProjectCreated notification, CancellationToken cancellationToken)
    {
        await this.emailSender.SendEmailAsync(
            "admin@ca.com", "system@ca.com",
            $"Project Created {notification.Name}", string.Empty);
    }
}
