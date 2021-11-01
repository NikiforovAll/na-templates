// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.SharedKernel.Exceptions;

using FluentValidation.Results;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.") => this.Errors = new Dictionary<string, string[]>();

    public ValidationException(IEnumerable<ValidationFailure> failures) : this() =>
        this.Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

    public IDictionary<string, string[]> Errors { get; }
}
