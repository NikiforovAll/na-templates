// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Worker.Consumers.MarkToDoItemCompleteConsumer;

using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

public class MarkToDoItemCompleteConsumerDefinition
    : ConsumerDefinition<MarkToDoItemCompleteConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<MarkToDoItemCompleteConsumer> consumerConfigurator)
    {
    }
}
