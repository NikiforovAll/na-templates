// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Infrastructure;

using Microsoft.Extensions.Logging;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, Exception> SavingAggregateWithoutEvents;

    static LoggerExtensions() => SavingAggregateWithoutEvents = LoggerMessage.Define(
            logLevel: LogLevel.Information,
            eventId: 1001,
            formatString: "Aggregate was not updated due to lack of uncommited events in the queue.");

    public static void LogAggregateWithoutEvents(this ILogger logger) => SavingAggregateWithoutEvents(logger, default!);
}
