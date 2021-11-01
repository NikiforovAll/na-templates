// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Console.Commands.Migrate;

using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;
using NikiforovAll.CA.Template.Infrastructure.Persistence;

public class MigrateCommand : Command
{
    public MigrateCommand()
        : base(name: "migrate", "Migrates the project database. WARNING: creates database if the specified database was not found.")
    {
    }

    public class Run : ICommandHandler
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<Run> logger;

        public Run(ApplicationDbContext context, ILogger<Run> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public IConsole Console { get; set; }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            await this.context.Database.EnsureCreatedAsync();

            this.logger.LogInformation("Done!");

            return 0;
        }
    }
}
