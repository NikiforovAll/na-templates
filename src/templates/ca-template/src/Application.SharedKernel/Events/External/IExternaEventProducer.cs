// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
#pragma warning disable CA1716 // Identifiers should not match keywords

namespace NikiforovAll.CA.Template.Application.SharedKernel.Events.External;

public interface IExternalEventProducer
{
    Task PublishAsync<T>(T @event) where T : class;
}
#pragma warning restore CA1716 // Identifiers should not match keywords
