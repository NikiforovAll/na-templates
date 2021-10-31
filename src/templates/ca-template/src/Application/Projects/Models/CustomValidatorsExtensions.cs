// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.Projects.Models;

using FluentValidation;

public static class CustomValidatorsExtensions
{
    public static IRuleBuilderOptions<T, string> WithinRange<T>(
        this IRuleBuilder<T, string> ruleBuilder, int minLength, int maxLength) =>
            ruleBuilder
                .MinimumLength(minLength).WithMessage("{PropertyName} must be at least {MinLength} characters.")
                .MaximumLength(maxLength).WithMessage("{PropertyName} must not exceed {MaxLength} characters.");

    public static IRuleBuilderOptions<T, string> ValidProjectName<T>(
        this IRuleBuilder<T, string> ruleBuilder, int minLength = 3, int maxLength = 255) =>
            ruleBuilder.WithinRange(minLength, maxLength);
}
