// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NikiforovAll.CA.Template.Application;
using NikiforovAll.CA.Template.Console;
using NikiforovAll.CA.Template.Console.Commands.Migrate;
using NikiforovAll.CA.Template.Console.Commands.SeedCommands;
using NikiforovAll.CA.Template.Infrastructure;
using Serilog;

var runner = BuildCommandLine()
    .UseHost(_ => Host.CreateDefaultBuilder(args), (builder) =>
    {
        builder.UseEnvironment("CLI")
        .UseSerilog()
        .ConfigureServices((hostContext, services) =>
        {
            Log.Logger = CreateLogger(services);
            var configuration = hostContext.Configuration;
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddCliContainer();
        })
        .UseCommandHandler<MigrateCommand, MigrateCommand.Run>()
        .UseCommandHandler<SeedProjectCommand, SeedProjectCommand.Run>();

    }).UseDefaults().Build();

await runner.InvokeAsync(args);

static CommandLineBuilder BuildCommandLine()
{
    var root = new RootCommand();

    root.AddCommand(new MigrateCommand());
    root.AddCommand(new SeedProjectCommand());

    root.AddGlobalOption(new Option<bool>("--silent", "Disables diagnostics output"));
    root.Handler = CommandHandler.Create(() => root.Invoke("-h"));

    return new CommandLineBuilder(root);
}

static Serilog.Core.Logger CreateLogger(IServiceCollection services)
{
    var scope = services.BuildServiceProvider();
    var parseResult = scope.GetRequiredService<ParseResult>();
    var isSilentLogger = parseResult.ValueForOption<bool>("--silent");
    var loggerConfiguration = new LoggerConfiguration()
        .ReadFrom.Configuration(scope.GetRequiredService<IConfiguration>());

    if (isSilentLogger)
    {
        loggerConfiguration
            .MinimumLevel
            .Override("Microsoft.EntityFrameworkCore",
                Serilog.Events.LogEventLevel.Warning);
    }

    return loggerConfiguration.CreateLogger();
}
