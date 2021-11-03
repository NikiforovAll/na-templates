// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Worker.Consumers.MarkToDoItemCompleteConsumer;

using MassTransit;
using MediatR;
using NikiforovAll.CA.Template.Application.ToDoItems.Commands.MarkComplete;
using NikiforovAll.CA.Template.Messaging.Contracts;

public class MarkToDoItemCompleteConsumer : IConsumer<IMarkToDoItemComplete>
{
    private readonly IMediator mediator;

    public MarkToDoItemCompleteConsumer(IMediator mediator) =>
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    public async Task Consume(ConsumeContext<IMarkToDoItemComplete> context)
    {
        var message = context.Message;
        var command = new MarkCompleteCommand
        {
            ItemId = message.ItemId,
            ProjectId = message.ProjectId,
        };
        await this.mediator.Send(command);
    }
}
