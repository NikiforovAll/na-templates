// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Domain.SharedKernel;

public interface IHasDomainEvent
{
    public List<DomainEvent> DomainEvents { get; }
}

public abstract class DomainEvent
{
    protected DomainEvent() => this.DateOccurred = DateTimeOffset.UtcNow;
    public bool IsPublished { get; set; }
    public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
}
