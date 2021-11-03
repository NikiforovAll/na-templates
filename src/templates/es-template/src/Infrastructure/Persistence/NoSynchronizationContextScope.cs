// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Infrastructure.Persistence;

public static class NoSynchronizationContextScope
{
    public static Disposable Enter()
    {
        var context = SynchronizationContext.Current;
        SynchronizationContext.SetSynchronizationContext(null);
        return new Disposable(context!);
    }

    public struct Disposable : IDisposable
    {
        private readonly SynchronizationContext synchronizationContext;

        public Disposable(SynchronizationContext synchronizationContext) =>
            this.synchronizationContext = synchronizationContext;

        public void Dispose() =>
            SynchronizationContext.SetSynchronizationContext(this.synchronizationContext);
    }
}
