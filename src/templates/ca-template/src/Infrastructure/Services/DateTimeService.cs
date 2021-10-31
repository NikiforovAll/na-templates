// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Infrastructure.Services;

using Nikiforovall.CA.Template.Application.SharedKernel.Interfaces;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}
