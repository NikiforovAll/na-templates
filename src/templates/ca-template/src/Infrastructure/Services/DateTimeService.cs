// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Infrastructure.Services;

using NikiforovAll.CA.Template.Application.SharedKernel.Interfaces;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}
