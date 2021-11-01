// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.SharedKernel.PipelineBehaviors;

using MediatR;
using Microsoft.Extensions.Logging;
using Nikiforovall.CA.Template.Application.SharedKernel;

public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> logger;

    public UnhandledExceptionBehavior(ILogger<TRequest> logger) => this.logger = logger;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception exception)
        {
            var requestName = typeof(TRequest).Name;

            this.logger.LogErrorCaught(requestName, request, exception);
            throw;
        }
    }
}
