// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Console.Commands.SeedCommands;

using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;
using Nikiforovall.CA.Template.Application.Interfaces;
using Spectre.Console;

public class SeedProjectCommand : Command
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

    public class Run : ICommandHandler
    {
        private readonly IApplicationDbContext context;
        private readonly ILogger<Run> logger;

        public Run(IApplicationDbContext context, ILogger<Run> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

            this.logger.LogInformation("Done!");

            return 0;
        }
    }
}
