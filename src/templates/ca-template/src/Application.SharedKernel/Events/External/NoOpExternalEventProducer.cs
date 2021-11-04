// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.SharedKernel.Events.External;

using System.Threading.Tasks;

public class NoOpExternalEventProducer : IExternalEventProducer
{
    public List<object> Produced { get; } = new();

    public Task PublishAsync<T>(T @event) where T : class
    {
        this.Produced.Add(@event);
        return Task.CompletedTask;
    }
}
