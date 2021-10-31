// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.SharedKernel.PipelineBehaviors;

using Application.SharedKernel.Utils;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Nikiforovall.CA.Template.Application.SharedKernel;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        this.validators = validators;
        this.logger = logger;
    }

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var context = new ValidationContext<TRequest>(request);
        var typeName = request.GetGenericTypeName();
        this.logger.LogValidationExecuting(typeName);
        var failures = this.validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            this.logger.LogValidationExecuted(typeName, request, failures);
            throw new Exceptions.ValidationException(failures);
        }

        return next();
    }
}


