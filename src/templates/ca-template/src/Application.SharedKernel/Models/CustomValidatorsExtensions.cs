// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.SharedKernel.Models;

using FluentValidation;

public static class CustomValidatorsExtensions
{
    public static IRuleBuilderOptions<T, int> ValidPageNumber<T>(this IRuleBuilder<T, int> ruleBuilder) =>
        ruleBuilder
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber should be at least greater than or equal to 1.");

    public static IRuleBuilderOptions<T, int> ValidPageSize<T>(this IRuleBuilder<T, int> ruleBuilder) =>
        ruleBuilder
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should be at least greater than or equal to 1.");
}
