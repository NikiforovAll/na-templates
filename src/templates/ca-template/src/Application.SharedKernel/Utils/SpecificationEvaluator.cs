// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.SharedKernel.Utils;

using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;

public static class SpecificationEvaluator
{
    public static IQueryable<T> ApplySpecification<T>(this DbSet<T> context, ISpecification<T> spec) where T : class =>
        Ardalis.Specification.EntityFrameworkCore
            .SpecificationEvaluator.Default
            .GetQuery(context.AsQueryable(), spec);
}
