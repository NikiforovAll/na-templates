// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Application.SharedKernel.Events;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NikiforovAll.ES.Template.Domain.SharedKernel.Events;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventHandler<TEvent, TEventHandler>(
        this IServiceCollection services
    )
        where TEvent : IEvent
        where TEventHandler : class, IEventHandler<TEvent> =>
            services
                .AddTransient<TEventHandler>()
                .AddTransient<INotificationHandler<TEvent>>(sp => sp.GetRequiredService<TEventHandler>())
                .AddTransient<IEventHandler<TEvent>>(sp => sp.GetRequiredService<TEventHandler>());
}
