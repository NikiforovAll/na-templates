// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.SharedKernel.Repositories;

using Ardalis.Specification;

/// <inheritdoc/>
public class SpecificationEvaluator : ISpecificationEvaluator
{
    // Will use singleton for default configuration.
    // Yet, it can be instantiated if necessary, with default or provided evaluators.
    public static SpecificationEvaluator Default { get; } = new SpecificationEvaluator();

    private readonly List<IEvaluator> evaluators = new();

    public SpecificationEvaluator() => this.evaluators.AddRange(new IEvaluator[]
        {
                WhereEvaluator.Instance,
                OrderEvaluator.Instance,
                PaginationEvaluator.Instance,
        });
    public SpecificationEvaluator(IEnumerable<IEvaluator> evaluators) =>
        this.evaluators.AddRange(evaluators);

    /// <inheritdoc/>
    public virtual IQueryable<TResult> GetQuery<T, TResult>(
        IQueryable<T> inputQuery, ISpecification<T, TResult> specification) where T : class
    {
        inputQuery = this.GetQuery(inputQuery, (ISpecification<T>)specification);

        return inputQuery.Select(specification.Selector);
    }

    /// <inheritdoc/>
    public virtual IQueryable<T> GetQuery<T>(
        IQueryable<T> inputQuery,
        ISpecification<T> specification,
        bool evaluateCriteriaOnly = false) where T : class
    {
        var evaluators = evaluateCriteriaOnly
            ? this.evaluators.Where(x => x.IsCriteriaEvaluator)
            : this.evaluators;

        foreach (var evaluator in evaluators)
        {
            inputQuery = evaluator.GetQuery(inputQuery, specification);
        }

        return inputQuery;
    }
}

public static class SpecificationEvaluatorExtensions
{
    public static IQueryable<T> ApplySpecification<T>(
        this IQueryable<T> q, ISpecification<T> spec) where T : class =>
            SpecificationEvaluator.Default.GetQuery(q.AsQueryable(), spec);
}
