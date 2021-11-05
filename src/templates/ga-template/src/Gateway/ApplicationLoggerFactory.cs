// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.GA.Template.Gateway;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

public static class ApplicationLoggerFactory
{
    public static Logger CreateLogger(IConfiguration configuration, IHostEnvironment hostEnvironment) =>
        new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application",
                configuration["DOTNET_APPLICATIONNAME"] ?? hostEnvironment.ApplicationName)
            .Enrich.WithProperty("Environment", hostEnvironment.EnvironmentName)
            .CreateLogger();
}
