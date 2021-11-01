// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.SharedKernel.PipelineBehaviors;

using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch timer;
    private readonly ILogger<TRequest> logger;

    public PerformanceBehavior(ILogger<TRequest> logger)
    {
        this.timer = new Stopwatch();
        this.logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        this.timer.Start();

        var response = await next();

        this.timer.Stop();

        var elapsedMilliseconds = this.timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;

            this.logger.LogLongRunningOperation(requestName, elapsedMilliseconds);
        }

        return response;
    }
}
