// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Domain.SharedKernel.Exceptions;
public class AggregateNotFoundException : Exception
{
    public AggregateNotFoundException(string typeName, Guid id)
        : base($"{typeName} with id '{id}' was not found") { }

    public static AggregateNotFoundException For<T>(Guid id)
    {
        return new AggregateNotFoundException(typeof(T).Name, id);
    }
}
