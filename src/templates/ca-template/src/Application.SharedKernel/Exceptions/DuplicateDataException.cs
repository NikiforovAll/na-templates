// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.SharedKernel.Exceptions;

public class DuplicateDataException : Exception
{
    public object? Key { get; }

    public DuplicateDataException()
        : base()
    {
    }

    public DuplicateDataException(string message)
        : base(message)
    {
    }

    public DuplicateDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DuplicateDataException(string name, object key)
        : base($"Entity \"{name}\" ({key}) already exists.") => this.Key = key;
}
