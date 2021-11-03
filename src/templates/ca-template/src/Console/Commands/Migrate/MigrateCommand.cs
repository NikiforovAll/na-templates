// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Console.Commands.Migrate;

using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;
using NikiforovAll.CA.Template.Infrastructure.Persistence;

public partial class MigrateCommand : Command
{
    public MigrateCommand()
        : base(
            name: "migrate",
            description: "Migrates the project database. WARNING: creates database if the specified database was not found.")
    {
    }

    public partial class Run : ICommandHandler
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<MigrateCommand> logger;

        [LoggerMessage(0, LogLevel.Information, "Done!")]
        static partial void LogDone(ILogger logger);

        public Run(ApplicationDbContext context, ILogger<MigrateCommand> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            await this.context.Database.EnsureCreatedAsync();

            LogDone(this.logger);

            return 0;
        }
    }
}
