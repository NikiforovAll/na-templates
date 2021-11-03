// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Worker.Consumers.CreateProjectConsumer;

using MassTransit;
using MediatR;
using NikiforovAll.CA.Template.Application.Projects.Commands.CreateProject;
using NikiforovAll.CA.Template.Messaging.Contracts;

public class CreateProjectConsumer : IConsumer<ICreateProject>
{
    private readonly IMediator mediator;

    public CreateProjectConsumer(IMediator mediator) =>
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    public async Task Consume(ConsumeContext<ICreateProject> context)
    {
        var message = context.Message;
        var command = new CreateProjectCommand
        {
            Name = message.Name,
            ColourCode = message.Colour,
        };
        await this.mediator.Send(command);
    }
}
