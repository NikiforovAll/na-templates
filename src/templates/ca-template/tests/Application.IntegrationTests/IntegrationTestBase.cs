// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.IntegrationTests;

using Nito.AsyncEx;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    private static readonly AsyncLock Mutex = new();

    private static bool initialized;

    protected Fixture Fixture { get; set; } = new Fixture();

    public virtual async Task InitializeAsync()
    {
        if (initialized)
        {
            return;
        }

        using (await Mutex.LockAsync())
        {
            if (initialized)
            {
                return;
            }
            await SliceFixture.ResetCheckpointAsync();
            initialized = true;
        }
    }

    public virtual Task DisposeAsync() => Task.CompletedTask;
}
