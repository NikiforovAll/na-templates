// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

// NOTE: this program is intended to be used as CLI
// Use `dotnet run -- help` for more details.

using Microsoft.Extensions.Hosting;
using Nikiforovall.ES.Template.Infrastructure;
using Oakton;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
        services.AddInfrastructure(hostContext.Configuration))
    .RunOaktonCommands(args);
