// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.SharedKernel;

using FluentValidation.Results;
using Microsoft.Extensions.Logging;

public static class LoggerExtensions
{
    private static readonly EventId ErrorCaught_EventId = 1000;
    private static readonly EventId RequestExecuting_EventId = 1001;
    private static readonly EventId RequestExecuted_EventId = 1002;
    private static readonly EventId PublishedDomainEvent_EventId = 1003;
    private static readonly EventId ValidationExecuting_EventId = 1004;
    private static readonly EventId ValidationExecuted_EventId = 1005;
    private static readonly EventId LongRunningOperationDetected_EventId = 1006;

    public static void LogValidationExecuting(this ILogger logger, string typeName) =>
        ValidationExecuting(logger, typeName, default!);

    public static void LogErrorCaught(
        this ILogger logger,
        string requestName,
        object request,
        Exception exception) => UnhandledExceptionCaught(logger, requestName, request, exception);

    public static void LogRequestStarted(
        this ILogger logger,
        string requestName,
        object command) => RequestExecuting(logger, requestName, command, default!);

    public static void LogRequestFinished(
        this ILogger logger,
        string requestName,
        object response) => RequestExecuted(logger, requestName, response, default!);

    public static void LogDomainEventPublished(
        this ILogger logger,
        string eventName) => PublishedDomainEvent(logger, eventName, default!);

    public static void LogValidationExecuted(
        this ILogger logger,
        string typeName,
        object request,
        List<ValidationFailure> failures) => ValidationExecuted(logger, typeName, request, failures, default!);

    public static void LogLongRunningOperation(
        this ILogger logger,
        string requestName,
        long elapsedMilliseconds) => LongRunningOperation(logger, elapsedMilliseconds, requestName, default!);

    #region LoggerMessageDefines
    private static readonly Action<ILogger, string, Exception> ValidationExecuting =
        LoggerMessage.Define<string>(
            logLevel: LogLevel.Information,
            eventId: ErrorCaught_EventId,
            formatString: "Validating command {CommandType}");

    private static readonly Action<ILogger, string, object, List<ValidationFailure>, Exception> ValidationExecuted =
        LoggerMessage.Define<string, object, List<ValidationFailure>>(
            logLevel: LogLevel.Information,
            eventId: ValidationExecuting_EventId,
            formatString: "Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}");

    private static readonly Action<ILogger, string, object, Exception> RequestExecuting =
    LoggerMessage.Define<string, object>(
        logLevel: LogLevel.Information,
        eventId: RequestExecuting_EventId,
        formatString: "----- Handling command {CommandName} ({@Command})");

    private static readonly Action<ILogger, string, object, Exception> RequestExecuted =
        LoggerMessage.Define<string, object>(
            logLevel: LogLevel.Information,
            eventId: RequestExecuted_EventId,
            formatString: "----- Command {CommandName} handled - response: {@Response}");

    private static readonly Action<ILogger, string, Exception> PublishedDomainEvent =
    LoggerMessage.Define<string>(
        logLevel: LogLevel.Information,
        eventId: PublishedDomainEvent_EventId,
        formatString: "Publishing domain event. Event - {Event}");

    private static readonly Action<ILogger, string, object, Exception> UnhandledExceptionCaught =
    LoggerMessage.Define<string, object>(
        logLevel: LogLevel.Error,
        eventId: ValidationExecuted_EventId,
        formatString: "Request: Unhandled Exception for Request {Name} {@Request}");

    private static readonly Action<ILogger, long, string, Exception> LongRunningOperation =
        LoggerMessage.Define<long, string>(
            logLevel: LogLevel.Warning,
            eventId: LongRunningOperationDetected_EventId,
            formatString: "Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds)");
    #endregion
}
