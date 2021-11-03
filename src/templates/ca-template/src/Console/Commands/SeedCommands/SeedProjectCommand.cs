// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Console.Commands.SeedCommands;

using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;
using NikiforovAll.CA.Template.Application.Interfaces;
using Spectre.Console;

public partial class SeedProjectCommand : Command
{
    public SeedProjectCommand()
        : base(name: "seed-projects", "Seeds projects into database.")
    {
        this.AddOption(new Option<bool>("--dry-run", "Skip insertion into the database"));
        this.AddOption(new Option<int>(
            aliases: new string[] { "--number-of-projects", "-nop" },
            getDefaultValue: () => 1,
            description: "The number of projects."));
    }

    public partial class Run : ICommandHandler
    {
        private readonly IApplicationDbContext context;
        private readonly ILogger<SeedProjectCommand> logger;

        [LoggerMessage(1, LogLevel.Information, "Done!")]
        static partial void LogDone(ILogger logger);

        public Run(IApplicationDbContext context, ILogger<SeedProjectCommand> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger;
        }

        public bool DryRun { get; set; }

        public int NumberOfProjects { get; set; }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var faker = new ProjectFaker();
            var projects = faker.Generate(this.NumberOfProjects);

            if (!this.DryRun)
            {
                await AnsiConsole.Status()
                    .Spinner(Spinner.Known.Material)
                    .Start("Inserting...", async ctx =>
                    {
                        this.context.Projects.AddRange(projects);
                        _ = await this.context.SaveChangesAsync(CancellationToken.None);
                    });
            }

            LogDone(this.logger);

            return 0;
        }
    }
}
