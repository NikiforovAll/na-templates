// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Infrastructure.Persistence;

using Marten;
using Marten.Events.Daemon.Resiliency;

public class MartenConfiguration
{
    private const string DefaultSchema = "public";

    public string ConnectionString { get; set; } = default!;

    public string WriteModelSchema { get; set; } = DefaultSchema;

    public string ReadModelSchema { get; set; } = DefaultSchema;

    public bool ShouldRecreateDatabase { get; set; }

    public DaemonMode DaemonMode { get; set; } = DaemonMode.Disabled;
}

internal static class MartenConfigurationExtensions
{
    internal static void SetupSchema(IDocumentStore documentStore, MartenConfiguration martenConfig, int retryLeft = 1)
    {
        try
        {
            if (martenConfig.ShouldRecreateDatabase)
            {
                documentStore.Advanced.Clean.CompletelyRemoveAll();
            }

            using (NoSynchronizationContextScope.Enter())
            {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                documentStore.Schema.ApplyAllConfiguredChangesToDatabaseAsync().Wait();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
            }
        }
        catch
        {
            if (retryLeft == 0)
            {
                throw;
            }

            Thread.Sleep(1000);
            SetupSchema(documentStore, martenConfig, --retryLeft);
        }
    }
}
