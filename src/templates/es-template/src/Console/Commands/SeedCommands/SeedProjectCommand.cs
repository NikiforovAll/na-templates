// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Console.Commands.SeedCommands;

using System.CommandLine;
using System.CommandLine.Invocation;
using Marten;
using Microsoft.Extensions.Logging;
using Spectre.Console;

public class SeedProjectCommand : Command
{
    public SeedProjectCommand()
        : base(name: "seed-projects", "Seeds projects into database as documents.")
    {
        this.AddOption(new Option<bool>("--dry-run", "Skip insertion into the database"));
        this.AddOption(new Option<int>(
            aliases: new string[] { "--number-of-projects", "-nop" },
            getDefaultValue: () => 1,
            description: "The number of projects."));
    }

    public class Run : ICommandHandler
    {
        private readonly ILogger<Run> logger;

        private readonly IDocumentStore db;

        public Run(ILogger<Run> logger, IDocumentStore db)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.db = db;
        }

        public bool DryRun { get; set; }

        public int NumberOfProjects { get; set; }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var faker = new ProjectFaker();
            var projects = faker.Generate(this.NumberOfProjects);

            if (!this.DryRun)
            {
                AnsiConsole.Status()
                    .Spinner(Spinner.Known.Material)
                    .Start("Inserting...", ctx => this.db.BulkInsert(projects, batchSize: 500));
            }

            this.logger.LogInformation("Done!");

            return 0;
        }
    }
}
